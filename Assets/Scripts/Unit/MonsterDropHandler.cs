using ProjectVS.Manager;
using ProjectVS.Monster;
using ProjectVS.Unit.Player;

using UnityEngine;

namespace ProjectVS.Unit.Monster
{
    public class MonsterDropHandler : MonoBehaviour
    {
        private bool _isActive;
        private JDW.PlayerConfig player = null;
        private MonsterController monster = null;

        private void Awake()
        {
            var player = PlayerSpawner.Instance.CurrentPlayer;
            var playerCmp = player.GetComponent<JDW.PlayerConfig>();
            var monCtrl = GetComponentInParent<MonsterController>();

            this.player = playerCmp;
            this.monster = monCtrl;

            _isActive = true;
            monCtrl.OnDeath -= RootingItem;
            monCtrl.OnDeath += RootingItem;
        }

        private void RootingItem()
        {
            this.player.Stats.AddExp(monster.Stats.Exp); 

            // 일단 현재는 죽이면 바로 들어가게 처리
            int goalPer =  Random.Range(1, 100 + 1);
            int diamondsPer =  Random.Range(1, 100 + 1);

            bool goldGetable = (goalPer > monster.Stats.DropGoldPer);
            bool diamondsGetable = (diamondsPer > monster.Stats.DropDiamondPer);

            if (goldGetable)
                PlayerDataManager.Instance.Gold += (monster.Stats.DropGold <= 0) ? 0 : monster.Stats.DropGold;

            if (diamondsGetable)
                PlayerDataManager.Instance.Diamonds += (monster.Stats.DropDiamond <= 0) ? 0 : monster.Stats.DropDiamond;
        }
    }
}
