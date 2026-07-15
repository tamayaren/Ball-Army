using UnityEngine;

public class KatanaCore : UnitCore
{
    public override float unitHealth { get; set; } = 135f;
    public override float unitRange { get; set; } = 50f;
    public override float unitSpeed { get; set; } = 4f;
    public override float actionCooldown { get; set; } = 1.5f;

    private float damage = 65f;
    private float pollingRate = .2f;
    private float pollingTime;

    protected float swordRange = 6f;

    private void Start()
    {
        TryGetComponent(out this.humanoid);
        TryGetComponent(out this.team);
    } 

    private void Decide()
    {
        if (this.target)
        {
            this.target.TryGetComponent(out Humanoid targetHumanoid);
            float distance = Vector3.Distance(this.transform.position, this.target.position);
            if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh) this.agent.SetDestination(this.target.position);
            
            this.agent.updateRotation = (distance >= this.swordRange);
            this.agent.isStopped = (distance <= this.swordRange);
            if (distance <= this.swordRange && this.canAct)
            {
                Quaternion targetRotation = Quaternion.LookRotation(this.target.position - this.transform.position);
                
                DoAction();
                this.transform.rotation = targetRotation;
                this.animator.SetTrigger("Attack");
                
                targetHumanoid.Damage(Random.Range(this.damage - 5f, this.damage + 5f));
                this.transform.position += this.transform.forward * this.unitSpeed;
            }
        }
    }
    
    private void Update()
    {
        base.Update();
        
        Decide();
    }
    
}
