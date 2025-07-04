using System.Collections;
using System.Collections.Generic;

using ProjectVS;
using ProjectVS.Interface;
using ProjectVS.Unit.Monster.Pattern;
using ProjectVS.Unit.Monster.Phase;

using UnityEngine;

public class MonsterSpawnPattern : MonsterPattern, IGroggyTrackable
{
    [field: SerializeField] private List<Vector2> spawnPoints;

    public int GroggyThreshold => 10;
    public bool IsFaild => false;

    public override void Init(MonsterPhaseController phaseController)
    {
        this.phaseController = phaseController;
        // currentCooldown = _cooldown;
        PatternState = MonsterPatternState.None;
    }

    protected override IEnumerator IE_PlayAction()
    {
        yield break;
    }

    void Start()
    {

    }

    void Update()
    {

    }

}
