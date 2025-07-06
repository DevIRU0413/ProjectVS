using System.Collections;
using System.Collections.Generic;

using ProjectVS.Manager;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.Uis.Sound.ButtonSoundMaker
{
    public class UIButtonSFXBinder : MonoBehaviour
    {
        [SerializeField] private AudioClip _clickSound;
        [SerializeField] private Button _button;


        private void Awake()
        {
            if (_clickSound == null)
                Debug.LogWarning("[UIButtonSFXBinder] AudioClip이 없습니다");

            if (_button == null)
                _button = GetComponent<Button>();
            if (_button != null)
                _button.onClick.AddListener(PlayClickSound);
        }

        private void PlayClickSound()
        {
            AudioManager.Instance.PlaySfx(_clickSound);
        }
    }
}
