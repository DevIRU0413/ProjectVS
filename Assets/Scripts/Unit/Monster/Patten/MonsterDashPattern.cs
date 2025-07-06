using System.Collections;

using ProjectVS.Interface;
using ProjectVS.Unit.Monster.Phase;
using ProjectVS.Unit.Player;

using UnityEngine;

namespace ProjectVS.Unit.Monster.Pattern
{
    public class MonsterDashPattern : MonsterPattern, IGroggyTrackable
    {
        [Header("Dash Info")]
        [SerializeField, Min(0.0f)] private float _dashSpeed = 20f;           // 대쉬 속도
        [SerializeField, Min(0.0f)] private float _dashDistance = 5f;         // 실제 대쉬 거리

        [SerializeField] private AudioClip _dashStartClip;
        [SerializeField] private AudioClip _dashImpactClip;

        [SerializeField] private GameObject _dashArea;

        protected GameObject _target;

        [Header("IGroggyTrackable")]
        [SerializeField, Min(0)] private int _groggyCountLine = 0;

        public int GroggyThreshold => _groggyCountLine;

        public bool IsFaild => false;

        public override void Init(MonsterPhaseController phaseController)
        {
            base.Init(phaseController);
            _target = PlayerSpawner.ForceInstance.CurrentPlayer;
            if (_dashArea != null) _dashArea.SetActive(false);
        }

        public override bool Condition()
        {
            if (_target == null) return false;
            return base.Condition();
        }

        public override void Enter()
        {
            phaseController.OwnerController.ChangeState(MonsterStateType.Idle, true);
            phaseController.OwnerController.DelegateMovementAuthority();
            base.Enter();

            if (_dashArea != null) _dashArea.SetActive(false);
        }

        protected override IEnumerator IE_PlayAction()
        {
            float currentTime = castDelay;
            if (_dashArea != null) _dashArea.SetActive(true);
            phaseController.OwnerController.Anim.PlayClip(patternCastClips, 1.0f, true);
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                Vector3 dir = _target.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                _dashArea.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
                yield return null;
            }
            if (_dashArea != null) _dashArea.SetActive(false);
            phaseController.OwnerController.Anim.Stop();
            phaseController.OwnerController.Anim.PlayClip(patternActionClips, 1.0f, true);

            // 애니메이션 or 이펙트 재생
            if (_dashStartClip != null)
                AudioSource.PlayClipAtPoint(_dashStartClip, transform.position);

            // 타겟 있다면
            if (_target != null)
            {
                Vector3 dashDir = (_target.transform.position - transform.position).normalized;
                Vector3 targetPos = transform.position + dashDir * _dashDistance;

                float dashTime = _dashDistance / _dashSpeed;
                float elapsed = 0f;
                Vector3 startPos = transform.position;

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
                phaseController.OwnerController.Anim.Stop();
                phaseController.OwnerController.Anim.PlayClip(patternRecoveryClips, 1.0f, true);
                yield return new WaitForSeconds(recoveryTime);
                phaseController.OwnerController.Anim.Stop();
            }

            PatternState = MonsterPatternState.Done;
        }

        public override void Exit()
        {
            if (_dashArea != null) _dashArea.SetActive(false);
            base.Exit();
            phaseController.OwnerController.RevokeMovementAuthority();
        }
    }
}
