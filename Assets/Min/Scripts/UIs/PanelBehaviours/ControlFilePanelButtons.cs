using System.Collections;
using System.Collections.Generic;

using ProjectVS.Manager;
using ProjectVS.Utils.UIManager;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.UIs.PanelBehaviours.ControlFilePanelButtons
{
    public class ControlFilePanelButtons : MonoBehaviour
    {
        [Header("선택 버튼")]
        [SerializeField] private Button _newButton;
        [SerializeField] private Button _loadButton;
        [SerializeField] private Button _deleteButton;

        [Header("색 설정")]
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _toggledColor = new Color(0.5f, 1f, 0.5f, 1f); // 연두색

        private bool _isNewButtonToggled = false;
        private bool _isLoadButtonToggled = false;
        private bool _isDeleteButtonToggled = false;


        private void OnEnable()
        {
            _isNewButtonToggled = false;
            _isLoadButtonToggled = false;
            _isDeleteButtonToggled = false;
            RenewButtonColor();
        }

        public void OnClickNewButton()
        {
            if (_isNewButtonToggled)
            {
                _isNewButtonToggled = false;
                RenewButtonColor();
                return;
            }

            _isNewButtonToggled = true;
            _isLoadButtonToggled = false;
            _isDeleteButtonToggled = false;


            RenewButtonColor();

            PlayerDataManager.ForceInstance.SavePlayerData(); // 이건 어떻게 동작하는지 확인해봐야될 듯
        }

        public void OnClickLoadButton()
        {
            if (_isLoadButtonToggled)
            {
                _isLoadButtonToggled = false;
                RenewButtonColor();
                return;
            }

            _isNewButtonToggled = false;
            _isLoadButtonToggled = true;
            _isDeleteButtonToggled = false;

            RenewButtonColor();

            PlayerDataManager.ForceInstance.LoadPlayerData(); // 이건 어떻게 동작하는지 확인해봐야될 듯
        }

        public void OnClickDeleteButton()
        {
            if (_isDeleteButtonToggled)
            {
                _isDeleteButtonToggled = false;
                RenewButtonColor();
                return;
            }

            _isNewButtonToggled = false;
            _isLoadButtonToggled = false;
            _isDeleteButtonToggled = true;

            RenewButtonColor();
        }

        public void OnClickFile1Button()
        {
            CheckWhatButtonToggled(1);
        }

        public void OnClickFile2Button()
        {
            CheckWhatButtonToggled(2);
        }

        public void OnClickFile3Button()
        {
            CheckWhatButtonToggled(3);
        }

        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }


        // 토글된 버튼 색상 피드백
        private void RenewButtonColor()
        {
            _newButton.image.color = _defaultColor;
            _loadButton.image.color = _defaultColor;
            _deleteButton.image.color = _defaultColor;

            if (_isNewButtonToggled) _newButton.image.color = _toggledColor;
            if (_isLoadButtonToggled) _loadButton.image.color = _toggledColor;
            if (_isDeleteButtonToggled) _deleteButton.image.color = _toggledColor;
        }


        // 선택된 버튼에 따라 파일을 확인하고 작업 수행
        private void CheckWhatButtonToggled(int index)
        {
            if (_isNewButtonToggled)
            {
                //if (인덱스에 파일이 있으면) return;

                // 새 파일 생성 - 인덱스로

                UIManager.Instance.Hide("Control File Panel");
                UIManager.Instance.Show("Character Select Panel");
            }
            if (_isLoadButtonToggled)
            {
                //if (인덱스에 파일이 없으면) return;

                // 로드 파일 - 인덱스로

                SceneLoader.Instance.LoadSceneAsync(SceneID.InGameScene);
            }
            if (_isDeleteButtonToggled)
            {
                //if (인덱스에 파일이 없으면) return;

                // 삭제 파일 - 인덱스로
            }
        }
    }
}
