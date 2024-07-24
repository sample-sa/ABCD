using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletScripts : MonoBehaviour
{
    [Header("Bullet variables")]
    [Space(15)]
    public float muzzleVel;
    public float minRicochetAngle;
    public float lifeSpan;
    public float ricochetChance;
    public float gravity;

    [Header("Bullet Drag variables")]
    [Space(15)]

    public float bulletConstant;
    public float bulletExponet;

    float count = 0;
    private Rigidbody rb;
    private RaycastHit rayHit;
    


    private float hBegin;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();

        hBegin = rb.position.y;
        Debug.Log("Start pos y " + hBegin);
        
        Debug.DrawRay(rb.position, rb.transform.forward * 10000 , Color.red, 2f);

        rb.velocity = rb.transform.forward * muzzleVel;

        rb.position = rb.position - rb.velocity * Time.fixedDeltaTime;

        

        Invoke("Destroy", lifeSpan);
        
    }

    void FixedUpdate()
    {
        count += Time.fixedDeltaTime;

        Vector3 startPos = rb.position + rb.velocity * Time.fixedDeltaTime;
        float maxLength = rb.velocity.magnitude * Time.fixedDeltaTime;

        ExteriorBallistics();

        Debug.DrawRay(startPos, rb.velocity * Time.fixedDeltaTime, Color.green, 2f);
        
        if(Physics.Raycast(startPos, rb.velocity.normalized, out rayHit, maxLength))

            {
                OnBulletHit();
            }        

    }

    void ExteriorBallistics()
    {
        float bulletDrag = GetBulletDrag(rb.velocity.magnitude);

        float velX = rb.velocity.x * bulletDrag;
        float velY = rb.velocity.y * bulletDrag;
        float velZ = rb.velocity.z * bulletDrag;

        rb.velocity -= new Vector3(velX, velY - gravity * Time.fixedDeltaTime, velZ);
    }

    float GetBulletDrag(float velocity)
    {
        float drag;

        drag = bulletConstant * MathF.Pow(velocity, bulletExponet);

        return drag * Time.fixedDeltaTime;
    }

    void OnBulletHit()
    {
        float angle = Vector3.Angle(-rayHit.normal, rb.velocity);

        Debug.Log("Height Diff: " + (rb.position.y - hBegin));
        Debug.Log("Time :" + count + "     End Speed" + rb.velocity.magnitude);

        if(angle > minRicochetAngle)
        {
            OnBulletRicochet();
        }

        else
        {
            Destroy();
        }


    }   

    void OnBulletRicochet()
    {
        float angle = Vector3.Angle(-rayHit.normal, rb.velocity);

        float velLost = (angle/90) / 1.5f;

        Vector3 newVel = Vector3.Reflect(rb.velocity, rayHit.normal) * velLost;
        //Make angle influence vel and add randomness.

        rb.velocity = newVel;
        rb.position = rayHit.point - (rb.velocity * Time.fixedDeltaTime) * 2;
        
    }

    void Destroy()
    {
        Destroy(gameObject);
    }



}
