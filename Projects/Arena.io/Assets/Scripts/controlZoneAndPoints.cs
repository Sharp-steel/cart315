using UnityEngine;
using System.Collections.Generic;

public class controlZoneAndPoints : MonoBehaviour
{
    public float pointsPerSecond = 1f;
    public bool isActive = false;

    public int allyScore = 0;
    public int enemyScore = 0;

    private List<GameObject> allies = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();

    void Update()
    {
        if (!isActive) return;

        bool allyPresent = allies.Count > 0;
        bool enemyPresent = enemies.Count > 0;

        if (allyPresent && !enemyPresent)
        {
            allyScore += Mathf.RoundToInt(pointsPerSecond * Time.deltaTime);
        }
        else if (enemyPresent && !allyPresent)
        {
            enemyScore += Mathf.RoundToInt(pointsPerSecond * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Teammate"))
        {
            if (!allies.Contains(collision.gameObject))
                allies.Add(collision.gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            if (!enemies.Contains(collision.gameObject))
                enemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (allies.Contains(collision.gameObject))
            allies.Remove(collision.gameObject);
        
        if (enemies.Contains(collision.gameObject))
            enemies.Remove(collision.gameObject);
    }

    public bool HasAllies() => allies.Count > 0;
    public bool HasEnemies() => enemies.Count > 0;

    public Vector3 GetPosition() => transform.position;
}
