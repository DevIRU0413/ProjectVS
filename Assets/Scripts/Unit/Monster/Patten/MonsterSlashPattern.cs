using System.Collections;
using System.Collections.Generic;

using ProjectVS.Interface;
using ProjectVS.Unit.Monster.Pattern;

using UnityEngine;

namespace ProjectVS.Unit.Monster
{
    public class MonsterSlashPattern : MonsterPattern
    {
        [Header("Slash Settings")]
        [SerializeField, Min(1)] private int slashCount = 3;
        [SerializeField, Min(0f)] private float slashInterval = 0.5f;
        [SerializeField] private List<AnimationClip> slashClips;

        [Header("Hit Detection")]
        [SerializeField] private Collider2D slashCollider; // Optional
        [SerializeField] private float scanRadius = 1.5f;
        [SerializeField] private LayerMask targetLayer;

        private int _successCount = 0;

        protected override IEnumerator IE_PlayAction()
        {
            _successCount = 0;

            for (int i = 0; i < slashCount; i++)
            {
                // 1. 애니메이션 재생
                var clip = (i < slashClips.Count) ? slashClips[i] : patternActionClips;
                PlayAnimation(clip);

                yield return new WaitForSeconds(0.2f); // 애니메이션 타격 타이밍 지연

                // 2. 충돌 감지
                if (CheckHit())
                    _successCount++;

                yield return new WaitForSeconds(slashInterval);
            }

            yield return new WaitForSeconds(recoveryTime);

            // 결과 평가
            if (_successCount == 0)
                TriggerGroggy(); // 외부 컨트롤러에서 카운트 증가 시도

            PatternState = MonsterPatternState.Done;
        }

        private bool CheckHit()
        {
            var targets = Physics2D.OverlapCircleAll(transform.position, scanRadius, targetLayer);
            foreach (var hit in targets)
            {
                if (hit.TryGetComponent(out IDamageable target))
                {
                    target.TakeDamage(new DamageInfo(10, (hit.transform.position - transform.position).normalized)); // 데미지 값 예시
                    return true;
                }
            }

            return false;
        }

        private void PlayAnimation(AnimationClip clip)
        {
            if (clip != null)
            {
                phaseController.OnwerController.Anim.PlayClip(clip);
            }
        }
        private void TriggerGroggy()
        {
            // GroggyController에 위임: 추적하고 있는 패턴 실패로 기록
            // 예: MonsterGroggyController.Instance.NotifyFailure(this);
        }
    }
}
