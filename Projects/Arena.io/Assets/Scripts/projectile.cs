using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    private float activeTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ShootBullet(Transform shootPoint)
    {
        activeTimer = 0;
        rb.velocity = Vector2.zero;
        transform.position = shootPoint.position;
        transform.rotation = shootPoint.rotation;
        gameObject.SetActive(true);
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        activeTimer += Time.deltaTime;
        if (activeTimer >= lifetime)
            gameObject.SetActive(false);
    }
}
