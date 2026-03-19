using UnityEngine;

public class player : MonoBehaviour

{
    public Rigidbody2D body;
    public float speed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        Vector2 movement = new Vector2(x,y).normalized;
        body.linearVelocity = movement * speed;
    }
}
