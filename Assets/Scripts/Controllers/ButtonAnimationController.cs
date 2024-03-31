using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimationController : MonoBehaviour
{
    private Animator _playerAnimator;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }
    public void OnButtonClick(string animationName)
    {
        //Get Reference of Player Animator
        GetPlayerAnimator();

        AnimatorControllerParameter[] parameters = _playerAnimator.parameters;

        // Loop through each parameter and turn off the corresponding animation
        foreach (AnimatorControllerParameter parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                _playerAnimator.SetBool(parameter.name, false);
            }
        }

        // Turn on the animation specified by the parameter
        _playerAnimator.SetBool(animationName, true);

        //To Get Global animation name in any instance of the project
        PlayerPrefs.SetString("MainCharacterAnimation", animationName);
    }

    private void GetPlayerAnimator()
    {
        if (!_playerAnimator)
        {
            _playerAnimator = GameController.Instance.mainCharacterAnimator;
        }
    }
}
