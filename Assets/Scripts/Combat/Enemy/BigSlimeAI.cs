using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeAI : SlimeAI
{
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private Transform projectileSpawn;
    private float lastShootTime;
    
    protected override void Update()
    {
        if (enemyHealth.IsDying) return;
        
        base.Update();
        HandleShooting();
    }
    
    private void HandleShooting()
    {
        if (PlayerInRange() && Time.time - lastShootTime >= shootCooldown)
        {
            Vector2 targetPosition = player.position;
            Vector2 shootDirection = (targetPosition - (Vector2)projectileSpawn.position).normalized;

            GameObject projectile = ObjectPoolManager.Instance.Get("SlimeProjectile");
            if (projectile != null)
            {
                projectile.transform.position = projectileSpawn.position;
                projectile.transform.rotation = Quaternion.identity;

                SlimeProjectile slimeProjectile = projectile.GetComponent<SlimeProjectile>();
                if (slimeProjectile != null)
                {
                    slimeProjectile.SetDirection(shootDirection);
                }
            }

            lastShootTime = Time.time;
        }
    }
}
