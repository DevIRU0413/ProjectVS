using System.Collections;

using ProjectVS.Phase;
using ProjectVS.Unit.Player;

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ProjectVS.Monster.Pattern
{
    public class MonsterDashPattern : MonsterPattern
    {
        [Header("Dash Info")]
        [SerializeField, Min(0.0f)] private float _dashSpeed = 20f;           // 대쉬 속도
        [SerializeField, Min(0.0f)] private float _dashDistance = 5f;         // 실제 대쉬 거리

        [SerializeField] private AudioClip _dashStartClip;
        [SerializeField] private AudioClip _dashImpactClip;

        protected GameObject _target;

        public override void Init(MonsterPhaseController phaseController)
        {
            base.Init(phaseController);
            _target = PlayerSpawner.ForceInstance.CurrentPlayer;

        }

        public override bool Condition()
        {
            if (_target == null) return false;
            return base.Condition();
        }

        public override void Enter()
        {
            phaseController.OnwerController.ChangeState(MonsterStateType.Idle, true);
            phaseController.OnwerController.LockChangeState();
            base.Enter();
        }

        protected override IEnumerator IE_PlayAction()
        {
            if (castDelay > 0f)
            {
                phaseController.OnwerController.Anim.PlayClip(patternCastClips, true);
                yield return new WaitForSeconds(castDelay);
                phaseController.OnwerController.Anim.Stop();
                phaseController.OnwerController.Anim.PlayClip(patternActionClips, true);
            }


            if (_target != null)
            {
                Vector3 dashDir = (_target.transform.position - transform.position).normalized;
                Vector3 targetPos = transform.position + dashDir * _dashDistance;

                float dashTime = _dashDistance / _dashSpeed;
                float elapsed = 0f;
                Vector3 startPos = transform.position;

                // 애니메이션 or 이펙트 재생
                if (_dashStartClip != null)
                    AudioSource.PlayClipAtPoint(_dashStartClip, transform.position);

                while (elapsed < dashTime)
                {
                    transform.position = Vector3.Lerp(startPos, targetPos, elapsed / dashTime);
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                transform.position = targetPos;

                // 대쉬 후 충돌 or 충격 이펙트
                if (_dashImpactClip != null)
                    AudioSource.PlayClipAtPoint(_dashImpactClip, transform.position);
            }

            if (recoveryTime > 0f)
            {
                phaseController.OnwerController.Anim.Stop();
                phaseController.OnwerController.Anim.PlayClip(patternRecoveryClips, true);
                yield return new WaitForSeconds(recoveryTime);
                phaseController.OnwerController.Anim.Stop();
            }

            PatternState = MonsterPatternState.Done;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;
            // Gizmos.DrawWireSphere(transform.position, _dashRange);
        }
    }
}
