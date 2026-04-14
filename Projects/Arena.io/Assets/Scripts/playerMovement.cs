using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed;
    
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        
        Vector2 movement = new Vector2(x,y).normalized;
        body.linearVelocity = movement * speed;
    }
}