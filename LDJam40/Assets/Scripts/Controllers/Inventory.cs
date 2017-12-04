using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {
    public static Inventory instance {get; protected set;}
    //  public int maxSize;
    public float maxWeightCapacity { get; protected set; }
    public int maxSize { get; protected set; }
    public delegate void OnInventoryChanged(int index);
    public event OnInventoryChanged onInventoryChanged;
    public List<Extractable> itemsStored{ get; protected set; } 
    int maxInventorySpaces = 5;
    public int currentWeight
    {
        get
        {
            if (itemsStored == null)
                return 0;
            if (itemsStored.Count == 0)
                return 0;
            int weight = 0;
            itemsStored.ForEach(item => weight += item.weight);
            return weight;
        }
    }
    public Inventory(float maxWeight = 1000, int maxSpaces = 10)
    {
        instance = this;
        maxWeightCapacity = maxWeight;
        maxSize = maxSpaces;
        itemsStored = new List<Extractable>();
    }

    public Extractable PeekItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= itemsStored.Count)
            return null;
        return itemsStored[itemIndex];
    }

    public virtual bool AddItem(Extractable item, int count = 1)
    {
        if (currentWeight + item.weight > maxWeightCapacity)
        {
            Debug.Log("Carrying too much!");

            return false;
        }
        for(int i = 0; i < count; i++){
            
            itemsStored.Add(item);
        }
        Debug.Log(count + " " + item.extractableType + " added to inventory");
        if (onInventoryChanged != null)
            onInventoryChanged(itemsStored.Count);
        return true;
    }

    public virtual bool RemoveItem(Extractable itemToRemove)
    {
        if (itemToRemove == null)
            return false;
        Debug.Log(itemToRemove.extractableType + " removed from inventory.");
        itemsStored.Remove(itemToRemove);
        if (onInventoryChanged != null)
            onInventoryChanged(itemsStored.Count);
        return true;
    }
    public int GetAllOf(ExtractableType itemType){
        List<Extractable>toRemove = new List<Extractable>();
        int count = 0;
        for (int i = 0; i < itemsStored.Count; i++)
        {
            if (itemsStored[i].extractableType == itemType){
                toRemove.Add(itemsStored[i]);

            }
        }
        if (toRemove.Count > 0){
            foreach(Extractable item in toRemove){
                if (RemoveItem(item) == false)
                    continue;
                count++;
            }
        }
        return count;
    }
    
    public int TotalInstancesOfItem(Extractable item)
    {
        if (itemsStored.Contains(item) == false)
            return 0;
        int count = 0;
        for (int i = 0; i < itemsStored.Count; i++)
        {
            if (itemsStored[i] == item)
                count++;
        }
        return count;
    }
     public int TotalInstancesOfItem(ExtractableType itemType)
    {
        int count = 0;
        for (int i = 0; i < itemsStored.Count; i++)
        {
            if (itemsStored[i].extractableType == itemType)
                count++;
        }
        return count;
    }
}
