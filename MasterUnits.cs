using Music;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterUnits : MonoBehaviour, Colleague, IObserverDeadUnit
{
    public List<ZoneCreator> zones;

    void Awake()
    {
        for (int i = 0; i < zones.Count; i++)
        {
            zones[i].Attach(this);
            zones[i].NewUnit = NewUnit;
        }

        
    }

    public void NewUnit()
    {
        Send(TypeMessage.NewUnit);
        for (int i = 0; i < zones.Count; i++)
        {

           for(int y = 0; y < zones[i].CreateUnits.Count; y++)
           {
                if(zones[i].CreateUnits[y].GetComponent<IUnit>().ObjectIsAvalible == null)
                {
                    zones[i].CreateUnits[y].GetComponent<IUnit>().ObjectIsAvalible = Send;
                    zones[i].CreateUnits[y].GetComponent<IUnit>().ObjectIsUnavalible = Send;
                }
           }

        }
    }
    public Mediator mediator { get; set; }

    public void TakeScore()
    {

    }

    public GameObject TakePlayer()
    {
        GameObject obj = null;
        for (int i = 0; i < zones.Count; i++)
        {
            obj = zones[i].TakePlayer();
            if( obj!= null)
            {
                return obj;
            }
        }
        return null;
    }

    /// <summary>
    /// получение из медиатра
    /// </summary>
    /// <param name="message"></param>
    public void Notify(TypeMessage message)
    {
        switch (message)
        {
            case TypeMessage.UpdateUnits:
                for (int i = 0; i < zones.Count; i++)
                {
                    zones[i].CreateUnit();
                    NewUnit();
                }
                break;
            case TypeMessage.DeleteAllUnits:
         
                for(int i =0; i< zones.Count;i++)
                {
                    zones[i].DeleteAllUnits();
                }
                break;
        }
    }


    public void Notify(TypeMessage message, Vector3 _transphorm)
    {
        throw new System.NotImplementedException();
    }


    /// <summary>
    /// отправка в медиатор
    /// </summary>
    /// <param name="message"></param>
    public void Send(TypeMessage message)
    {
        mediator.Send(message, this);
    }

    public void Send(TypeMessage message, Vector3 _transphorm)
    {
        mediator.Send(message, _transphorm, this);
    }



    public void UpdateDead(GameObject _object, DeadCommand deadCommand)
    {
        switch(deadCommand)
        {
            case DeadCommand.kill:
                Send(TypeMessage.PlayerKillUnits, _object.transform.position);
                break;
            case DeadCommand.destroy:
                Send(TypeMessage.DeadUnit, _object.transform.position);
                break;
            case DeadCommand.playerDied:
                Send(TypeMessage.PlaerIsDead);
                break;
        }

      

    }


    public List<ZoneEnemyCreateData> Save()
    {
        List<ZoneEnemyCreateData> zoneEnemyCreateData = new List<ZoneEnemyCreateData>();
        for (int i=0;i< zones.Count;i++)
        {

            zoneEnemyCreateData.Add(zones[i].Save());
            
        }
        return zoneEnemyCreateData  ;
    }

    public void Load(List<ZoneEnemyCreateData> structs)
    {
      
        for (int i = 0; i < structs.Count; i++)
        {
            zones[i].DeleteAllUnits();
            zones[i].Load(structs[i]);
            for (int y = 0; y < zones[i].CreateUnits.Count; y++)
            {
                if (zones[i].CreateUnits[y].GetComponent<IUnit>().ObjectIsAvalible == null)
                {
                    zones[i].CreateUnits[y].GetComponent<IUnit>().ObjectIsAvalible = Send;
                    zones[i].CreateUnits[y].GetComponent<IUnit>().ObjectIsUnavalible = Send;
                }
            }
        }

        Send(TypeMessage.NewUnit);

    }

   
}

