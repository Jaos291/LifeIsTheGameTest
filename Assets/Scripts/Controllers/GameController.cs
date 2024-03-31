using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //---------Singleton Logic
    public static GameController Instance;

    [SerializeField] private GameObject _mainCharacter;
    public GameObject[] worldObjects;
    public Camera mainCamera;
    public Transform weaponPosition;


    //---------If we need any component reference of any object above, please list it below this point.
    private Animator _animator;

    //---------If we need to reference of any component, please list it below this point as public but hidden.

    [HideInInspector] public Animator mainCharacterAnimator;


    [HideInInspector] public int _weaponIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (_mainCharacter)
        {
            _animator = _mainCharacter.GetComponent<Animator>();
            mainCharacterAnimator = _animator;

            if (PlayerPrefs.HasKey("MainCharacterAnimation"))
            {
                string currentAnimation = PlayerPrefs.GetString("MainCharacterAnimation");
                _animator.SetBool(currentAnimation, true);
            }
        }
    }

    public void AddWeapon(GameObject weapon)
    {
        worldObjects[_weaponIndex] = weapon;
        _weaponIndex++;
    }
}
