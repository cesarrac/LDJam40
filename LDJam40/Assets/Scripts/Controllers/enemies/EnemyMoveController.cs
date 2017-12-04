using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveController : MonoBehaviour {
	public Vector2 facingDirection {get; protected set;}
	Tile curTile;
	private Tile nextTile, destTile;
    private Tile DestTile
    {
        get
        {
            return destTile;
        }

        set
        {
            if (destTile != value)
            {
                destTile = value;
                path = null;   // If this is a new destination, then we need to invalidate pathfinding.
            }
        }
    }
    float distToTravel;
    float movePercent;
    public bool isMoving {get; protected set;}
	float speed = 3;
	Vector2 destDirection;
	public delegate void DestinationReached();
    public event DestinationReached onDestReached;
	Path_AStar path;
    AI_Controller aI_Controller;
	float X
    {
        get { 
			if (nextTile == null)
				return curTile.X;
			return Mathf.Lerp(curTile.X, nextTile.X, movePercent); 
			}
    }

    float Y
    {
        get { 
			if (nextTile == null)
				return curTile.Y;
			return Mathf.Lerp(curTile.Y, nextTile.Y, movePercent); }
    }
	Area activeArea;
	public void Init(float moveSpeed){
  		activeArea = AreaController.instance.active_area;
        speed = moveSpeed;
		curTile = activeArea.GetTile(transform.position);
        aI_Controller = GetComponent<AI_Controller>();
       
	}
	public void SetDesiredPos(Vector2 desired)
    {
		//Debug.Log("SetDesiredPos");
        
        movePercent = 0; 
        curTile = activeArea.GetTile(transform.position);
		DestTile = activeArea.GetTile(desired);

        

		if (path == null || path.Length() == 0)
        {
            path = new Path_AStar(activeArea, curTile, DestTile);
            if (path == null || path.Length() == 0){
                Debug.LogError("Path is null!");
                return;
            }
        }
		Tile firstTileInPath = path.Dequeue();

        isMoving = true;
        aI_Controller.anim.SetTrigger("moving");

    }

	private void Update()
    {
        if (isMoving)
        {
            DoMovement();
			
        }
        transform.position = new Vector2(X, Y);
    }
	void DoMovement(){
		if (curTile == DestTile)
        {
            // We made it
            DestReached();
            return;
        }
		if (path.Length() <= 0){
			
            DestReached();
			return;
		}
		if (nextTile == null || nextTile == curTile)
        {
            // Get the next tile

            nextTile = path.Dequeue();

            SetDirection();
        }
		
		ENTERABILITY nextTileEnterability = nextTile.CanEnter();
        switch (nextTileEnterability)
        {
            case ENTERABILITY.Never:
                DestReached();
                return;
            default:
             if (isMoving == false)
                    isMoving = true;
                break;
               
        }

        if (nextTile == curTile)
        {
            isMoving = false;
            return;
        }
        // How much distance can we travel this frame?
		float distToTravel = Mathf.Sqrt(
        	Mathf.Pow(curTile.X - nextTile.X, 2) +
            Mathf.Pow(curTile.Y - nextTile.Y, 2) );

        // How much distance can we travel this frame?
        float distThisFrame = speed / nextTile.MovementCost * Time.deltaTime;

        float perThisFrame = distThisFrame / distToTravel;

       movePercent += perThisFrame;

        if (movePercent >= 1)
        {
            curTile = nextTile;
            movePercent = 0;
        }
	}

    void StopMove(){
        isMoving = false;
		movePercent = 0;
		path = null;
        aI_Controller.anim.SetTrigger("idle");
    }
	void DestReached(){
		StopMove();
		destTile = curTile;
		Debug.Log("Destination reached");
	/* 	if (onDestReached != null){
			onDestReached();
		} */
	}
	public void SetDirection()
    {
		if (nextTile == null)
			return;
        if (nextTile.X > curTile.X)
        {
            facingDirection = Vector2.right;
        }
        else if (nextTile.X < curTile.X)
        {
            facingDirection = Vector2.left;
        }
        else if (nextTile.Y > curTile.Y)
        {
            facingDirection = Vector2.up;
        }
        else 
        {
            facingDirection = Vector2.down;
        }

        aI_Controller.anim.SetFloat("x", facingDirection.x);
        aI_Controller.anim.SetFloat("y", facingDirection.y);
    }
}
