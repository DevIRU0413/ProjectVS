using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;
using StandingToFullBodyShotPairClass = ProjectVS.CharacterImages.StandingToFullBodyShotPair.StandingToFullBodyShotPair;


namespace ProjectVS.CharacterImages
{
    public class FullBodyShotToggleManager : MonoBehaviour
    {
        [SerializeField] private List<StandingToFullBodyShotPairClass> _standingToFullBodyShotPairs;
        [SerializeField] private Transform _uiRoot; // 사용하는 UI Canvas

        private Dictionary<string, GameObject> _instantiatedPanels = new();

        private void Start()
        {
            foreach (var pair in _standingToFullBodyShotPairs)
            {
                pair.StandingShotButton.onClick.AddListener(() =>
                {
                    string panelKey = pair.StandingShotButton.name;

                    if (_instantiatedPanels.ContainsKey(panelKey))
                    {
                        UIManager.Instance.Show(panelKey);
                    }
                    else
                    {
                        GameObject panel = Instantiate(pair.FullBodyShotPrefab, _uiRoot);
                        _instantiatedPanels[panelKey] = panel;
                        UIManager.Instance.RegisterPanel(panelKey, panel);
                        UIManager.Instance.Show(panelKey);
                    }
                });
            }
        }
    }
}

