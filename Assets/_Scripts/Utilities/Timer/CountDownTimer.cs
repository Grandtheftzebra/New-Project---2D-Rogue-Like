using UnityEngine;

namespace ImprovedTimers
{
    public class CountDownTimer : Timer
    {
        public CountDownTimer(float startTime) : base(startTime)
        {
        }
        
        public override void Tick()
        {
            if (IsRunning && CountDownTime > 0)
                CountDownTime -= Time.deltaTime;
            
            if (IsRunning && CountDownTime <= 0)
                Stop();
        }

        public override bool IsFinished => CountDownTime <= 0;
    }
}