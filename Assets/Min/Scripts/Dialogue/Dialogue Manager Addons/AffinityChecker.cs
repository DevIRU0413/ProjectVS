using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Dialogue.DialogueData;
using ProjectVS.Shop.NPCAffinityModel;


public class AffinityChecker
{
    public bool CanShow(DialogueData data)
    {
        return NPCAffinityModel.Instance.AffinityLevel >= data.NeedAffinity;
    }
}
