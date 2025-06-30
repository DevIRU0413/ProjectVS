namespace ProjectVS
{
    #region GameManager Enums

    public enum GamePlayType
    {
        Build,
        Test,
    }

    public enum GameState
    {
        Play,   // 플레이
        Pause,  // 일시 정지
    }
    public enum SceneID
    {
        None = 0,
        PrologScene,
        MainMenuScene,
        InGameScene,
        StoreScene,
        LoadingScene,
    }

    #endregion

    #region Stage Enums
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

    #endregion

    public enum UnitStaus
    {
        MaxHp,
        Atk,
        Dfs,
        Spd,
        AtkSpd,
    }

    #region Player Enums
    public enum PlayerStateType
    {
        Idle,
        Move,
        Win,
        Death,
    }

    // 플레이어가 선택 가능한 클레스
    public enum CharacterClass
    {
        None,   // None 일때, 클래스 선택 씬으로 전환 예정
        Sword,  // 검
        Axe,    // 도끼
        Magic   // 마법
    }

    #endregion

    #region Monster Enums
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
    #endregion
}
