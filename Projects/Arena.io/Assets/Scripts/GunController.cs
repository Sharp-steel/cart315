using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private float cooldown = 0.25f;
    private float cooldownTimer;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firepoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    private void Shooting()
    {
        if (cooldownTimer < cooldown) return;
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation, null);
        bullet.GetComponent<projectile>().ShootBullet(firepoint);
        cooldownTimer = 0;
    }
    
    #region Input
    private void OnShoot()
    {
        Shooting();
    }
    #endregion
}
