using UnityEngine;
using UnityEngine.Rendering;

public class DayNightCycle : MonoBehaviour
{
    public Volume volume;
    public float tick;
    public float seconds;
    public float mins;
    public SpriteRenderer[] squares; 
    
    void Start()
    {
        volume = gameObject.GetComponent<Volume>();
    }
    
    void FixedUpdate() 
    {
        Timer();
    }
 
    public void Timer()
    {
        seconds += Time.fixedDeltaTime * tick;
        if (seconds >= 15)
        {
            seconds = 0;
            mins += 0.25f;
        }
        ControlVolume();
    }
 
    public void ControlVolume() //600 seconds = 10 minutes
    {
        if(mins>=0.5 && mins<0.66) //Turns to day
        {
            volume.weight = 1 - (float)seconds / 10;
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i].color = new Color(squares[i].color.r, squares[i].color.g, squares[i].color.b, 1 -(float)seconds / 10); // make squares colourless
            }
        }
 
        if(mins>=2.5 && mins<2.66)  //Turns to Night
        {
            volume.weight =  (float)seconds / 10;
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i].color = new Color(squares[i].color.r, squares[i].color.g, squares[i].color.b, (float)seconds / 10); // make squares coloured
            }
        }
        
        if(mins>=3 && mins<3.16) //Turns to day
        {
            volume.weight = 1 - (float)seconds / 10;
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i].color = new Color(squares[i].color.r, squares[i].color.g, squares[i].color.b, 1 -(float)seconds / 10); // make squares colourless
            }
        }
        
        if(mins>=5 && mins<5.16)  //Turns to Night
        {
            volume.weight =  (float)seconds / 10;
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i].color = new Color(squares[i].color.r, squares[i].color.g, squares[i].color.b, (float)seconds / 10); // make squares coloured
            }
        }
        
        if(mins>=5.5 && mins<5.66) //Turns to day
        {
            volume.weight = 1 - (float)seconds / 10;
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i].color = new Color(squares[i].color.r, squares[i].color.g, squares[i].color.b, 1 -(float)seconds / 10); // make squares colourless
            }
        }
        
        if(mins>=7.5 && mins<7.66)  //Turns to Night
        {
            volume.weight =  (float)seconds / 10;
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i].color = new Color(squares[i].color.r, squares[i].color.g, squares[i].color.b, (float)seconds / 10); // make squares coloured
            }
        }
    }
}