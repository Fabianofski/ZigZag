using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TubeMeshGeneration;
using UnityEngine;
using UnityEngine.Serialization;
using UnityAtoms.BaseAtoms;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] public Material mat;
    private Tube _mainTube;
    [SerializeField] private VoidEvent removeTubeInVoid;
    [SerializeField] public GameObject removeTrigger;
    [SerializeField] public GameObject scoreTrigger;
    [SerializeField] private float decreaseFactor;
    
    private void Start()
    {
        _mainTube = new Tube(this);
        removeTubeInVoid.UnregisterAll();
        removeTubeInVoid.Register(RemoveTubeInVoid);

        bool temp = true;
        for (int i = 0; i < 10; i++)
        {
            _mainTube.AddVertices(temp);
            temp = !temp;
        }
        
        GetComponent<MeshRenderer>().material = mat;
        GetComponent<MeshFilter>().mesh = _mainTube.Terrain;
    }


    private void RemoveTubeInVoid()
    {
        _mainTube.RemoveLastVertices();
    }
}
