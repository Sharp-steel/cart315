using UnityEngine;

public class bulletDamage : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth playerHealth = collision.gameObject.GetComponent<playerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        
        else if (collision.gameObject.tag == "Teammate")
        {
            teammateHealth teammateHealth = collision.gameObject.GetComponent<teammateHealth>();

            if (teammateHealth != null)
            {
                teammateHealth.TakeDamage(damage);
            }
        }
        
        else if (collision.gameObject.tag == "Enemy")
        {
            enemyHealth enemyHealth = collision.gameObject.GetComponent<enemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
