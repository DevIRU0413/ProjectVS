using System.Collections;
using System.Collections.Generic;

using ProjectVS.Interface;
using ProjectVS.Unit.Monster.Pattern;
using ProjectVS.Unit.Monster.Phase;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Unit.Monster
{
    public class MonsterSlashPattern : MonsterPattern, IGroggyTrackable
    {
        [Header("Slash Settings")]
        [SerializeField, Min(1)] private int _slashCount = 3;
        [SerializeField] private List<AnimationClip> _slashClips;
        [SerializeField, Min(0f)] private float _normalSlashInterval = 0.5f;
        [SerializeField, Min(0f)] private float _finishSlashActionDelay = 2.0f;

        [Header("Hit Detection")]
        [SerializeField] private HitScanner _normalHitScanner;
        [SerializeField] private HitScanner _finishHitScanner;
        [SerializeField] private float      _attackRange = 4.0f;
        [SerializeField] private LayerMask  _targetLayer;

        private int _hitSuccessCount = 0;
        private Collider2D[] targets = new Collider2D[1];

        [Header("IGroggyTrackable")]
        [SerializeField, Min(0)] private int _groggyCountLine = 0;
        public int GroggyThreshold => _groggyCountLine;
        public bool IsFaild => _hitSuccessCount <= 0;

        // HB 스프라이터
        private HitBoxSpriter _normalHitSpriter;
        private HitBoxSpriter _finishHitSpriter;

        public override void Init(MonsterPhaseController phaseController)
        {
            base.Init(phaseController);
            if (_normalHitScanner != null)
            {
                _normalHitSpriter = _normalHitScanner.GetComponentInChildren<HitBoxSpriter>();
                _normalHitScanner.gameObject.SetActive(false);
            }
            if (_finishHitScanner != null)
            {
                _finishHitSpriter = _normalHitScanner.GetComponentInChildren<HitBoxSpriter>();
                _finishHitScanner.gameObject.SetActive(false);
            }
        }

        public override bool Condition()
        {
            return _normalHitScanner != null &&
                   phaseController.OwnerController.Target != null &&
                   base.Condition();
        }

        public override void Enter()
        {
            phaseController.OwnerController.ChangeState(MonsterStateType.Idle, true);
            phaseController.OwnerController.DelegateMovementAuthority();
            base.Enter();

            _hitSuccessCount = 0;
            if (_normalHitScanner != null) _normalHitScanner.gameObject.SetActive(false);
            if (_finishHitScanner != null) _finishHitScanner.gameObject.SetActive(false);
        }

        protected override IEnumerator IE_PlayAction()
        {
            // 세팅이 이상할 시, 초기 종료
            if (_normalHitScanner == null || _finishHitScanner == null)
            {
                PatternState = MonsterPatternState.Done;
                yield break;
            }

            // 필요 값들
            var thisTr = transform;
            var targetTr = phaseController.OwnerController.Target.transform;
            Vector3 hitBoxPoint = CalculateHitBoxPosition(thisTr, targetTr);

            // 캐스팅 시간에서 애니메이션 시간 빼주기
            float elapsed = 0f;
            var clip = (0 < _slashClips.Count) ? _slashClips[0] : patternActionClips;
            elapsed = (clip == null) ? 0.0f : clip.length;

            // 범위 활성화
            _normalHitSpriter.changeTime = castDelay - elapsed;
            _normalHitScanner.gameObject.SetActive(true);

            // 플레이어 위치에 공격 예상 범위 포인트 갱신
            while (elapsed < castDelay)
            {
                elapsed += Time.deltaTime;
                hitBoxPoint = CalculateHitBoxPosition(thisTr, targetTr);
                _normalHitScanner.transform.position = hitBoxPoint;
                yield return null;
            }

            // 서클 위치 마지막 갱신
            hitBoxPoint = CalculateHitBoxPosition(thisTr, targetTr);
            _normalHitScanner.transform.position = hitBoxPoint;
            _finishHitScanner.transform.position = hitBoxPoint;

            _normalHitScanner.gameObject.SetActive(false);

            for (int i = 0; i < _slashCount - 1; i++)
            {
                var slashClip = (i < _slashClips.Count) ? _slashClips[i] : patternActionClips;
                phaseController.OwnerController.Anim.Stop();
                phaseController.OwnerController.Anim.PlayClip(slashClip);

                _normalHitSpriter.changeTime = slashClip.length - 0.1f;
                _normalHitScanner.gameObject.SetActive(true);
                yield return new WaitForSeconds(slashClip.length - 0.1f);
                _normalHitScanner.gameObject.SetActive(false);

                if (_normalHitScanner.Scan(targets) > 0)
                {
                    _hitSuccessCount++;
                    Debug.Log($"{typeof(MonsterSlashPattern).Name} Hit > {_hitSuccessCount}");
                }
                yield return new WaitForSeconds(_normalSlashInterval);
            }
            _normalHitScanner.gameObject.SetActive(false);

            if (_slashClips.Count > 0)
            {
                var slashClip = (0 < _slashClips.Count) ? _slashClips[_slashClips.Count - 1] : patternActionClips;
                float finishCastTime = _finishSlashActionDelay - slashClip.length;

                _finishHitSpriter.changeTime = finishCastTime;
                _finishHitScanner.gameObject.SetActive(true);
                yield return new WaitForSeconds(finishCastTime);

                phaseController.OwnerController.Anim.Stop();
                phaseController.OwnerController.Anim.PlayClip(slashClip);
                yield return new WaitForSeconds(slashClip.length - 0.1f);
                _finishHitScanner.gameObject.SetActive(false);

                if (_finishHitScanner.Scan(targets) > 0)
                {
                    _hitSuccessCount++;
                    var damageable = targets[0].GetComponentInParent<Damageable>();
                    var dir = damageable.gameObject.transform.position - hitBoxPoint;

                    if (damageable != null)
                    {
                        var dmg = phaseController.OwnerController.Stats.AtkSpd;
                        damageable.ApplyDamage(new DamageInfo(amount: dmg, direction: dir));
                    }
                    Debug.Log($"{typeof(MonsterSlashPattern).Name} Hit > {_hitSuccessCount}");
                }
            }

            yield return new WaitForSeconds(recoveryTime);
            PatternState = MonsterPatternState.Done;
        }

        private Vector3 CalculateHitBoxPosition(Transform thisTr, Transform targetTr)
        {
            Vector3 dir = targetTr.position - thisTr.position;
            float distance = dir.magnitude;

            if (_attackRange < distance)
                return thisTr.position + dir.normalized * _attackRange;
            return targetTr.position;
        }

        private bool CheckHit(Vector2 center, float radius, LayerMask mask, Collider2D[] buffer)
        {
            int count = OverlapScanUtility.CircleScan(center, radius, mask, buffer);
            for (int i = 0; i < count; i++)
            {
                var hit = buffer[i];
                if (hit == null) continue;

                IDamageable target = hit.GetComponentInParent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(new DamageInfo(10, (hit.transform.position - transform.position).normalized));
                    Debug.Log($"Slash Hit > {_hitSuccessCount}");
                    return true;
                }
            }
            return false;
        }

        public override void Exit()
        {
            base.Exit();
            phaseController.OwnerController.RevokeMovementAuthority();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
