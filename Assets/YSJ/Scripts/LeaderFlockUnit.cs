using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class LeaderFlockUnit : MonoBehaviour
    {
        [Header("Flocking Settings")]
        public float moveSpeed = 5f;
        public float neighborRadius = 3f;
        public float separationDistance = 1f;

        [Range(0f, 5f)] public float weightToLeader = 1.5f;
        [Range(0f, 5f)] public float weightSeparation = 1.5f;
        [Range(0f, 5f)] public float weightCohesion = 1.0f;
        [Range(0f, 5f)] public float weightAlignment = 1.0f;

        [Header("Leadership")]
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private float leaderDetectRadius = 5f;
        [SerializeField] private bool isLeader = false;
        [SerializeField] private float leaderBecomeDelay = 3f;

        public static int currentLeaders = 0;
        public static int maxLeadersAllowed = 2;

        private float noLeaderTime = 0f;

        private Rigidbody2D rb;
        private Transform leader;
        private List<LeaderFlockUnit> allUnits;

        private Vector2 defaultDirection;
        private Vector2 desiredDirection;

        public void Init(Transform leader, List<LeaderFlockUnit> units, Vector2 defaultDirection)
        {
            this.leader = leader;
            this.allUnits = units;

            if (leader == transform)
                BecomeLeader();

            this.defaultDirection = (defaultDirection == Vector2.zero) ? Random.insideUnitCircle.normalized : defaultDirection.normalized;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!isLeader && !IsLeaderNearby())
            {
                noLeaderTime += Time.deltaTime;

                if (noLeaderTime >= leaderBecomeDelay && currentLeaders < maxLeadersAllowed)
                    BecomeLeader();
            }
            else
            {
                noLeaderTime = 0f;
            }

            if (isLeader)
            {
                desiredDirection = (defaultDirection == Vector2.zero) ? Random.insideUnitCircle.normalized : defaultDirection.normalized;
                CheckForBetterLeader();
            }
        }

        private void FixedUpdate()
        {
            Vector2 velocity = defaultDirection;

            if (leader != null)
            {
                velocity += ToLeader() * weightToLeader;
                var leaderUnit = leader.GetComponent<LeaderFlockUnit>();
                if (leaderUnit != null)
                    velocity = Vector2.Lerp(velocity, leaderUnit.desiredDirection, 0.1f);
            }

            velocity += Separation() * weightSeparation;
            velocity += Cohesion() * weightCohesion;
            velocity += Alignment() * weightAlignment;

            rb.velocity = Vector2.Lerp(rb.velocity, velocity.normalized * moveSpeed, 0.2f);
        }

        private void CheckForBetterLeader()
        {
            foreach (var unit in allUnits)
            {
                if (unit == this || !unit.isLeader) continue;
                float dist = Vector2.Distance(transform.position, unit.transform.position);

                if (dist < leaderDetectRadius && unit.GetLeaderPriority() > GetLeaderPriority())
                {
                    DemoteLeader();
                    return;
                }
            }
        }

        private bool IsLeaderNearby()
        {
            foreach (var unit in allUnits)
            {
                if (unit == this) continue;
                if (unit.isLeader && Vector2.Distance(transform.position, unit.transform.position) < leaderDetectRadius)
                    return true;
            }
            return false;
        }

        private void BecomeLeader()
        {
            isLeader = true;
            leader = this.transform;
            currentLeaders++;

            transform.localScale *= 1.2f;
            if (spriteRenderer != null)
                spriteRenderer.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        private void DemoteLeader()
        {
            isLeader = false;
            currentLeaders--;

            transform.localScale /= 1.2f;
            if (spriteRenderer != null)
                spriteRenderer.GetComponent<SpriteRenderer>().color = Color.white;

            Vector2 lastDir = desiredDirection;
            Transform newLeader = FindClosestLeader();

            if (newLeader != null)
            {
                var newUnit = newLeader.GetComponent<LeaderFlockUnit>();
                newUnit.InheritDirection(lastDir);
            }

            leader = newLeader;
        }

        public void InheritDirection(Vector2 dir)
        {
            desiredDirection = dir.normalized;
        }

        public Vector2 GetDirection()
        {
            return desiredDirection;
        }

        private Transform FindClosestLeader()
        {
            float minDist = float.MaxValue;
            Transform closest = null;

            foreach (var unit in allUnits)
            {
                if (unit == this || !unit.isLeader) continue;
                float dist = Vector2.Distance(transform.position, unit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = unit.transform;
                }
            }
            return closest;
        }

        private float GetLeaderPriority()
        {
            Vector2 center = GetFlockCenter();
            float distToCenter = Vector2.Distance(transform.position, center);
            return -distToCenter;
        }

        private Vector2 GetFlockCenter()
        {
            Vector2 center = Vector2.zero;
            int count = 0;

            foreach (var unit in allUnits)
            {
                if (unit == null) continue;
                center += (Vector2)unit.transform.position;
                count++;
            }

            return count > 0 ? center / count : transform.position;
        }

        private Vector2 ToLeader()
        {
            return (leader.position - transform.position).normalized;
        }

        private Vector2 Separation()
        {
            Vector2 force = Vector2.zero;
            int count = 0;

            foreach (var unit in allUnits)
            {
                if (unit == this) continue;
                float dist = Vector2.Distance(transform.position, unit.transform.position);
                if (dist < separationDistance)
                {
                    force += (Vector2)(transform.position - unit.transform.position) / dist;
                    count++;
                }
            }
            return count > 0 ? (force / count).normalized : Vector2.zero;
        }

        private Vector2 Cohesion()
        {
            Vector2 center = Vector2.zero;
            int count = 0;

            foreach (var unit in allUnits)
            {
                if (unit == this) continue;
                float dist = Vector2.Distance(transform.position, unit.transform.position);
                if (dist < neighborRadius)
                {
                    center += (Vector2)unit.transform.position;
                    count++;
                }
            }
            if (count == 0) return Vector2.zero;
            center /= count;
            return (center - (Vector2)transform.position).normalized;
        }

        private Vector2 Alignment()
        {
            Vector2 avgVelocity = Vector2.zero;
            int count = 0;

            foreach (var unit in allUnits)
            {
                if (unit == this) continue;
                float dist = Vector2.Distance(transform.position, unit.transform.position);
                if (dist < neighborRadius)
                {
                    avgVelocity += unit.rb.velocity;
                    count++;
                }
            }
            return count > 0 ? (avgVelocity / count).normalized : Vector2.zero;
        }
    }
}
