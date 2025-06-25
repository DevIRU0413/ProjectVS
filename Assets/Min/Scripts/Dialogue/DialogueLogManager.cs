using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;


namespace ProjectVS.Dialogue.DialogueLogManager
{
    public class DialogueLogManager : MonoBehaviour
    {
        [SerializeField] private Transform _logContentTransform;
        [SerializeField] private GameObject _logBoxPrefab;

        private List<GameObject> _logBoxes = new();

        // TODO: 오브젝트 풀링 등 해야될 듯
        // TODO: GetComponent도 안할 방법 찾아야 될 듯
        public void AddLogBox(string content)
        {
            GameObject box = Instantiate(_logBoxPrefab, _logContentTransform);
            TMP_Text text = box.GetComponentInChildren<TMP_Text>();
            text.text = content;

            _logBoxes.Add(box);
        }

        // MEMO: 순회 중 Destroy 됐던가??
        public void ClearLog()
        {
            foreach (GameObject box in _logBoxes)
            {
                Destroy(box);
            }

            _logBoxes.Clear();
        }
    }
}
