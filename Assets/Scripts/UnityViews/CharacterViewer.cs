using Game.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewer : MonoBehaviour
{
    public Animator Animator;
    public Transform Graphics;
    internal void UpdateView(TranformCmp playerData,Vector3 endPoint)
    {
        transform.position = playerData.Position;
        var direction = (endPoint - transform.position);
        Animator.SetBool("Move", direction.magnitude > 0.1);
        var forward = direction.normalized;
        if(forward.sqrMagnitude>float.Epsilon)
            Graphics.forward = forward;
    }
}
