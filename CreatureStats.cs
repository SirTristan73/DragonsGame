using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Creature", menuName = "CreatureScriptableObject")]
public class CreatureStats : ScriptableObject
{   
    // Тут тоже пока срач, ещё в процессе раздумий
    public CreatureType CreatureType;
    public CurrentCreatureLevel CreatureLevel;
    [SerializeField] public GameObject _model;

    [SerializeField] private Stats _stats;
    public Stats BaseStats => _stats;
}

[Serializable]
public struct Stats
{
    public float _health;
    public float _speed;
    public int _points;
}

[Serializable]
public enum CreatureType
{
    Player = 0,
    Enemy = 1,
    Boss = 2,
}

[Serializable]
public enum CurrentCreatureLevel
{
    Default = 0,
    FirstUpgrade = 1,
    SecondUpgrade = 2,
}
