using ProjectPV.Definitions.Buff;

using UnityEngine;

namespace ProjectPV.Core.Buff
{
    public class BuffInstance
    {
        public BuffData data;
        public float remainingTime;
        public string sourceId => data.id;

        public BuffInstance(BuffData data)
        {
            this.data = data;
            this.remainingTime = data.duration;
        }

        public void Tick(float deltaTime)
        {
            if (data.duration <= 0) return; // 무한 지속
            remainingTime -= deltaTime;
        }

        public bool IsExpired => data.duration > 0 && remainingTime <= 0;
    }
}
