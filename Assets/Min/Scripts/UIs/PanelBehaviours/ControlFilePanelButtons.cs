using System;
using System.Collections;
using System.Collections.Generic;

using ProjectVS.Manager;
using ProjectVS.Utils.UIManager;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.UIs.PanelBehaviours.ControlFilePanelButtons
{
    // TODO: PlayerDataManager 관련 주석 해제
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

        private int _currentFileIndex = -1;


        private void OnEnable()
        {
            _isNewButtonToggled = false;
            _isLoadButtonToggled = false;
            _isDeleteButtonToggled = false;
            RenewButtonColor();

            _currentFileIndex = -1;
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

            // 캐릭터 팝업 띄어주기
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
            CheckWhatButtonToggled();
            _currentFileIndex = 1;
        }

        public void OnClickFile2Button()
        {
            CheckWhatButtonToggled();

            _currentFileIndex = 2;
        }

        public void OnClickFile3Button()
        {
            CheckWhatButtonToggled();

            _currentFileIndex = 3;
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
        private void CheckWhatButtonToggled()
        {
            if (_isNewButtonToggled)
            {
                //if (PlayerDataManager.ForceInstance.CheckPlayerData(_currentFileIndex)) return;

                // 새 파일 생성 - 인덱스로
                PlayerDataManager.ForceInstance.SavePlayerData(_currentFileIndex);

                UIManager.Instance.Hide("Control File Panel");
                UIManager.Instance.Show("Character Select Panel");
            }
            if (_isLoadButtonToggled)
            {
                //if (!PlayerDataManager.ForceInstance.CheckPlayerData(_currentFileIndex)) return;

                // 로드 파일 - 인덱스로
                PlayerDataManager.ForceInstance.LoadPlayerData(_currentFileIndex);
                SceneLoader.Instance.LoadSceneAsync(SceneID.InGameScene);
                Debug.Log("로드");
            }
            if (_isDeleteButtonToggled)
            {
                //if (!PlayerDataManager.ForceInstance.CheckPlayerData(_currentFileIndex)) return;


                UIManager.Instance.Show("Delete Check Panel");

                Debug.Log($"[ControlFilePanelButtons] _isDeleteButtonToggled");

                // 삭제 파일 - 인덱스로
                PlayerDataManager.ForceInstance.DeletePlayerData(_currentFileIndex);
                Debug.Log("삭제");
            }
        }

        public void OnClickYesButton()
        {
            //if (!PlayerDataManager.ForceInstance.CheckPlayerData(_currentFileIndex)) return;

            UIManager.Instance.CloseTopPanel();

            _currentFileIndex = -1;
        }

        public void OnClickNoButton()
        {
            UIManager.Instance.CloseTopPanel();

            _isNewButtonToggled = false;
            _isLoadButtonToggled = false;
            _isDeleteButtonToggled = false;

            RenewButtonColor();

            _currentFileIndex = -1;
        }
    }
}
