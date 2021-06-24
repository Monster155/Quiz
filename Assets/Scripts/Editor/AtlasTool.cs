using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AtlasTool : Editor
{
    [MenuItem("Tools/Slice Atlas")]
    public static void SliceAtlas()
    {
        if (!Directory.Exists("Assets/Images/Solo"))
        {
            Directory.CreateDirectory("Assets/Images/Solo");
        }

        var dir = new DirectoryInfo("Assets/Images/Atlases");
        var imgs = dir.GetFiles("*.png");
        for (int i = 0; i < imgs.Length; i++)
        {
            DealPng(imgs[i]);
        }
    }

    public static void DealPng(FileInfo file)
    {
        var outdir = "Assets/Images/Solo/" + file.Name.Replace(".png", "");
        if (!Directory.Exists(outdir))
        {
            Directory.CreateDirectory(outdir);
        }

        string path = "Assets/Images/Atlases/" + file.Name;
        var assets2 = AssetDatabase.LoadAllAssetsAtPath(path);
        for (int i = 0; i < assets2.Length; i++)
        {
            Debug.Log("Image sliced: " + assets2[i]);
            if (assets2[i] is Sprite)
            {
                var sp = assets2[i] as Sprite;
                Texture2D t2d = new Texture2D((int) sp.rect.width, (int) sp.rect.height, TextureFormat.RGBA32, false);
                var aslasTexture = sp.texture;
                t2d.SetPixels(aslasTexture.GetPixels((int) sp.rect.x, (int) sp.rect.y, (int) sp.rect.width,
                    (int) sp.rect.height));
                t2d.Apply();
                File.WriteAllBytes(outdir + "/" + sp.name + ".png", t2d.EncodeToPNG());
            }
        }
    }
}