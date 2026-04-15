using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class AITargeting : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    public teleport teleport;
    private AIDestinationSetter ai;
    private controlZoneAndPoints currentZone;
    private controlZoneAndPoints lastZone;
    
    [SerializeField] private float targetOffsetRadius = 1.5f;
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float separationRadius = 1.2f;
    [SerializeField] private float separationStrength = 2f;
    [SerializeField] private float zoneRadius = 2.5f;
    
    public List<Transform> enemies = new List<Transform>();
    public List<Transform> teammates = new List<Transform>();
    public Transform player;
    
    [Range(0f, 1f)] public float dayDetection = 0.8f;
    
    [Range(0f, 1f)] public float attackRate = 0.7f;
    [Range(0f, 1f)] public float captureRate = 0.8f;
    [Range(0f, 1f)] public float wanderRate = 0.2f;
    
    private enum AIState { CaptureZone, DefendZone, AttackEnemy, Retreat, Survey }
    private AIState currentState;
    private float stateTimer;
    private Transform tempTarget;
    private Vector2 wanderDirection;
    
    void Awake()
    {
        tempTarget = new GameObject("TempTarget").transform;
        tempTarget.parent = transform;
    }
    
    void Start()
    {
        ai = GetComponent<AIDestinationSetter>();
    }
    
    void Update()
    {
        if (ai == null || dayNightCycle == null) return;
        currentZone = teleport.currentZone;

        if (currentZone != lastZone)
        {
            lastZone = currentZone;

            stateTimer = 0f;

            if (currentZone != null)
            {
                tempTarget.position = currentZone.GetPosition();
                Debug.Log("AI switched to zone: " + currentZone.name);
            }
        }
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            ChooseState();
            stateTimer = Random.Range(1.5f, 3f);
        }
        ExecuteState();
    }
    
    void ChooseState()
    {
        if (currentZone == null)
        {
            currentState = AIState.Survey;
            return;
        }

        bool isEnemyAI = CompareTag("Enemy");
        bool alliesOnZone = currentZone.HasAllies();
        bool enemiesOnZone = currentZone.HasEnemies();
        float rand = Random.value;
        bool isNight = !IsDayTime();

        if (!alliesOnZone && !enemiesOnZone)
        {
            currentState = AIState.CaptureZone;
            return;
        }

        if (isEnemyAI)
        {
            currentState = (alliesOnZone && (isNight || rand < attackRate)) ? AIState.AttackEnemy : AIState.DefendZone;
        }
        else
        {
            currentState = (enemiesOnZone && (isNight || rand < attackRate)) ? AIState.AttackEnemy : AIState.DefendZone;
        }

        if (rand < wanderRate) currentState = AIState.Survey;
    }
    
    void ExecuteState()
    {
        switch (currentState)
        {
            case AIState.CaptureZone: MoveToZone(); break;
            case AIState.DefendZone: CircleZone(); break;
            case AIState.AttackEnemy: AttackNearest(); break;
            case AIState.Retreat: RetreatFromEnemies(); break;
            case AIState.Survey: Wander(); break;
        }
    }
    
    void MoveToZone()
    {
        Vector3 zoneCenter = currentZone.GetPosition();
        float dist = Vector2.Distance(transform.position, zoneCenter);
        float dynamicRadius = Mathf.Lerp(0.5f, zoneRadius, dist / 5f);
        Vector2 offset = Random.insideUnitCircle * dynamicRadius;
        offset += (Vector2)(transform.right * Mathf.Sin(Time.time * 2f));
        SetTargetPosition(zoneCenter + new Vector3(offset.x, offset.y, 0));
    }
    
    void CircleZone()
    {
        float orbitSpeed = 1.5f;
        float angle = Time.time * orbitSpeed;
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * zoneRadius;
        offset += new Vector3(
            Mathf.Sin(Time.time * 3f),
            Mathf.Cos(Time.time * 2f),
            0
        ) * 0.5f;
        SetTargetPosition(currentZone.GetPosition() + offset);
    }
    
    void AttackNearest()
    {
        List<Transform> targets = CompareTag("Enemy") ? GetAllAllies() : enemies;
        Transform target = GetClosest(targets);
        if (target != null)
        {
            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            Vector3 predictedPos = target.position;
            if (rb != null)
            {
                predictedPos += (Vector3)rb.linearVelocity * 0.5f;
            }
            Vector3 strafe = Vector3.Cross(
                (predictedPos - transform.position).normalized,
                Vector3.forward
            ) * Mathf.Sin(Time.time * 4f);
            SetTargetPosition(predictedPos + strafe);
        }
    }
    
    void RetreatFromEnemies()
    {
        List<Transform> targets = CompareTag("Enemy") ? GetAllAllies() : enemies;
        Transform threat = GetClosest(targets);
        if (threat != null)
        {
            Vector3 away = (transform.position - threat.position).normalized;
            Vector3 side = Vector3.Cross(away, Vector3.forward) * Mathf.Sin(Time.time * 3f);
            Vector3 retreatPos = transform.position + away * 3f + side * 1.5f;
            SetTargetPosition(retreatPos);
        }
    }
    
    void Wander()
    {
        wanderDirection += Random.insideUnitCircle * 0.2f;
        wanderDirection = wanderDirection.normalized;
        Vector3 targetPos = transform.position + new Vector3(wanderDirection.x, wanderDirection.y, 0) * wanderRadius;
        SetTargetPosition(targetPos);
    }
    
    void SetTargetPosition(Vector3 pos)
    {
        var nn = AstarPath.active.GetNearest(pos);
        if (nn.node != null && nn.node.Walkable)
        {
            pos = (Vector3)nn.position;
        }
        Vector3 separation = GetSeparationOffset();
        Vector2 offset = Random.insideUnitCircle * targetOffsetRadius;
        tempTarget.position = pos + separation + new Vector3(offset.x, offset.y, 0);
        ai.target = tempTarget;
    }
    
    Vector3 GetSeparationOffset()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        Vector3 offset = Vector3.zero;

        foreach (var col in nearby)
        {
            if (col.gameObject == gameObject) continue;
            if (col.CompareTag("Enemy") || col.CompareTag("Teammate"))
            {
                Vector3 diff = transform.position - col.transform.position;
                float dist = diff.magnitude;
                if (dist > 0) offset += diff.normalized / dist;
            }
        }
        return offset * separationStrength;
    }

    Transform GetClosest(List<Transform> targets)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;
        foreach (Transform t in targets)
        {
            if (t == null) continue;
            float distance = Vector2.Distance(transform.position, t.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = t;
            }
        }
        return closest;
    }

    List<Transform> GetAllAllies()
    {
        List<Transform> allies = new List<Transform>(teammates);
        if (player != null) allies.Add(player);
        return allies;
    }

    bool IsDayTime()
    {
        float m = dayNightCycle.mins;
        return (m >= 0.5f && m < 0.66f) ||
               (m >= 3f && m < 3.16f) ||
               (m >= 5.5f && m < 5.66f);
    }
}