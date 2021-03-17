using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapone
{
    #region Datas
    [SerializeField]
    float _energyCost;
    public float energyCost { get { return _energyCost; } set { _energyCost = value; } }
    [SerializeField]
    bool _reload;
    public bool reload { get { return _reload; } set { _reload = value; } }
    [SerializeField]
    bool _is_active_shot;
    public bool is_active_shot { get { return _is_active_shot; } set { _is_active_shot = value; } }
    [SerializeField]
    bool _shot;
    public bool shot { get { return _shot; } set { _shot = value; } }

    [SerializeField]
    float Damage;

    [SerializeField]
    float _poverShoot;
    public float poverShoot { get { return _poverShoot; } set { _poverShoot = value; } }

    [SerializeField]
    float _megaPowerShoot;
    public float MegaPowerShoot { get { return _megaPowerShoot; }
        set { _megaPowerShoot = value; } }


    [SerializeField]
    float _maxCriticalDamage;
    public float maxCriticalDamage { get { return _maxCriticalDamage; }
        set { _maxCriticalDamage = value; } }

    [SerializeField]
    float _maxСhanceCriticalDamage;
    public float maxСhanceCriticalDamage { get { return _maxСhanceCriticalDamage; } 
        set { _maxСhanceCriticalDamage = value; } }

    [SerializeField]
    string _pathToAmmo;
    public string pathToAmmo
    {
        get
        {
            return _pathToAmmo;
        }
        set
        {
            _pathToAmmo = value;
        }
    }


    public IUnit unit { get; set; }
    #endregion

    public Gun(IUnit _unit)
    {
        unit = _unit;
        _poverShoot = 10f;
        energyCost = 5;
        _maxСhanceCriticalDamage = 0;
        _maxCriticalDamage = 1.2f;
        _megaPowerShoot = 0;
    }

    public Gun(IUnit _unit, float power, float energyCost, float _maxCriticalDamage)
    {
        unit = _unit;
        _poverShoot = power;
        this.energyCost = energyCost;
        _maxСhanceCriticalDamage = 0;
        _maxCriticalDamage = 1.2f;
        _megaPowerShoot = 0;
    }

   
    public void UpdateDamageBulet(float value)
    {
        Damage = value;
    }
    public void Shot(Vector2 vector, Vector2 poitShoot )
    {
        GameObject go = Instantiate(Resources.Load(_pathToAmmo), 
            (unit.moveStruct.position + poitShoot), Quaternion.identity) as GameObject;
        var temp = go.GetComponent<IBullet>();

        temp.damage = temp.damage + temp.damage / 100 * Damage;


        if (_maxСhanceCriticalDamage != 0 && _maxCriticalDamage != 0 
            && Random.Range(0, 100) <= _maxСhanceCriticalDamage)
        {
            temp.damage *= _maxCriticalDamage;
        }

        if (MegaPowerShoot != 0 && Random.Range(0,100)<= MegaPowerShoot)
        {
            temp.damage *= 2;
        }
       

        temp.master = unit;
        temp.vector = vector;
        temp.speed = _poverShoot;

       
    }

    
}


