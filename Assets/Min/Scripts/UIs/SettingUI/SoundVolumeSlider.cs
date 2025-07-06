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

        private void OnEnable()
        {
            _bgmSlider.value = AudioManager.Instance.GetBgmVolume();
            _sfxSlider.value = AudioManager.Instance.GetSfxVolume();
        }


        private void Update()
        {
            AudioManager.Instance.SetBgmVolume(_bgmSlider.value);
            AudioManager.Instance.SetSfxVolume(_sfxSlider.value);
        }
    }
}
