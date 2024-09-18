using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "StatsContainer", menuName = "CreateStats")]
public class BaseStats : ScriptableObject
{
    public int Armor = 10;
    public int Health = 20;
}
