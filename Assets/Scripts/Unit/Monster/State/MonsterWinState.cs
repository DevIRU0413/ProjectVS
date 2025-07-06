using ProjectVS;

using UnityEngine;

namespace ProjectVS.Monster.State
{
    public class MonsterWinState : MonsterState
    {
        private const int STATE_VALUE = (int)MonsterStateType.Win;
        private int _aniHashState = 0;

        public MonsterWinState(MonsterController controller, Animator animator) : base(controller, animator) { }

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

        public override void Update() { }
    }
}
