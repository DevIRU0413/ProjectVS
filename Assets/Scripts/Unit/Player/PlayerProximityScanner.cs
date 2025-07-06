using ProjectVS.Monster;

using UnityEngine;

namespace ProjectVS.Unit.Player
{
    public class PlayerProximityScanner : ProximityScanner
    {
        protected override void Awake()
        {
            base.Awake();
            // AddCondition(MonsterCheckCondition); // Test 떄문에 주석
        }

        private bool MonsterCheckCondition(GameObject go)
        {
            var monster = go.GetComponentInParent<MonsterController>();
            return (monster != null);
        }
    }
}
