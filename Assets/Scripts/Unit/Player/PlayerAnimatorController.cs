using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectVS.Unit.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        private Animator _animator;
        private PlayerController _controller;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _controller = GetComponent<PlayerController>();
        }

        private void Update()
        {
            // 이동 속도에 따라 Speed 파라미터 조정
            float speed = _controller.MoveDirection.magnitude;
            _animator.SetFloat(SpeedHash, speed);
        }
    }
}
