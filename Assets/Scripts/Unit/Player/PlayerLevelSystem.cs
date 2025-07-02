using System;

using UnityEngine;

namespace ProjectVS.Unit.Player
{
    public class PlayerLevelSystem : MonoBehaviour
    {
        private PlayerStats _stats;

        public event Action<int> OnLevelUp;

        public void Init(PlayerStats stats)
        {
            _stats = stats;
        }

        public void AddExp(float amount)
        {
            _stats.CurrentExp += amount;
            while (_stats.CurrentExp >= _stats.MaxExp)
            {
                _stats.CurrentExp -= _stats.MaxExp;
                _stats.Level++;
                _stats.MaxExp *= 1.2f;

                ApplyLevelGrowth(_stats.CharacterClass);
                OnLevelUp?.Invoke(_stats.Level);
            }
        }

        private void ApplyLevelGrowth(CharacterClass classType)
        {
            switch (classType)
            {
                case CharacterClass.Axe:
                    _stats.SetIncreaseBaseStats(UnitStaus.MaxHp, 10);
                    _stats.SetIncreaseBaseStats(UnitStaus.Atk, 8);
                    break;
            }
        }
    }

}
