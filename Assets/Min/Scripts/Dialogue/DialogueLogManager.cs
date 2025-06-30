using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


using LogBoxClass = ProjectVS.Dialogue.LogBox.LogBox;


namespace ProjectVS.Dialogue.DialogueLogManager
{
    public class DialogueLogManager : MonoBehaviour
    {
        [SerializeField] private Transform _logContentTransform;
        [SerializeField] private LogBoxClass _logBoxPrefab;

        private List<LogBoxClass> _logBoxes = new();

        // TODO: 오브젝트 풀링 등 해야될 듯
        public void AddLogBox(string name, string content)
        {
            LogBoxClass box = Instantiate(_logBoxPrefab, _logContentTransform);
            box.NameText.text = name;
            box.ContentText.text = content;

            _logBoxes.Add(box);
        }


        public void ClearLogBox()
        {
            foreach (LogBoxClass box in _logBoxes)
            {
                Destroy(box.gameObject);
            }

            _logBoxes.Clear();
        }
    }
}
