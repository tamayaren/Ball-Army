using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.VirtualTexturing;

public class Humanoid : MonoBehaviour
{
    public float health
    {
        get => this._health;
        set
        {
            this._health = Mathf.Clamp(Mathf.Round(value), 0, this.maxHealth);

            if (this._health <= 0f)
            {
                this.state = HumanoidState.Dead;
            }
        }
    }
    private float _health;

    public float maxHealth
    {
        get => this._maxHealth;
        set => this._maxHealth = value;
    }
    private float _maxHealth;

    public UnityEvent<HumanoidState> StateChanged = new UnityEvent<HumanoidState>();

    public HumanoidState state
    {
        get => this._state;
        set
        {
            this._state = value;
            this.StateChanged?.Invoke(value);
        }
    }
    private HumanoidState _state;
    public bool iFrame = false;
    
    public bool Damage(float damage)
    {
        if (this.iFrame) return false;
        if (this.state == HumanoidState.Dead) return false;
        
        this.health -= damage;
        return true;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(this.transform.position + new Vector3(0,1f, 0), 
            $"State: {this.state}\nHealth: {this.health}\nMaxHealth: {this.maxHealth}");
    }
#endif
}

public enum HumanoidState
{
    Alive,
    Dead
}
