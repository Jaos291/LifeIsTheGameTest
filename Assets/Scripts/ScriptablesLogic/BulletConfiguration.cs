using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "Bullet")]

public class BulletConfiguration : ScriptableObject
{
    [Header("Behaviours:")]
    public bool parabolic;
    public float parabolicSpeed;
    public float parabolicGravity;

    public bool orbital;
    public string[] tagsToRotateAround;
    public float orbitationalSpeed;

    public bool special;
    public float specialSpeed;
    public float speciaGravity;
    public GameObject explotion;
    public float explotionRange;
    public float explotionForce;
    public bool explodeOnTouch;
    public LayerMask whatIsEnemies;

    private void OnValidate()
    {
        // Ensure only one boolean is checked at a time, if none is checked, parabolic is default
        if (parabolic)
        {
            orbital = false;
            special = false;
        }
        else if (orbital)
        {
            parabolic = false;
            special = false;
        }
        else if (special)
        {
            parabolic = false;
            orbital = false;
        }
    }
}
