using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractable : MonoBehaviour{

    int yield, capacity; // how much at a time , how much total
    float yieldRate; // how fast you get it
    float pProgress;
    float curProgress {
        get {return pProgress;}
        set{pProgress = Mathf.Clamp(value, 0, yieldRate + 1);}
    }
    CircleCollider2D circleCollider;
    void OnEnable(){
        curProgress = 0;
        circleCollider = GetComponentInChildren<CircleCollider2D>();

    }
    void StartExtraction(){
         StopCoroutine("DecreaseProgress");
    }
    void ContinueExtraction(GameObject _extractor){
        // if extractor steps out of bounds, timer stops
        if (circleCollider.OverlapPoint(_extractor.transform.position) == false)
        {
            Debug.Log(_extractor.name + " is too far to extract from " + transform.position);
            EndExtraction();
            return;
        }

        if (curProgress >= yieldRate){
            YieldExtract();
            curProgress = 0;
        }else{
            curProgress += Time.deltaTime;
        }
    }
    void EndExtraction(){
        StartCoroutine("DecreaseProgress");
    }
    void YieldExtract(){
        Debug.Log("yielding extract!");
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
}