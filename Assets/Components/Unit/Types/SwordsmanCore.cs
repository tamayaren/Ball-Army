using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SwordsmanCore : UnitCore
{
    public override float unitHealth { get; set; } = 200f;
    public override float unitRange { get; set; } = 50f;
    public override float unitSpeed { get; set; } = 8f;
    public override float actionCooldown { get; set; } = 1f;

    private float damage = 25f;
    private float pollingRate = .2f;
    private float pollingTime;

    protected float swordRange = 3f;

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
            }
        }
    }
    
    private void Update()
    {
        base.Update();
        
        Decide();
    }
    
}
