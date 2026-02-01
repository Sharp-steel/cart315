using UnityEngine;

public class BasketScript : MonoBehaviour {
    
    public float basket_x = 0;
    public float speed = .1f;
    
    public KeyCode left, right;

    public float score;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(left) && basket_x > -9f) {
            basket_x -= speed;
            
        }
        if (Input.GetKey(right) && basket_x < 9f) {
            basket_x += speed;
        }

        this.transform.position = new Vector3(
            basket_x,
            transform.position.y,
            transform.position.z
        );
        
        transform.position = new Vector3(basket_x, -4.0f, 0);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.tag);
        // Get value of building points
        int pointValue;
        if (other.gameObject.tag == "Mall") pointValue = 1;
        else pointValue -= 1;
        
        // Tells the UIManager script how much to change the score by
        UIManager.S.UpdateScore(pointValue);

        Destroy(other.gameObject);
    }
}
