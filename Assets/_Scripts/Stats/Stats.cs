using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private readonly StatsMediator _mediator;
    private readonly BaseStats _baseStats;

    public StatsMediator Mediator => _mediator; 

    public Stats(StatsMediator mediator, BaseStats baseStats)
    {
        _mediator = mediator;
        _baseStats = baseStats;
    }

    public int Armor
    {
        get
        {
            StatQuery query = new StatQuery(StatType.Armor, _baseStats.Armor);
            _mediator.PerformStatQuery(this, query);

            return query.StatValue;
        }
    }

    public int Health
    {
        get
        {
            StatQuery query = new StatQuery(StatType.Health, _baseStats.Health);
            _mediator.PerformStatQuery(this, query);

            return query.StatValue;
        }
    }

    public override string ToString() => $"Armor Value is {Armor} | Health Value is {Health}";
}
public enum StatType 
{ 
    Armor,
    Health
}
