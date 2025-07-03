using System.Collections;

using ProjectVS.Interface;
using ProjectVS.Phase;

using UnityEngine;

namespace ProjectVS.Monster.Pattern
{
    public abstract class MonsterPattern : MonoBehaviour, IPatternLifecycle
    {
        [field: Header("MonsterPattern Information")]
        [field: SerializeField] protected AnimationClip patternCastClips;
        [field: SerializeField] protected AnimationClip patternActionClips;
        [field: SerializeField] protected AnimationClip patternRecoveryClips;

        [field: SerializeField, Min(0.0f)]
        protected float castDelay { get; private set; } = 0.0f;

        [field: SerializeField, Min(0.0f)]
        protected float recoveryTime { get; private set; } = 0.0f;

        [field: SerializeField, Min(0.0f)]
        private float _cooldown = 1.0f;

        public MonsterPatternState PatternState { get; protected set; } = MonsterPatternState.None;

        protected MonsterPhaseController phaseController;
        protected float currentCooldown;

        public virtual void Init(MonsterPhaseController phaseController)
        {
            this.phaseController = phaseController;
            currentCooldown = _cooldown;
            PatternState = MonsterPatternState.None;
        }

        public virtual bool Condition()
        {
            return phaseController != null
                && !phaseController.OnwerController.IsStateLock
                && !phaseController.OnwerController.IsDeath
                && currentCooldown <= 0;
        }

        protected abstract IEnumerator IE_PlayAction();

        public void Update()
        {
            if (phaseController == null || phaseController.IsPatternControlLock)
                return;

            if (currentCooldown > 0)
                currentCooldown -= Time.deltaTime;
        }

        #region Interface Methods

        public virtual void Enter()
        {
            phaseController.OnwerController.LockChangeState();
        }

        public void Action()
        {
            PatternState = MonsterPatternState.Play;
            StartCoroutine(IE_PlayAction());
        }

        public virtual void Exit()
        {
            phaseController.OnwerController.UnLockChangeState();
            currentCooldown = _cooldown;
            PatternState = MonsterPatternState.None;
        }

        public virtual void Clear()
        {
            currentCooldown = _cooldown;
            PatternState = MonsterPatternState.None;
        }

        #endregion
    }
}
