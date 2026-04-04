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
            SceneManager.LoadScene("EndOfGame");
        }
    }
}
