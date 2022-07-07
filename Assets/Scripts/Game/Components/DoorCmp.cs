using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Components
{
    public struct DoorCmp: IEcsAutoReset<DoorCmp>
    {
        public Vector3 Position;
        public Vector3 Size;
        public Vector3 EulerRotation;
        public float OpeningProgress;
        public float OpeningSpeed;

        public void AutoReset(ref DoorCmp c)
        {
            c.OpeningProgress = 1;
            c.OpeningSpeed = 0.1f;
        }
    }
}