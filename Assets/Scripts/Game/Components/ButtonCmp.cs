using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Components
{
    public struct ButtonCmp : IEcsAutoReset<ButtonCmp>
    {
        public Vector2 Position;
        public float Radius;
        public bool IsPressed;
        public int DoorEntity;

        public void AutoReset(ref ButtonCmp b)
        {
            b.DoorEntity = -1;
            b.IsPressed = false;
        }

        public bool Contains(Vector2 point)
        {
            return (point - Position).magnitude < Radius;
        }
    }
}