using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    public GameObject gunflash;
   


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            FireGun();
            gunflash.SetActive(true);
            Invoke("StopGunFlash", 0.5f);
        }
    }
    private void StopGunFlash()
    {
        gunflash.SetActive(false);
    }

    private void FireGun()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hitInfo, 100f))
        {
            GameObject hitZombie = hitInfo.collider.gameObject;
            if (hitZombie.tag == "Zombie")
            {
                hitZombie.SetActive(false);
            }
        }
    }
}
