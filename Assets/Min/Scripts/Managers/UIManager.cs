using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using ProjectVS.Utils.PriorityQueue;
using ProjectVS.Utils.Singleton;
using ProjectVS.UIs.UIBase;
using ProjectVS.Util;
using ProjectVS.Interface;


namespace ProjectVS.Utils.UIManager
{
    public class UIManager : SimpleSingleton<UIManager>, IManager
    {
        private Dictionary<string, GameObject> _uiPanels = new();
        private Stack<GameObject> _uiStack = new();

        private PriorityQueue<Action> _animationQueue = new();
        private bool _isAnimating = false;

        public int PanelCount => _uiPanels.Count;
        public int Priority => (int)ManagerPriority.UIManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        private void Update()
        {
            if (_isAnimating) return;

            // TODO: 플레이어 컨트롤러로 이관해야될 수도

            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (_uiStack.Count == 0)
                {
                    Show("Pause Buttons Panel");
                    return;
                }

                if (_uiStack.Count == 1)
                {
                    GameObject topPanel = _uiStack.Peek();
                    if (topPanel.name == "Main Menu Panel")
                    {
                        return;
                    }
                }

                Debug.Log($"[UIManager] Escape 키 입력으로 최상위 패널 닫음: {_uiStack.Peek().name}");
                ForceCloseTopPanel();
            }
        }

        public void RegisterPanel(string key, GameObject panel)
        {
            if (!_uiPanels.ContainsKey(key))
            {
                _uiPanels.Add(key, panel);
            }
        }

        private void OnAnimationComplete()
        {
            _isAnimating = false;

            if (_animationQueue.Count > 0)
            {
                _isAnimating = true;
                _animationQueue.Dequeue().Invoke();
            }
        }

        private void Enqueue(Action action, int priority)
        {
            if (_isAnimating)
            {
                _animationQueue.Enqueue(action, priority);
            }
            else
            {
                _isAnimating = true;
                action.Invoke();
            }
        }

        public void Show(string key)
        {
            if (_uiPanels.TryGetValue(key, out GameObject panel))
            {

                Enqueue(() =>
                {
                    var uiBase = panel.GetComponent<UIBase>();
                    if (uiBase != null) uiBase.AnimateShow(OnAnimationComplete);
                    else { panel.SetActive(true); OnAnimationComplete(); }

                    if (!_uiStack.Contains(panel))
                    {
                        _uiStack.Push(panel);
                    }
                }, priority: 1);
            }
        }

        // 이미지 패널용 오버로딩
        public void Show(GameObject panel, string key = null)
        {
            if (panel == null) return;

            string panelKey = key ?? panel.name;

            if (!_uiPanels.ContainsKey(panelKey))
            {
                RegisterPanel(panelKey, panel);
            }

            Show(panelKey);
        }

        public void Hide(string key)
        {
            if (_uiPanels.TryGetValue(key, out GameObject panel))
            {
                if (!panel.activeSelf)
                {
                    Debug.Log($"[UIManager] {panel.name}이 비활성화 되어있어 Hide 메서드 실행 건너뜀.");
                    return;
                }

                Enqueue(() =>
                {
                    var uiBase = panel.GetComponent<UIBase>();
                    if (uiBase != null) uiBase.AnimateHide(OnAnimationComplete);
                    else { panel.SetActive(false); OnAnimationComplete(); }
                }, priority: 0);
            }
        }

        // 이미지 패널용 오버로딩
        public void Hide(GameObject panel, string key = null)
        {
            if (panel == null) return;
            if (!panel.activeSelf)
            {
                Debug.Log($"[UIManager] {panel.name}이 비활성화 되어있어 Hide 메서드 실행 건너뜀.");
                return;
            }


            string panelKey = key ?? panel.name;

            if (_uiPanels.ContainsKey(panelKey))
            {
                Hide(panelKey);
            }
        }

        public void CloseTopPanel()
        {
            if (_uiStack.Count <= 1) return;

            GameObject topPanel = _uiStack.Pop();
            GameObject previousPanel = _uiStack.Peek();

            Enqueue(() =>
            {
                var topUI = topPanel.GetComponent<UIBase>();
                var prevUI = previousPanel.GetComponent<UIBase>();

                void OnCloseComplete()
                {
                    if (prevUI != null) prevUI.AnimateShow(OnAnimationComplete);
                    else { previousPanel.SetActive(true); OnAnimationComplete(); }
                }

                if (topUI != null) topUI.AnimateHide(OnCloseComplete);
                else { topPanel.SetActive(false); OnCloseComplete(); }
            }, priority: 0);
        }


        // 패널을 아예 닫아야될 때 사용하는 메서드
        public void ForceCloseTopPanel()
        {
            if (_uiStack.Count == 0)
            {
                Debug.LogWarning("[UIManager] 스택에 패널이 없습니다");
                return;
            }

            GameObject topPanel = _uiStack.Pop();
            GameObject previousPanel = _uiStack.Count > 0 ? _uiStack.Peek() : null;

            Enqueue(() =>
            {
                var topUI = topPanel.GetComponent<UIBase>();
                var prevUI = previousPanel?.GetComponent<UIBase>();

                void OnCloseComplete()
                {
                    if (previousPanel != null)
                    {
                        if (prevUI != null) prevUI.AnimateShow(OnAnimationComplete);
                        else { previousPanel.SetActive(true); OnAnimationComplete(); }
                    }
                    else
                    {
                        OnAnimationComplete(); // 이전 패널이 없으면 그냥 끝
                    }
                }

                if (topUI != null) topUI.AnimateHide(OnCloseComplete);
                else { topPanel.SetActive(false); OnCloseComplete(); }
            }, priority: 0);
        }


        public void ShowAsFirst(string key)
        {
            if (_uiPanels.TryGetValue(key, out GameObject panel))
            {
                _uiStack.Clear();
                var uiBase = panel.GetComponent<UIBase>();
                panel.SetActive(true);
                uiBase?.ShowImmediately();
                _uiStack.Push(panel);
            }
        }

        // 씬 이동 시 호출해야될 메서드
        public void ClearAll()
        {
            foreach (var panel in _uiPanels.Values)
            {
                var uiBase = panel.GetComponent<UIBase>();
                if (uiBase != null) uiBase.HideImmediately();
                else panel.SetActive(false);
            }

            _uiStack.Clear();
            _animationQueue.Clear();
            _isAnimating = false;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Cleanup()
        {
            throw new NotImplementedException();
        }

        public GameObject GetGameObject()
        {
            throw new NotImplementedException();
        }
    }
}
