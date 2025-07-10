namespace ProjectPV.Core.Stat
{
    [System.Serializable]
    public class StatModifier
    {
        public UnitStatType StatType { get; private set; }
        public float Additive { get; private set; } = 0f;
        public float Multiplier { get; private set; } = 1f;

        public StatModifier(UnitStatType type, float add, float mul)
        {
            StatType = type;
            Additive = add;
            Multiplier = mul;
        }
    }
}
