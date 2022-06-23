using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Pathfinding;
using UnityEngine.UI;

public abstract class BaseEnemy : Enemy
{
    public List<Flask> Flasks;

    private bool stun;

    public void UpdateBaseEnemy()
    {
        UpdateEnemy();
    }

    public abstract void LevelUpWave(int Wave);
    public bool GetVariableStun()
    {
        return stun;
    }

    [PunRPC]
    public void StunPun(float timeStun)
    {
        EnemySpriteBody.color = Color.black;
        stun = true;
        aIPath.maxSpeed = 0;
        Invoke("StopStun", timeStun);
    }

    public void StopStun()
    {
        EnemySpriteBody.color = Color.white;
        aIPath.maxSpeed = Speed;
        stun = false;
    }

    public override void DestroyHimself()
    {
        if (Flasks != null)
        {
            List<Flask> FlasksRandom = new List<Flask>();
            for (int i = 0; i < Flasks.Count; i++)
            {
                if (Random.Range(0f, 100f) < Flasks[i].DropChance)
                {
                    FlasksRandom.Add(Flasks[i]);
                }
            }
            if (FlasksRandom.Count > 0)
            {
                PhotonNetwork.InstantiateRoomObject(FlasksRandom[Random.Range(0, FlasksRandom.Count)].name, transform.position, Quaternion.identity);
            }
        }

        GameManager.Instance.EnemyDie(this);

        if (PV != null)
            PV.RPC("DestroyEnemy", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void SynchronizingDataEnemy(float _MaxHp, float _Hp, float _Damage)//Peredelat !
    {
        MaxHp = _MaxHp;
        Hp = _Hp;
        Damage = _Damage;
    }
}
