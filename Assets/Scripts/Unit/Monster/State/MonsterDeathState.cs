using ProjectVS;

using UnityEngine;

namespace ProjectVS.Monster.State
{
    public class MonsterDeathState : MonsterState
    {
        private const int STATE_VALUE = (int)MonsterStateType.Death;
        private int _aniHashState = 0;

        public MonsterDeathState(MonsterController controller, Animator animator) : base(controller, animator) { }

        protected override void Init()
        {
            base.Init();
            _aniHashState = Animator.StringToHash("State");

        }

        public override void Enter()
        {
            controller.LockChangeState();
            animator.SetInteger(_aniHashState, STATE_VALUE);
        }

        public override void Update()
        {
        }

        public override void Exit() { }

    }
}
