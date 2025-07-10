using System.Collections.Generic;

using ProjectPV.Core.Stat;

namespace ProjectVS.Core.Stat
{
    public interface IStatProvider
    {
        /// <summary> 이 프로바이더의 고유 ID (예: "equipment:sword001") </summary>
        string Id { get; }

        /// <summary> 제공하는 스탯 보정 목록 </summary>
        IEnumerable<StatModifier> GetModifiers();
    }
}
