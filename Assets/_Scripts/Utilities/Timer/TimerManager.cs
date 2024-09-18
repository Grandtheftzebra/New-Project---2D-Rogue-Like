using System.Collections.Generic;

namespace ImprovedTimers
{
    /// <summary>
    /// Managers all of our current Timers.
    /// Timers get constantly updated via UpdateAllTimer
    /// We can add timer, remove a specific one or remove all timers.
    /// </summary>
    public static class TimerManager
    {
        private static readonly List<Timer> timers = new();

        public static void AddTimer(Timer timer) => timers.Add(timer);
        public static void RemoveTimer(Timer timer) => timers.Remove(timer);

        public static void UpdateAllTimer()
        {
            // Why use new List<Timer>(timers):
            // 1. Avoids Exceptions: Since the loop iterates over a copy of the list, modifications to the original timers list during the iteration won’t affect the iteration itself, thus avoiding InvalidOperationException.
            // 2. Predictable Behavior: The copied list is static during the loop’s execution. Any changes to the original timers list (e.g., adding or removing timers) won’t impact the ongoing iteration, leading to more predictable and stable behavior.
            
            foreach (var timer in new List<Timer>(timers)) 
                timer.Tick();
        }

        public static void RemoveAllTimer() => timers.Clear();
    }
}