using System;
using System.Collections.Generic;

using ProjectVS.Interface;
using ProjectVS.Monster.Data;
using ProjectVS.Monster.State;
using ProjectVS.Stage;
using ProjectVS.Unit.Monster;
using ProjectVS.Unit.Monster.Phase;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Monster
{
    [RequireComponent(typeof(Rigidbody2D), typeof(MonsterPhaseController))]
    public class MonsterController : MonoBehaviour, IDamageable, IPoolable
    {
        #region Serialized Fields
        [SerializeField] private GameObject _body;
        [SerializeField] private float _stopMoveRange = 0.1f;
        [field: SerializeField] public int MonsterID { get; private set; } = -1;
        [field: SerializeField] public string Name { get; private set; } = "N/A";
        [field: SerializeField] public MonsterStateType CurrentStateType { get; private set; } = MonsterStateType.None;
        [field: SerializeField] public bool IsStateLock { get; private set; } = false;
        [field: SerializeField] public GameObject Target { get; private set; }
        [field: Header("Death State")]
        [field: SerializeField, Min(0)] public float DespawnDelay { get; private set; } = 1.0f;
        #endregion

        #region Public Accessors
        public bool IsDeath => CurrentStateType == MonsterStateType.Death;
        public bool IsMove => MoveDirection.sqrMagnitude > _stopMoveRange * _stopMoveRange;
        public bool IsWin => false;
        public MonsterAnimationPlayer Anim { get; private set; }
        public MonsterStats Stats { get; private set; }
        public Vector3 MoveDirection { get; private set; } = Vector3.zero;
        public Action OnHit { get; set; }
        public Action OnDeath;
        public Action OnSpawn { get; set; }
        public Action OnDespawn { get; set; }
        #endregion

        #region Private State
        private Vector3 _bodyScale;
        private bool _isInit = false;
        private bool _isMovementDelegated = false;
        private Dictionary<MonsterStateType, MonsterState> _states = new();
        private MonsterState _currentState;
        #endregion

        #region Unity Callbacks
        private void Awake() => Init();

        private void Update()
        {
            if (Stats?.CurrentHp <= 0 && CurrentStateType != MonsterStateType.Death)
            {
                UnLockChangeState();
                ChangeState(MonsterStateType.Death, true);
            }

            if (!_isMovementDelegated && Target != null)
                SetMoveDirection(Target.transform.position);

            if (!_currentState.UseFixedUpdate)
                _currentState?.Update();
        }

        private void FixedUpdate()
        {
            if (_currentState?.UseFixedUpdate == true)
                _currentState.Update();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + MoveDirection.normalized * 3.0f);
            Gizmos.DrawWireSphere(transform.position, _stopMoveRange);
        }
        #endregion

        #region Initialization
        private void Init()
        {
            if (_isInit) return;

            SetupRigidbody();
            CacheBodyScale();
            SetupAnimator();
            SetupStates();
            SettingData();
            SetupInitialState();

            _isInit = true;
        }

        private void SetupRigidbody()
        {
            var rb = gameObject.GetOrAddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        private void CacheBodyScale()
        {
            if (_body != null)
                _bodyScale = _body.transform.localScale;
        }

        private void SetupAnimator()
        {
            var anim = GetComponentInChildren<Animator>();
            Anim = new MonsterAnimationPlayer(anim, this);
        }

        private void SetupStates()
        {
            _states.Add(MonsterStateType.Idle, new MonsterIdleState(this, Anim.Animator));
            _states.Add(MonsterStateType.Move, new MonsterMoveState(this, Anim.Animator));
            _states.Add(MonsterStateType.Win, new MonsterWinState(this, Anim.Animator));
            _states.Add(MonsterStateType.Death, new MonsterDeathState(this, Anim.Animator));
        }

        private void SetupInitialState()
        {
            if (Stats == null)
                ChangeState(MonsterStateType.Death, true);
            else
                ChangeState(Stats.CurrentHp > 0 ? MonsterStateType.Idle : MonsterStateType.Death);
        }
        #endregion

        #region State Control
        public void ChangeState(MonsterStateType type, bool force = false)
        {
            if (IsStateLock || (!force && CurrentStateType == type) || (CurrentStateType == MonsterStateType.Death && !force))
                return;

            _currentState?.Exit();
            CurrentStateType = type;
            _currentState = _states[type];
            _currentState.Enter();
        }

        public void LockChangeState() => IsStateLock = true;
        public void UnLockChangeState() => IsStateLock = false;
        #endregion

        #region Movement
        public void DelegateMovementAuthority() => _isMovementDelegated = true;
        public void RevokeMovementAuthority() => _isMovementDelegated = false;

        public void SetMoveDirection(Vector3 targetPoint, bool raw = false)
        {
            MoveDirection = raw ? targetPoint : (targetPoint - transform.position);
            if (_body == null) return;

            if (MoveDirection.x > 0.01f)
                _body.transform.localScale = _bodyScale;
            else if (MoveDirection.x < -0.01f)
                _body.transform.localScale = new Vector3(-_bodyScale.x, _bodyScale.y, _bodyScale.z);
        }

        public void SetTarget(GameObject target) => Target = target;
        #endregion

        #region IDamageable
        public void TakeDamage(DamageInfo info)
        {
            Stats.CurrentHp -= info.Amount;
            Debug.Log($"{gameObject.name} 피격 → HP: {Stats.CurrentHp}/{Stats.CurrentMaxHp}");
            OnHit?.Invoke();
        }
        #endregion

        #region IPoolable
        public void OnSpawned()
        {
            SettingData();
            if (Stats != null)
                Stats.CurrentHp = Stats.CurrentMaxHp;

            ChangeState(Stats.CurrentHp > 0 ? MonsterStateType.Idle : MonsterStateType.Death, true);
            Target = Unit.Player.PlayerSpawner.Instance.CurrentPlayer;

            OnSpawn?.Invoke();
        }

        public void OnDespawned() => OnDespawn?.Invoke();
        #endregion

        #region Stat Setting
        private void SettingData()
        {
            if (StageManager.Instance.Context != null)
            {
                var config = StageManager.Instance.Context.Spawner.GetMonsterStatsConfig(MonsterID);
                if (config != null)
                {
                    Name = config.MonsterName;
                    Stats = new MonsterStats(config.Hp, config.ATK, config.DFS, config.SPD, config.ATKSPD,
                        config.Exp, config.DropGold, config.DropGoldPer, config.DropDiamond, config.DropDiamondPer);
                    Debug.Log(ToString());
                    return;
                }
            }

            // fallback
            if (Stats == null)
                Stats = new MonsterStats(1, 1, 1, 1, 1, 0, 0, 0, 0, 0);
            Debug.Log(ToString());
        }
        #endregion

        public override string ToString()
        {
            return $"[MonsterController]\n" +
                   $"- ID              : {MonsterID}\n" +
                   $"- Name            : {Name}\n" +
                   $"- State           : {CurrentStateType} (Locked: {IsStateLock})\n" +
                   $"- Target          : {(Target != null ? Target.name : "None")}\n" +
                   $"- Position        : {transform.position}\n" +
                   $"- Move Direction  : {MoveDirection}\n" +
                   $"- Delegated Move? : {_isMovementDelegated}\n" +
                   $"- IsDead          : {IsDeath}\n" +
                   $"- Stats           :\n{(Stats != null ? Stats.ToString() : "  - (No Stats)")}";
        }
    }
}
