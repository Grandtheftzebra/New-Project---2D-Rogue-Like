using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

namespace UnityUtilities.LowLevel
{
    public static class PlayerLoopUtilities
    {
        /// <summary>
        /// Recursively iterates over every subsystem if the root playerLoopSystem type is not equal to the given generic type of this method until it either finds a match or not.
        /// Creates a list of PlayerLoopSystem. If the list is not null, we add the subSystemList of the playerLoopSys to the list.
        /// Then we check for any system in the list that matches the systemToRemove by type and updateDelegate.
        /// If a match is found, we remove it from the list and update the subSystemList of the playerLoopSys.
        /// Finally, we handle any nested subsystems for removal.
        /// </summary>
        /// <param name="playerLoopSys">The PlayerLoopSystem object to be modified, passed by reference.</param>
        /// <param name="systemToRemove">The PlayerLoopSystem object to remove.</param>
        /// <typeparam name="T">The type used to identify where to remove the system.</typeparam>
        public static void RemoveSystem<T>(ref PlayerLoopSystem playerLoopSys, in PlayerLoopSystem systemToRemove)
        {
            if (playerLoopSys.subSystemList == null) 
                return;

            var playerLoopSystemList = new List<PlayerLoopSystem>(playerLoopSys.subSystemList);
            for (int i = 0; i < playerLoopSystemList.Count; i++)
            {
                if (playerLoopSystemList[i].type == systemToRemove.type &&
                    playerLoopSystemList[i].updateDelegate == systemToRemove.updateDelegate)
                {
                    playerLoopSystemList.RemoveAt(i);
                    playerLoopSys.subSystemList = playerLoopSystemList.ToArray();
                }
            }

            HandleSubSystemForRemoval<T>(ref playerLoopSys, systemToRemove);
        }

        /// <summary>
        /// Recursively iterates over the subSystemList of the given playerLoopSys to handle nested subsystem removal.
        /// An empty subSystemList of the passed ref argument playerLoopSys is the recursion anchor.
        /// If not empty, it iterates over each entry in the subSystemList and calls the RemoveSystem method on it.
        /// </summary>
        /// <param name="playerLoopSys">The PlayerLoopSystem object to be modified, passed by reference.</param>
        /// <param name="systemToRemove">The PlayerLoopSystem object to remove.</param>
        /// <typeparam name="T">The type used to identify where to remove the system.</typeparam>
        private static void HandleSubSystemForRemoval<T>(ref PlayerLoopSystem playerLoopSys, in PlayerLoopSystem systemToRemove)
        {
            if (playerLoopSys.subSystemList == null)
                return;

            for (int i = 0; i < playerLoopSys.subSystemList.Length; i++)
            {
                RemoveSystem<T>(ref playerLoopSys.subSystemList[i], systemToRemove);
            }
        }

        /// <summary>
        /// Recursively iterates over every subsystem if the root playerLoopSystem type is not equal to the given generic type of this method until it either finds a match or not.
        /// Creates a list of PlayerLoopSystem, and if this is not null, we add the subSystemList of the playerLoopSys to the end of the list via AddRange.
        /// Then we insert our systemToInsert to that list at the specified index.
        /// Finally, we assign our created playerLoopSystemList as the subSystemList of the playerLoopSys.
        /// </summary>
        /// <param name="playerLoopSys">The PlayerLoopSystem object to be modified, passed by reference.</param>
        /// <param name="systemToInsert">The PlayerLoopSystem object to insert.</param>
        /// <param name="index">The index at which to insert the systemToInsert.</param>
        /// <typeparam name="T">The type used to identify where to insert the system.</typeparam>
        /// <returns>True if the system was successfully inserted, otherwise false.</returns>
        public static bool InsertSystem<T>(
            ref PlayerLoopSystem playerLoopSys, 
            in PlayerLoopSystem systemToInsert,
            int index)
        {
            if (playerLoopSys.type != typeof(T))
                return HandleSubSystemLoop<T>(ref playerLoopSys, in systemToInsert, index);

            List<PlayerLoopSystem> playerLoopSystemList = new();
            
            if (playerLoopSys.subSystemList != null)
                playerLoopSystemList.AddRange(playerLoopSys.subSystemList);
            
            playerLoopSystemList.Insert(index, systemToInsert);
            playerLoopSys.subSystemList = playerLoopSystemList.ToArray();

            return true;
        }

        /// <summary>
        /// Recursively iterates over the subSystemList of the given ref playerLoopSys.
        /// An empty subSystemList of the passed ref argument playerLoopSys is the recursion anchor.
        /// If not empty, it iterates over each entry in the subSystemList and calls the InsertSystem method on it.
        /// If it finds no match it will automatically return false;
        /// </summary>
        /// <param name="playerLoopSys">The PlayerLoopSystem object to be modified, passed by reference.</param>
        /// <param name="systemToInsert">The PlayerLoopSystem object to insert.</param>
        /// <param name="index">The index at which to insert the systemToInsert.</param>
        /// <typeparam name="T">The type used to identify where to insert the system.</typeparam>
        /// <returns>True if the system was successfully inserted, otherwise false.</returns>
        private static bool HandleSubSystemLoop<T>(ref PlayerLoopSystem playerLoopSys, in PlayerLoopSystem systemToInsert, int index)
        {
            if (playerLoopSys.subSystemList == null)
                return false;

            for (int i = 0; i < playerLoopSys.subSystemList.Length; i++)
            {
                if(!InsertSystem<T>(ref playerLoopSys.subSystemList[i], in systemToInsert, index))
                    continue;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Displays the PlayerLoop.
        /// Creates a StringBuilder object to store the different subsystems in the PlayerLoopSystem.
        /// Iterates over each subsystem in the PlayerLoopSystem and appends its information to the StringBuilder.
        /// Once all subsystems are processed, the information is displayed in the Logs.
        /// </summary>
        /// <param name="playerLoopSys">The PlayerLoopSystem object to iterate over.</param>
        public static void PrintPlayerLoop(PlayerLoopSystem playerLoopSys)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Unity Player Loop started:");

            foreach (PlayerLoopSystem subSystem in playerLoopSys.subSystemList)
                PrintSubSystem(subSystem, sb, 0);

            Debug.Log(sb.ToString());
        }

        /// <summary>
        /// Helper method for PrintPlayerLoop.
        /// Recursively iterates over each passed subsystem and appends its information to the StringBuilder.
        /// Subsystems of the passed PlayerLoopSystem are indented with -- based on their depth level.
        /// </summary>
        /// <param name="system">The subsystem from the PrintPlayerLoop method.</param>
        /// <param name="sb">The StringBuilder object from the PrintPlayerLoop method.</param>
        /// <param name="level">The current depth level of traversal in the subsystem hierarchy.</param>
        private static void PrintSubSystem(PlayerLoopSystem system, StringBuilder sb, int level)
        {
            sb.Append('-', level * 2).AppendLine(system.type.ToString());

            if (system.subSystemList == null || system.subSystemList.Length == 0) // Recursion anchor
                return;

            foreach (PlayerLoopSystem subSystem in system.subSystemList)
                PrintSubSystem(subSystem, sb, level + 1);
        }
    }
}