using System;
using UnityEngine;

namespace ImprovedTimers
{
    public abstract class Timer : IDisposable
    {
        public float CountDownTime { get; protected set; }
        public bool IsRunning { get; private set; }

        protected float startTime;

        public float Progress => Mathf.Clamp01(CountDownTime / startTime);

        public event Action OnTimerStart;
        public event Action OnTimerStop;

        protected Timer(float startTime)
        {
            this.startTime = startTime;
        }

        public void Start()
        {
            CountDownTime = startTime;

            if (!IsRunning)
            {
                IsRunning = true;
                TimerManager.AddTimer(this);
                OnTimerStart?.Invoke();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                TimerManager.RemoveTimer(this);
                OnTimerStop?.Invoke();
            }
        }
        
        public abstract void Tick();
        public abstract bool IsFinished { get; }

        public void Pause() => IsRunning = false;
        public void Continue() => IsRunning = true;

        public virtual void ResetTimer() => CountDownTime = startTime;
        public virtual void ResetWithNewTime(float newTime)
        {
            startTime = newTime;
            ResetTimer();
        }
        
        private bool isDisposed;
        
        ~Timer()
        {
            Dispose(false);
        }

        // Call Dispose to ensure deregestration of the timer from the TimerManager
        // when the consumer is done with the timer or being destroyed
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposed)
                return;
            
            if (isDisposing)
                TimerManager.RemoveTimer(this);

            isDisposed = true;
        }
    }
}