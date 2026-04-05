using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class AITargeting : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    private AIDestinationSetter ai;
    
    public List<Transform> enemies = new List<Transform>();
    public List<Transform> teammates = new List<Transform>();
    public Transform player;
    
    [Range(0f, 1f)] public float dayDetection = 0.8f;
    
    void Start()
    {
        ai = GetComponent<AIDestinationSetter>();
    }
    
    void Update()
    {
        if (ai == null || dayNightCycle == null) return;
        
        if (Time.frameCount % 10 != 0) return;
        
        ChooseTarget();
    }

    void ChooseTarget()
    {
        bool isDay = IsDayTime();

        List<Transform> correctTargets = null;
        List<Transform> wrongTargets = null;

        if (CompareTag("Enemy"))
        {
            correctTargets = GetAllAllies();
            wrongTargets = enemies;
        }
        else if (CompareTag("Teammate"))
        {
            correctTargets = enemies;
            wrongTargets = GetAllAllies();
        }

        Transform target = null;

        if (!isDay)
        {
            target = GetClosest(correctTargets);
        }
        else
        {
            bool chooseCorrect = Random.value <= dayDetection;

            if (chooseCorrect || wrongTargets.Count == 0)
                target = GetClosest(correctTargets);
            else
                target = GetClosest(wrongTargets);
        }
        
        if (target != null)
        {
            ai.target = target;
        }
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
        
        if (player != null)
            allies.Add(player);
        
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