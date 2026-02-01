using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public int score = 0;

    public TMP_Text scoreText;

    public static UIManager S;

    void Awake() {
        S = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // scoreText.text = "Test";
        UpdateScore(0);
    }

    // Update is called once per frame
    public void UpdateScore(int pointValue)
    {
        // How much are we updating the score by?
        int _pointValue = pointValue;
        // Add / Subtract that value
        score += _pointValue;
        // Make that new value a String
        string scoreString = score.ToString();
        // Change the score text to the new String
        scoreText.text = "Score: " + scoreString;
        
        // scoreText.text = (score += pointValue).ToString()
    }
}
