using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Components
{
    public struct ButtonCmp : IEcsAutoReset<ButtonCmp>
    {
        public float Radius;
        public bool IsPressed;
        public int DoorEntity;

        public void AutoReset(ref ButtonCmp b)
        {
            b.DoorEntity = -1;
            b.IsPressed = false;
        }

        public bool Contains(Vector3 point,Vector3 point2)
        {
            Vector2 a = new Vector2(point.x, point.z);
            Vector2 b = new Vector2(point2.x, point2.z);
            return (a - b).magnitude < Radius;
        }
    }
}