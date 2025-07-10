using ProjectPV.Definitions.Buff;
using ProjectVS.Core.Buff;
using System.Collections.Generic;

public class BuffSystem
{
    private readonly UnitStats _unitStat;
    private readonly Dictionary<string, BuffInstance> _activeBuffs = new();

    public BuffSystem(UnitStats stat) => _unitStat = stat;

    public void AddBuff(BuffData data)
    {
        if (_activeBuffs.TryGetValue(data.ID, out var existing))
        {
            existing.RefreshOrStack();
            return;
        }

        var instance = new BuffInstance(data);
        _activeBuffs[instance.Id] = instance;
        _unitStat.AddProvider(instance);
    }

    public void RemoveBuff(string buffId)
    {
        string fullId = $"buff:{buffId}";
        if (_activeBuffs.Remove(buffId))
            _unitStat.RemoveProvider(fullId);
    }

    public void Tick(float dt)
    {
        List<string> expired = null;

        foreach (var kv in _activeBuffs)
        {
            kv.Value.Tick(dt);
            if (kv.Value.IsExpired)
            {
                expired ??= new List<string>();
                expired.Add(kv.Key);
            }
        }

        if (expired != null)
        {
            foreach (var id in expired)
                RemoveBuff(id);
        }
    }
}
