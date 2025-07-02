using System;

using ProjectVS.Interface.UI;
using ProjectVS.Manager;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ProjectVS.UI.View
{
    public class TimerView : MonoBehaviour, ITimerView
    {
        public TextMeshProUGUI timeText;

        public void SetTimeText(string timeStr)
        {
            timeText.text = timeStr;
        }
    }
}
