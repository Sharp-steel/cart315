using UnityEngine;
using TMPro;

public class liveScore : MonoBehaviour
{
    public TextMeshProUGUI allyText;
    public TextMeshProUGUI enemyText;

    void Update()
    {
        if (PointManager.Instance == null) return;

        allyText.text = "Allies: " + Mathf.FloorToInt(PointManager.Instance.totalAllyScore);
        enemyText.text = "Enemies: " + Mathf.FloorToInt(PointManager.Instance.totalEnemyScore);
    }
}
