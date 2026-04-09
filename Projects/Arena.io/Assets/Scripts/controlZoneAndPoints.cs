using UnityEngine;
using System.Collections.Generic;

public class controlZoneAndPoints : MonoBehaviour
{
    public float allyScore = 0f;
    public float enemyScore = 0f;
    public float pps = 1f;
    public bool isActive = false;

    private int allyCount = 0;
    private int enemyCount = 0;

    private float pointTimer = 0f;
    public int arenaIndex;

    private void Update()
    {
        if (!isActive) return;
        
        allyCount = Mathf.Max(0, allyCount);
        enemyCount = Mathf.Max(0, enemyCount);

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
        return allyCount > 0;
    }
    
    public bool HasEnemies()
    {
        return enemyCount > 0;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Teammate"))
        {
            allyCount++;
        }
        else if (other.CompareTag("Enemy"))
        {
            enemyCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Teammate"))
        {
            allyCount--;
        }
        else if (other.CompareTag("Enemy"))
        {
            enemyCount--;
        }
    }
}