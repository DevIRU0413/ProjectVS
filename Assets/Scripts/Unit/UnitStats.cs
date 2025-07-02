using UnityEngine;

namespace ProjectVS.Unit
{
    [System.Serializable]
    public class UnitStats
    {
        // base (직렬화 필요)
        [SerializeField] private float _hp;
        [SerializeField] private float _baseMaxHp;
        [SerializeField] private float _baseAtk;
        [SerializeField] private float _baseDfs;
        [SerializeField] private float _baseSpd;
        [SerializeField] private float _baseAtkSpd;

        // weight (직렬화 필요 없음)
        private float _weightMaxHp;
        private float _weightAtk;
        private float _weightDfs;
        private float _weightSpd;
        private float _weightAtkSpd;

        // current
        public float CurrentHp
        {
            get => _hp;
            set => _hp = value;
        }

        public float CurrentMaxHp => _baseMaxHp * _weightMaxHp;

        public float CurrentAtk => _baseAtk * _weightAtk;
        public float CurrentDfs => _baseDfs * _weightDfs;
        public float CurrentSpd => _baseSpd * _weightSpd;
        public float AtkSpd => _baseAtkSpd * _weightAtkSpd;

        public UnitStats(float currentHp, float baseMaxHp, float baseAtk, float baseDfs, float baseSpd, float baseAtkSpd)
        {
            // base
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
            _hp = currentHp;
        }

        // weight, get set
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

        // base, get set
        public void SetIncreaseBaseStats(UnitStaus stat, float value)
        {
            switch (stat)
            {
                case UnitStaus.MaxHp: _baseMaxHp += value; break;
                case UnitStaus.Atk: _baseAtk += value; break;
                case UnitStaus.Dfs: _baseDfs += value; break;
                case UnitStaus.Spd: _baseSpd += value; break;
                case UnitStaus.AtkSpd: _baseAtkSpd += value; break;
            }
        }
        public float GetBaseStat(UnitStaus stat)
        {
            switch (stat)
            {
                case UnitStaus.MaxHp: return _baseMaxHp;
                case UnitStaus.Atk: return _baseAtk;
                case UnitStaus.Dfs: return _baseDfs;
                case UnitStaus.Spd: return _baseSpd;
                case UnitStaus.AtkSpd: return _baseAtkSpd;
                default: return 0f;
            }
        }
    }
}
