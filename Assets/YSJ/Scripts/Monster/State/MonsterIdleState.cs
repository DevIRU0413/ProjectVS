using PVS;

using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace ProjectVS.Monster.State
{
    public class MonsterIdleState : MonsterState
    {
        private const int STATE_VALUE = (int)MonsterStateType.Idle;
        private int _aniHashState = 0;

        public MonsterIdleState(MonsterController controller, Animator animator) : base(controller, animator) { }
        protected override void Init()
        {
            base.Init();
            _aniHashState = Animator.StringToHash("State");
        }

        public override void Enter()
        {
            animator.SetInteger(_aniHashState, STATE_VALUE);
        }

        public override void Exit() { }

        public override void Update()
        {
            // 거리 체크
            if (controller.IsMove)
            {
                controller.ChangeState(MonsterStateType.Move);
            }
            else if (controller.IsWin)
            {
                controller.ChangeState(MonsterStateType.Win);
            }
        }
    }
}
