using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[Serializable]
public class UnitCore : MonoBehaviour
{
    public virtual float unitHealth { get; set; } = 100f;
    public virtual float unitRange { get; set; } = 100f;
    public virtual float unitSpeed { get; set; } = 6f;
    public virtual float actionCooldown { get; set; } = .15f;

    private float pollingRate = .2f;
    private float pollingTime;

    public bool canAct {
        get => this._canAct;
        set
        {
            this._canAct = value;
            this.ActionChanged.Invoke(value);
        }
    }

    private bool _canAct = true;
    public UnityEvent<bool> ActionChanged = new UnityEvent<bool>();
    
    public NavMeshAgent agent;
    public Transform target;
    public UnitTeam team;
    public Humanoid humanoid;
    public Animator animator;

    protected void Awake()
    {
        TryGetComponent(out this.agent);
        TryGetComponent(out this.team);
        TryGetComponent(out this.humanoid);
        TryGetComponent(out this.animator);

        this.agent.speed = this.unitSpeed;
        if (this.humanoid != null)
        {
            this.humanoid.maxHealth = this.unitHealth;
            this.humanoid.health = this.unitHealth;
        }
        
        this.humanoid.StateChanged.AddListener(state =>
        {
            if (state == HumanoidState.Dead)
            {
                this.animator.SetTrigger("Dead");
                this.agent.isStopped = true;
                this.agent.enabled = false;

                StartCoroutine(Destroy());
            }
        });
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    private void FindTarget()
    {
        if (!this.target)
            this.target = AIEvaluator.instance.FindTarget(this.team, this.unitRange);
        else
        {
            this.target.TryGetComponent(out Humanoid targetHumanoid);
            float distance = Vector3.Distance(this.transform.position, this.target.position);

            if (targetHumanoid != null)
                this.target = targetHumanoid.state == HumanoidState.Dead ? null : this.target;
            
            if (distance > this.unitRange)
                this.target = null;
        }
    }

    private IEnumerator ActionCooldown()
    {
        yield return new WaitForSeconds(this.actionCooldown);
        this.canAct = !this.canAct;
    }

    public void DoAction()
    {
        this.canAct = false;
        StartCoroutine(ActionCooldown());
    }

    public void Update()
    {
        this.pollingTime += Time.deltaTime;
        if (this.pollingTime >= this.pollingRate)
        {
            this.pollingTime = 0f;
            FindTarget();
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(this.transform.position + new Vector3(0,5f, 0), 
            $"CanAct: {this.canAct}");
    }
#endif
}
