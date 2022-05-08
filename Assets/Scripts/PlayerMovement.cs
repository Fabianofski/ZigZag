using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _dir = new Vector2(1,1);
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float increaseOverTime;
    [SerializeField] private BoolVariable isDead;

    private void Update()
    {
        if (isDead.Value) return;
        
        transform.Translate(_dir * speed * Time.deltaTime);
        
        if (speed >= maxSpeed)
            speed = maxSpeed;
        else
            speed += Time.deltaTime / increaseOverTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 3) return;
        Debug.Log("Dead");
        isDead.Value = true;
    }

    private void OnChangeDirection()
    {
        _dir.y = -_dir.y;
    }
}
