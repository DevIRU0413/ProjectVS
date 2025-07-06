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
        [SerializeField] private HitBoxSpriter normalSlashHitBox;
        [SerializeField] private HitBoxSpriter finishSlashHitBox;
        [SerializeField] private float normalScanRadius = 1.5f;
        [SerializeField] private float finishScanRadius = 1.5f;
        [SerializeField] private float attackRange = 4.0f;
        [SerializeField] private LayerMask targetLayer;

        private int _successCount = 0;
        private Collider2D[] targets = new Collider2D[5];

        [Header("IGroggyTrackable")]
        [SerializeField, Min(0)] private int _groggyCountLine = 0;
        public int GroggyThreshold => _groggyCountLine;
        public bool IsFaild => _successCount <= 0;

        public override void Init(MonsterPhaseController phaseController)
        {
            base.Init(phaseController);
            if (normalSlashHitBox != null) normalSlashHitBox.gameObject.SetActive(false);
            if (finishSlashHitBox != null) finishSlashHitBox.gameObject.SetActive(false);
        }

        public override bool Condition()
        {
            return normalSlashHitBox != null &&
                   phaseController.OwnerController.Target != null &&
                   base.Condition();
        }

        public override void Enter()
        {
            phaseController.OwnerController.ChangeState(MonsterStateType.Idle, true);
            phaseController.OwnerController.LockChangeState();
            phaseController.OwnerController.DelegateMovementAuthority();

            base.Enter();

            _successCount = 0;
            if (normalSlashHitBox != null) normalSlashHitBox.gameObject.SetActive(false);
            if (finishSlashHitBox != null) finishSlashHitBox.gameObject.SetActive(false);
        }

        protected override IEnumerator IE_PlayAction()
        {
            _successCount = 0;

            var thisTr = transform;
            var targetTr = phaseController.OwnerController.Target.transform;

            normalSlashHitBox.gameObject.SetActive(true);
            Vector3 hitBoxPoint = CalculateHitBoxPosition(thisTr, targetTr);
            normalSlashHitBox.transform.position = hitBoxPoint;

            float elapsed = 0f;
            while (elapsed < castDelay)
            {
                elapsed += Time.deltaTime;
                hitBoxPoint = CalculateHitBoxPosition(thisTr, targetTr);
                normalSlashHitBox.transform.position = hitBoxPoint;
                yield return null;
            }

            finishSlashHitBox.transform.position = hitBoxPoint;
            normalSlashHitBox.gameObject.SetActive(false);

            for (int i = 0; i < _slashCount - 1; i++)
            {
                var clip = (i < _slashClips.Count) ? _slashClips[i] : patternActionClips;
                PlaySlashAnimation(clip, normalSlashHitBox);
                yield return new WaitForSeconds(clip.length);
                normalSlashHitBox.gameObject.SetActive(false);

                if (CheckHit(normalSlashHitBox.transform.position, normalScanRadius, targetLayer, targets))
                    _successCount++;

                yield return new WaitForSeconds(_normalSlashInterval);
            }

            normalSlashHitBox.gameObject.SetActive(false);

            if (_slashClips.Count > 0)
            {
                finishSlashHitBox.gameObject.SetActive(true);
                finishSlashHitBox.changeTime = _finishSlashActionDelay + 0.2f;
                yield return new WaitForSeconds(_finishSlashActionDelay - 0.2f);

                var clip = _slashClips[^1];
                phaseController.OwnerController.Anim.Stop();
                phaseController.OwnerController.Anim.PlayClip(clip);

                yield return new WaitForSeconds(clip.length * 0.5f);
                finishSlashHitBox.gameObject.SetActive(false);

                if (CheckHit(finishSlashHitBox.transform.position, finishScanRadius, targetLayer, targets))
                    _successCount++;
            }

            yield return new WaitForSeconds(recoveryTime);
            PatternState = MonsterPatternState.Done;
        }

        private Vector3 CalculateHitBoxPosition(Transform thisTr, Transform targetTr)
        {
            Vector3 dir = targetTr.position - thisTr.position;
            float distance = dir.magnitude;

            if (attackRange < distance)
                return thisTr.position + dir.normalized * attackRange;
            return targetTr.position;
        }

        private void PlaySlashAnimation(AnimationClip clip, HitBoxSpriter hitBox)
        {
            hitBox.gameObject.SetActive(true);
            hitBox.changeTime = clip.length;

            var anim = phaseController.OwnerController.Anim;
            anim.Stop();
            anim.PlayClip(clip);
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
                    Debug.Log($"Slash Hit > {_successCount}");
                    return true;
                }
            }
            return false;
        }

        public override void Exit()
        {
            base.Exit();
            phaseController.OwnerController.UnLockChangeState();
            phaseController.OwnerController.RevokeMovementAuthority();
        }

        private void OnDrawGizmos()
        {
            if (normalSlashHitBox != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(normalSlashHitBox.transform.position, normalScanRadius);
            }

            if (finishSlashHitBox != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(finishSlashHitBox.transform.position, finishScanRadius);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
