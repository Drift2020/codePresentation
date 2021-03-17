using Music;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// подпишется на мастера юнитов, чтобы при обновлении проверять где нужно спавнить ботов
public class ZoneCreator : MonoBehaviour , IObserverDeadUnit, ISubjectDeadUnit
{
    public List<GameObject> CreateUnits = new List<GameObject>();
    public List<GameObject> units = new List<GameObject>();
    public List<GameObject> pointSpawns = new List<GameObject>();
    public int maxCount;
    List<IObserverDeadUnit> _observelDied = new List<IObserverDeadUnit>();

    void Start()
    {
        
        for (int i =0; i < CreateUnits.Count; i++)
        {
            CreateUnits[i].GetComponent<IUnit>().Attach(this);
        }
    }
    public GameObject TakePlayer()
    {
        for (int y = 0; y < CreateUnits.Count; y++)
        {

            try
            {
                if (CreateUnits[y].GetComponent<Player>()!=null)
                {
                    return CreateUnits[y];
                }
            }
            catch { }           
         
        }
        return null;
    }

    public Action NewUnit;
    public void CreateUnit()
    {
       
        for(int i = 0; (CreateUnits.Count < maxCount  || maxCount == -1) && i < pointSpawns.Count; i++)
        {
            var temp = Instantiate(units[0], pointSpawns[i].transform.position, Quaternion.identity);
            CreateUnits.Add(temp);
            temp.GetComponent<IUnit>().Attach(this);
            NewUnit();
        }
       
    }


    public void DeleteAllUnits()
    {
        for(int i = 0; i < CreateUnits.Count;i++)
        {
            if (CreateUnits[i].GetComponent<Player>() == null)
            {
                NotifyDead(CreateUnits[i], DeadCommand.destroy);
            }
        }
    }

    public void UpdateDead(GameObject _object, DeadCommand deadCommand)
    {
        NotifyDead(_object, deadCommand);
    }
    //----------------------------------------------------
    public void Attach(IObserverDeadUnit observer)
    {
        _observelDied.Add(observer);
    }

    public void Detach(IObserverDeadUnit observer)
    {
        _observelDied.Remove(observer);
    }

    public void NotifyDead(GameObject _object, DeadCommand deadCommand)
    {
        for (var i = 0; i< _observelDied.Count; i++)
        {        
            _observelDied[i].UpdateDead(_object, deadCommand);
        }
        

    }


    public ZoneEnemyCreateData Save()
    {

        List<EnemySaveData> enemySaveDatas = new List<EnemySaveData>();
        for (int i = 0; i < CreateUnits.Count; i++)
        {
            if (CreateUnits[i].GetComponent<IUnit>() != null)
            {
                enemySaveDatas.Add(CreateUnits[i].GetComponent<IUnit>().Save());
            }
            else
            {
                Debug.Log(gameObject.name + " empty slot when save");
            }
        }

        return new ZoneEnemyCreateData(enemySaveDatas);
    }

    public void Load(ZoneEnemyCreateData zone)
    {
        for (var i = 0; i < zone.enemySaveDatas.Count; i++)
        {
            for (int y = 0; y < units.Count; y++)
            {

                try
                {             
                    if (zone.enemySaveDatas[i].Type == units[y].GetComponent<SoldatNormal>().GetType().ToString())
                    {
                        var temp = Instantiate(units[y], new Vector3(zone.enemySaveDatas[i].Position.x, zone.enemySaveDatas[i].Position.y, 0), Quaternion.identity);
                        CreateUnits.Add(temp);
                        temp.GetComponent<IUnit>().Attach(this);
                    }
                }
                catch
                {
                    Debug.LogWarning(string.Format("ZoneCreator:{0}, can't create {1},{2}", gameObject.name, zone.enemySaveDatas[i].Type, units[y].GetType().ToString()));
                }                                         
            }

        
        }
    }
}

