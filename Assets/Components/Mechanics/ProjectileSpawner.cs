using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    public void SpawnProjectile(Transform origin, Vector3 target, float speed, float damage, Team team, float spread)
    {
        GameObject projectile = Instantiate(this.projectilePrefab, origin.position, origin.rotation);
        projectile.TryGetComponent(out Projectile projectileComponent);
        
        projectileComponent.Initialize(speed, team, damage);
        projectile.transform.LookAt(target + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread)));
    }
}
