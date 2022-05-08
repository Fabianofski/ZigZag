using System;
using System.Collections;
using System.Collections.Generic;
using TubeMeshGeneration;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.Serialization;

public class RemoveTubeTrigger : MonoBehaviour
{

    [SerializeField] private VoidEvent removeTubeTriggerEvent;

    private void Awake()
    {
        transform.localScale = new Vector3(Tube.GetSize(), Tube.GetSize(), 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        removeTubeTriggerEvent.Raise();
        Destroy(gameObject);
    }
}
