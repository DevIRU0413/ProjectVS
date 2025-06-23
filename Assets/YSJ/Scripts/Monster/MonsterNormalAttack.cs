using System;
using System.Collections;

using PVS.Player;

using Scripts.Util;

using Unity.VisualScripting;

using UnityEngine;

namespace PVS.Monster
{
    public class MonsterNormalAttack : MonsterPatten
    {
        [Header("Target Search")]
        [SerializeField] private string _inputSearchTargetTag = "Player";
        [SerializeField] private Collider2D _searchCollider;

        [Header("Attack Info")]

        [SerializeField]
        [Min(0.0f)]
        private float _damageMultiplier = 0.0f;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private AudioClip _attackClip;
        [SerializeField] private AudioClip _attackCastClip;

        private string _searchTargetTag;

        private Collider2DAction _collider2DAction;
        private PlayerController _target;

        public override void Init(MonsterPhaseController phaseController)
        {
            base.Init(phaseController);

            _searchTargetTag = _inputSearchTargetTag;

            if (_searchCollider == null)
            {
                Debug.LogWarning("Search collider is not assigned.");
                enabled = false;
                return;
            }

            _searchCollider.isTrigger = true;
            var circle = _searchCollider as CircleCollider2D;
            if (circle != null)
                circle.radius = phaseController.OnwerController.Status.CurrentAtkRange;

            _collider2DAction = _searchCollider.GetOrAddComponent<Collider2DAction>();

            _collider2DAction.OnTriggerEnterAction -= AddTarget;
            _collider2DAction.OnTriggerEnterAction += AddTarget;

            _collider2DAction.OnTriggerExitAction -= RemoveTarget;
            _collider2DAction.OnTriggerExitAction += RemoveTarget;
        }

        public override bool Condition()
        {
            return _target != null && base.Condition();
        }

        protected override IEnumerator IE_PlayAction()
        {
            // 선딜
            if (CastDelay > 0f)
                yield return new WaitForSeconds(CastDelay);

            // 실제 공격 처리
            if (_target != null)
            {
                float damage = _phaseController.OnwerController.Status.CurrentAtk * _damageMultiplier;

                if (_projectilePrefab == null)
                    PerformMeleeAttack(damage);
                else
                    PerformRangedAttack(damage);
            }

            // 후딜
            if (RecoveryTime > 0f)
                yield return new WaitForSeconds(RecoveryTime);

            // 종료
            PattenState = MonsterPattenState.Done;
            yield break;
        }
        private void AddTarget(Collider2D coll)
        {
            if (!coll.CompareTag(_searchTargetTag)) return;

            var pc = coll.GetComponentInParent<PlayerController>();
            if (pc != null)
                _target = pc;
        }

        private void RemoveTarget(Collider2D coll)
        {
            if (!coll.CompareTag(_searchTargetTag)) return;

            var pc = coll.GetComponentInParent<PlayerController>();
            if (pc != null && pc == _target)
                _target = null;
        }

        private void PerformMeleeAttack(float damage)
        {
            _target.Status.CurrentHp -= damage;

            Debug.Log($"[Monster] {_phaseController.name} attacks {_target.name} for {damage} / Target HP {_target.Status.CurrentHp + damage} > {_target.Status.CurrentHp}");

            // _target.OnHit?.Invoke();
        }

        private void PerformRangedAttack(float damage)
        {
            // 발사체 발사.
        }
    }
}
