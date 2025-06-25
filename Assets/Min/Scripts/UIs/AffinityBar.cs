using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using NPCAffinityModelClass = ProjectVS.Shop.NPCAffinityModel.NPCAffinityModel;


namespace ProjectVS.UIs.AffinityBar
{
    public class AffinityBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private NPCAffinityModelClass _npcAffinityModel;
        [SerializeField] private TMP_Text _affinityText;


        private void Awake()
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
            }
            if (_affinityText == null)
            {
                _affinityText = GetComponentInChildren<TMP_Text>();
            }
        }

        private void OnEnable()
        {
            _slider.value = _npcAffinityModel.Affinity;
            _affinityText.text = $"호감도: {_npcAffinityModel.Affinity} / 100";
        }
    }
}
