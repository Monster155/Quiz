using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void LevelFinished()
    {
        particleSystem.Play();
    }
}
