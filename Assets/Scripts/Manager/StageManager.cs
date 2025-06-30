using System.Collections;
using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.Interface;
using ProjectVS.Monster;
using ProjectVS.UI.Model;
using ProjectVS.UI.Presenter;
using ProjectVS.UI.View;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Manager
{
    // 0. 스테이지 흐름 관리
    // 1. 타이머 타임 오버 체크
    // 2. 일시 정지 시, 팝업
    public class StageManager : SimpleSingleton<StageManager>, IGameStateListener
    {
        [Header("Stage Config Data")]
        [SerializeField] private StageDataSO stageData;

        [field: Header("Stage State")]
        [field: SerializeField] public StageFlowState StageFlowState { get; private set; } = StageFlowState.None;
        [field: SerializeField] public StageResult StageResult { get; private set; } = StageResult.None;

        [Header("UI")]                                              
        [SerializeField] private TimerView _timerView;  // timer  HUD
        [SerializeField] private TimerView _pausedView; // paused pupup
        // [SerializeField] private TimerView _resultView; // result pupup

        // 내부 상태
        private PlayerConfig _player;
        private TimerPresenter _timerPresenter;

        private MonsterController _bossCtrl;

        private void OnEnable()
        {
            GameManager.Instance.OnStateChanged -= OnGameStateChanged;
            GameManager.Instance.OnStateChanged += OnGameStateChanged;
        }
        private void OnDisable()
        {
            GameManager.Instance.OnStateChanged -= OnGameStateChanged;
        }
        protected override void OnDestroy()
        {
            GameManager.Instance.OnStateChanged -= OnGameStateChanged;
        }

        private void Start()
        {
            InitPlayer();
            InitTimer();
            StageFlowState = StageFlowState.Enter;
        }
        private void Update()
        {
            StageFlow();
            _timerPresenter?.Update(Time.deltaTime);
        }

        #region Init
        private void InitPlayer()
        {
            // 플레이어 소환

            // _player = GameManager.Instance.Player;
        }
        private void InitTimer()
        {
            if (_timerView == null) return;

            var model = new TimerModel(stageData.clearTime);
            _timerPresenter = new TimerPresenter(model, _timerView);
            _timerPresenter.StartTimer();
        }

        #endregion

        #region Stage Flow
        private void StageFlow()
        {
            switch (StageFlowState)
            {
                case StageFlowState.Enter:
                    HandleEnter();
                    break;
                case StageFlowState.Play:
                    HandlePlay();
                    break;
                case StageFlowState.Paused:
                    HandlePause();
                    break;
                case StageFlowState.Exit:
                    HandleExit();
                    break;
            }
        }

        private void HandleEnter()
        {
            Debug.Log("[Stage] Enter");
            StageFlowState = StageFlowState.Play;
        }

        private void HandlePlay()
        {
            if (_player == null || StageResult != StageResult.None) return;

            if (_player.Stats.CurrentHp <= 0)
            {
                StageResult = StageResult.Lose;
                StageFlowState = StageFlowState.Exit;
            }
            else if (WinCondition())
            {
                StageResult = StageResult.Win;
                StageFlowState = StageFlowState.Exit;
            }
        }

        private void HandlePause()
        {
            // 일시정지 중 처리
        }

        private void HandleExit()
        {
            Debug.Log($"[Stage] 결과: {StageResult}");
            // _resultView?.ShowResult(StageResult == StageResult.Win);
        }
        #endregion

        #region Flow Condition
        private bool WinCondition()
        {
            // 보스 프리팹이 있다면, 시간이 다 되어도 보스가 죽어야지 클리어
            // 보스 프리팹이 없다면, 시간이 다 되면 자동 클리어
            return _timerPresenter.IsTimeOver() && ((stageData.bossPrefab != null && _bossCtrl.IsDeath) || (stageData.bossPrefab == null));
        }

        #endregion

        #region ETC
        // 임시
        private void SpawnBoss()
        {
            if (stageData.bossPrefab == null) return;

            var boss = Instantiate(stageData.bossPrefab, GetBossSpawnPoint(), Quaternion.identity);

            if (boss.TryGetComponent(out MonsterController ctrl))
            {
                _bossCtrl = ctrl;
                ctrl.OnDeath += () =>
                {
                    StageResult = StageResult.Win;
                    StageFlowState = StageFlowState.Exit;
                };
            }
        }
        private Vector3 GetRandomSpawnPoint()
        {
            Vector2 dir = Random.insideUnitCircle;
            return _player.transform.position + new Vector3(dir.x, dir.y, 0.0f) * 5f;
        }
        private Vector3 GetBossSpawnPoint()
        {
            return _player.transform.position + Vector3.right * 10f;
        }

        public void OnGameStateChanged(GameState state)
        {
        }

        #endregion
    }
}
