using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityUtilities.LowLevel;

namespace ImprovedTimers
{
    internal static class TimerBootstrapper
    {
        private static PlayerLoopSystem _timerSystem; 
        
        /// <summary>
        /// [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        /// is added because we want to run Initialize as soon as all the Assemblies have been loaded.
        ///
        /// 
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)] 
        internal static void Initialize()
        {
            PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (!InsertTimerManager<Update>(ref currentPlayerLoop, 0))
            {
                Debug.LogWarning($"Improved Timers not initialized, unable to register TimerManagaer into the Update Loop");
                return;
            }
            
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);
            PlayerLoopUtilities.PrintPlayerLoop(currentPlayerLoop);

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeState;
            EditorApplication.playModeStateChanged += OnPlayModeState;

            // Removes our custom playerLoop insertions after exiting play mode
            static void OnPlayModeState(PlayModeStateChange state)
            {
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
                    RemoveTimerManager<Update>(ref currentPlayerLoop);
                    PlayerLoop.SetPlayerLoop(currentPlayerLoop);
                    
                    TimerManager.RemoveAllTimer(); // Have to use it because Unity does not guarantee the removal of all statics
                }
            }
#endif
        }
        

        /// <summary>
        /// Generate static method to select in which PlayerLoop the TimerManager is implemented.
        /// TimerManager is created via obect initialization. We need to specify it's type and pass in a method that is stored in the updateDelegate.
        /// After object creation we call InsertSystem and pass the (ref playerLoopSys) and (in _timerSystem) and the index
        /// </summary>
        /// <param name="playerLoopSys">Ref parameter that contains a playerLoopSystem</param>
        /// <param name="index">The index where we place the PlayerLoopSystem</param>
        /// <typeparam name="T">Can be any main system of the PlayerLoopSystem type</typeparam>
        /// <returns></returns>
        private static bool InsertTimerManager<T>(ref PlayerLoopSystem playerLoopSys, int index)
        {
            _timerSystem = new PlayerLoopSystem()
            {
                type = typeof(TimerManager),
                updateDelegate = TimerManager.UpdateAllTimer,
                subSystemList = null
            };
            
            return PlayerLoopUtilities.InsertSystem<T>(ref playerLoopSys, in _timerSystem, index);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerLoopSys"></param>
        /// <typeparam name="T"></typeparam>
        private static void RemoveTimerManager<T>(ref PlayerLoopSystem playerLoopSys)
        {
            PlayerLoopUtilities.RemoveSystem<T>(ref playerLoopSys, in _timerSystem);
        }
    }
}