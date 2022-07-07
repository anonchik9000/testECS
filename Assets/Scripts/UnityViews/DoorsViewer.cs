using Game.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsViewer : MonoBehaviour
{
    public GameObject Prefab;
    private Dictionary<int, Transform> _doorCache;

    private void Awake()
    {
        _doorCache = new Dictionary<int, Transform>();
    }
    internal void UpdateView(int doorEntity, DoorCmp doorData, TranformCmp transformData)
    {
        if(!_doorCache.ContainsKey(doorEntity))
        {
            var clone = GameObject.Instantiate(Prefab);
            clone.transform.parent = transform;
            _doorCache[doorEntity] = clone.transform;
        }
        var door = _doorCache[doorEntity];
        door.localScale = transformData.Size;
        var position = transformData.Position;
        position.y -= (1 - doorData.OpeningProgress) * transformData.Size.y;
        door.position = position;
        door.localEulerAngles = transformData.EulerRotation;


    }
}
