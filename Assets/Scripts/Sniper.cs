using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Sniper :  Player
{
    public GameObject BulletOb;
    public float delay;
    private float timeWitoutFire;
    private Vector3 bulletFuterPosition;

    void Start()
    {
        StartPlayer();
        timeWitoutFire = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayer();
        if (photonView.IsMine)
        {
            timeWitoutFire += Time.deltaTime;
            if (Input.GetMouseButton(0) && timeWitoutFire > delay)
            {
                timeWitoutFire = 0;
                Fire();
            }
        }
    }

    private void Fire()
    {
        bulletFuterPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bulletFuterPosition.z = transform.position.z;
        GameObject bulletGameoject = PhotonNetwork.Instantiate(BulletOb.name, transform.position, Quaternion.identity);
        //GameObject bulletGameoject = Instantiate(BulletOb.gameObject, transform.position, Quaternion.identity);
        Bullet b = bulletGameoject.GetComponent<Bullet>();
        b.damage = Damage;
        b.SetTargetPositon(bulletFuterPosition);
    }
}
