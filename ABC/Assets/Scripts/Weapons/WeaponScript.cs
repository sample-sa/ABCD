using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponScript : MonoBehaviour
{
    
    public float fireRate;
    
    public GameObject bullet;

    // public RecoilScript recoilScript;

    public GameObject muzzle;

    public UnityEvent onBulletFired;

    private bool canFire;

    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckFire();
    }

    void CheckFire()
    {
        if(Input.GetButton("Fire") && canFire)
        {
            canFire = false;

            OnFire();

            Invoke("ResetFire", 1/(fireRate/60));

            Debug.Log(1/(fireRate/60));
        }
    }

    void ResetFire()
    {
        canFire = true;
    }

    void OnFire()
    {
        onBulletFired.Invoke();

        GameObject bulletClone = Instantiate(bullet, muzzle.transform.position, muzzle.transform.rotation);

        Rigidbody bulletRBody = bulletClone.GetComponent<Rigidbody>();
    }
}
