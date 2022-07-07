using Game.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsViewer : MonoBehaviour
{
    public GameObject Prefab;
    public Color Pressing;
    public Color Hold;
    private Dictionary<int, MeshRenderer> _doorCache;

    private void Awake()
    {
        _doorCache = new Dictionary<int, MeshRenderer>();
    }

    internal void UpdateView(int buttonEntity, ButtonCmp buttonData)
    {
        if (!_doorCache.ContainsKey(buttonEntity))
        {
            var clone = GameObject.Instantiate(Prefab);
            clone.transform.parent = transform;
            clone.transform.position = new Vector3(buttonData.Position.x, 0, buttonData.Position.y);
            clone.transform.localScale = new Vector3(buttonData.Radius, clone.transform.localScale.y, buttonData.Radius);
            _doorCache[buttonEntity] = clone.GetComponent<MeshRenderer>();
        }
        _doorCache[buttonEntity].material.color = buttonData.IsPressed ? Pressing : Hold;
    }
}
