namespace ProjectVS
{
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

    // 플레이어가 선택 가능한 클레스
    public enum CharacterClass
    {
        Sword,  // 검
        Axe,    // 도끼
        Magic   // 마법
    }
}
