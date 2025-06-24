using System;

using UnityEngine;

namespace ProjectVS
{
    public class Collider2DAction : MonoBehaviour
    {
        public Action<Collider2D> OnTriggerEnterAction;
        public Action<Collider2D> OnTriggerStayAction;
        public Action<Collider2D> OnTriggerExitAction;

        public Action<Collision2D> OnCollisionEnterAction;
        public Action<Collision2D> OnCollisionStayAction;
        public Action<Collision2D> OnCollisionExitAction;

        // Trigger
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerEnterAction?.Invoke(collision);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            OnTriggerStayAction?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            OnTriggerExitAction?.Invoke(collision);
        }

        // Collision
        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEnterAction?.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            OnCollisionStayAction?.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            OnCollisionExitAction?.Invoke(collision);
        }
    }
}
