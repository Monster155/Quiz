using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CellScript : MonoBehaviour, IClickable
{
    public event Action CorrectButtonTapped;

    public GameObject imageGameObject;

    private bool _isAnswer = false;
    private bool _isFirst = false;
    private ParticleController _particleController;

    public bool IsAnswer
    {
        set => _isAnswer = value;
    }

    public bool IsFirst
    {
        set => _isFirst = value;
    }

    public ParticleController ParticleController
    {
        set => _particleController = value;
    }

    private void Start()
    {
        if (_isFirst)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1.2f, 1).OnComplete(
                () => transform.DOScale(1f, 0.4f));
        }
    }

    public void Tapped()
    {
        if (_isAnswer)
        {
            // TODO object in rect bounce
            Sequence easeInBoundSequence = DOTween.Sequence();
            var scale = imageGameObject.transform.localScale;
            easeInBoundSequence.Append(imageGameObject.transform.DOScale(scale * 1.5f, 0.5f));
            easeInBoundSequence.Append(imageGameObject.transform.DOScale(scale, 0.5f));
            // TODO stars particles
            _particleController.LevelFinished();
            StartCoroutine(DelayBeforeNextLevel(1));
        }
        else
        {
            Sequence easeInBoundSequence = DOTween.Sequence();
            var positionX = transform.localPosition.x;
            for (int i = 10; i >= 0; i -= 2)
            {
                easeInBoundSequence.Append(transform.DOLocalMoveX(
                    i % 4 == 0 ? positionX - i : positionX + i,
                    0.2f));
            }

            /*easeInBoundSequence.Append(transform.DOLocalMoveX(positionX + 10, 0.2f));
            easeInBoundSequence.Append(transform.DOLocalMoveX(positionX - 8, 0.2f));
            easeInBoundSequence.Append(transform.DOLocalMoveX(positionX + 6, 0.2f));
            easeInBoundSequence.Append(transform.DOLocalMoveX(positionX - 4, 0.2f));
            easeInBoundSequence.Append(transform.DOLocalMoveX(positionX + 2, 0.2f));
            easeInBoundSequence.Append(transform.DOLocalMoveX(positionX - 0, 0.2f));*/
        }

        Debug.Log("Cell Tapped");
    }

    IEnumerator DelayBeforeNextLevel(float duration)
    {
        yield return new WaitForSeconds(duration);
        CorrectButtonTapped?.Invoke();
    }
}