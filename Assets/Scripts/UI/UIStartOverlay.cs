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

    private void Awake()
    {
        gameObject.SetActive(showStartOverlay.Value);
    }
}