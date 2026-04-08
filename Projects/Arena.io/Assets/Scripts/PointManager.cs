using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    public List<controlZoneAndPoints> zones = new List<controlZoneAndPoints>();

    public float totalAllyScore;
    public float totalEnemyScore;
    
    public List<float> allyScoresPerArena = new List<float>();
    public List<float> enemyScoresPerArena = new List<float>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Arena1")
        {
            zones.Clear();
            var foundZones = FindObjectsOfType<controlZoneAndPoints>();
            zones = new List<controlZoneAndPoints>(foundZones);
            zones.Sort((a, b) => a.name.CompareTo(b.name));
            ResetScores();
        }
    }
    
    private void Update()
    {
        CalculateTotals();
    }

    void CalculateTotals()
    {
        totalAllyScore = 0;
        totalEnemyScore = 0;

        foreach (var zone in zones)
        {
            if (zone == null) continue;

            totalAllyScore += zone.allyScore;
            totalEnemyScore += zone.enemyScore;
        }
    }
    
    public void SaveFinalScores()
    {
        allyScoresPerArena.Clear();
        enemyScoresPerArena.Clear();

        totalAllyScore = 0;
        totalEnemyScore = 0;

        foreach (var zone in zones)
        {
            if (zone == null) continue;

            allyScoresPerArena.Add(zone.allyScore);
            enemyScoresPerArena.Add(zone.enemyScore);

            totalAllyScore += zone.allyScore;
            totalEnemyScore += zone.enemyScore;
        }
    }
    
    public void ResetScores()
    {
        totalAllyScore = 0;
        totalEnemyScore = 0;
        
        allyScoresPerArena.Clear();
        enemyScoresPerArena.Clear();

        foreach (var zone in zones)
        {
            if (zone == null) continue;

            zone.allyScore = 0;
            zone.enemyScore = 0;
        }
    }
}
