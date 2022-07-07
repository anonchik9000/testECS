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
    internal void UpdateView(int doorEntity, DoorCmp doorData)
    {
        if(!_doorCache.ContainsKey(doorEntity))
        {
            var clone = GameObject.Instantiate(Prefab);
            clone.transform.parent = transform;
            _doorCache[doorEntity] = clone.transform;
        }
        var door = _doorCache[doorEntity];
        door.localScale = doorData.Size;
        var position = doorData.Position;
        position.y -= (1 - doorData.OpeningProgress) * doorData.Size.y;
        door.position = position;
        door.localEulerAngles = doorData.EulerRotation;


    }
}
