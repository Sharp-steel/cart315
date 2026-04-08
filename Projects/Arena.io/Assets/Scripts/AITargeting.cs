using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class AITargeting : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    private AIDestinationSetter ai;
    private controlZoneAndPoints currentZone;
    
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
        
        FindActiveZone();
        
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            ChooseState();
            stateTimer = Random.Range(1.5f, 3f);
        }

        ExecuteState();
    }
    
    void FindActiveZone()
    {
        controlZoneAndPoints[] zones = FindObjectsOfType<controlZoneAndPoints>();
        foreach (var zone in zones)
        {
            if (zone.isActive)
            {
                currentZone = zone;
                return;
            }
        }
        currentZone = null;
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
        Vector2 offset = Random.insideUnitCircle.normalized * zoneRadius;
        SetTargetPosition(currentZone.GetPosition() + new Vector3(offset.x, offset.y, 0));
    }
    
    void CircleZone()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * zoneRadius;
        SetTargetPosition(currentZone.GetPosition() + offset);
    }
    
    void AttackNearest()
    {
        List<Transform> targets = CompareTag("Enemy") ? GetAllAllies() : enemies;
        Transform target = GetClosest(targets);
        if (target != null) SetTargetPosition(target.position);
    }
    
    void RetreatFromEnemies()
    {
        List<Transform> targets = CompareTag("Enemy") ? GetAllAllies() : enemies;
        Transform threat = GetClosest(targets);
        if (threat != null)
        {
            Vector3 dir = (transform.position - threat.position).normalized;
            SetTargetPosition(transform.position + dir * 3f);
        }
    }
    
    void Wander()
    {
        Vector2 random = Random.insideUnitCircle * wanderRadius;
        SetTargetPosition(transform.position + new Vector3(random.x, random.y, 0));
    }
    
    void SetTargetPosition(Vector3 pos)
    {
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