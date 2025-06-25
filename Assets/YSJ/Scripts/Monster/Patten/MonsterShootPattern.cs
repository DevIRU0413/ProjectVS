using System.Collections;
using UnityEngine;
using ProjectVS.Player;
using ProjectVS.Phase;
using PVS;
using System.Collections.Generic;
using ProjectVS.Manager;

namespace ProjectVS.Monster.Pattern
{
    public class MonsterShootPattern : MonsterPattern
    {
        [Header("Attack Info")]
        [SerializeField, Min(0.0f)] private float _shootableRange = 1.0f;

        [SerializeField] private GameObject _projectilePrefab;

        [SerializeField] private AudioClip _attackClip;
        [SerializeField] private AudioClip _attackCastClip;

        [Header("Projectile Config")]
        [SerializeField, Min(1)] private int totalProjectiles = 24;
        [SerializeField, Min(1)] private int multiShotCount = 3;
        [SerializeField, Min(0f)] private float multiShotSpacing = 5f;
        [SerializeField, Range(0f, 360f)] private float spreadAngle = 360f;
        [SerializeField, Min(1)] private int step = 1;
        [SerializeField, Min(0f)] private float stepDelay = 0.5f;

        [SerializeField] private List<float> stepBaseAngleList = new(); // 사용 시, 랜덤한 

        protected PlayerConfig _target;

        public override void Init(MonsterPhaseController phaseController)
        {
            base.Init(phaseController);
            _target = GameManager.ForceInstance.player;
        }

        public override bool Condition()
        {
            return _target != null && (_target.transform.position - transform.position).magnitude <= _shootableRange && base.Condition();
        }

        public override void Enter()
        {
            _phaseController.OnwerController.ChangeState(MonsterStateType.Idle, true);
            base.Enter();
        }

        protected override IEnumerator IE_PlayAction()
        {
            if (CastDelay > 0f)
                yield return new WaitForSeconds(CastDelay);

            if (_target != null && _projectilePrefab != null)
            {
                yield return FireMultiPattern(
                    _projectilePrefab,
                    transform.position,
                    totalProjectiles,
                    multiShotCount,
                    multiShotSpacing,
                    spreadAngle,
                    step,
                    stepDelay,
                    _target.transform,
                    stepBaseAngleList
                );
            }

            if (RecoveryTime > 0f)
                yield return new WaitForSeconds(RecoveryTime);

            PatternState = MonsterPatternState.Done;
        }

        private IEnumerator FireMultiPattern(
        GameObject projectilePrefab,
        Vector3 origin,
        int totalProjectiles,
        int multiShotCount,
        float multiShotSpacing,
        float spreadAngle,
        int step,
        float stepDelay,
        Transform target = null,
        List<float> stepBaseAngleList = null)
        {
            int totalGroups = totalProjectiles / multiShotCount;
            int groupsPerStep = totalGroups / step;
            float groupAngleStep = spreadAngle / groupsPerStep;

            Vector3 dir = target.position - origin;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            float baseStartAngle = angle;

            for (int s = 0; s < step; s++)
            {
                if (stepBaseAngleList != null && stepBaseAngleList.Count > 0)
                    baseStartAngle = stepBaseAngleList[s];

                for (int g = 0; g < groupsPerStep; g++)
                {
                    int globalIndex = s * groupsPerStep + g;
                    float groupBaseAngle = baseStartAngle + globalIndex * groupAngleStep;
                    Vector2 baseDir = Quaternion.Euler(0, 0, groupBaseAngle) * Vector2.right;

                    for (int i = 0; i < multiShotCount; i++)
                    {
                        float offset = (i - (multiShotCount - 1) / 2f) * multiShotSpacing;
                        Vector2 finalDir = Quaternion.Euler(0, 0, offset) * baseDir;

                        GameObject clone = GameObject.Instantiate(projectilePrefab, origin, Quaternion.identity);
                        clone.GetComponent<Projectile>()?.Fire(finalDir, target);
                    }
                }

                if (s < step - 1 && stepDelay > 0f)
                    yield return new WaitForSeconds(stepDelay);
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _shootableRange);
        }
    }
}
