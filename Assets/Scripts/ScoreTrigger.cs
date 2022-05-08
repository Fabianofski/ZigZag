using System;
using System.Collections;
using System.Collections.Generic;
using TubeMeshGeneration;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.Serialization;

public class ScoreTrigger : MonoBehaviour
{

    [SerializeField] private IntVariable score;
    private void Awake()
    {
        transform.localScale = new Vector3(Tube.GetSize(), Tube.GetSize(), 1);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        score.Value++;
        Destroy(gameObject);
    }
}