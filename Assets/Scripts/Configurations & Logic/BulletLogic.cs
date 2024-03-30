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

        LoadBulletConfiguration();
    }

    private void Update()
    {
        BulletBehavior();
    }

    private void BulletBehavior()
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
        initialVelocity.y -= gravity * Time.deltaTime;

        transform.position += initialVelocity * Time.deltaTime;
    }

    private void LoadBulletConfiguration()
    {
        if (bulletConfiguration.parabolic)
        {
            speed = bulletConfiguration.parabolicSpeed;
            gravity = bulletConfiguration.parabolicGravity;
            initialVelocity = transform.forward * speed;
        }
        else if (bulletConfiguration.orbital)
        {
            speed = bulletConfiguration.orbitationalSpeed;
        }
        else if (bulletConfiguration.special)
        {
            speed = bulletConfiguration.specialSpeed;
            gravity = bulletConfiguration.speciaGravity;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (bulletConfiguration.special && bulletConfiguration.explotion && bulletConfiguration.explodeOnTouch && collision.gameObject.tag.Equals(bulletConfiguration.tagToExplodeAtTouch))
        {
            GameObject explotion = Instantiate(bulletConfiguration.explotion, transform.position, Quaternion.identity);
            explotion.transform.parent = transform;
            Collider[] enemies = Physics.OverlapSphere(transform.position, bulletConfiguration.explotionRange, bulletConfiguration.whatIsEnemies);
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].GetComponent<Rigidbody>())
                {
                    enemies[i].GetComponent<Rigidbody>().AddExplosionForce(bulletConfiguration.explotionForce, transform.position, bulletConfiguration.explotionRange);
                }
            }

            Invoke("DelayDestroy", 1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bulletConfiguration.explotionRange);
    }


    private void DelayDestroy()
    {
        Destroy(gameObject);
    }

}
