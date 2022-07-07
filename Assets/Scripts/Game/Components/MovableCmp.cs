using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Components {
    public struct MovableCmp: IEcsAutoReset<MovableCmp>
    {
        public Vector3 EndPoint;
        public float Speed;
        public float SqrCmpDelta;

        public void AutoReset(ref MovableCmp c)
        {
            c.Speed = 3;
            c.SqrCmpDelta = 0.01f;
        }
    }
}