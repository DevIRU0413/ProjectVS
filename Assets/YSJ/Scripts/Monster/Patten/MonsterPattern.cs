using System.Collections;
using UnityEngine;
using ProjectVS.Interface;
using ProjectVS.Phase;
using ProjectVS;

namespace ProjectVS.Monster.Pattern
{
    public abstract class MonsterPattern : MonoBehaviour, IStateLifecycle
    {
        [field: Header("MonsterPattern Information")]
        [field: SerializeField, Min(0.0f)]
        protected float CastDelay { get; private set; } = 0.0f;

        [field: SerializeField, Min(0.0f)]
        protected float RecoveryTime { get; private set; } = 0.0f;

        [field: SerializeField, Min(0.0f)]
        private float _cooldown = 1.0f;

        [field: SerializeField]
        public MonsterPatternState PatternState { get; protected set; } = MonsterPatternState.None;

        protected MonsterPhaseController _phaseController;
        protected float _currentCooldown;

        public virtual void Init(MonsterPhaseController phaseController)
        {
            _phaseController = phaseController;
            _currentCooldown = _cooldown;
            PatternState = MonsterPatternState.None;
        }

        public virtual bool Condition()
        {
            return _phaseController != null
                && !_phaseController.OnwerController.IsStateLock
                && !_phaseController.OnwerController.IsDeath
                && _currentCooldown <= 0;
        }

        protected abstract IEnumerator IE_PlayAction();

        public void Update()
        {
            if (_phaseController == null || _phaseController.IsPatternControlLock)
                return;

            if (_currentCooldown > 0)
                _currentCooldown -= Time.deltaTime;
        }

        #region Interface Methods

        public virtual void Enter()
        {
            _phaseController.OnwerController.LockChangeState();
        }

        public void Action()
        {
            PatternState = MonsterPatternState.Play;
            StartCoroutine(IE_PlayAction());
        }

        public virtual void Exit()
        {
            _phaseController.OnwerController.UnLockChangeState();
            _currentCooldown = _cooldown;
            PatternState = MonsterPatternState.None;
        }

        public virtual void Clear()
        {
            _currentCooldown = _cooldown;
            PatternState = MonsterPatternState.None;
        }

        #endregion
    }
}
