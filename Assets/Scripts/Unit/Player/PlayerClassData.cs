using System.Collections.Generic;

namespace ProjectVS
{
    public static class PlayerClassData
    {
        // 클레스별 초기 스탯을 설정해 놓은 보관함
        public static readonly Dictionary<CharacterClass, PlayerStats> DefaultStats = new Dictionary<CharacterClass, PlayerStats>
        {
            {CharacterClass.Axe, new PlayerStats(1, CharacterClass.Axe, 20,30,10,3,0.5f) }, // 도끼 클레스는 튼튼하고 강하지만 느림
            {CharacterClass.Sword, new PlayerStats(1, CharacterClass.Sword, 25,20,8,6,2) },// 검 클레스는 안정적이고 빠름
            {CharacterClass.Magic, new PlayerStats(1, CharacterClass.Magic, 15,25,5,4,1) } // 마법 클레스는 공격 사거리가 길지만 약함
        };
    }
}
