using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerWeaponLogic : MonoBehaviour
{
    //References
    [SerializeField] private WeaponConfiguration _weaponConfiguration;
    private Camera _camera;
    private bool shooting, readyToShoot, reloading;
    private GameObject _currentBullet;

    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public Transform attackPoint;

    private void Awake()
    {
        _weaponConfiguration.bulletsLeft = _weaponConfiguration.magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        FireInput();

        if (ammunitionDisplay) ammunitionDisplay.SetText(
            _weaponConfiguration.bulletsLeft / _weaponConfiguration.bulletsPerTap + "/" + _weaponConfiguration.magazineSize / _weaponConfiguration.bulletsPerTap);
    }

    private void FireInput()
    {
        //Allowed to hold fire button?
        if (_weaponConfiguration.allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Shooting
        if (readyToShoot && shooting && !reloading && _weaponConfiguration.bulletsLeft > 0)
        {
            _weaponConfiguration.bulletsShot = 0;

            Fire();
        }

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && _weaponConfiguration.bulletsLeft < _weaponConfiguration.magazineSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && _weaponConfiguration.bulletsLeft <= 0) Reload();
    }

    private void Fire()
    {
        readyToShoot = false;
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = UnityEngine.Random.Range(-_weaponConfiguration.spread, _weaponConfiguration.spread);
        float y = UnityEngine.Random.Range(-_weaponConfiguration.spread, _weaponConfiguration.spread);

        Vector3 directionWithSpread = directionWithoutSpread + (new Vector3(x,y,0));
        _currentBullet = Instantiate(_weaponConfiguration.bullet, attackPoint.position, Quaternion.identity);
        _currentBullet.transform.forward = directionWithSpread.normalized;

        _currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * _weaponConfiguration.shootForce, ForceMode.Impulse);
        _currentBullet.GetComponent<Rigidbody>().AddForce(_camera.transform.up * _weaponConfiguration.upwardForce, ForceMode.Impulse);

        if (muzzleFlash) Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);


        _weaponConfiguration.bulletsLeft--;
        _weaponConfiguration.bulletsShot++;

        if (_weaponConfiguration.allowInvoke)
        {
            Invoke("ResetFire", _weaponConfiguration.timeBetweenShooting);
            _weaponConfiguration.allowInvoke = false;
        }

        if (_weaponConfiguration.bulletsShot < _weaponConfiguration.bulletsPerTap && _weaponConfiguration.bulletsLeft > 0)
        {
            Invoke("Fire", _weaponConfiguration.timeBetweenShots);
        }

    }

    private void ResetFire()
    {
        _weaponConfiguration.allowInvoke = true;
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", _weaponConfiguration.reloadTime);
    }

    private void ReloadFinised()
    {
        _weaponConfiguration.bulletsLeft = _weaponConfiguration.magazineSize;
        reloading = false;
    }
}
