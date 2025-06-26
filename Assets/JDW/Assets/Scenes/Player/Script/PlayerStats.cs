using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterClass
{// 플레이어가 선택 가능한 클레스
    Sword, // 검
    Axe, // 도끼
    Magic // 마법
}
    [System.Serializable]
    public class PlayerStats
    {
        public float Health;      //체력
        public float Attack;      //공격력
        public float Defense;     //방어력
        public float MoveSpeed;   //이동속도
        public float AttackSpeed; //공격속도

        //플레이어를 생성할 때 사용할 수 있도록 값을 받음
        public PlayerStats(float health, float attack, float defense, float moveSpeed, float attackSpeed)
        {
            Health = health;
            Attack = attack;
            Defense = defense;
            MoveSpeed = moveSpeed;
            AttackSpeed = attackSpeed;
        }
        public PlayerStats Clone()
        {//원본 값이 수정되지 않도록 클론을 이용
            return new PlayerStats(Health, Attack, Defense, MoveSpeed, AttackSpeed);
        }
    }
    public static class PlayerClassData
    {// 클레스별 초기 스탯을 설정해 놓은 보관함
        public static readonly Dictionary<CharacterClass, PlayerStats> DefaultStats = new Dictionary<CharacterClass, PlayerStats>
        {
            {CharacterClass.Axe, new PlayerStats(20,30,10,3,1) }, // 도끼 클레스는 튼튼하고 강하지만 느림
            {CharacterClass.Sword, new PlayerStats(25,20,8,6,3) },// 검 클레스는 안정적이고 빠름
            {CharacterClass.Magic, new PlayerStats(15,25,5,4,2) } // 마법 클레스는 공격 사거리가 길지만 약함
        };
    }

