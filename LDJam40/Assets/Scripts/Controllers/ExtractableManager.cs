using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractableManager:MonoBehaviour{
    public static ExtractableManager instance {get; protected set;}
    Extractable[] prototypes;
    // defaults:
    int radiantWeight = 5;
    int radiantYieldRate = 5;
    int radiantCap = 100;
    int radiantYield = 5;

    void Awake(){
        instance = this;
    }
    void Start(){
        
        prototypes = new Extractable[]{
            new Extractable(ExtractableType.Radiant, radiantWeight,  radiantYield, radiantYieldRate, radiantCap)
        };
    }
    Extractable GetPrototype(){
        return prototypes[Random.Range(0, prototypes.Length)];
    }
    public void PlaceExtractable(Tile tile){
        if (tile == null)
            return;
        Extractable newExtractable = Extractable.PlaceInstance(GetPrototype(), tile);
        if (newExtractable == null)
            return;
        
    }
    /* public void PoolExtractable(Extractable extractable){
        if (extractGObjs.ContainsKey(extractable) == false)
            return;
        
        if (extractable.baseTile.RemoveExtractable() == false)
            return;

        pool.PoolObject(extractGObjs[extractable]);
        extractGObjs.Remove(extractable);

    } */
}