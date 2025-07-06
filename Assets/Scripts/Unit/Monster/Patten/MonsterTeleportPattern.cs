using System.Collections;
using System.Collections.Generic;

using ProjectVS.Interface;
using ProjectVS.Unit.Monster.Phase;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Unit.Monster.Pattern
{
    public class MonsterTeleportPattern : MonsterPattern
    {
        // 점프 > 사라짐 > 여러개의 스프라이트 넣어줄 수 있음
        [Header("Teleport Info")]
        [SerializeField, Min(0.0f)] private float _teleportTriggerRange = 1.0f;     // 텔레포트 트리거 조건이 되는 거리
        [SerializeField, Min(0.0f)] private float _teleportHideTime = 0.0f;         // 텔레포트로 사라져 있는 시간 (숨어 있는 시간)
        [SerializeField, Min(0.0f)] private float _teleportShowDelayTime = 0.0f;    // 텔레포트로 사라져 있을 때, 나타나기까지의 딜레이 시간

        [Header("About Target Config")]
        [SerializeField] private bool _isTargetPlayer = true;                       // 타겟이 플레이어로 지정할 것인가(플레이어면 시작할 때마다, 플레이어로 갱신)
        [SerializeField] private GameObject _target;                                // 어떠한 대상의 위치로 텔레포트를 진행 할 것이냐.
        [SerializeField, Min(0.0f)] private float _teleportNearTargetRange = 0.0f;  // 현재 오브젝트가 등장가능 범위(0 이면 플레이어 위치로 고정)

        [Header("Body Collider")]
        [SerializeField] private Collider2D _bodyCollider2d;

        [Header("Hit Scanner")]
        [SerializeField] private HitScanner _hitScanner;                            // 텔레포트 위치에 공격 할거 때 사용할 스캐너
        private HitBoxSpriter _hitBoxSpriter;                                       // 텔레포트 위치에 공격 할 범위 (가시적 범위)
        private Collider2D[] _buffer = new Collider2D[1];

        [Header("About Hide & Show")]
        [SerializeField] private AnimationClip _hideClip;                           // 숨어질 때, 애니메이션 클립
        [SerializeField] private List<SpriteRenderer> _hideSpriteRenderers = new(); // 숨어질 때, 같이 숨겨질 SpriteRenderer들

        Transform _thisTr;
        Transform _targetTr;

        public override void Init(MonsterPhaseController phaseController)
        {
            base.Init(phaseController);

            if (_hitScanner != null)
            {
                _hitBoxSpriter = _hitScanner.GetComponentInChildren<HitBoxSpriter>();
                _hitScanner.gameObject.SetActive(false);
            }

            _thisTr = transform;
            _targetTr = phaseController.OwnerController.Target.transform;
        }


        public override bool Condition()
        {
            float distance = (_targetTr.position - _thisTr.position).magnitude;
            return (_teleportTriggerRange >= distance) && base.Condition();
        }

        public override void Enter()
        {
            phaseController.OwnerController.ChangeState(MonsterStateType.Idle, true);
            phaseController.OwnerController.DelegateMovementAuthority();
            base.Enter();

            if (_isTargetPlayer)
                _target = phaseController.OwnerController.Target;

            if (_hitScanner != null)
                _hitScanner.gameObject.SetActive(false);
        }

        protected override IEnumerator IE_PlayAction()
        {
            // * 캐스팅 관련

            float castTime = 0.0f;
            // 캐스팅 딜레이 없는데, 애니메이션은 없는 경우 > 캐스팅 없음
            // 캐스팅 딜레이 없는데, 애니메이션은 있는 경우 > 애니메이션 진행, 진행 시간 만큼 기다리기
            if (castDelay <= 0 && patternCastClips != null)
            {
                castTime = patternCastClips.length;
                phaseController.OwnerController.Anim.PlayClip(patternCastClips);
            }
            // 캐스팅 딜레이 있는데, 애니메이션은 없는 경우 > 캐스팅 딜레이 진행
            else if (castDelay > 0 && patternCastClips == null)
            {
                castTime = castDelay;
            }
            // 캐스팅 딜레이 있는데, 애니메이션은 있는 경우
            else if (castDelay > 0 && patternCastClips != null)
            {
                //      > 애니메이션 진행, 캐스팅 시간은 (캐스팅 시간 < 애니메이션 시간이 긴 경우) 애니메이션 시간 사용
                //      > 애니메이션 진행, 캐스팅 시간은 (캐스팅 시간 > 애니메이션 시간이 긴 경우) 애니메이션 시간을 뺀, 캐스팅 시간 사용
                if (castDelay < patternCastClips.length)
                    castTime = patternCastClips.length;
                else
                {
                    castTime = castDelay - patternCastClips.length;
                    yield return new WaitForSeconds(patternCastClips.length);
                }
                phaseController.OwnerController.Anim.PlayClip(patternCastClips);
            }
            yield return new WaitForSeconds(castTime);





            // * Action 관련

            // * > Hide Action
            float hideAfterDelay = _teleportHideTime;
            // 애니메이션 클립이 있다면 애니메이션 진행
            if (_hideClip != null)
            {
                phaseController.OwnerController.Anim.PlayClip(_hideClip);
                float hideClipLength = _hideClip.length;
                hideAfterDelay -= hideClipLength;
                yield return new WaitForSeconds(hideClipLength);
            }

            // 애니메이션 진행후, 스프라이트 숨기기 
            Hide();
            if (_hitBoxSpriter != null)
                _hitScanner.gameObject.SetActive(true);
            while (hideAfterDelay > 0)
            {
                Vector3 teleportPos = _target.transform.position;
                if (_teleportNearTargetRange == 0)
                {
                    // 위치 할당
                    phaseController.transform.position = teleportPos;
                    _hitScanner.gameObject.transform.position = phaseController.transform.position;
                }
                else
                {
                    // 랜덤한 각도와 거리
                    float angle = Random.Range(0.0f, 360.0f);
                    float mag = Random.Range(0.0f, _teleportNearTargetRange);

                    // 타겟 기준으로 지정 범위 근처로 지정
                    Vector3 randomPos = teleportPos + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f).normalized * mag;

                    // 위치 할당
                    _hitScanner.gameObject.transform.position = randomPos;
                    phaseController.transform.position = randomPos;
                }

                hideAfterDelay -= Time.deltaTime;
                yield return null;
                // yield return new WaitForSeconds(hideAfterDelay); // 숨어 있는 시간
            }

            // * > Show Action
            float showDelay = _teleportShowDelayTime;
            float showClipLength = 0.0f;
            if (patternActionClips != null)
            {
                // 애니메이션 진행 시간
                showClipLength = patternActionClips.length;
                showDelay -= showClipLength;
            }

            // 스캐너가 있다면 범위 활성화
            if (_hitBoxSpriter != null)
            {
                _hitBoxSpriter.gameObject.SetActive(false);
                yield return null;
                _hitBoxSpriter.changeTime = showDelay + showClipLength - Time.deltaTime;
                _hitBoxSpriter.gameObject.SetActive(true);
            }

            if (showDelay > 0)
                yield return new WaitForSeconds(showDelay);

            // Show 보여주기
            Show();
            // 애니메이션 존재 시, 진행
            if (patternActionClips != null)
                phaseController.OwnerController.Anim.PlayClip(patternActionClips);

            // Show 클립 딜레이 시간
            if (showClipLength > 0)
                yield return new WaitForSeconds(showClipLength);

            // 스캐너, 스캔한 옵젝이 데미지를 줄 수 있다면, 데미지를 줌
            ScannerHit();





            // * 후딜레이 관련

            float recoveryTime = 0.0f;
            // 캐스팅 딜레이 없는데, 애니메이션은 없는 경우 > 캐스팅 없음
            // 캐스팅 딜레이 없는데, 애니메이션은 있는 경우 > 애니메이션 진행, 진행 시간 만큼 기다리기
            if (castDelay <= 0 && patternRecoveryClips != null)
            {
                recoveryTime = patternRecoveryClips.length;
                phaseController.OwnerController.Anim.PlayClip(patternRecoveryClips);
            }
            // 캐스팅 딜레이 있는데, 애니메이션은 없는 경우 > 캐스팅 딜레이 진행
            else if (castDelay > 0 && patternRecoveryClips == null)
            {
                recoveryTime = castDelay;
            }
            // 캐스팅 딜레이 있는데, 애니메이션은 있는 경우
            else if (castDelay > 0 && patternRecoveryClips != null)
            {
                //      > 애니메이션 진행, 캐스팅 시간은 (캐스팅 시간 < 애니메이션 시간이 긴 경우) 애니메이션 시간 사용
                //      > 애니메이션 진행, 캐스팅 시간은 (캐스팅 시간 > 애니메이션 시간이 긴 경우) 애니메이션 시간을 뺀, 캐스팅 시간 사용
                if (castDelay < patternRecoveryClips.length)
                    recoveryTime = patternRecoveryClips.length;
                else
                {
                    recoveryTime = castDelay - patternRecoveryClips.length;
                    yield return new WaitForSeconds(patternRecoveryClips.length);
                }
                phaseController.OwnerController.Anim.PlayClip(patternRecoveryClips);
            }
            yield return new WaitForSeconds(recoveryTime);


            // * End
            PatternState = MonsterPatternState.Done;
            yield break;
        }

        private void Hide()
        {
            _bodyCollider2d?.gameObject.SetActive(false);
            if (_hideSpriteRenderers.Count > 0)
                foreach (var sRenderer in _hideSpriteRenderers)
                    sRenderer.gameObject.SetActive(false);
        }

        private void Show()
        {
            _bodyCollider2d?.gameObject.SetActive(true);
            if (_hideSpriteRenderers.Count > 0)
                foreach (var sRenderer in _hideSpriteRenderers)
                    sRenderer.gameObject.SetActive(true);
        }

        private void ScannerHit()
        {
            if (_hitScanner != null)
            {
                if (_hitScanner.GetScanCount(_buffer) > 0)
                {
                    GameObject go = _buffer[0].gameObject;
                    IDamageable damageableGo = go.GetComponentInParent<IDamageable>();
                    // 데미지를 줄 수 있다면
                    if (damageableGo != null)
                    {
                        // 데미지 관련 정보 정리
                        Vector2 dir = _target.transform.position - this.transform.position;
                        DamageInfo info = new DamageInfo(1.0f, dir.normalized);

                        damageableGo.TakeDamage(info);
                        Debug.Log($"Hit > {damageableGo}");
                    }
                }
                _hitScanner.gameObject.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _teleportTriggerRange);
        }
    }
}
