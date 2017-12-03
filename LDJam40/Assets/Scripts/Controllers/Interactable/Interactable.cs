using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    [Header("Max Distance for Interact to trigger")]
    //public float maxDistance = 1.0f;
    [HideInInspector]
    public Vector3 interactionWorldPoint;
    public CircleCollider2D circleCollider;
    public float desiredRadius = 1.0f;
    [HideInInspector]
    public GameObject interactor;

    public virtual void Init(Vector3 worldPoint)
    {
        interactionWorldPoint = worldPoint;
        if (circleCollider == null)
            circleCollider = GetComponentInChildren<CircleCollider2D>();
        if (circleCollider != null)
            circleCollider.radius = desiredRadius;
    }
    public virtual void Interact()
    {
       // Debug.Log("Interacting with " + gameObject.name);
    }

    public virtual void TryInteract(GameObject user)
    {
        interactor = user;
        //if (Vector2.Distance(interactionWorldPoint, user.transform.position) <= maxDistance)
        if (circleCollider.OverlapPoint(interactor.transform.position) == true)
        {
            Interact();
        }
        else
        {
            Debug.Log(user.name + " is too far to interact with " + interactionWorldPoint);
        }
    }
}
