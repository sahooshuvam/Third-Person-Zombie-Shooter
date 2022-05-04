using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    public GameObject gunflash;
    Animator animator;

    int ammo = 30;
    int maxAmmo = 30;
    int maxAmmokitAmmo = 30;

    [SerializeField] private Text ammoText;

    ScoreManager score;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ammoText.text = ammo.ToString();
        score = GameObject.FindGameObjectWithTag("Player").GetComponent<ScoreManager>();      
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (ammo >0)
            {
                FireGun();
                animator.SetTrigger("isShooting");
                ammo--;
                gunflash.SetActive(true);
                Invoke("StopGunFlash", 0.5f);
                ammoText.text = ammo.ToString();
            }
            
        }
    }
    private void StopGunFlash()
    {
        gunflash.SetActive(false);
    }

    private void FireGun()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        Debug.DrawRay(ray.origin, ray.direction * 30f, Color.red, 2f);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100f))
        {
            GameObject hitZombie = hitInfo.collider.gameObject;
            if (hitZombie.tag == "Zombie")
            {
                score.ScoreCalculator(10);
                hitZombie.GetComponent<ZombieController>().KillZombie();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AmmoBox")
        {
            int ammoNeeded = maxAmmo - ammo;
            if (maxAmmokitAmmo >= ammoNeeded)
                ammo = ammo + ammoNeeded;
            else
                ammo = ammo + maxAmmokitAmmo;

            ammoText.text = ammo.ToString();
            Destroy(other.gameObject);
        }
    }
}
