using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate Transform GetEnemyTransform();
public class SmartGun : MonoBehaviour, IWeapone
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
    float _poverShoot;
    public float poverShoot { get { return _poverShoot; } set { _poverShoot = value; } }


    [SerializeField]
    float _poverShootIsControl;
    public float poverShootIsControl { get { return _poverShootIsControl; } set { _poverShootIsControl = value; } }

    [SerializeField]
    float _poverShootIsNotControl;
    public float poverShootIsNotControl { get { return _poverShootIsNotControl; } set { _poverShootIsNotControl = value; } }

    public GetEnemyTransform GetEnemyTransform;


    #region Spetials
    [SerializeField]
    float _megaPowerShoot;
    public float MegaPowerShoot
    {
        get { return _megaPowerShoot; }
        set { _megaPowerShoot = value; }
    }


    [SerializeField]
    float _maxCriticalDamage;
    public float maxCriticalDamage
    {
        get { return _maxCriticalDamage; }
        set { _maxCriticalDamage = value; }
    }

    [SerializeField]
    float _maxСhanceCriticalDamage;
    public float maxСhanceCriticalDamage
    {
        get { return _maxСhanceCriticalDamage; }
        set { _maxСhanceCriticalDamage = value; }
    }
    #endregion


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

    [SerializeField]
    float Damage;
    public void UpdateDamageBulet(float value)
    {
        Damage = value;
    }

    public void Shot(Vector2 vector, Vector2 poitShoot)
    {
        GameObject go = Instantiate(Resources.Load(_pathToAmmo),
            (unit.moveStruct.position + poitShoot), Quaternion.identity) as GameObject;
        var temp = go.GetComponent<ISmartBullet>();

        temp.damage = temp.damage + temp.damage / 100 * Damage;
        temp.targetUnit = GetEnemyTransform();

        if (_maxСhanceCriticalDamage != 0 && _maxCriticalDamage != 0
            && Random.Range(0, 100) <= _maxСhanceCriticalDamage)
        {
            temp.damage *= _maxCriticalDamage;
        }

        if (MegaPowerShoot != 0 && Random.Range(0, 100) <= MegaPowerShoot)
        {
            temp.damage *= 2;
        }


        temp.master = unit;
        temp.vector = vector;
        if(_poverShoot==1)
        {
            temp.speed = _poverShootIsControl;
        }
        else
        {
            temp.speed = _poverShootIsNotControl;
        }

      


    }


    
}
