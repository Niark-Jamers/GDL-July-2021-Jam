using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVelocityCinematique : MonoBehaviour
{
    Animator animator;
    Vector3 lastPos;

    void Start()
    {
        lastPos = transform.parent.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 d = transform.parent.position - lastPos;
        Debug.Log(d);
        animator.SetFloat("Velocity", d.x * 10);
        lastPos = transform.parent.position;
    }
}
