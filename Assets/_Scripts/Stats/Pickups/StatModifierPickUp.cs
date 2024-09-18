using System;
using UnityEngine;

public class StatModifierPickUp : Pickup
{
    [SerializeField] private StatType _statType = StatType.Armor;
    [SerializeField] private StatOperations _statOperation;
    [SerializeField] private int _value = 10;
    [SerializeField] private float _duration = 2f;
    
    protected override void GainPickUpEffect(Entity entity)
    {
        StatModifier _statModifier = _statOperation switch
        {
            StatOperations.Add => new SimpleStatModifier(_duration, _statType, (x) => x + _value),
            StatOperations.Multiply => new SimpleStatModifier(_duration, _statType, (x) => x * _value),
            StatOperations.Remove => new SimpleStatModifier(_duration, _statType, (x) => x - _value),
            _ => throw new ArgumentOutOfRangeException("Error while pattern matching a viable StatOperation. Check script StatModifierPickUp")
        };
        
        entity.Stats.Mediator.AddStatModifierAndRegisterQuery(_statModifier); // TODO: Find out why stat is not adjusted
        
        //Debug.Log(entity.Stats); // TODO: Stats is null, via Awake in the Entity class, no idea why. It is proper initialized when put in PlayerController.
    }

}
public enum StatOperations
{
    Add,
    Multiply,
    Remove
}
