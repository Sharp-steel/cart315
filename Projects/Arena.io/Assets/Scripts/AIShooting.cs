using UnityEngine;
using Pathfinding;

public class AIShooting : MonoBehaviour
{
    public float range = 6f;

    public GunController gun;
    
    private Transform target;
    private AIDestinationSetter ai;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ai = GetComponent<AIDestinationSetter>();
        gun = GetComponent<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ai == null || ai.target == null || gun == null) return;
        target = ai.target;
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= range)
        {
            AimAtTarget();
            gun.Shooting();
        }
    }

    void AimAtTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle  = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
