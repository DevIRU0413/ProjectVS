using PVS;

using UnityEngine;

namespace ProjectVS.Monster.State
{
    public class MonsterMoveState : MonsterState
    {
        public MonsterMoveState(MonsterController controller) : base(controller) { }

        private float _moveSpeed;

        public override void Enter()
        {
            _moveSpeed = controller.Status.CurrentSpd;
        }

        public override void Update()
        {
            if (controller.MoveDirection != Vector3.zero)
                controller.transform.Translate(_moveSpeed * Time.deltaTime * controller.MoveDirection);
            else
                controller.ChangeState(MonsterStateType.Idle);
        }
    }
}
