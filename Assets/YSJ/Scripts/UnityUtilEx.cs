﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Util
{
    public static class UnityUtilEx
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var comp = go.GetComponent<T>();
            if (comp == null)
                comp = go.AddComponent<T>();
            return comp;
        }

        public static GameObject FindOrCreateGameObject(string name) 
        {
            GameObject go = GameObject.Find(name);
            if (go == null)
                go = new GameObject(name);
            return go;
        }

        public static T FindNearestTarget<T>(this Transform origin, List<GameObject> targets, Func<GameObject, bool> condition = null) where T : Component
        {
            float minDist = float.MaxValue;
            GameObject nearest = null;
            Vector3 originPos = origin.position;

            foreach (var obj in targets)
            {
                if (obj == null) continue;

                var target = obj;
                if(condition(target) == false) continue;

                float dist = Vector2.Distance(originPos, target.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = target;
                }
            }

            return nearest?.GetComponent<T>();
        }
    }
}
