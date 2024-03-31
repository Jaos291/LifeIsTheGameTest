using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerWeaponLogic : MonoBehaviour
{
    [SerializeField] private WeaponConfiguration _weaponConfiguration;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private TextMeshProUGUI ammunitionDisplay;
    [SerializeField] private Transform attackPoint;
    private Camera _camera;
    private bool shooting, readyToShoot, reloading;
    private GameObject _currentBulletInstance;

    private void Awake()
    {
        _weaponConfiguration.bulletsLeft = _weaponConfiguration.magazineSize;
        readyToShoot = false;
    }

    private void Start()
    {
        _camera = GameController.Instance.mainCamera;
    }

    private void Update()
    {
        HandleInput();
        UpdateAmmunitionDisplay();
    }

    private void HandleInput()
    {
        shooting = _weaponConfiguration.allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && shooting && !reloading && _weaponConfiguration.bulletsLeft > 0)
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && _weaponConfiguration.bulletsLeft < _weaponConfiguration.magazineSize && !reloading)
        {
            Reload();
        }

        if (readyToShoot && shooting && !reloading && _weaponConfiguration.bulletsLeft <= 0)
        {
            Reload();
        }
    }

    private void Fire()
    {
        readyToShoot = false;
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        // Apply spread to the direction
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        float x = UnityEngine.Random.Range(-_weaponConfiguration.spread, _weaponConfiguration.spread);
        float y = UnityEngine.Random.Range(-_weaponConfiguration.spread, _weaponConfiguration.spread);
        directionWithoutSpread += _camera.transform.right * x + _camera.transform.up * y;

        _currentBulletInstance = Instantiate(_weaponConfiguration.bullet, attackPoint.position, Quaternion.identity);
        _currentBulletInstance.transform.forward = directionWithoutSpread.normalized;
        _currentBulletInstance.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * _weaponConfiguration.shootForce, ForceMode.Impulse);
        _currentBulletInstance.GetComponent<Rigidbody>().AddForce(_camera.transform.up * _weaponConfiguration.upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        _weaponConfiguration.bulletsLeft--;
        if (_weaponConfiguration.allowInvoke)
        {
            StartCoroutine(ResetFireDelay());
        }
    }

    private IEnumerator ResetFireDelay()
    {
        yield return new WaitForSeconds(_weaponConfiguration.timeBetweenShooting);
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        StartCoroutine(ReloadFinished());
    }

    private IEnumerator ReloadFinished()
    {
        yield return new WaitForSeconds(_weaponConfiguration.reloadTime);
        _weaponConfiguration.bulletsLeft = _weaponConfiguration.magazineSize;
        reloading = false;
    }

    private void UpdateAmmunitionDisplay()
    {
        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText($"{_weaponConfiguration.bulletsLeft / _weaponConfiguration.bulletsPerTap}/{_weaponConfiguration.magazineSize / _weaponConfiguration.bulletsPerTap}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReadyToShoot();
            AddToWeaponArray();
            DeactivateWeapon();
        }
    }

    private void ReadyToShoot()
    {
        readyToShoot = true;
    }

    private void AddToWeaponArray()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.worldObjects[GameController.Instance._weaponIndex] = gameObject;
            Transform weaponTransform = GameController.Instance.worldObjects[GameController.Instance._weaponIndex].transform;
            weaponTransform.SetParent(GameController.Instance.weaponPosition);
            weaponTransform.position = GameController.Instance.weaponPosition.position;
            weaponTransform.localRotation = Quaternion.identity;
            GameController.Instance._weaponIndex++;
        }
    }

    private void DeactivateWeapon()
    {
        gameObject.SetActive(false);
    }
}
