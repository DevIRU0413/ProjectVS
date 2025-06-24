using PVS;

using UnityEngine;

namespace ProjectVS.Monster.State
{
    public class MonsterIdleState : MonsterState
    {
        public MonsterIdleState(MonsterController controller) : base(controller) { }

        public override void Enter()
        {
            Debug.Log("Entered Idle State");
            // 애니메이션 시작 처리
        }

        public override void Exit()
        {
            Debug.Log("Exited Idle State");
        }

        public override void Update()
        {
            if (controller.Target != null)
            {
                controller.ChangeState(MonsterStateType.Move);
            }
        }
    }
}
