using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityAtoms.BaseAtoms;
using TubeMeshGeneration;

public class UIStartOverlay : MonoBehaviour
{

    [SerializeField] private BoolVariable showStartOverlay;
    [SerializeField] private BoolVariable playerIsPaused;
    
    [SerializeField] private GameObject[] tweenIn;
    [SerializeField] private LeanTweenType tweenType;
    
    private void Awake()
    {
        playerIsPaused.Value = showStartOverlay.Value;
        gameObject.SetActive(showStartOverlay.Value);
        TweenUIElements();
    }

    private void TweenUIElements()
    {
        foreach (var obj in tweenIn) obj.transform.localScale = Vector3.zero;
        foreach (var obj in tweenIn) LeanTween.scale(obj, Vector3.one, 1f).setEase(tweenType);
    }
    
    public void StartGame()
    {
        showStartOverlay.Value = false;
        playerIsPaused.Value = false;
        gameObject.SetActive(false);
    }
}