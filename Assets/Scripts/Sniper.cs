using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Sniper : Player
{
    public GameObject BulletOb;
    public float delay;
    private float timeWitoutFire;
    private Vector3 bulletFuturePosition;

    [SerializeField]
    private SniperStats sniperStats;
    private int lvl;

    bool fire = false;

    void Start()
    {
        timeWitoutFire = 0;
        lvl = 0;
        Damage = sniperStats.SniperLevels[lvl].Damage;
        MaxHp = sniperStats.SniperLevels[lvl].MaxHp;
        Speed = sniperStats.SniperLevels[lvl].Speed;
        StartPlayer();
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
                bulletFuturePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bulletFuturePosition.z = transform.position.z;
                if (transform.position.x < bulletFuturePosition.x)
                {
                    Flip(false);
                }
                else if (transform.position.x > bulletFuturePosition.x)
                {
                    Flip(true);
                }
                fire = true;
                animator.SetBool("Fire", fire);
                rb.bodyType = RigidbodyType2D.Static;
            }
            if(fire)
            {
                if (transform.position.x < bulletFuturePosition.x)
                {
                    Flip(false);
                }
                else if (transform.position.x > bulletFuturePosition.x)
                {
                    Flip(true);
                }
            }
        }
    }

    private void Fire()
    {
        bulletFuturePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bulletFuturePosition.z = transform.position.z;
        GameObject bulletGameoject = PhotonNetwork.Instantiate(BulletOb.name, transform.position, Quaternion.identity);
        //GameObject bulletGameoject = Instantiate(BulletOb.gameObject, transform.position, Quaternion.identity);
        Bullet b = bulletGameoject.GetComponent<Bullet>();
        b.damage = Damage;
        b.SetTargetPositon(bulletFuturePosition, this);
        fire = false;
        animator.SetBool("Fire", fire);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void LevelUpStat(int NumStat, TMP_Text textLevel)
    {
        switch (NumStat)
        {
            case 0:
                if (LVLMaxHp < sniperStats.SniperLevels.Count)
                {
                    LVLMaxHp++;
                    Hp += sniperStats.SniperLevels[LVLMaxHp].MaxHp - MaxHp;
                    MaxHp = sniperStats.SniperLevels[LVLMaxHp].MaxHp;
                }
                break;
            case 1:
                if (LVLDamage < sniperStats.SniperLevels.Count)
                {
                    LVLDamage++;
                    Damage = sniperStats.SniperLevels[LVLDamage].Damage;
                    textLevel.text = "Level:" + (LVLDamage + 1);
                }
                break;
            case 2:
                LVLSpeed++;
                if (LVLSpeed < sniperStats.SniperLevels.Count)
                    Speed = sniperStats.SniperLevels[LVLSpeed].Speed;
                break;
            default:
                break;
        }
        photonView.RPC("SetDataCharacteristics", RpcTarget.AllBuffered, Hp, Damage, Speed);
    }

    [PunRPC]
    public void SetDataCharacteristics(float _Hp, float _Damage, float _Speed)
    {
        Hp = _Hp;
        Damage = _Damage;
        Speed = _Speed;
    }
}
