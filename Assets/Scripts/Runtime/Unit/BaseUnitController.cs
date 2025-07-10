using ProjectPV.Core.Stat;

using ProjectVS.Core.Buff;
using ProjectVS.FSM;
using ProjectVS.Runtime.Unit.FSM;

using UnityEngine;

namespace ProjectVS.Runtime.Unit
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseUnitController : MonoBehaviour
    {
        // ===== Core =====
        public UnitStats Stats { get; protected set; }
        public BuffSystem Buffs { get; protected set; }
        public UnitStateMachine FSM { get; protected set; }

        // ===== External Input =====
        // 해당 파트를 이동 입력 제공자 생성후 수정 예정
        // protected IMoveInputProvider _moveInput;

        // ===== Modules =====
        // 해당 파트를 차후 모듈 생성 후 수정 예정
        /*protected MovementModule _movement;
        protected AttackModule _attack;*/

        // ===== Components =====
        protected Rigidbody _rigid;
        protected Animator _anim;
        [SerializeField] protected Transform modelRoot;

        // ===== Mono =====
        protected virtual void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
            _anim = GetComponentInChildren<Animator>();

            Stats = new UnitStats();
            Buffs = new BuffSystem(Stats);
            FSM = new UnitStateMachine(this);

            // 해당 파트를 차후 모듈 생성 후 수정 예정
           /* _movement = new MovementModule(this, _rigid, modelRoot);
            _attack = new AttackModule(this);*/
        }

        protected virtual void Start()
        {
            FSM.ChangeState(UnitStateType.Idle);
        }

        protected virtual void Update()
        {
            float dt = Time.deltaTime;

            i/*f (!FSM.IsControllable)
                return;

            _movement?.Tick(dt);
            _attack?.Tick(dt);*/
            Buffs?.Tick(dt);
        }

        // ===== External Control =====

        // 해당 입력 방식을 받아서
        /*public void SetMoveInput(IMoveInputProvider provider)
        {
            _moveInput = provider;
        }*/

        /*public Vector3 GetMoveDirection()
        {
            return _moveInput?.GetMoveDirection() ?? Vector3.zero;
        }*/

        public void PlayAnimation(string name)
        {
            _anim?.Play(name);
        }

        public void TakeDamage(float amount)
        {
            float dmg = Mathf.Max(amount - Stats.GetFinalStat(UnitStatType.Dfs), 1f);
            Stats.CurrentHp -= dmg;

            if (Stats.CurrentHp <= 0f)
                FSM.ChangeState(UnitStateType.Dead);
        }

        public bool IsDead => Stats.CurrentHp <= 0f;
    }
}
