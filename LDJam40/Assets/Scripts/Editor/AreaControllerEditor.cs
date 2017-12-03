using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AreaController))]
public class AreaControllerEditor : Editor {
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Interior")){
            AreaController controller = (AreaController)target;
            controller.GenerateArea(1, 5, 5);
        }
         if (GUILayout.Button("Generate Exterior")){
            AreaController controller = (AreaController)target;
            controller.GenerateArea(0, 25, 25);
        }
    }
}