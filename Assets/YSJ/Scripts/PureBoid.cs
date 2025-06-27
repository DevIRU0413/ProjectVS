using UnityEngine;
using System.Collections.Generic;
using ProjectVS.Monster;

[RequireComponent(typeof(Rigidbody2D))]
public class PureBoid : MonoBehaviour
{
    public float neighborRadius = 10f;
    public float separationDistance = 2f;

    [Range(0f, 5f)] public float weightFixed = 1.6f;
    [Range(0f, 5f)] public float weightSeparation = 1.5f;
    [Range(0f, 5f)] public float weightAlignment = 1.0f;
    [Range(0f, 5f)] public float weightCohesion = 1.0f;

    private Rigidbody2D _rb;
    private List<PureBoid> _allBoids;

    private Vector2 _fixedDirection;

    private MonsterController monsterController;

    public void Init(List<PureBoid> boids, Vector2 fixedDirection)
    {
        this._allBoids = boids;
        _fixedDirection = fixedDirection.normalized; 
    }

    public void SetValue(float neighborRadius, float separationDistance)
    {
        this.neighborRadius = neighborRadius;
        this.separationDistance = separationDistance;
    }

    public void SetWeight(float weightFixed, float weightSeparation, float weightAlignment, float weightCohesion)
    {
        this.weightFixed = weightFixed; 
        this.weightSeparation = weightSeparation;
        this.weightAlignment = weightAlignment;
        this.weightCohesion = weightCohesion;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0.0f;
        monsterController = GetComponent<MonsterController>();
        monsterController.DelegateMovementAuthority();
    }

    private void Update()
    {
        if (monsterController == null) return;

        Vector2 force = GetForce();
        monsterController.SetMoveDirection(_fixedDirection * weightFixed + force.normalized, true);
    }

    private Vector2 GetForce()
    {
        Vector2 force = Vector2.zero;

        force += Separation() * weightSeparation;   // 분리
        force += Alignment() * weightAlignment;     // 정렬
        force += Cohesion() * weightCohesion;       // 응집력

        return force;
    }

    private Vector2 Separation()
    {
        Vector2 steer = Vector2.zero;
        int count = 0;

        foreach (var boid in _allBoids)
        {
            if (boid == this) continue;
            float dist = Vector2.Distance(transform.position, boid.transform.position);
            if (dist < separationDistance)
            {
                steer += (Vector2)(transform.position - boid.transform.position) / dist;
                count++;
            }
        }
        return count > 0 ? (steer / count).normalized : Vector2.zero;
    }
    private Vector2 Alignment()
    {
        Vector2 avgVelocity = Vector2.zero;
        int count = 0;

        foreach (var boid in _allBoids)
        {
            if (boid == this) continue;
            float dist = Vector2.Distance(transform.position, boid.transform.position);
            if (dist < neighborRadius)
            {
                avgVelocity += boid._rb.velocity;
                count++;
            }
        }
        return count > 0 ? (avgVelocity / count).normalized : Vector2.zero;
    }
    private Vector2 Cohesion()
    {
        Vector2 center = Vector2.zero;
        int count = 0;

        foreach (var boid in _allBoids)
        {
            if (boid == this) continue;
            float dist = Vector2.Distance(transform.position, boid.transform.position);
            if (dist < neighborRadius)
            {
                center += (Vector2)boid.transform.position;
                count++;
            }
        }
        if (count == 0) return Vector2.zero;
        center /= count;
        return (center - (Vector2)transform.position).normalized;
    }
}
