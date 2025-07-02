using ProjectVS.Monster;

using UnityEngine;

namespace ProjectVS.Unit.Player
{
    public class PlayerProximityScanner : ProximityScanner
    {
        protected override void Awake()
        {
            base.Awake();
            // AddCondition(MonsterCheckCondition);
        }

        private bool MonsterCheckCondition(GameObject go)
        {
            var monster = go.GetComponentInParent<MonsterController>();
            return (monster != null);
        }
    }
}
