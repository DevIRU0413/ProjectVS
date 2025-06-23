using UnityEngine;

namespace PVS.Monster.State
{
    public class MonsterMoveState : MonsterState
    {
        public MonsterMoveState(MonsterController controller) : base(controller) { }

        private float m_moveSpeed;

        public override void Enter()
        {
            m_moveSpeed = _controller.Status.CurrentSpd;
        }

        public override void Update()
        {
            if (_controller.MoveDirection != Vector3.zero)
                _controller.transform.Translate(m_moveSpeed * Time.deltaTime * _controller.MoveDirection);
            else
                _controller.ChangeState(MonsterStateType.Idle);
        }
    }
}
