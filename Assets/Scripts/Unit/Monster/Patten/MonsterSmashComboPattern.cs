using System.Collections;
using System.Collections.Generic;

using ProjectVS.Interface;

using UnityEngine;

namespace ProjectVS.Unit.Monster.Pattern
{
    public class MonsterSmashComboPattern : MonsterPattern, IGroggyTrackable
    {
        [SerializeField] private List<AnimationClip> smashClips;
        [SerializeField] private int comboCount = 3;
        [SerializeField] private int groggyThreshold = 1;

        [SerializeField] private float scanRadius = 1.5f;
        [SerializeField] private LayerMask targetLayer;

        public bool IsFaild { get; private set; } = false;
        public int GroggyThreshold => groggyThreshold;

        private int _failCount = 0;

        protected override IEnumerator IE_PlayAction()
        {
            _failCount = 0;
            IsFaild = false;

            for (int i = 0; i < comboCount; i++)
            {
                var clip = i < smashClips.Count ? smashClips[i] : patternActionClips;
                PlayAnimation(clip);
                yield return new WaitForSeconds(0.25f);

                bool hit = CheckHit();
                if (!hit) _failCount++;

                yield return new WaitForSeconds(recoveryTime);
            }

            IsFaild = (_failCount == comboCount);
            PatternState = MonsterPatternState.Done;
        }

        private bool CheckHit()
        {
            /*var targets = Physics2D.OverlapCircleNonAlloc(transform.position, scanRadius, targetLayer);
            foreach (var target in targets)
            {
                if (target.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(new DamageInfo(10, Vector2.zero));
                    return true;
                }
            }
            return false;*/
            return true;
        }

        private void PlayAnimation(AnimationClip clip)
        {
            if (clip != null)
                phaseController.OnwerController.Anim.PlayClip(clip);
        }
    }
}
