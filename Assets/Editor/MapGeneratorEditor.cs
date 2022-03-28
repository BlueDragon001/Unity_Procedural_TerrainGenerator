using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator) target;
        
        base.OnInspectorGUI();
        if(GUILayout.Button("UwU Press me Onii Chan")){
            mapGen.GenerateMap();
            
        }
        if(mapGen.autoUpdate){
            mapGen.GenerateMap();
        }
        
    }
}
