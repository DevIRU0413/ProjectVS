using System.Collections;
using System.Collections.Generic;

using ProjectVS.Manager;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.UIs.SettingUI.SoundVolumeSlider
{
    public class SoundVolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _bgmSlider;


        private void Update()
        {
            AudioManager.Instance.SetBgmVolume(_bgmSlider.value);
            AudioManager.Instance.SetSfxVolume(_sfxSlider.value);
        }
    }
}
