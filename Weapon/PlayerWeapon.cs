using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "WeaponVariant")]
public class PlayerWeapon : ScriptableObject
{
    public GameObject _projectile;
    public WeaponLevel _weponLevel;
    [SerializeField] private WeaponStats _weaponStats;
    public WeaponStats BaseWeaponStats => _weaponStats;


}



[Serializable]
public struct WeaponStats

{
    public float _fireRate;
    public float _projSpeed;

}

[Serializable]
public enum WeaponLevel

{
    Basic = 0,
    First = 1,
    Second = 2,
}
