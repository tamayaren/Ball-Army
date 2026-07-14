using UnityEngine;

public class ArcherCore : UnitCore
{
    public override float unitHealth { get; set; } = 125f;
    public override float unitRange { get; set; } = 50f;
    public override float unitSpeed { get; set; } = 8f;
    public override float actionCooldown { get; set; } = .25f;

    private float damage = 15f;
    private float pollingRate = .2f;
    private float pollingTime;

    protected float archerRange = 10f;

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
            this.agent.SetDestination(this.target.position);
            
            this.agent.updateRotation = (distance >= this.archerRange);
            this.agent.isStopped = (distance <= this.archerRange);
            if (distance <= this.archerRange && this.canAct)
            {
                Quaternion targetRotation = Quaternion.LookRotation(this.target.position - this.transform.position);
                
                DoAction();
                this.transform.rotation = targetRotation;
                this.animator.SetTrigger("Attack");
                
                targetHumanoid.Damage(Random.Range(this.damage - 5f, this.damage + 5f));
            }
        }
    }
    
    private void Update()
    {
        base.Update();
        
        Decide();
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.unitRange);
    }
#endif
}
