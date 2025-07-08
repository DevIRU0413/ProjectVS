using System.Collections.Generic;
using UnityEngine;
using ProjectVS.Interface;
using ProjectVS.Util;

namespace ProjectVS.Managers
{
    public class ManagerGroup : MonoBehaviour
    {
        #region Singleton
        private static ManagerGroup _instance;
        public static ManagerGroup Instance
        {
            get
            {
                if (_instance == null)
                {
                    string groupName = $"@{typeof(ManagerGroup).Name}";
                    GameObject go = GameObject.Find(groupName);
                    if (go == null)
                    {
                        go = new GameObject(groupName);
                        DontDestroyOnLoad(go);
                    }

                    _instance = go.GetOrAddComponent<ManagerGroup>();
                }

                return _instance;
            }
        }
        #endregion

        private List<IManager> _unregisteredManagers = new();
        private List<IManager> _registeredManagers = new();

        private bool _isManagersInitialized = false;

        public bool IsUseAble() => _isManagersInitialized;

        #region Register
        public void RegisterManager(IManager manager)
        {
            if (manager == null) return;

            System.Type type = manager.GetType();
            if (ContainsManagerOfType(type))
            {
                Debug.LogWarning($"[RegisterManager] 이미 {type.Name} 이 존재하여 등록되지 않습니다.");
                return;
            }

            _unregisteredManagers.Add(manager);
        }

        public void RegisterManager(GameObject managerObject)
        {
            if (managerObject == null) return;

            IManager newManager = managerObject.GetComponent<IManager>();
            if (newManager == null) return;

            System.Type type = newManager.GetType();

            if (ContainsManagerOfType(type))
            {
                Debug.LogWarning($"[RegisterManager] 이미 {type.Name} 이 존재하여 새 오브젝트는 삭제됩니다.");
                Destroy(managerObject);
                return;
            }

            _unregisteredManagers.Add(newManager);
        }

        public void RegisterManager(params IManager[] managers)
        {
            foreach (IManager m in managers)
                RegisterManager(m);
        }

        public void RegisterManager(params GameObject[] managerObjects)
        {
            foreach (GameObject go in managerObjects)
                RegisterManager(go);
        }
        #endregion

        #region Init & Cleanup
        public void InitializeManagers()
        {
            _isManagersInitialized = false;
            SortManagersByPriorityAscending(_unregisteredManagers);

            foreach (var manager in _unregisteredManagers)
            {
                manager.Initialize();
                GameObject go = manager.GetGameObject();
                if (go == null)
                {
                    Debug.LogError($"[Dnot Init] {manager.GetType().Name} 의 GameObject가 null입니다.");
                    continue;
                }

                Debug.Log($"[Init] {go.name}");
                _registeredManagers.Add(manager);
                go.transform.parent = transform;
            }

            _unregisteredManagers.Clear();
            _isManagersInitialized = true;
        }

        public void CleanupManagers()
        {
            for (int i = _registeredManagers.Count - 1; i >= 0; i--)
            {
                IManager manager = _registeredManagers[i];
                GameObject go = manager.GetGameObject();

                if (go == null)
                {
                    _registeredManagers.RemoveAt(i);
                    continue;
                }

                manager.Cleanup();
                Debug.Log($"[Cleanup] {go.name}");
            }
        }

        public void ClearManagers(bool forceClear = false)
        {
            for (int i = _registeredManagers.Count - 1; i >= 0; i--)
            {
                IManager manager = _registeredManagers[i];
                if (!manager.IsDontDestroy || forceClear)
                {
                    GameObject go = manager.GetGameObject();
                    if (go == null)
                    {
                        _registeredManagers.RemoveAt(i);
                        continue;
                    }

                    manager.Cleanup();
                    string name = go.name;
                    Destroy(go);
                    Debug.Log($"[Clear] {name}");
                }
            }
        }

        public void ClearAllManagers()
        {
            ClearManagers(true);
        }
        #endregion

        #region Utils
        private bool ContainsManagerOfType(System.Type type)
        {
            foreach (var m in _registeredManagers)
                if (m.GetType() == type) return true;

            foreach (var m in _unregisteredManagers)
                if (m.GetType() == type) return true;

            return false;
        }

        public T GetManager<T>() where T : class, IManager
        {
            foreach (var m in _registeredManagers)
                if (m is T t) return t;

            foreach (var m in _unregisteredManagers)
                if (m is T t) return t;

            return null;
        }

        private void SortManagersByPriorityAscending(List<IManager> list)
        {
            list.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }

        private void SortManagersByPriorityDescending(List<IManager> list)
        {
            list.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }
        #endregion
    }
}
