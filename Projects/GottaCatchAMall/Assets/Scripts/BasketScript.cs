using UnityEngine;

public class BasketScript : MonoBehaviour {
    public KeyCode left;
    public KeyCode right;

    public float speed = 1f;

    public float basket_x, basket_y;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(left)) {
            //move left
            basket_x -= speed;
        }
           
        if (Input.GetKeyDown(right)) {
            //move right
            basket_x += speed;
        }
        
        transform.position = new Vector3(basket_x, -4.0f, 0);
    }
}
