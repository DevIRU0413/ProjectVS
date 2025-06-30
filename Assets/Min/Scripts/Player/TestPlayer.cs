using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using ProjectVS.Utils.UIManager;
using ShopNPCInteractionTriggerClass = ProjectVS.Shop.ShopNPCInteractionTrigger.ShopNPCInteractionTrigger;


namespace ProjectVS.Player.TestMove
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] ShopNPCInteractionTriggerClass _shopNPCInteractionTrigger;

        [SerializeField] private float _moveSpeed = 5f;
        private Vector2 _moveDirection;
        public Vector2 MoveDirection => _moveDirection;


        private void Update()
        {
            GetInput();
            InteractWithNPC();
        }
        private void FixedUpdate()
        {
            Move();
        }


        private void GetInput()
        {
            _moveDirection = Vector2.zero;

            //if (Input.GetKey(KeyCode.W))
            //{
            //    _moveDirection += Vector2.up;
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    _moveDirection += Vector2.down;
            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //    _moveDirection += Vector2.left;
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    _moveDirection += Vector2.right;
            //}

            if (Keyboard.current.wKey.isPressed)
            {
                _moveDirection += Vector2.up;
            }
            if (Keyboard.current.sKey.isPressed)
            {
                _moveDirection += Vector2.down;
            }
            if (Keyboard.current.aKey.isPressed)
            {
                _moveDirection += Vector2.left;
            }
            if (Keyboard.current.dKey.isPressed)
            {
                _moveDirection += Vector2.right;
            }



            _moveDirection = _moveDirection.normalized;
        }

        private void Move()
        {
            transform.Translate(_moveDirection * _moveSpeed * Time.deltaTime);
        }

        private void InteractWithNPC()
        {
            //if (Input.GetKeyDown(KeyCode.E) && _shopNPCInteractionTrigger.CanInteract)
            //{
            //    UIManager.Instance.Show("Event Select Panel");
            //}

            if (Keyboard.current.eKey.wasPressedThisFrame && _shopNPCInteractionTrigger.CanInteract)
            {
                UIManager.Instance.Show("Event Select Panel");
            }
        }
    }
}
