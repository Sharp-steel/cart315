using UnityEngine;
using UnityEngine.Rendering;

public class DayNightCycle : MonoBehaviour
{
    public Volume volume;
    public float tick;
    public float seconds;
    public float mins;
    public SpriteRenderer[] squares;

    private Color[] originalColors;
    public bool freezeTime = false;

    void Start()
    {
        volume = gameObject.GetComponent<Volume>();
        
        originalColors = new Color[squares.Length];
        for (int i = 0; i < squares.Length; i++)
        {
            originalColors[i] = squares[i].color;
        }
    }

    void FixedUpdate()
    {
        if (!freezeTime)
        {
            Timer();
        }
    }

    public void Timer()
    {
        seconds += Time.fixedDeltaTime * tick;

        if (seconds >= 15f)
        {
            seconds = 0f;
            mins += 0.25f;
        }
        
        if (mins >= 10f)
        {
            mins = 0f;
            seconds = 0f;
            freezeTime = true;
            return;
        }

        ControlVolume();
    }

    public void ControlVolume()
    {
        float t = seconds / 15f;

        // Turn to Day
        if ((mins >= 0.5f && mins < 0.66f) ||
            (mins >= 3f && mins < 3.16f) ||
            (mins >= 5.5f && mins < 5.66f))
        {
            volume.weight = 1 - t;

            for (int i = 0; i < squares.Length; i++)
            {
                squares[i].color = Color.Lerp(originalColors[i], Color.white, t);
            }
        }

        // Turn to Night
        if ((mins >= 2.5f && mins < 2.66f) ||
            (mins >= 5f && mins < 5.16f) ||
            (mins >= 7.5f && mins < 7.66f))
        {
            volume.weight = t;

            for (int i = 0; i < squares.Length; i++)
            {
                squares[i].color = Color.Lerp(Color.white, originalColors[i], t);
            }
        }
    }
}