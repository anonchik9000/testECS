using Game.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterViewer : MonoBehaviour
{
    internal void UpdateView(MovableCmp playerData)
    {
        transform.position = new Vector3(playerData.Position.x, transform.position.y, playerData.Position.y);
    }
}
