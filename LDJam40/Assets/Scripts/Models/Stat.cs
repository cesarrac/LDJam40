using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { Damage, MoveSpeed, Armor, YieldModifier, YieldRateModifier }
public class Stat  {

   // public StatType statType;
    [SerializeField]
    float statValue;
    public StatType statType { get; protected set; }
    List<float> modifiers;

    public Stat(float value, StatType sType)
    {
        statType = sType;
        statValue = value;
        modifiers = new List<float>();
    }

    public float GetValue()
    {
        float finalValue = statValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(float modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(float modifier)
    {
        modifiers.Remove(modifier);
    }

    public void ClearAllModifiers()
    {
        modifiers.Clear();
    }
}
