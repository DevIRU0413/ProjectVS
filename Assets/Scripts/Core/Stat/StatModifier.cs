namespace ProjectPV.Core.Stat
{
    [System.Serializable]
    public class StatModifier
    {
        public string sourceId;              // Buff ID, Equipment ID 등
        public UnitStatType statType;
        public float additive = 0f;          // +10 같은 고정 증가
        public float multiplier = 1f;        // *1.2 같은 배율 증가

        public StatModifier(string id, UnitStatType type, float add, float mul)
        {
            sourceId = id;
            statType = type;
            additive = add;
            multiplier = mul;
        }
    }
}
