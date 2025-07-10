using ProjectPV.Definitions.Buff;

using UnityEngine;

namespace ProjectPV.Core.Buff
{
    public class BuffInstance
    {
        public BuffData Data { get; private set; }
        public float RemainingTime { get; private set; }
        public int Stack { get; private set; } = 1;

        public string SourceId => Data.id;
        public bool IsStackable => Data.isStackable;
        public bool IsExpired => Data.duration > 0 && RemainingTime <= 0;

        public BuffInstance(BuffData data)
        {
            Data = data;
            RemainingTime = data.duration;
        }

        /// <summary>
        /// 타이머 감소 처리
        /// </summary>
        public void Tick(float deltaTime)
        {
            if (Data.duration <= 0) return; // 무한 지속
            RemainingTime -= deltaTime;
        }

        /// <summary>
        /// 버프 중첩 시 처리 (스택 증가 또는 리셋)
        /// </summary>
        public void RefreshOrStack()
        {
            if (IsStackable)
            {
                if (Stack < Data.maxStack)
                    Stack++;
            }

            RemainingTime = Data.duration;
        }

        /// <summary>
        /// 강제로 스택 초기화
        /// </summary>
        public void ResetStack()
        {
            Stack = 1;
            RemainingTime = Data.duration;
        }

        /// <summary>
        /// 해당 스택 수에 따른 가중치 반환 (예: Tick 데미지 등)
        /// </summary>
        public float GetStackMultiplier()
        {
            return Stack;
        }
    }
}
