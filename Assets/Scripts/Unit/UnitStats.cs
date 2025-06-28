namespace ProjectVS.Unit
{
    [System.Serializable]
    public class UnitStats
    {
        // base
        private float _baseHp;          // 체력
        private float _baseMaxHp;       // 최대 체력
        private float _baseAtk;         // 공격력
        private float _baseDfs;         // 방어력
        private float _baseSpd;         // 이속
        private float _baseAtkSpd;      // 공속

        // weight
        private float _weightMaxHp;       // 최대 체력
        private float _weightAtk;         // 공격력
        private float _weightDfs;         // 방어력
        private float _weightSpd;         // 이속
        private float _weightAtkSpd;      // 공속

        // current
        public float CurrentHp;         // 체력
        public float CurrentMaxHp;      // 최대 체력

        public float CurrentAtk;        // 공격력
        public float CurrentDfs;        // 방어력
        public float CurrentSpd;        // 이동속도

        public float AtkSpd;            // 공격속도

        public UnitStats(float baseMaxHp, float baseAtk, float baseDfs, float baseSpd, float baseAtkSpd)
        {
            // base
            _baseHp = baseMaxHp;
            _baseMaxHp = baseMaxHp;
            _baseAtk = baseAtk;
            _baseDfs = baseDfs;
            _baseSpd = baseSpd;
            _baseAtkSpd = baseAtkSpd;

            // weight
            _weightMaxHp = 1.0f;
            _weightAtk = 1.0f;
            _weightDfs = 1.0f;
            _weightSpd = 1.0f;
            _weightAtkSpd = 1.0f;

            // current
            CurrentMaxHp = _baseMaxHp * _weightMaxHp;
            CurrentHp = CurrentMaxHp;

            CurrentAtk = _baseAtk * _weightAtk;
            CurrentDfs = _baseDfs * _weightDfs;
            CurrentSpd = _baseSpd * _weightSpd;
            AtkSpd = _baseAtkSpd * _weightAtkSpd;
        }
        public void SetWeightStats(UnitStaus stats, float weight)
        {
            switch (stats)
            {
                case UnitStaus.MaxHp: _weightMaxHp = weight; break;
                case UnitStaus.Atk: _weightAtk = weight; break;
                case UnitStaus.Dfs: _weightDfs = weight; break;
                case UnitStaus.Spd: _weightSpd = weight; break;
                case UnitStaus.AtkSpd: _weightAtkSpd = weight; break;
            }
        }
        public float GetWeightStats(UnitStaus stats, float weight)
        {
            switch (stats)
            {
                case UnitStaus.MaxHp: return _weightMaxHp;
                case UnitStaus.Atk: return _weightAtk;
                case UnitStaus.Dfs: return _weightDfs;
                case UnitStaus.Spd: return _weightSpd;
                case UnitStaus.AtkSpd: return _weightAtkSpd;
                default: return 0f;
            }
        }
    }
}
