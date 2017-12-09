using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeroController))]
public class HeroEditor : Editor {
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        if (GUILayout.Button("Get 10 ore")){
            HeroController controller = (HeroController)target;
            controller.TestAddOre(10);
        }
    }
}