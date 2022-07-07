using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class InOutData
    {
        //In
        public Vector2 CurrentPosition;
        public float DeltaTime;
        public bool PositionChanged;
        //Out
        public bool MovableUpdate;
        public List<int> DoorsUpdate;
        public List<int> ButtonsUpdate;
    }
}
