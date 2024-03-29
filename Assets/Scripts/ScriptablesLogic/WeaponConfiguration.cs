using UnityEngine;

public class WeaponConfiguration : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    [Header("Shooting")]
    public float cadence;
    

    [Header("Reloading")]
    public int maxCapacity;
    public float fireRate;

    public GameObject bullet;
}
