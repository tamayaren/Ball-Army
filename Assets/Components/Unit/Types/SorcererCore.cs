using UnityEngine;

public class SorcererCore : UnitCore
{
    public override float unitHealth { get; set; } = 125f;
    public override float unitRange { get; set; } = 50f;
    public override float unitSpeed { get; set; } = 8f;
    public override float actionCooldown { get; set; } = 2f;
    
    private ProjectileSpawner projectileSpawner;
    private float damage = 10f;
    private float pollingRate = .2f;
    private float pollingTime;
    

    protected float archerRange = 10f;

    private void Start()
    {
        TryGetComponent(out this.humanoid);
        TryGetComponent(out this.team);
        TryGetComponent(out this.projectileSpawner);
    } 

    private void Decide()
    {
        if (this.target)
        {
            this.target.TryGetComponent(out Humanoid targetHumanoid);
            float distance = Vector3.Distance(this.transform.position, this.target.position);
            if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh) this.agent.SetDestination(this.target.position);
            
            this.agent.updateRotation = (distance >= this.archerRange);
            this.agent.isStopped = (distance <= this.archerRange);
            if (distance <= this.archerRange && this.canAct)
            {
                Quaternion targetRotation = Quaternion.LookRotation(this.target.position - this.transform.position);
                
                DoAction();
                this.transform.rotation = targetRotation;
                this.animator.SetTrigger("Attack");
                
                for (int i = 0; i < 6; i++)
                    this.projectileSpawner.SpawnProjectile(this.transform, this.target.position, Random.Range(.1f, .3f), this.damage, this.team.team, 5f);
            }
        }
    }
    
    private void Update()
    {
        base.Update();
        
        Decide();
    }
    
}
