using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour
{
    public float respawnDelay = 5f;
    public bool isAlly = true;

    private Health health;
    private Transform spawnPoint;
    
    private SpriteRenderer[] renderers;
    private Collider2D[] colliders;

    void Awake()
    {
        health = GetComponent<Health>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    void OnEnable()
    {
        if (health != null)
            health.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        if (health != null)
            health.OnDeath -= HandleDeath;
    }

    void HandleDeath()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        SetActiveState(false);
        
        yield return new WaitForSeconds(respawnDelay);

        SetSpawnPoint();
        
        if (spawnPoint != null)
            transform.position = spawnPoint.position;
        
        health.ResetHealth();
        
        SetActiveState(true);
    }
    
    void SetActiveState(bool state)
    {
        foreach (var r in renderers)
            r.enabled = state;

        foreach (var c in colliders)
            c.enabled = state;
        
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this) // don’t disable Respawn itself
                script.enabled = state;
        }
    }

    void SetSpawnPoint()
    {
        GameObject currentArena = null;
        
        for (int i = 1; i <= 4; i++)
        {
            GameObject arena = GameObject.Find("Arena" + i);
            if (arena != null && arena.activeInHierarchy)
            {
                currentArena = arena;
                break;
            }
        }

        if (currentArena != null)
        {
            string spawnName = isAlly ? "AllySpawn" : "EnemySpawn";
            Transform sp = currentArena.transform.Find(spawnName);

            if (sp != null)
                spawnPoint = sp;
            else 
                Debug.LogWarning($"Spawn point '{spawnName}' not found in {currentArena.name}");
        }

        else
        {
            Debug.LogWarning("No active arena can be found");
        }
    }
}
