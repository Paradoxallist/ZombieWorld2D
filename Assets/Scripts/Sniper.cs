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
    private Vector3 bulletFuterPosition;

    [SerializeField]
    private SniperStats sniperStats;
    private int lvl;

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
        b.SetTargetPositon(bulletFuterPosition, this);
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
    }
}
