using UnityEngine;
using UnityEngine.SceneManagement;

public class switchToStartScreen : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        if (PointManager.Instance != null)
        {
            PointManager.Instance.ResetScores();
        }
        
        SceneManager.LoadScene(sceneName);
    }
}
