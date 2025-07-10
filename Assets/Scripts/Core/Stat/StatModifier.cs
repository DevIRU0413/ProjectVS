namespace ProjectPV.Core.Stat
{
    [System.Serializable]
    public class StatModifier
    {
        public UnitStatType statsType;
        public float additive = 0f;
        public float multiplier = 1f;

        public StatModifier(UnitStatType type, float add, float mul)
        {
            statsType = type;
            additive = add;
            multiplier = mul;
        }
    }
}
