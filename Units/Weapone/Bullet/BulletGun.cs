using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun : MonoBehaviour, IBullet
{

    [SerializeField]
    float _time ;
    [SerializeField]
    float spead_shot_time ;
    Animator animator;

    bool isDestroy;
    void Start()
    {
         _time = 10;
        spead_shot_time = 4;
        isDestroy = false;
        animator = gameObject.GetComponent<Animator>();
    }

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
    [SerializeField]
    private float _damage;
  
    public float speed { get
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

    void Update()
    {
         
        if (!isDestroy)
        {
            DestroyBullet();
            if (Is_Collision())
            {
                raycastHit2D.collider.gameObject.GetComponent<IUnit>().TakeDamage(_damage);
                isDestroy = true;
                animator.SetBool("isDestroy", isDestroy);
               
            }
            if (!isDestroy)
            {
                transform.position = new Vector2(transform.position.x + vector.x * _speed * Time.fixedDeltaTime, transform.position.y + vector.y * _speed * Time.fixedDeltaTime);
            }
        }

    }

    void DestroyBullet()
    {
        if (_time <= 0)
        {
            Destroy(gameObject);
        }
        _time -= Time.deltaTime * spead_shot_time;
    }

    void DestroyBulletHit()
    {
        
            Destroy(gameObject);

    }

    RaycastHit2D raycastHit2D;
    [SerializeField]
    private LayerMask platformMask;
    [SerializeField]
    float sizeLine = .04f;
    [SerializeField]
    private BoxCollider2D boxCollider2D;
    bool Is_Collision()
    {
        raycastHit2D = Physics2D.Raycast(boxCollider2D.bounds.center, (_vector.x == 1 ? Vector2.right: Vector2.left), boxCollider2D.bounds.extents.x + sizeLine, platformMask);
        Color t = Color.green;
        Debug.DrawRay(boxCollider2D.bounds.center, (_vector.x == 1 ? Vector2.right : Vector2.left) * (boxCollider2D.bounds.extents.x + sizeLine));    
        
      
        return (raycastHit2D.collider != null &&
            ( (master.stateStruct.isControling && !raycastHit2D.collider.gameObject.GetComponent<IUnit>().stateStruct.isControling) || 
            (!master.stateStruct.isControling && raycastHit2D.collider.gameObject.GetComponent<IUnit>().stateStruct.isControling)));

    }
}
