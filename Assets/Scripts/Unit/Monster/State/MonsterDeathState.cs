using ProjectVS.Util;

using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace ProjectVS.Monster.State
{
    public class MonsterDeathState : MonsterState
    {
        private const int STATE_VALUE = (int)MonsterStateType.Death;
        private int _aniHashState = 0;
        private float _deathDelay = 0.0f;
        private bool _isDead = false;

        public MonsterDeathState(MonsterController controller, Animator animator) : base(controller, animator) { }

        protected override void Init()
        {
            base.Init();
            _aniHashState = Animator.StringToHash("State");
        }

        public override void Enter()
        {
            animator.SetInteger(_aniHashState, STATE_VALUE);

            _deathDelay = 0.0f;
            AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
            foreach (AnimatorClipInfo clipInfo in clipInfos)
            {
                if (clipInfo.Equals(default))
                {
                    var clip = clipInfo.clip;
                    if (clip != null)
                    {
                        var clipLength = clip.length;
                        if (clipLength > _deathDelay)
                            _deathDelay = clipLength;
                    }
                }
            }
            controller.OnDeath?.Invoke();
            _isDead = false;
        }

        public override void Update()
        {
            _deathDelay -= Time.deltaTime;
            if (_deathDelay <= 0.0f && _isDead == false)
            {
                PoolManager.Instance.Despawn(controller.gameObject);
                _isDead = true;
            }
        }

        public override void Exit() { }

    }
}
