using UnityEngine;
using UnityEngine.SceneManagement;

public class switchToGameOver : MonoBehaviour
{
    public DayNightCycle dayNightCycle;
    
    private bool gameOver = false;

    // Update is called once per frame
    void Update()
    {
        if (dayNightCycle.freezeTime)
        {
            gameOver = true;
            if (PointManager.Instance != null)
            {
                PointManager.Instance.SaveFinalScores();
            }
            SceneManager.LoadScene("EndOfGame");
        }
    }
}
