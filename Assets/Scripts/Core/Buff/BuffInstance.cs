using System.Collections.Generic;

using ProjectPV.Core.Stat;
using ProjectPV.Definitions.Buff;

using ProjectVS.Core.Stat;

using UnityEngine;

namespace ProjectVS.Core.Buff
{
    public class BuffInstance : IStatProvider
    {
        public readonly BuffData Data;
        public int Stack { get; private set; }
        public float RemainingTime { get; private set; }

        public string Id => $"buff:{Data.ID}";

        public BuffInstance(BuffData data)
        {
            Data = data;
            Stack = 1;
            RemainingTime = data.Duration;
        }

        public void Tick(float dt)
        {
            RemainingTime -= dt;
        }

        public bool IsExpired => RemainingTime <= 0f;

        public void RefreshOrStack()
        {
            if (Data.IsStackable)
                Stack++;
            else
                RemainingTime = Data.Duration;
        }

        public IEnumerable<StatModifier> GetModifiers()
        {
            foreach (var mod in Data.Modifiers)
            {
                yield return new StatModifier(
                    mod.StatType,
                    mod.Additive * Stack,
                    Mathf.Pow(mod.Multiplier, Stack)
                );
            }
        }
    }
}
