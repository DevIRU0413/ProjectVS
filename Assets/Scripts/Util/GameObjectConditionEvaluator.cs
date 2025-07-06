using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace ProjectVS.Util
{
    public class GameObjectConditionEvaluator
    {
        private readonly List<Func<GameObject, bool>> _conditions = new();

        public void Add(Func<GameObject, bool> condition)
        {
            if (_conditions.Contains(condition)) return;
            _conditions.Add(condition);
        }

        public void Clear() => _conditions.Clear();

        public bool EvaluateAll(GameObject go) => _conditions.All(cond => cond(go));

        public bool EvaluateAny(GameObject go) => _conditions.Any(cond => cond(go));
    }
}
