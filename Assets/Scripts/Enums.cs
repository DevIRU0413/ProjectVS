namespace ProjectVS
{
    public enum ManagerPriority
    {
        None,                   // Error

        GameManager,

        ResourceManager,        // Default Manager
        SceneManagerEx,
        AudioManager,
        UIManager,

        PlayerDataManager,      // Data Manager 
        ItemManager,
        NPCAffinityModel,
        DialogueManager,
        StageManager,
    }

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
        Pause,     // 일시 정지
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

    public enum SpawnGroupType
    {
        None = 0,
        Radius,     // 반지름 거리에 소환
        Line,       // 일짜로 쭉 소환
        Circle,     // 원으로 쭉 소환
        Grid,       // 격자
        PureBoid,   // 무리 지어 소환
    }

    public enum UnitStaus
    {
        MaxHp,
        Atk,
        Dfs,
        Spd,
        AtkSpd,
    }

    #region Item Enums
    /*public enum ItemRank
    {
        Sub = 0,         // 서브 아이템: 기본 재료 또는 일반 등급 아이템 (예: 쿠나이, 활 등)
        Anvil = 1,       // 강화 아이템: 아이템 강화에 사용 (예: 모루)
        Unique = 2,      // 고유 아이템: 특별한 효과를 가진 독립 아이템 (예: 검, 도끼, 지팡이)
        Composite = 3    // 조합 아이템: 서브 아이템을 조합해서 생성된 상위 아이템 (예: 연노, 인법첩 등)
    }*/

    public enum ItemType
    {
        Money = 0,      // 돈
        Enforce = 1,    // 강화 관련
        Potion = 2,     // 물약
        Attack = 3,     // 공격
        Passive = 4     // 패시브
    }

    public enum ItemEffect
    {
        None = 0,          // 아무 효과 없음 (골드, 다이아 등 재화)
        Restore = 1,       // 체력 회복 (포션 등) - n만큼 체력 회복
        Swing = 2,         // 근접 휘두르기 공격 (검, 도끼) - 마우스 방향으로 휘두르기
        Shot = 3,          // 직선 투사체 발사 (지팡이) - 마우스 방향으로 n 데미지 투사체
        RandomShot = 4,    // 랜덤 방향 투사체 (활, 레이저) - 주변에 무작위 발사
        Spin = 5,          // 회전형 근거리 범위 공격 (단검, 수리검, 화염구 등) - 캐릭터 중심으로 회전 데미지
        HPStatUP = 6,      // 체력 스탯 상승 (투구류, 마법로브 등) - 최대 체력 증가
        DefStatUP = 7,     // 방어력 상승 (체인메일, 로브 등) - 방어력 증가
        SpeedStatUP = 8,   // 이동 속도 상승 (부츠류 등) - 스피드 증가
        ThreeShot = 9,     // 삼점사 (연노) - 전방 3방향 투사체 발사
        Tornado = 10,      // 회오리 범위 피해 (인법첩) - 직선 회오리 공격
        Throw = 11,        // 투척 공격 (마름쇠) - 주변 랜덤 위치에 범위 공격
        Fiveway = 12,      // 5방향 투사체 (불대문자) - 전방 5갈래 투사체
        Bomb = 13,         // 폭발형 투사체 (기폭찰) - 명중 시 폭발 범위 피해
        Double = 14,       // 2히트 효과 (쌍검) - 한 발에 2번 피해 적용
        Eightway = 15      // 8방향 발사 (쿠나이 등) - 8갈래 전방위 투사체
    }

    public enum ItemSetEffect
    {
        None = 0,         // 세트 효과 없음

        TheftSet = 1,     // 도적 세트: 이동 속도 up, 공격 속도 down, 방어력 down
                          // 구성: 머리띠, 레더 아머, 가죽 부츠, 단검

        MagicianSet = 2,  // 마법사 세트: 공격 속도 down, 공격력 up
                          // 구성: 꼬깔 모자, 마법 로브, 마법 부츠, 화염구

        RobinSet = 3,     // 로빈 후드 세트: 투사체 크기 증가
                          // 구성: 후드, 로브, 암살 부츠, 연노

        NinjaSet = 4,     // 닌자 세트: 투사체 수 +1
                          // 구성: 마스크, 암살 로브, 암살 부츠, 인법첩

        KnightSet = 5     // 기사 세트: 방어력 up, 이동 속도 down
                          // 구성: 배틀 헬름, 판금 갑옷, 배틀 부츠
    }
    #endregion

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
