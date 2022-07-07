using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GameConfig
    {
        public Vector2 PlayerPosition;
        public DoorConfig[] DoorConfigs;
        public ButtonConfig[] ButtonConfigs;
    }
    [Serializable]
    public class DoorConfig
    {
        public Vector3 Position;
        public Vector3 Size;
        public Vector3 EulerRotation;
    }
    [Serializable]
    public class ButtonConfig
    {
        public Vector2 Position;
        public float Radius;
        public int DoorIndex;
    }
}
