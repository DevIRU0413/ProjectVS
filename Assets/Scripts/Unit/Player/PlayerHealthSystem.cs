using System;

using UnityEngine;

namespace ProjectVS.Unit.Player
{
    public class PlayerHealthSystem : MonoBehaviour
    {
        private PlayerStats _stats;

        public event Action OnDeath;

        public void Init(PlayerStats stats)
        {
            _stats = stats;
            _stats.CurrentHp = _stats.CurrentMaxHp;
        }

        public void TakeDamage(float dmg)
        {
            float finalDmg = Mathf.Max(1f, dmg - _stats.CurrentDfs);
            _stats.CurrentHp -= finalDmg;

            if (_stats.CurrentHp <= 0)
            {
                _stats.CurrentHp = 0;
                OnDeath?.Invoke();
            }
        }

        public void Heal(float amount)
        {
            _stats.CurrentHp = Mathf.Min(_stats.CurrentHp + amount, _stats.CurrentMaxHp);
        }
    }

}
