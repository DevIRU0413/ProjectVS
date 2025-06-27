namespace ProjectVS.Monster.Data
{
    public class MonsterStatus
    {
        // 나중, 능력 증가 수치나, 기본 능력치 같이 받아서 처리
        public MonsterStatus(float hp, float atk, float spd, float atkRange, int ExpPoint)
        {
            // base
            m_baseHp = hp;
            m_baseAtk = atk;
            m_baseSpd = spd;
            m_baseAtkRange = atkRange;
            this.ExpPoint = ExpPoint;

            // current
            CurrentMaxHp = m_baseHp;
            CurrentHp = CurrentMaxHp;
            CurrentAtk = m_baseAtk;
            CurrentSpd = m_baseSpd;
            CurrentAtkRange = m_baseAtkRange;
        }

        private float m_baseHp;
        private float m_baseAtk;
        private float m_baseSpd;
        private float m_baseAtkRange;
        public int ExpPoint;

        public float CurrentHp;
        public float CurrentMaxHp;
        public float CurrentAtk;
        public float CurrentSpd;
        public float CurrentAtkRange;

        // 나중에 아래에 함수 만들어서 처리
    }
}
