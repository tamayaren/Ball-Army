using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static float thickness = 1f;
    [SerializeField] private bool projectileEnabled = false;
    
    [SerializeField] private float speed = 10f;
    [SerializeField] private Team bulletTeam;
    [SerializeField] private float damage;

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
    
    public void Initialize(float newSpeed, Team originTeam, float originDamage)
    {
        this.speed = newSpeed;
        this.bulletTeam = originTeam;
        this.damage = originDamage;
        
        this.projectileEnabled = true;
        
        StartCoroutine(Despawn());
    }

    private void FixedUpdate()
    {
        if (!this.projectileEnabled) return;
        this.transform.position += this.transform.forward - new Vector3(0, -.15f, 0) * (this.speed * Time.fixedDeltaTime);
        
        RaycastHit hit;
        if (Physics.SphereCast(this.transform.position, thickness, Vector3.forward, out hit, 6f))
        {
            if (!hit.collider) return;
            if (hit.collider.CompareTag($"BodyPart"))
            {
                UnitTeam hitTeam = hit.collider.GetComponentInParent<UnitTeam>();
                Humanoid hitHumanoid =  hit.collider.GetComponentInParent<Humanoid>();
            
                if (hitTeam.team != this.bulletTeam)
                {
                    hitHumanoid.Damage(Random.Range(this.damage - 3f, this.damage + 3f));
                    Destroy(this.gameObject);
                }
            }    
        }
    }
}
