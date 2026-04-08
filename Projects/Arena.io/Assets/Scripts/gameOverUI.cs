using UnityEngine;
using TMPro;

public class gameOverUI : MonoBehaviour
{
    public TextMeshProUGUI winner;
    public TextMeshProUGUI totalScoreAllies;
    public TextMeshProUGUI totalScoreEnemies;
    public TextMeshProUGUI arenaScoreAllies;
    public TextMeshProUGUI arenaScoreEnemies;

    void Start()
    {
        DisplayResults();
    }

    void DisplayResults()
    {
        if (PointManager.Instance == null)
        {
            Debug.LogError("PointManager is NULL!");
            return;
        }
        
        var manager = PointManager.Instance;

        float ally = manager.totalAllyScore;
        float enemy = manager.totalEnemyScore;
        
        if (ally > enemy)
            winner.text = "The Winners Are... The Allies!";
        else if (enemy > ally)
            winner.text = "The Winners Are... The Enemies!";
        else
            winner.text = "This wasn't supposed to happen... you drew?";
        
        totalScoreAllies.text = $"Total Ally Points: {Mathf.FloorToInt(ally)}";
        totalScoreEnemies.text = $"Total Enemy Points: {Mathf.FloorToInt(enemy)}";
        
        string allyText = "";
        string enemyText = "";

        for (int i = 0; i < manager.allyScoresPerArena.Count; i++)
        {
            allyText += $"Arena {i + 1} Points: {Mathf.FloorToInt(manager.allyScoresPerArena[i])}\n";
            enemyText += $"Arena {i + 1} Points: {Mathf.FloorToInt(manager.enemyScoresPerArena[i])}\n";
        }

        arenaScoreAllies.text = allyText;
        arenaScoreEnemies.text = enemyText;
    }
}
