using UnityEngine;

namespace ProjectVS.Unit.Player
{
    [System.Serializable]
    public class PlayerStats : UnitStats
    {
        public int Level = 1;                   // 현재 레벨
        public CharacterClass CharacterClass;   // 직업

        public float CurrentExp = 0f;           // 현재 경험치
        public float MaxExp = 100f;             // 레벨업 까지 필요한 경험치

        public bool AddExp(float amount)
        {
            CurrentExp += amount;
            if (CurrentExp >= MaxExp)
            {
                LevelUp();
                return true;
            }
            return false;
        }
        private void LevelUp()
        {
            CurrentExp -= MaxExp;
            Level++;
            MaxExp *= 1.2f;

            // base 계열 증가 → Current 계열 자동 반영
            switch (CharacterClass)
            {
                case CharacterClass.Axe:
                    AddBaseStats(10, 8, 2, 0, 0);
                    break;
                case CharacterClass.Sword:
                    AddBaseStats(7, 5, 8, 0, 0);
                    break;
                case CharacterClass.Magic:
                    AddBaseStats(5, 7, 8, 0, 0);
                    break;
            }

            Debug.Log($"레벨 업, 현재 레벨 : {Level}, 남은 경험치 : {CurrentExp}, 다음 레벨까지 : {MaxExp}");
        }
        public void AddBaseStats(float hp, float atk, float dfs, float spd, float atkSpd)
        {
            SetIncreaseBaseStats(UnitStaus.MaxHp, hp);
            SetIncreaseBaseStats(UnitStaus.Atk, atk);
            SetIncreaseBaseStats(UnitStaus.Dfs, dfs);
            SetIncreaseBaseStats(UnitStaus.Spd, spd);
            SetIncreaseBaseStats(UnitStaus.AtkSpd, atkSpd);
        }

        #region 생성자
        /// <summary>
        /// 초기 PlayerDataManager에서 기본 데이터 필요
        /// </summary>
        public PlayerStats DefaultStats()
        {
            PlayerStats defalutStats = new PlayerStats(1, CharacterClass.None, 0, 0, 0, 0, 0);

            defalutStats.CurrentExp = 0;
            defalutStats.MaxExp = int.MaxValue;

            return defalutStats;
        }
        /// <summary>
        /// 테스트용 Player Stats
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        public PlayerStats TestStats(CharacterClass classType)
        {
            switch (classType)
            {
                case CharacterClass.Axe:
                    return new PlayerStats(1, CharacterClass.Axe, 20, 30, 10, 3, 0.5f);// 도끼 클레스는 튼튼하고 강하지만 느림
                case CharacterClass.Sword:
                    return new PlayerStats(1, CharacterClass.Sword, 25, 20, 8, 6, 2);  // 검 클레스는 안정적이고 빠름
                case CharacterClass.Magic:
                    return new PlayerStats(1, CharacterClass.Magic, 15, 25, 5, 4, 1);  // 마법 클레스는 공격 사거리가 길지만 약함
                default:
                    return new PlayerStats(1, CharacterClass.Axe, 20, 30, 10, 3, 0.5f);// 도끼 클레스는 튼튼하고 강하지만 느림
            }
        }
        /// <summary>
        /// 클론 생성 Player Stats
        /// </summary>
        public PlayerStats Clone()
        {
            PlayerStats clone = new PlayerStats(
        Level,
        CharacterClass,
        GetBaseStat(UnitStaus.MaxHp),
        GetBaseStat(UnitStaus.Atk),
        GetBaseStat(UnitStaus.Dfs),
        GetBaseStat(UnitStaus.Spd),
        GetBaseStat(UnitStaus.AtkSpd)
        );

            clone.CurrentExp = this.CurrentExp;
            clone.MaxExp = this.MaxExp;
            return clone;
        }
        #endregion
        public PlayerStats() : base(0, 0, 0, 0, 0, 0) { }
        public PlayerStats(int level, CharacterClass charClass, float baseMaxHp, float baseAtk, float baseDfs, float baseSpd, float baseAtkSpd)
            : base(baseMaxHp, baseMaxHp, baseAtk, baseDfs, baseSpd, baseAtkSpd)
        {
            Level = level;
            CharacterClass = charClass;
        }

    }
}
