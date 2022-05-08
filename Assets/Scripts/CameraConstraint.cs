using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstraint : MonoBehaviour
{

    [SerializeField] private Transform player;
    
    void Update()
    {
        transform.position = new Vector2(Mathf.Clamp(player.position.x, 1, Mathf.Infinity), transform.position.y);
    }
}
