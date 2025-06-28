namespace ProjectVS
{
    public enum StageFlowState
    {
        None = 0,
        Enter,      // 인게임 진입
        Play,       // 정상적인 플레이
        Paused,     // 일시 정지
        Exit,       // 인게임 종료
    }

    public enum StageResult
    {
        None = 0,
        Win,
        Lose,
        Draw,    // 선택
        Abort    // 선택
    }

    public enum PlayerStateType
    {
        Idle,
        Move,
        Win,
        Death,
    }

    public enum MonsterStateType
    {
        None,
        Idle,
        Move,
        Win,
        Death,
    }

    public enum MonsterPatternState
    {
        None,
        Play,
        Done,
    }

    public enum UnitStaus
    {
        MaxHp,
        Atk,
        Dfs,
        Spd,
        AtkSpd,
    }


    // 플레이어가 선택 가능한 클레스
    public enum CharacterClass
    {
        Sword,  // 검
        Axe,    // 도끼
        Magic   // 마법
    }
}
