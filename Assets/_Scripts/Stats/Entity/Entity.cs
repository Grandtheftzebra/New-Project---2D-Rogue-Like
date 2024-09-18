using System;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Entity : MonoBehaviour, IVisitable
{
    [SerializeField, InlineEditor, Required] public BaseStats _baseStats;
    public Stats Stats { get; private set; }

    public virtual void Awake()
    {
        Stats = new Stats(new StatsMediator(), _baseStats);
    }

    public virtual void Update()
    {
        Stats.Mediator.Update();
    }

    public void Accept(IVisitor visitor) => visitor.Visit(this);
}
