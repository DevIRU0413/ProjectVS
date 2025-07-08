using System.Collections;
using System.Collections.Generic;

using ProjectVS.Stage;

using UnityEngine;


namespace ProjectVS.Cheat.StageCheater
{
    public class StageCheater : MonoBehaviour
    {
        Manager.Stage.StageFlowMachine _flowMachine;

        private void Start()
        {
            _flowMachine = StageManager.Instance.FlowMachine;
        }

        public void OnClickStageClear()
        {
            _flowMachine.CheatWin();
        }
    }
}
