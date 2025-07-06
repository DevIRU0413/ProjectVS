using ProjectVS.Data;
using ProjectVS.Monster.Spawner;
using ProjectVS.Unit.Player;

using UnityEngine;

namespace ProjectVS.Monster
{
    public class MonsterSpawnController : MonoBehaviour
    {
        // 초기화 여부
        private bool _instantiated = false;

        // 소환기
        [SerializeField] private RadiusSpawner      _radiusSpawner;
        [SerializeField] private LineSpawner        _lineSpawner;
        [SerializeField] private CircleSpawner      _circleSpawner;
        [SerializeField] private GridSpawner        _gridSpawner;
        [SerializeField] private PureBoidSpawner    _pureBoidSpawner;

        // 필수 데이터
        private GameObject _target;
        private Vector3 _targetLastPoint;

        private int _currentSpawnCount = 0;
        private int _maxSpawnCount = 0;
        [SerializeField] private MonsterSpawnConfigSO _config;

        // 초기화
        public void Init(GameObject target, int maxCount, MonsterSpawnConfigSO config)
        {
            _target = target;
            _maxSpawnCount = maxCount;
            _config = config;

            if (_target == null || _config == null || _config.spawnEntries == null || _config.spawnEntries.Count <= 0)
                return;

            Enter();
            _instantiated = true;
        }

        // 초기 진입
        private void Enter()
        {
            _radiusSpawner = new();
            _lineSpawner = new();
            _circleSpawner = new();
            _gridSpawner = new();
            _pureBoidSpawner = new();

            foreach (var entry in _config.spawnEntries)
                InitSpawnEntry(entry);
        }

        // 외부 업데이트
        public void Update()
        {
            if (!_instantiated) return;

            // 타겟 미싱 시, 예외 처리
            if (_target == null)
            {
                _target = PlayerSpawner.Instance?.CurrentPlayer;
                _targetLastPoint = _target.transform.position;
            }
            else
            {
                _targetLastPoint = _target.transform.position;
            }

            foreach (var entry in _config.spawnEntries)
            {
                if (entry.SpawnGroupType == SpawnGroupType.None)
                    continue;

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
            SpawnerBase spanwer = null;
            Vector3 point = _target.transform.position;
            int count = entry.GroupUnitSpawnCount;

            switch (entry.SpawnGroupType)
            {
                case SpawnGroupType.Radius:
                    _radiusSpawner.radius = _config.radius;
                    spanwer = _radiusSpawner;
                    break;

                case SpawnGroupType.Line:
                    _lineSpawner.isReverseLine = _config.isReverseLine;
                    _lineSpawner.offset = _config.offset;
                    _lineSpawner.distance = _config.distance;
                    _lineSpawner.directionList = _config.directionList;
                    _lineSpawner.spawnLifeCycle = _config.lineSpawnLifeCycle;
                    spanwer = _lineSpawner;
                    break;

                case SpawnGroupType.Circle:
                    _circleSpawner.radius = _config.circleRadius;
                    _circleSpawner.spawnLifeCycle = _config.circleSpawnLifeCycle;
                    spanwer = _circleSpawner;
                    break;

                case SpawnGroupType.Grid:
                    _gridSpawner.GridSize = _config.gridSize;
                    _gridSpawner.Spacing = _config.gridSpacing;
                    spanwer = _gridSpawner;
                    break;

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
                    spanwer = _pureBoidSpawner;
                    break;

                default:
                    Debug.LogError($"No spawner exists for type [{entry.SpawnGroupType}]");
                    return;
            }

            spanwer.SpawnUnits(_target, point, count);
        }

        private bool CanSpawn(SpawnEntry entry)
        {
            if (entry == null) return false;

            int afterSpawnedCount = entry.GroupUnitSpawnCount + _currentSpawnCount + 1;
            return _currentSpawnCount < _maxSpawnCount && afterSpawnedCount <= _maxSpawnCount;
        }

        private void InitSpawnEntry(SpawnEntry entry)
        {
            entry.timer = 0f;
            entry.activeTime = 0f;
            SpawnerBase spanwer = null;
            switch (entry.SpawnGroupType)
            {
                case SpawnGroupType.Radius:
                    spanwer = _radiusSpawner;
                    break;

                case SpawnGroupType.Line:
                    spanwer = _lineSpawner;
                    break;

                case SpawnGroupType.Circle:
                    spanwer = _circleSpawner;
                    break;

                case SpawnGroupType.Grid:
                    spanwer = _gridSpawner;
                    break;

                case SpawnGroupType.PureBoid:
                    spanwer = _pureBoidSpawner;
                    break;

                default:
                    Debug.LogError($"No spawner exists for type [{entry.SpawnGroupType}]");
                    return;
            }

            spanwer.SetSpawnableObjectList(entry.SpawnableObjects);
            foreach (var monster in entry.SpawnableObjects)
                ProjectVS.Util.PoolManager.ForceInstance.CreatePool(monster.name, monster, _maxSpawnCount / 2);

            if (entry.AutoStart && CanSpawn(entry))
                Spawn(entry);
        }
    }
}
