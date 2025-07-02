using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.JDW
{
    public class GameManager : SimpleSingleton<GameManager>
    {
        [HideInInspector] public PlayerConfig Player;
        public int CurrentClassIndex { get; private set; }
        private GameObject _currentPlayerInstance;
    }
}

