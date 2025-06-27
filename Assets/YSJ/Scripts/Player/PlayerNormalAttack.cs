using System.Collections.Generic;

using ProjectVS.Monster;
using ProjectVS.Util;

using ProjectVS;

using UnityEngine;

namespace ProjectVS.Player
{
    public class PlayerNormalAttack : MonoBehaviour
    {
        [Header("Required Information")]
        [SerializeField] private string _inputSearchTargetTag = "Monster";
        [SerializeField] private Collider2D _searchCollider;

        private Collider2DAction _collider2DAction;

        private string _searchTargetTag;

        private List<GameObject> m_monsters = new();
        private MonsterController m_nearestMonster;

        private void Start()
        {
            _searchTargetTag = _inputSearchTargetTag;

            if (_searchCollider == null)
            {
                this.enabled = false;
                return;
            }

            _searchCollider.isTrigger = true;
            _collider2DAction = _searchCollider.gameObject.GetOrAddComponent<Collider2DAction>();

            _collider2DAction.OnTriggerEnterAction -= AddTarget;
            _collider2DAction.OnTriggerEnterAction += AddTarget;

            _collider2DAction.OnTriggerExitAction += RemoveTarget;
            _collider2DAction.OnTriggerExitAction += RemoveTarget;
        }

        // 탐색 조건
        private bool IsMonsterTargetableCondition(GameObject go)
        {
            MonsterController ctrl = go?.GetComponent<MonsterController>();
            if (ctrl == null)
                return false;

            return (ctrl.CurrentStateType != MonsterStateType.Death);
        }

        private void AddTarget(Collider2D coll)
        {
            if (coll.CompareTag(_searchTargetTag) == false) return;

            var targetCmp = coll.GetComponentInParent<MonsterController>();
            if (targetCmp == null) return;

            GameObject targetGo = targetCmp.gameObject;
            if (m_monsters.Contains(targetGo) == true) return;

            m_monsters?.Add(targetGo);
            // Debug.Log($"Add Monster: {targetCmp}");
        }

        private void RemoveTarget(Collider2D coll)
        {
            if (coll.CompareTag(_searchTargetTag) == false) return;

            var targetCmp = coll.GetComponentInParent<MonsterController>();
            if (targetCmp == null) return;

            GameObject targetGo = targetCmp.gameObject;
            if (m_monsters.Contains(targetGo) == false) return;

            m_monsters?.Remove(targetGo);
            // Debug.Log($"Remove Monster: {monsterCtrlCmp}");
        }

        private void FixedUpdate()
        {
            m_nearestMonster = transform.FindNearestTarget<MonsterController>(m_monsters, IsMonsterTargetableCondition);
        }

        // 임시
        private void OnDrawGizmos()
        {
            if (m_nearestMonster != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, m_nearestMonster.transform.position);
            }
        }
    }
}
