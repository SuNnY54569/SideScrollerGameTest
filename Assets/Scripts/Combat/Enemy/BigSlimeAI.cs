using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeAI : SlimeAI
{
    [Header("Projectile Attack Settings")]
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private Transform projectileSpawnPoint;

    private float lastShootTime;
    
    protected override void Update()
    {
        if (enemyHealth.IsDying) return;
        
        base.Update();
        TryShootAtPlayer();
    }
    
    private void TryShootAtPlayer()
    {
        if (!PlayerInRange()) return;

        if (Time.time - lastShootTime >= shootCooldown)
        {
            ShootProjectileAtPlayer();
            lastShootTime = Time.time;
        }
    }
    
    private void ShootProjectileAtPlayer()
    {
        Vector2 targetPosition = player.position;
        Vector2 shootDirection = (targetPosition - (Vector2)projectileSpawnPoint.position).normalized;

        GameObject projectile = ObjectPoolManager.Instance.Get("SlimeProjectile");
        if (projectile != null)
        {
            projectile.transform.position = projectileSpawnPoint.position;
            projectile.transform.rotation = Quaternion.identity;

            SlimeProjectile slimeProjectile = projectile.GetComponent<SlimeProjectile>();
            if (slimeProjectile != null)
            {
                slimeProjectile.SetDirection(shootDirection);
            }
        }
    }
}
