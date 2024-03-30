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
    private string newTag = "Orbitable";
    private GameObject objectToRotate;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (bulletConfiguration.parabolic)
        {
            speed = bulletConfiguration.parabolicSpeed;
            gravity = bulletConfiguration.gravity;
            initialVelocity = transform.forward * speed;
        }else if (bulletConfiguration.orbital)
        {
            speed = bulletConfiguration.orbitationalSpeed;
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
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
                gameObject.tag = newTag;
                objectToRotate = other.gameObject;
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
        if (objectToRotate)
        {
            objectToRotate.gameObject.transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * speed);
        }

    }

    private void SpecialShot()
    {

    }

}
