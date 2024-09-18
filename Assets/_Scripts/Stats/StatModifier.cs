using System;
using ImprovedTimers;
using UnityEngine;

public class SimpleStatModifier : StatModifier
{
    private readonly StatType _statType;
    private readonly Func<int, int> _adjustStat;
    
    public SimpleStatModifier(float duration, StatType statType, Func<int, int> adjustStat) : base(duration)
    {
        _statType = statType;
        _adjustStat = adjustStat;
    }

    public override void HandleQuery(object sender, StatQuery query)
    {
        if (query.StatType == _statType)
            query.StatValue = _adjustStat(query.StatValue);
    }
}

public abstract class StatModifier : IDisposable
{
    public bool IsDisposable { get; private set; }
    
    public event Action<StatModifier> OnDispose;
    private CountDownTimer _timer;
    
    protected StatModifier(float duration)
    {
        if (duration <= 0)
            return;

        _timer = new CountDownTimer(duration);
        Debug.Log($"Timer created with duration of {duration}");
        _timer.OnTimerStop += () => { IsDisposable = true;};
        _timer.Start();
    }
    
    public abstract void HandleQuery(object sender, StatQuery statQuery);

    public void Dispose() => OnDispose?.Invoke(this);
    
}