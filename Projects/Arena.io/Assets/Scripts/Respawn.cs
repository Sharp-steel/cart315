using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour
{
    public float respawnDelay = 5f;
    public bool isAlly = true;

    private Health health;
    private teleport teleportSystem;
    private Transform spawnPoint;
    
    private SpriteRenderer[] renderers;
    private Collider2D[] colliders;
    
    private int deathArenaIndex;
    
    void Awake()
    {
        health = GetComponent<Health>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();
        teleportSystem = FindObjectOfType<teleport>();
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
        if (teleportSystem != null)
            deathArenaIndex = teleportSystem.CurrentArenaIndex;
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
            if (script != this)
                script.enabled = state;
        }
    }

    void SetSpawnPoint()
    {
        if (teleportSystem == null)
        {
            Debug.LogWarning("Teleport system not found!");
            return;
        }

        int index = deathArenaIndex;

        if (index < 0 || index >= teleportSystem.arenas.Length)
        {
            Debug.LogWarning("Invalid arena index");
            return;
        }

        Transform arena = teleportSystem.arenas[index];

        string spawnName = isAlly ? "AllySpawn" : "EnemySpawn";
        Transform sp = arena.Find(spawnName);

        if (sp != null)
            spawnPoint = sp;
        else
            Debug.LogWarning($"Spawn point '{spawnName}' not found in {arena.name}");
    }
}