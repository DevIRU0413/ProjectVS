using ProjectVS.Player;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Manager
{
    public class GameManager : SimpleSingleton<GameManager>
    {
        private bool _isInit = false;

        private GameObject _playerGO;

        public PlayerController Player { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            _playerGO = GameObject.FindGameObjectWithTag("Player");
            if (_playerGO == null) return;

            Player = _playerGO.GetComponentInParent<PlayerController>();
            if (Player == null) return;

            _isInit = true;
        }

        private void Update()
        {
            if (!_isInit) return;

            if (Player.CurrentStateType != PVS.PlayerStateType.Death)
            {

            }

        }
    }
}
