using PVS;

using UnityEngine;
using UnityEngine.EventSystems;

using static UnityEngine.RuleTile.TilingRuleOutput;

namespace ProjectVS.Monster.State
{
    public class MonsterMoveState : MonsterState
    {
        private const int STATE_VALUE = (int)MonsterStateType.Move;
        private int _aniHashState = 0;

        private float _moveSpeed;
        public MonsterMoveState(MonsterController controller, Animator animator) : base(controller, animator) { }

        protected override void Init()
        {
            base.Init();
            _aniHashState = Animator.StringToHash("State");
        }

        public override void Enter()
        {
            base.Enter();
            animator.SetInteger(_aniHashState, STATE_VALUE);

            _moveSpeed = controller.Status.CurrentSpd;
        }
        public override void Update()
        {
            if (controller.IsMove)
                controller.transform.Translate(_moveSpeed * Time.deltaTime * controller.MoveDirection.normalized);
            else
                controller.ChangeState(MonsterStateType.Idle);
        }
    }
}
