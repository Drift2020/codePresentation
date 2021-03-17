using UnityEngine;


public class ControlEnemyGraund : IControl
{
    RaycastHit2D[] raycastHit;
    RaycastHit2D raycastJump;
    RaycastHit2D raycastWalk;
    RaycastHit2D raycastWalk2;

    LayerMask MaskEnemy;
    LayerMask MaskWalk;
    LayerMask MaskJump;

    StateAI MyState;

    float sizeLine;

    CapsuleCollider2D capsuleCollider2D;

    IUnit unit;


    public ControlEnemyGraund(GameObject point)
    {
        unit = point.GetComponent<IUnit>();
        capsuleCollider2D = point.GetComponent<CapsuleCollider2D>();
        MyState = StateAI.Patrolling;
        MaskEnemy = LayerMask.GetMask("Unit");
        MaskWalk = LayerMask.GetMask(new string[] { "FloorCollision", "StairsCollision" });
        MaskJump = LayerMask.GetMask("FloorCollision");
    }


    Vector3 TempDirection = new Vector3();

    Vector2 saveDirection = new Vector2();
    float saveDirectionIsHit = 0;
    float saveDirectionIsWalk = 0;

    /// <summary>
    ///  Первая проверка скрытности, если её нет перейти к обычному режиму
    /// обноружен враг, включить таймер скрытности, если пропал враг, включить таймер обнуления, пауза таймера скрытности
    /// если враг появился, обнулить таймер обнуления, продолжить таймера скрытности,
    /// таймер скрытности закончился, нападать, таймер обнуления закончился , обнулить таймер скрытности
    /// 
    /// таймер обнуления - AISoldat состояние на StateAI.Patrolling
    /// </summary>
    #region Reserver

    bool TimerReserver()
    {
        if (unit.stateStruct.reserveTime <= 0)
        {    
            return true;
        }
        unit.stateStruct.reserveTime -= Time.deltaTime;

        return false;
    }

   
    #endregion

    public void Move(ref ButtonStruct state_Move, ref MoveStruct moveStruct)
    {
        if (moveStruct.FlipX) // left side
        {
            saveDirectionIsHit = -positionLineHit;
            saveDirectionIsWalk = -positionLineWalk;

            if (state_Move.moveInput.x > 0)
            {
                state_Move.moveInput.x *= -1;
            }
        }
        else if (!moveStruct.FlipX) // right side
        {
            saveDirectionIsHit = positionLineHit;
            saveDirectionIsWalk = positionLineWalk;

            if (state_Move.moveInput.x < 0)
            {
                state_Move.moveInput.x *= -1;
            }
        }


        if (IsWalk(new Vector2((moveStruct.FlipX ? -1 : 1), 0)))
        {
            state_Move.isMove = true;
        }
        else
        {
            state_Move.isMove = false;
        }

        if (IsJump(new Vector2((moveStruct.FlipX ? -1 : 1), 0)))
        {
            state_Move.isJump = true;
        }
        else
        {
            state_Move.isJump = false;
        }


        var tempHit = IsHit(new Vector2((moveStruct.FlipX ? -1 : 1), 0));
        var skill = isPunch(tempHit);

        //скрытность для AISoldat
        if (skill || tempHit)
        {
            unit.stateStruct.isReserveSee = true;
        }
        else
        {
            unit.stateStruct.isReserveSee = false;
        }
        //

        if (skill)
        {        
            if (unit.stateStruct.reserveTime == 0)
            {
                state_Move.isPunch = true;
            }
            else
            {
                if (TimerReserver())
                {
                    state_Move.isPunch = true;
                }
                else
                {
                    state_Move.isPunch = false;
                }
            }
        }
        else if (!skill)
        {          
            state_Move.isPunch = false;
        }

        if (tempHit)
        {        
            if (unit.stateStruct.reserveTime == 0)
            {
                state_Move.isShot  = true;
            }
            else
            {
                if (TimerReserver())
                {
                    state_Move.isShot = true;
                }
                else
                {
                    state_Move.isShot = false;
                }
            }
          
        }
        else if (!tempHit)
        {
            state_Move.isShot = false;
        }


    }


    #region is functions
    public bool IsJump(Vector2 direction)
    {
        if (direction.x < 0)
        {
            saveDirection = Vector2.left;
        }
        else if (direction.x > 0)
        {
            saveDirection = Vector2.right;
        }

        raycastJump = Physics2D.Raycast(capsuleCollider2D.bounds.center, saveDirection, capsuleCollider2D.bounds.extents.y + sizeLine, MaskJump);
        Color t = Color.blue;
        Debug.DrawRay(capsuleCollider2D.bounds.center, saveDirection * (capsuleCollider2D.bounds.extents.y + sizeLine), t);

        return (raycastJump.collider != null);
    }

    [SerializeField]
    float sizeLineHit = 0.8f;
    [SerializeField]
    float positionLineHit = 0.2f;


    /// <summary>

    /// 
    /// функция которая при обноружении врага, а после его потери 
    /// и если он в радиусе N от юнита, то юнит будет следовать за ним
    /// если враг пропадает из радиуса N 
    /// то юнит переходит из состояние преследование в состояния поиска
    /// 
    /// 
    /// </summary>
    public Transform GetEnemyTransform()
    {
        for (int i = 0; i < raycastHit.Length; i++)
        {
            if (raycastHit[i].collider != null &&
                  raycastHit[i].collider.gameObject.GetComponent<IUnit>().stateStruct.isControling)
            {
                  return raycastHit[i].collider.transform;             
            }
        }
        return null;
    }

   
    public bool IsHit(Vector2 direction)
    {    
        Vector3 temp = new Vector3(capsuleCollider2D.bounds.center.x , capsuleCollider2D.bounds.center.y, capsuleCollider2D.bounds.center.z);
        raycastHit = Physics2D.RaycastAll(temp, saveDirection, capsuleCollider2D.bounds.extents.y + sizeLineHit, MaskEnemy);
       
        Color t = Color.red;
        Debug.DrawRay(temp, saveDirection * (capsuleCollider2D.bounds.extents.y + sizeLineHit),t);
        for (int i = 0; i < raycastHit.Length; i++)
        {
            if (raycastHit[i].collider != null &&
                 raycastHit[i].collider.gameObject.GetComponent<IUnit>().stateStruct.isControling)
                return true;
        }
        return false;
    }

    public bool isPunch(bool isHit)
    {
        if (isHit)
        {

            return(Mathf.Abs(capsuleCollider2D.transform.position.x) - Mathf.Abs(GetEnemyTransform().position.x) >= -0.3 &&
                   Mathf.Abs(capsuleCollider2D.transform.position.x) - Mathf.Abs(GetEnemyTransform().position.x) <= 0.3);
        }
        return false;
    }


    [SerializeField]
    float sizeLineWalk = 0.01f;
    [SerializeField]
    float positionLineWalk = 0.2f;

    Color t1 = Color.green;
    public bool IsWalk(Vector2 direction)
    {      

        Vector3 temp = new Vector3(capsuleCollider2D.bounds.center.x + saveDirectionIsWalk, capsuleCollider2D.bounds.center.y -0.01f - capsuleCollider2D.size.y/2, capsuleCollider2D.bounds.center.z);

        Vector3 temp2 = new Vector3(capsuleCollider2D.bounds.center.x + saveDirectionIsWalk, capsuleCollider2D.bounds.center.y - 0.15f - capsuleCollider2D.size.y / 2, capsuleCollider2D.bounds.center.z);

        raycastWalk = Physics2D.Raycast(temp, saveDirection, sizeLineWalk, MaskWalk);
       
        raycastWalk2 = Physics2D.Raycast(temp2, Vector2.down, (0.1f), MaskWalk);
        
        Debug.DrawRay(temp, saveDirection * (sizeLineWalk),t1);
        Debug.DrawRay(temp2, Vector2.down * (0.1f), t1);

        return (raycastWalk.collider != null || raycastWalk2.collider != null);
    }

    #endregion
}
