namespace ProjectVS.Player
{
    public class PlayerStatus
    {
        // 나중, 능력 증가 수치나, 기본 능력치 같이 받아서 처리
        public PlayerStatus(float hp, float atk, float spd, float atkRange)
        {
            // base
            m_baseHp = hp;
            m_baseAtk = atk;
            m_baseSpd = spd;
            m_baseAtkRange = atkRange;

            // current
            CurrentMaxHp = m_baseHp;
            CurrentHp = CurrentMaxHp;

            CurrentAtk = m_baseAtk;
            CurrentSpd = m_baseSpd;

            CurrentAtkRange = m_baseAtkRange;

            CurrentHaveExp = 0;
            CurrentMaxExp = 100;
        }

        // base
        private float m_baseHp;
        private float m_baseAtk;
        private float m_baseSpd;
        private float m_baseAtkRange;

        // current
        public float CurrentHp;
        public float CurrentMaxHp;

        public float CurrentAtk;
        public float CurrentSpd;

        public float CurrentAtkRange;

        public float CurrentHaveExp;
        public float CurrentMaxExp;

        // 나중에 아래에 함수 만들어서 처리
    }
}
