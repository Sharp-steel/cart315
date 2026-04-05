using UnityEngine;

public class bulletDamage : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        
        else if (collision.gameObject.tag == "Teammate")
        {
            Health teammateHealth = collision.gameObject.GetComponent<Health>();

            if (teammateHealth != null)
            {
                teammateHealth.TakeDamage(damage);
            }
        }
        
        else if (collision.gameObject.tag == "Enemy")
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
