using ProjectVS.Unit;

namespace ProjectVS.Monster.Data
{
    public class MonsterStats : UnitStats
    {
        private int _exp;

        private int _dropGold;
        private int _dropGoldPer;

        private int _dropDiamond;
        private int _dropDiamondPer;

        public int Exp => _exp;                         // 경험치

        public int DropGold => _dropGold;               // 골드
        public int DropGoldPer => _dropGoldPer;         // 골드 확률

        public int DropDiamond => _dropDiamond;         // 다이야
        public int DropDiamondPer => _dropDiamondPer;   // 다이야 확률

        public MonsterStats(float baseMaxHp, float baseAtk, float baseDfs, float baseSpd, float baseAtkSpd,
            int exp,
            int dropGold, int dropGoldPer,
            int dropDiamond, int dropDiamondPer)
            : base(baseMaxHp, baseMaxHp, baseAtk, baseDfs, baseSpd, baseAtkSpd)
        {
            _exp = exp;
            _dropGold = dropGold;
            _dropGoldPer = dropGoldPer;

            _dropDiamond = dropDiamond;
            _dropDiamondPer = dropDiamondPer;
        }

        public override string ToString()
        {
            return $"[MonsterStats]\n" +
                   $"- HP         : {CurrentMaxHp}\n" +
                   $"- ATK        : {CurrentAtk}\n" +
                   $"- DFS        : {CurrentDfs}\n" +
                   $"- SPD        : {CurrentSpd}\n" +
                   $"- ATK SPD    : {CurrentAtkSpd}\n" +
                   $"- EXP        : {_exp}\n" +
                   $"- Gold       : {_dropGold} ({_dropGoldPer}%)\n" +
                   $"- Diamond    : {_dropDiamond} ({_dropDiamondPer}%)";
        }
    }
}
