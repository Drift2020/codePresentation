using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEnemyFlight : IControl
{
    #region RaycastHit2D
    byte countRaycastHits;
    RaycastHit2D[][] raycastHit;
    RaycastHit2D raycastJump;
    RaycastHit2D raycastWalk;
    RaycastHit2D raycastWalk2;
    #endregion

    #region LayerMask
    LayerMask MaskEnemy;
    LayerMask MaskWalk;
    LayerMask MaskJump;
    #endregion

    #region Var
    StateAI MyState;

    float sizeLine;

    CapsuleCollider2D capsuleCollider2D;

    IUnit unit;
    #endregion


    #region Direction
    Vector3 TempDirection = new Vector3();
    Vector2 saveDirection = new Vector2();

    float saveDirectionIsHit = 0;
    float saveDirectionIsWalk = 0;

    public float angle = 180;
    #endregion

    #region Hit
    Color HitColor = Color.red;
    [SerializeField]
    float sizeLineHit = 0.8f;
    [SerializeField]
    float positionLineHit = 0.2f;
    #endregion

    #region Flight
    Color FlightColor = Color.green;
    [SerializeField]
    float sizeLineFlight = 0.01f;
    [SerializeField]
    float positionLineFlight = 0.2f;
    #endregion

    #region Walk
    [SerializeField]
    float sizeLineWalk = 0.01f;
    [SerializeField]
    float positionLineWalk = 0.2f;
    Color ColorLineWalk = Color.green;
    #endregion

    public ControlEnemyFlight(GameObject point)
    {
        unit = point.GetComponent<IUnit>();
        capsuleCollider2D = point.GetComponent<CapsuleCollider2D>();
        MyState = StateAI.Patrolling;
        MaskEnemy = LayerMask.GetMask("Unit");
        MaskWalk = LayerMask.GetMask(new string[] { "FloorCollision", "StairsCollision" });
        MaskJump = LayerMask.GetMask("FloorCollision");
        countRaycastHits = 11;
        raycastHit = new RaycastHit2D[countRaycastHits][];
    }

    public Transform GetEnemyTransform()
    {
        for (int i = 0; i < raycastHit.Length; i++)
        {
            for (int x = 0; x < raycastHit[i].Length; x++)
            {
                if (raycastHit[i][x].collider != null &&
                  raycastHit[i][x].collider.gameObject.GetComponent<IUnit>().
                  stateStruct.isControling &&
                  raycastHit[i][x].collider.gameObject.GetComponent<IUnit>() != unit)
                {
                    return raycastHit[i][x].collider.transform;
                }
            }
        }
        return null;
    }

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
                state_Move.isShot = true;
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


    bool TimerReserver()
    {
        if (unit.stateStruct.reserveTime <= 0)
        {
            return true;
        }
        unit.stateStruct.reserveTime -= Time.deltaTime;

        return false;
    }
    #region Is function
    
    public bool IsWalk(Vector2 direction)
    {
        Debug.Log("Переделать или сделать для взлёта наверх");

        Vector3 temp = new Vector3(capsuleCollider2D.bounds.center.x + saveDirectionIsWalk, capsuleCollider2D.bounds.center.y - 0.01f - capsuleCollider2D.size.y / 2, capsuleCollider2D.bounds.center.z);

        Vector3 temp2 = new Vector3(capsuleCollider2D.bounds.center.x + saveDirectionIsWalk, capsuleCollider2D.bounds.center.y - 0.15f - capsuleCollider2D.size.y / 2, capsuleCollider2D.bounds.center.z);

        raycastWalk = Physics2D.Raycast(temp, saveDirection, sizeLineWalk, MaskWalk);

        raycastWalk2 = Physics2D.Raycast(temp2, Vector2.down, (0.1f), MaskWalk);
         
        Debug.DrawRay(temp, saveDirection * (sizeLineWalk), ColorLineWalk);
        Debug.DrawRay(temp2, Vector2.down * (0.1f), ColorLineWalk);

        return (raycastWalk.collider != null || raycastWalk2.collider != null);
    }

    public bool IsJump(Vector2 direction)
    {
        return true;
    }
    
    public bool isPunch(bool isHit)
    {
        if (isHit)
        {

            return (Mathf.Abs(capsuleCollider2D.transform.position.x) - Mathf.Abs(GetEnemyTransform().position.x) >= -0.3 &&
                   Mathf.Abs(capsuleCollider2D.transform.position.x) - Mathf.Abs(GetEnemyTransform().position.x) <= 0.3);
        }
        return false;
    }

    public bool IsHit(Vector2 direction)
    {
        Vector3 temp = new Vector3(capsuleCollider2D.bounds.center.x, capsuleCollider2D.bounds.center.y, capsuleCollider2D.bounds.center.z);


        float j = 0.2f;//3.3f;
        for (int i = 0; i < countRaycastHits; i++)
        {
            float x = Mathf.Sin(j);
            float y = Mathf.Cos(j);
            j += angle * Mathf.Deg2Rad / countRaycastHits;

            Vector2 tempVector = new Vector2(x* direction.x, y);
            raycastHit[i] = Physics2D.RaycastAll(temp, tempVector, capsuleCollider2D.bounds.extents.y + sizeLineHit, MaskEnemy);
            Debug.DrawRay(temp, tempVector * (capsuleCollider2D.bounds.extents.y + sizeLineHit), HitColor);          
        }
        


        for (int i = 0; i < raycastHit.Length; i++)
        {
            for (int x = 0; x < raycastHit[i].Length; x++)
            {
                if (raycastHit[i][x].collider != null &&
                 raycastHit[i][x].collider.gameObject.GetComponent<IUnit>().stateStruct.isControling)
                    return true;
            }
        }
        return false;
    }

    #endregion
}
