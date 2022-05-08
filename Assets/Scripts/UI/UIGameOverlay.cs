using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TubeMeshGeneration;

public class UIGameOverlay : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private LeanTweenType tweenType;


    public void UpdateScore(int score)
    {
        LeanTween.scale(scoreText.gameObject, new Vector3(1.2f, 1.2f, 1.2f), .25f).setEase(tweenType).setOnComplete( ()=>
            LeanTween.scale(scoreText.gameObject, Vector3.one, .25f).setEase(tweenType));
        scoreText.text = score + "";
    }

}
