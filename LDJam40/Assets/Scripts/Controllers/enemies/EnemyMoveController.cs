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
	Stat speed;
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
        speed = new Stat(moveSpeed, StatType.MoveSpeed);
		curTile = activeArea.GetTile(transform.position);
        aI_Controller = GetComponent<AI_Controller>();
       
       // Modify this unit's speed to add randomness to each unit
       speed.AddModifier(Random.Range(-0.5f, 0.5f));
	}
	public void SetDesiredPos(Vector2 desired)
    {
		//Debug.Log("SetDesiredPos");
        
       // movePercent = 0; 
       // curTile = activeArea.GetTile(transform.position);
		DestTile = activeArea.GetTile(desired);

        
        aI_Controller.anim.ResetTrigger("idle");
        aI_Controller.anim.SetTrigger("moving");

        isMoving = true;

    }

	private void Update()
    {
        if (isMoving)
        {
            DoMovement();
			
            transform.position = new Vector2(X, Y);
        }
    }
	void DoMovement(){
		if (curTile == DestTile)
        {
              // We made it
            StopMove();
            return;
        }
       
		
	    if (nextTile == null || nextTile == curTile)
        {
            // Get the next tile
            if (path == null || path.Length() == 0)
            {
                // Generate path
                path = new Path_AStar(activeArea, curTile, DestTile);
                if (path.Length() == 0)
                {
                    StopMove();
                   // Debug.LogError("Enemy's PathAStar did not return a path!");
                    return;
                }

                // Ignore the first tile since we are on it
                nextTile = path.Dequeue();
            }

            nextTile = path.Dequeue();

            SetDirection();

            ENTERABILITY nextTileEnterability = nextTile.CanEnter();
            switch (nextTileEnterability)
            {
                case ENTERABILITY.Never:
                    // Cant go on
                    StopMove();
                    return;
                
            }

            if (nextTile == curTile)
            {
                StopMove();
                return;
            }
       
    
        }
            // How much distance can we travel this frame?
        float distToTravel = Mathf.Sqrt(
            Mathf.Pow(curTile.X - nextTile.X, 2) +
            Mathf.Pow(curTile.Y - nextTile.Y, 2) );

        // How much distance can we travel this frame?
        float distThisFrame = speed.GetValue() / nextTile.MovementCost * Time.deltaTime;

        float perThisFrame = distThisFrame / distToTravel;

        movePercent += perThisFrame;

        if (movePercent >= 1)
        {
            movePercent = 0;
            curTile = nextTile;
        }
		
	
	}

    void StopMove(){
        isMoving = false;
		movePercent = 0;
        
        SetDirection();
		path = null;
        //aI_Controller.anim.SetTrigger("idle");
    }
	void DestReached(){
		StopMove();
		//curTile = nextTile = destTile;
        nextTile = null;
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
