using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponConfiguration : ScriptableObject
{
    [Header("Info")]
    public string weaponName;


    [Header("Forces applied to bullets")]
    public float shootForce;
    public float upwardForce;

    [Header("Shooting")]
    public float timeBetweenShooting;
    public float spread;
    public float timeBetweenShots;
    public float reloadTime;

    public int bulletsPerTap;

    [Header("Allow Press Button")]
    public bool allowButtonHold;


    [Header("Reloading")]
    public int magazineSize;
    public int bulletsLeft;
    public int bulletsShot;

    public float fireRate;

    public GameObject bullet;

    public bool allowInvoke;
}
