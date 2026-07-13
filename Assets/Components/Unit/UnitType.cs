using System;
using Unity.VisualScripting;
using UnityEngine;

public class UnitType : MonoBehaviour
{
    public Unit type;

    [SerializeReference] private UnitCore[] coreComponent;

    private void Awake()
    {
        UnitCore unitCore = this.coreComponent[(int)this.type];
        if (unitCore != null)
        {
            Type unitType = unitCore.GetType();
            this.gameObject.AddComponent(unitType);
        }
    }
}
    
public enum Unit
{
    Swordsman,
    Archer,
    Katana,
    Sorcerer
}