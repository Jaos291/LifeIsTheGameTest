using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public BulletConfiguration bulletConfiguration;
    private float speed;
    private float gravity;
    private Vector3 initialVelocity;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (bulletConfiguration.parabolic)
        {
            speed = bulletConfiguration.parabolicSpeed;
            gravity = bulletConfiguration.gravity;
            initialVelocity = transform.forward * speed;
        }
        else if (bulletConfiguration.special)
        {

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (string orbitable in bulletConfiguration.tagsToRotateAround)
        {
            if (bulletConfiguration.orbital && other.gameObject.CompareTag(orbitable))
            {
                Debug.Log("Entered");
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();

                LeanTween.rotateAround(other.gameObject, transform.position, bulletConfiguration.orbitationalSpeed, bulletConfiguration.timeToCompleteOneRotation);
            }
        }
    }

    private void Update()
    {
        ShotBullet();
    }

    private void ShotBullet()
    {
        if (bulletConfiguration.parabolic)
        {
            ParabolicShot();
        }else if(bulletConfiguration.orbital){

            OrbitalShot();
        }
        else
        {
            SpecialShot();
        }
    }

    public void ParabolicShot()
    {
        initialVelocity.y -= gravity * Time.deltaTime;

        transform.position += initialVelocity * Time.deltaTime;
    }

    private void OrbitalShot()
    {
        
    }

    private void SpecialShot()
    {

    }

}
