using UnityEngine;
using System.Collections.Generic;

public class controlZoneAndPoints : MonoBehaviour
{
    public float allyScore = 0f;
    public float enemyScore = 0f;
    public float pps = 1f;
    public bool isActive = false;

    public List<Transform> allies = new List<Transform>();
    public List<Transform> enemies = new List<Transform>();

    private float pointTimer = 0f;

    private void Update()
    {
        if (!isActive) return;

        int allyCount = allies.Count;
        int enemyCount = enemies.Count;
        
        if (allyCount + enemyCount == 0)
        {
            pointTimer = 0f;
            return;
        }
        
        pointTimer += Time.deltaTime;
        
        if (pointTimer >= 1f)
        {
            if (allyCount > enemyCount)
                allyScore += pps;
            else if (enemyCount > allyCount)
                enemyScore += pps;

            pointTimer = 0f;
        }
    }
    
    public bool HasAllies()
    {
        return allies.Count > 0;
    }
    
    public bool HasEnemies()
    {
        return enemies.Count > 0;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Teammate"))
        {
            if (!allies.Contains(other.transform))
                allies.Add(other.transform);
        }
        else if (other.CompareTag("Enemy"))
        {
            if (!enemies.Contains(other.transform))
                enemies.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Teammate"))
            allies.Remove(other.transform);
        else if (other.CompareTag("Enemy"))
            enemies.Remove(other.transform);
    }
}
