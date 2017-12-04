using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExtractableType{
    Radiant
}
public class Extractable {

    public Tile baseTile {get; protected set;}
    public int weight {get; protected set;}
    public ExtractableType extractableType {get; protected set;}
    public int yield {get; protected set;}
    public float yieldRate {get; protected set;}
    public int capacity {get; protected set;}
    public string spriteID = string.Empty; // for re-using the same sprite when re-loading area
    public Extractable(ExtractableType _extractableType, int _weight, int maxYield, float rate, int maxCap)
    {
        yield = Random.Range(1, maxYield);
        capacity = Random.Range(1, maxCap);
        extractableType = _extractableType;
        yieldRate = rate;
        weight = _weight;
    }

    protected Extractable(Extractable b)
    {
        yield = b.yield;
        capacity = b.capacity;
        yieldRate = b.yieldRate;
        extractableType = b.extractableType;
        weight = b.weight;
    }
	
    public static Extractable PlaceInstance(Extractable prototype, Tile tileBase)
    {
        Extractable newExtractable = new Extractable(prototype.Clone());
        if (tileBase.PlaceExtractable(newExtractable) == false){
            // placement failed
            return null;
        }
        return newExtractable;
    }

    Extractable Clone()
    {
        return this;
    }

    public int YieldExtract(int curYield){
        if (curYield > yield)
            return 0;
        if (capacity - curYield <= 0){
            return capacity;
        }
        capacity -= curYield;
        return curYield;
    }
    public bool IsDepleted(){
        if (capacity <= 0)
            return true;
        return false;
    }
    public bool CanRemove(){
        if (baseTile.RemoveExtractable() == false)
            return true;
        return false;
    }
}