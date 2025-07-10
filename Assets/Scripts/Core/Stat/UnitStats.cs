using ProjectPV.Core.Stat;
using ProjectVS.Core.Stat;
using System.Collections.Generic;

using UnityEngine;

public class UnitStats
{
    private float _currentHp;

    private readonly Dictionary<UnitStatType, float> _baseStats = new();
    private readonly Dictionary<string, IStatProvider> _providers = new();

    private readonly Dictionary<UnitStatType, float> _additiveCache = new();
    private readonly Dictionary<UnitStatType, float> _multiplierCache = new();

    public float CurrentHp
    {
        get => _currentHp;
        set => _currentHp = Mathf.Clamp(value, 0, GetFinalStat(UnitStatType.MaxHp));
    }

    public void SetBaseStat(UnitStatType type, float value) => _baseStats[type] = value;
    public float GetBaseStat(UnitStatType type) => _baseStats.TryGetValue(type, out var val) ? val : 0f;

    public void AddProvider(IStatProvider provider)
    {
        _providers[provider.Id] = provider;
        Recalculate();
    }

    public void RemoveProvider(string id)
    {
        if (_providers.Remove(id))
            Recalculate();
    }

    public void ClearProviders()
    {
        _providers.Clear();
        Recalculate();
    }

    public float GetFinalStat(UnitStatType type)
    {
        float baseValue = GetBaseStat(type);
        float additive = _additiveCache.TryGetValue(type, out var a) ? a : 0f;
        float multiplier = _multiplierCache.TryGetValue(type, out var m) ? m : 1f;
        return (baseValue + additive) * multiplier;
    }

    private void Recalculate()
    {
        _additiveCache.Clear();
        _multiplierCache.Clear();

        foreach (var provider in _providers.Values)
        {
            foreach (var mod in provider.GetModifiers())
            {
                if (!_additiveCache.ContainsKey(mod.StatType))
                    _additiveCache[mod.StatType] = 0f;

                if (!_multiplierCache.ContainsKey(mod.StatType))
                    _multiplierCache[mod.StatType] = 1f;

                _additiveCache[mod.StatType] += mod.Additive;
                _multiplierCache[mod.StatType] *= mod.Multiplier;
            }
        }
    }
}
