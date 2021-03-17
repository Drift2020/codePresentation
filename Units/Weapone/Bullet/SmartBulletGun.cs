using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBulletGun : MonoBehaviour, ISmartBullet
{
    #region Data

    #region Bullet var
    public float damage
    {
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }
    [Header("Bullet param")]
    [SerializeField]
    private float _damage;

    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }
    [SerializeField]
    private float _speed;

    public Vector2 vector
    {
        get
        {
            return _vector;
        }
        set
        {
            _vector = value;
        }
    }
    [SerializeField]
    private Vector2 _vector;
   
  

    public IUnit master { get; set; }

    bool isDestroy;
    #endregion

    #region Timers

    #region Destroy Timer

    [Header("Destroy")]
    [SerializeField]
    float _time;
    [SerializeField]
    float spead_shot_time;
    #endregion

    #region Smart Timer
    
    public Transform targetUnit
    {
        get { return _targetUnit; }
        set { _targetUnit = value; }
    }
    [Header("Smart")]
    [SerializeField]
    Transform _targetUnit;

    [SerializeField]
    float _timeProsecution;
    public float timeProsecution
    {
        get { return _timeProsecution; }
        set { _timeProsecution = value; }
    }
    #endregion

    #endregion

    #region Unity dataes
    [SerializeField]
    private BoxCollider2D boxCollider2D;
    Animator animator;

    Rigidbody2D rigidbody2D;
    #endregion

    #region reycast
    RaycastHit2D raycastHit2D;
    [SerializeField]
    private LayerMask platformMask;
    [SerializeField]
    float sizeLine = .04f;
    #endregion

    #endregion Date

    void Start()
    {
      
        isDestroy = false;
        animator = gameObject.GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }


    void Update()
    {

        if (!isDestroy)
        {
            DestroyBullet();
            if (Is_Collision())
            {
                raycastHit2D.collider.gameObject.GetComponent<IUnit>().TakeDamage(_damage);
                isDestroy = true;
                rigidbody2D.bodyType = RigidbodyType2D.Static;
             
                animator.SetBool("isDestroy", isDestroy);

            }
            if (!isDestroy)
            {
                Prosecution();
            }
        }

    }



    #region Destroy functions
    void DestroyBullet()
    {
        if (_time <= 0)
        {
            Destroy(gameObject);
        }
        _time -= Time.deltaTime;
    }
    void DestroyBulletHit()
    {

        Destroy(gameObject);

    }
    #endregion

    #region Smart function
    
    void Prosecution()
    {
        if (_timeProsecution >= 0 && _targetUnit!=null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetUnit.position, 
                _speed * Time.fixedDeltaTime);        
         
            Vector3 vectorToTarget = _targetUnit.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);

            _vector = new Vector2(((transform.position.x - targetUnit.position.x) < 0 ? 1 : -1),
                  ((transform.position.y - targetUnit.position.y) < 0 ? 1 : -1));

           
            _timeProsecution -= Time.deltaTime;
        }
        else
        {         
            if (_timeProsecution > -100)
            {
                _timeProsecution = -100;

                Quaternion q = Quaternion.AngleAxis((_vector.x > 0 ?   0: 180), Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);

               
            }
            else if(_timeProsecution == -100)
            {
                rigidbody2D.AddRelativeForce(Vector2.right * (_speed * Time.fixedDeltaTime * 2),
                   ForceMode2D.Impulse);
                _timeProsecution = -1000;
            }
        
        }
        
    }

    void moveWhereRotated()
    { 
        float radians = transform.rotation.z * Mathf.Deg2Rad;
        _vector.x += _speed * Mathf.Cos(radians);
        _vector.y += _speed * Mathf.Sin(radians);
    }

#endregion

#region Is functions
/// <summary>
/// Поворот в сторону полёта
/// </summary>
/// <returns></returns>
bool Is_Collision()
    {
        raycastHit2D = Physics2D.Raycast(boxCollider2D.bounds.center, (_vector.x == 1 ? Vector2.right : Vector2.left), boxCollider2D.bounds.extents.x + sizeLine, platformMask);
        Color t = Color.green;
        Debug.DrawRay(boxCollider2D.bounds.center, (_vector.x == 1 ? Vector2.right : Vector2.left) * (boxCollider2D.bounds.extents.x + sizeLine));


        return (raycastHit2D.collider != null &&
            ((master.stateStruct.isControling && !raycastHit2D.collider.gameObject.GetComponent<IUnit>().stateStruct.isControling) ||
            (!master.stateStruct.isControling && raycastHit2D.collider.gameObject.GetComponent<IUnit>().stateStruct.isControling)));

    }
    #endregion


}
