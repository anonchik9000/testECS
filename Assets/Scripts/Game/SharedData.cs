using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SharedData
    {
        //In
        public float DeltaTime;
        //Out
        public bool CharacterUpdate;
        public List<int> DoorsUpdate;
        public List<int> ButtonsUpdate;
    }
}
