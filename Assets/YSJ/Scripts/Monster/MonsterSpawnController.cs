using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Data;
using ProjectVS.Monster.Spawner;
using ProjectVS.Unit.Monster;
using ProjectVS.Unit.Player;
using ProjectVS.Util;
using ProjectVS.Utils.CsvTable;

namespace ProjectVS.Monster
{
    public class MonsterSpawnController
    {
        // 초기화 여부
        private bool _instantiated = false;

        // 소환기
        private RadiusSpawner _radiusSpawner;
        private LineSpawner _lineSpawner;
        private CircleSpawner _circleSpawner;
        private GridSpawner _gridSpawner;
        private PureBoidSpawner _pureBoidSpawner;

        // 필수 데이터
        private GameObject _target;
        private Vector3 _targetLastPoint;

        private int _currentSpawnCount = 0;
        private int _maxSpawnCount = 0;

        private MonsterSpawnConfigSO _config;

        private Dictionary<int, MonsterStatsConfig> _monsterDatas = new();

        public void Init(GameObject target, int maxCount, MonsterSpawnConfigSO config)
        {
            _target = target;
            _maxSpawnCount = maxCount;
            _config = config;

            if (_target == null || _config == null || _config.spawnEntries == null || _config.spawnEntries.Count <= 0)
                return;

            if (_monsterDatas.Count == 0)
            {
                var table = new CsvTable("StreamingAssets/DataSheet/MonsterData.tsv", '\t');
                var list = MonsterDataParser.Parse(table);
                SetDataListUp(list);
            }

            InitSpawners();

            foreach (var entry in _config.spawnEntries)
                InitSpawnEntry(entry);

            _instantiated = true;
        }

        public MonsterStatsConfig GetMonsterStatsConfig(int id)
        {
            return _monsterDatas.TryGetValue(id, out var config) ? config : null;
        }

        public void Update()
        {
            if (!_instantiated) return;

            UpdateTarget();

            foreach (var entry in _config.spawnEntries)
            {
                if (entry.SpawnGroupType == SpawnGroupType.None) continue;

                entry.timer += Time.deltaTime;
                entry.activeTime += Time.deltaTime;

                if (entry.StopAfterTime > 0 && entry.activeTime >= entry.StopAfterTime)
                    continue;

                if (entry.timer >= entry.Interval && CanSpawn(entry))
                {
                    Spawn(entry);
                    entry.timer = 0f;
                }
            }
        }

        private void Spawn(SpawnEntry entry)
        {
            var spawner = GetSpawner(entry.SpawnGroupType);
            if (spawner == null) return;

            Vector3 spawnPoint = _target.transform.position;
            spawner.SpawnUnits(_target, spawnPoint, entry.GroupUnitSpawnCount);
        }

        private bool CanSpawn(SpawnEntry entry)
        {
            if (entry == null) return false;

            int afterSpawn = entry.GroupUnitSpawnCount + _currentSpawnCount + 1;
            return _currentSpawnCount < _maxSpawnCount && afterSpawn <= _maxSpawnCount;
        }

        private void InitSpawnEntry(SpawnEntry entry)
        {
            entry.timer = 0f;
            entry.activeTime = 0f;

            var spawner = GetSpawner(entry.SpawnGroupType);
            if (spawner == null)
            {
                Debug.LogError($"No spawner exists for type [{entry.SpawnGroupType}]");
                return;
            }

            spawner.SetSpawnableObjectList(entry.SpawnableObjects);

            foreach (var monster in entry.SpawnableObjects)
                PoolManager.ForceInstance.CreatePool(monster.name, monster, _maxSpawnCount / 2);

            if (entry.AutoStart && CanSpawn(entry))
                Spawn(entry);
        }

        private void SetDataListUp(List<MonsterStatsConfig> list)
        {
            if (list == null || list.Count == 0)
            {
                Debug.LogError("Monster data list is empty.");
                return;
            }

            foreach (var monster in list)
                _monsterDatas[monster.ID] = monster;
        }

        private void InitSpawners()
        {
            _radiusSpawner = new();
            _lineSpawner = new();
            _circleSpawner = new();
            _gridSpawner = new();
            _pureBoidSpawner = new();
        }

        private void UpdateTarget()
        {
            if (_target == null)
            {
                _target = PlayerSpawner.Instance?.CurrentPlayer;
                if (_target != null)
                    _targetLastPoint = _target.transform.position;
            }
            else
            {
                _targetLastPoint = _target.transform.position;
            }
        }

        private SpawnerBase GetSpawner(SpawnGroupType type)
        {
            switch (type)
            {
                case SpawnGroupType.Radius:
                    _radiusSpawner.radius = _config.radius;
                    return _radiusSpawner;

                case SpawnGroupType.Line:
                    _lineSpawner.isReverseLine = _config.isReverseLine;
                    _lineSpawner.offset = _config.offset;
                    _lineSpawner.distance = _config.distance;
                    _lineSpawner.directionList = _config.directionList;
                    _lineSpawner.spawnLifeCycle = _config.lineSpawnLifeCycle;
                    return _lineSpawner;

                case SpawnGroupType.Circle:
                    _circleSpawner.radius = _config.circleRadius;
                    _circleSpawner.spawnLifeCycle = _config.circleSpawnLifeCycle;
                    return _circleSpawner;

                case SpawnGroupType.Grid:
                    _gridSpawner.GridSize = _config.gridSize;
                    _gridSpawner.Spacing = _config.gridSpacing;
                    return _gridSpawner;

                case SpawnGroupType.PureBoid:
                    _pureBoidSpawner.moveSpeed = _config.moveSpeed;
                    _pureBoidSpawner.neighborRadius = _config.neighborRadius;
                    _pureBoidSpawner.separationDistance = _config.separationDistance;
                    _pureBoidSpawner.weightFixed = _config.weightFixed;
                    _pureBoidSpawner.weightSeparation = _config.weightSeparation;
                    _pureBoidSpawner.weightAlignment = _config.weightAlignment;
                    _pureBoidSpawner.weightCohesion = _config.weightCohesion;
                    _pureBoidSpawner.spawnRange = _config.spawnRange;
                    _pureBoidSpawner.spawnDistance = _config.spawnDistance;
                    _pureBoidSpawner._isRandomSpawnDirection = _config.isRandomSpawnDirection;
                    _pureBoidSpawner.spawnAngleList = _config.spawnAngleList;
                    return _pureBoidSpawner;

                default:
                    Debug.LogError($"Unknown spawner type: {type}");
                    return null;
            }
        }
    }
}
