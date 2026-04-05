using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;

    private float activeTimer;
    
    private Collider2D myCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    public void ShootBullet(Transform shootPoint)
    {
        activeTimer = 0;
        rb.linearVelocity = Vector2.zero;
        transform.position = shootPoint.position;
        transform.rotation = shootPoint.rotation;
        
        Collider2D shooterCollider = shootPoint.GetComponentInParent<Collider2D>();
        if (shooterCollider != null)
        {
            Physics2D.IgnoreCollision(myCollider, shooterCollider);
        }
        
        gameObject.SetActive(true);
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        activeTimer += Time.deltaTime;
        if (activeTimer >= lifetime)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
