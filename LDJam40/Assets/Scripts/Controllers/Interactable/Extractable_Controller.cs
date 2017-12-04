using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractable_Controller : Interactable {
    Extractable myExtractable;
    [Header("how much now and how much available")]
    int yield, capacity; // how much at a time and how much total
    [Header("How fast do you get it")]
    float yieldRate; // how fast you get it
    float pProgress;
    float curProgress {
        get {return pProgress;}
        set{pProgress = Mathf.Clamp(value, 0, yieldRate + 1);}
    }
    public float Progress { get{return curProgress;}}
    SpriteRenderer sprite_renderer;
    Animator anim;
    void OnEnable(){
        circleCollider = GetComponentInChildren<CircleCollider2D>();
        sprite_renderer = transform.GetChild(1).gameObject.GetComponentInChildren<SpriteRenderer>();
        anim = sprite_renderer.transform.GetChild(0).gameObject.GetComponentInChildren<Animator>();
        anim.gameObject.SetActive(false);
    }

    public void InitExtractable(Extractable thisExtractable){
        myExtractable = thisExtractable;
        if (myExtractable == null){
            Debug.LogError("Extract_Controller is not receivin an extractor class!");
            return;
        }
        // Get sprite:
        if (myExtractable.spriteID != string.Empty){
            sprite_renderer.sprite = SpriteManager.instance.GetSprite(myExtractable.spriteID);
        }else{
            sprite_renderer.sprite = SpriteManager.instance.GetOreSprite();
            myExtractable.spriteID = sprite_renderer.sprite.name;
        }
        curProgress = 0;
        yield = myExtractable.yield;
        yieldRate = myExtractable.yieldRate;
        capacity = myExtractable.capacity;
        Init(transform.position);
    }

    public override void TryInteract(GameObject user)
    {
        base.TryInteract(user);

        Debug.Log("trying to interact!");
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log(interactor.name + " has interacted with " + gameObject.name);
        StartExtraction();
    }
    void StartExtraction(){
        StopCoroutine("ContinueExtraction");   
         StopCoroutine("DecreaseProgress");
         anim.gameObject.SetActive(true);
         StartCoroutine("ContinueExtraction"); 
    }
    IEnumerator ContinueExtraction(){
        while(true){
            if (interactor == null || interactor.activeSelf == false){
                EndExtraction();
                yield break;
            }
        // if extractor steps out of bounds, timer stops
            if (circleCollider.OverlapPoint(interactor.transform.position) == false)
            {
                Debug.Log(interactor.name + " is now too far to extract from " + transform.position);
                EndExtraction();
                yield break;
            }

            if (curProgress >= yieldRate){
                YieldExtract();
                curProgress = 0;
            }else{
                curProgress += Time.deltaTime;
               // Debug.Log("extraction progress: " + curProgress + " out of " + yieldRate);
            }
            yield return null;
        }
    }
    void EndExtraction(){
        anim.gameObject.SetActive(false);
        StartCoroutine("DecreaseProgress");
    }
    void YieldExtract(){
        int curYield = yield;
        curYield = myExtractable.YieldExtract(curYield);
       
        SendYieldToExtractor(curYield);
        anim.SetTrigger("yielding");
        // Check if this extractable is depleted and needs to be pooled
        if (myExtractable.IsDepleted() == true){
            EndExtraction();
            if (myExtractable.CanRemove() == true){
                PoolThis();
            }
        }
    }
    void SendYieldToExtractor(int curYield){
        if (interactor == null || interactor.activeSelf == false){
                return;
        }
        // Send to inventory
        interactor.GetComponent<HeroController>().heroInventory.AddItem(myExtractable, curYield);
         Debug.Log("yielding " + curYield + " ore!");
    }
    IEnumerator DecreaseProgress(){
        while(true){
            if (curProgress <= 0)
                yield break;
            yield return new WaitForSeconds(0.25f);
            curProgress -= 0.01f;
            yield return null;
        }
    }
    void PoolThis(){
        // un-parent from tile gobj
        this.transform.SetParent(null);
        myExtractable = null;
        ObjectPool.instance.PoolObject(this.gameObject);
    }
}