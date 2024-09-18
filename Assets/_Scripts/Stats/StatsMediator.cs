using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMediator : MonoBehaviour
{
    private readonly LinkedList<StatModifier> _statModifiers = new();

    public event EventHandler<StatQuery> StatQueries;
    public void PerformStatQuery(object sender, StatQuery query) => StatQueries?.Invoke(sender, query);

    public void AddStatModifierAndRegisterQuery(StatModifier statModifier)
    {
        Debug.Log($"{statModifier} added!");
        _statModifiers.AddLast(statModifier);
        StatQueries += statModifier.HandleQuery;

        statModifier.OnDispose += (_) =>
        {
            _statModifiers.Remove(statModifier);
            Debug.Log($"{statModifier} successfully disposed");
            StatQueries -= statModifier.HandleQuery;
        };
    }

    public void Update()
    {
        var node = _statModifiers.First;

        while (node != null)
        {
            var nextNode = node.Next;

            if (node.Value.IsDisposable)
            {
                node.Value.Dispose(); 
                Debug.Log($"Disposed was called");
            }

            node = nextNode;
        }
    }
}

public class StatQuery
{
    public StatType StatType { get; }
    public int StatValue;

    public StatQuery(StatType statType, int statValue)
    {
        StatType = statType;
        StatValue = statValue;
    }
}