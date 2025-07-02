using System.Collections.Generic;

using ProjectVS.Monster.Spawner;

using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSpawnControllerConfigSO", menuName = "ProjectVS/MonsterSpawnControllerConfig")]
public class MonsterSpawnControllerConfigSO : ScriptableObject
{
    [Header("Spawn Entries")]
    public List<SpawnEntry> spawnEntries = new();

    [Header("Radius")]
    public float radius = 0f;

    [Header("Line")]
    public bool isReverseLine = false;
    public float offset = 1.5f;
    public float distance = 10f;
    public List<Vector2> directionList = new();

    [Header("Circle")]
    public float circleRadius = 0f;

    [Header("Grid")]
    public Vector2 gridSize = new Vector2(3, 3);
    public float gridSpacing = 2f;

    [Header("PureBoid")]
    public float moveSpeed = 5f;
    public float neighborRadius = 3f;
    public float separationDistance = 1f;

    [Range(0f, 5f)] public float weightFixed = 1.6f;
    [Range(0f, 5f)] public float weightSeparation = 1.5f;
    [Range(0f, 5f)] public float weightAlignment = 1.0f;
    [Range(0f, 5f)] public float weightCohesion = 1.0f;

    public Vector2 spawnRange = new Vector2(5, 5);
    public float spawnDistance = 10.0f;
    public bool isRandomSpawnDirection = true;
    public List<float> spawnAngleList = new();
}
