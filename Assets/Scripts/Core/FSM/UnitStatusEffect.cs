namespace ProjectPV.Core.FSM
{
    // 일단 생각 나는거 및 인터넷 찾아서 다 적음.
    public enum UnitStatusEffect
    {
        None,           // 기본 상태 (효과 없음)
        Stunned,        // 행동 불가
        Slowed,         // 이동 속도 감소
        Rooted,         // 이동 불가
        Silenced,       // 스킬 사용 불가
        Burning,        // 지속 데미지
        Poisoned,       // 지속 데미지 (중첩 가능)
        Frozen,         // 완전 정지
        Invincible,     // 무적
        Shielded,       // 방어막
        Taunted,        // 특정 타겟 강제 공격
        Feared,         // 무작위 이동
        Charmed         // 적을 아군처럼 행동
    }
}
