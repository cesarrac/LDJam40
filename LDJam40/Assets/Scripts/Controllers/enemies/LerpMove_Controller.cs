using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    N,E,S,W
}
public class LerpMove_Controller : MonoBehaviour {
	public Vector2 facingDirection {get; protected set;}
    Vector2 curPos;

	float pMoveX, pMoveY;
	int areaWidth = 100, areaHeight = 100;
    float move_x {
		get{return pMoveX;}
		set{
			pMoveX = Mathf.Clamp(value, 0, areaWidth);
		}
	}
	float move_y{
		get{return pMoveY;}
		set{
			pMoveY = Mathf.Clamp(value, 0, areaHeight);
		}
	}
	
    Vector2 desiredPos, lastPos;

    float distToTravel;

    float movePercent;

    public float moveSpeed = 3;

    bool returnsToLastPos;

    Action TimerActionCB;
    Action OnPositionReachedCB;

    bool isMoving;
    AI_Controller aI_Controller;
   // public Direction facingDirection = Direction.N;

    float X
    {
        get { return Mathf.Lerp(curPos.x, desiredPos.x, movePercent); }
    }

    float Y
    {
        get { return Mathf.Lerp(curPos.y, desiredPos.y, movePercent); }
    }

    void Start(){
        aI_Controller = GetComponent<AI_Controller>();
    }

    public void Init(float speed)
    {
        moveSpeed = 0.25f;
        curPos = desiredPos = transform.position;
        isMoving = false;
        OnPositionReachedCB = null;
        Debug.Log("enemy moving at speed " + moveSpeed);
    }

    public void SetDesiredPos(Vector2 desired, bool returnsToLast = false, Action onPosreachedCB = null)
    {
        movePercent = 0;
        curPos = transform.position;
        lastPos = curPos;
        desiredPos = desired;
        returnsToLastPos = returnsToLast;

        distToTravel = Mathf.Sqrt(
          Mathf.Pow(curPos.x - desiredPos.x, 2) +
          Mathf.Pow(curPos.y - desiredPos.y, 2));

        isMoving = true;

        OnPositionReachedCB = null;
        if (onPosreachedCB != null)
        {
            OnPositionReachedCB = onPosreachedCB;
        }

        SetDirection();

         aI_Controller.anim.SetTrigger("moving");
    }

    void Update()
    {
        if (isMoving)
        {
            LerpMovement();

            transform.position = new Vector2(X, Y);

            curPos = transform.position;
        }
    }

    void LerpMovement()
    {

        float distThisFrame = moveSpeed  * Time.deltaTime;

        float perThisFrame = distThisFrame / distToTravel;

        movePercent += perThisFrame;
        //curPos == desiredPos
        if (movePercent >= 1f)
        {
            movePercent = 0;
            isMoving = false;

            //Debug.Log(gameObject.name + "reached position " + transform.position);

            if (returnsToLastPos == true)
                SetDesiredPos(lastPos);

            if (OnPositionReachedCB != null)
            {
                OnPositionReachedCB();
            }
             aI_Controller.anim.SetTrigger("idle");
        }
        
    }

    public void SetDirection()
    {
           if (desiredPos.y > curPos.y)
        {
            facingDirection = Vector2.up;
        }
        else if (desiredPos.y < curPos.y)
        {
            facingDirection = Vector2.down;
        }
        else if (desiredPos.x > curPos.x)
        {
            facingDirection = Vector2.right;
        }
        else if (desiredPos.x < curPos.x)
        {
            facingDirection = Vector2.left;
        }
       
        aI_Controller.anim.SetFloat("x", facingDirection.x);
        aI_Controller.anim.SetFloat("y", facingDirection.y);

    }
	void ClampPosition(){
		move_x = transform.position.x;
		move_y = transform.position.y;
	
		transform.position = new Vector2(move_x, move_y);
	}
    void OnDisable()
    {
        isMoving = false;
        curPos = desiredPos = transform.transform.position;
        OnPositionReachedCB = null;
    }
}
