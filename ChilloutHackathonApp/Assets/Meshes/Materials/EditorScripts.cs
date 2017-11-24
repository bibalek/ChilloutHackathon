using UnityEngine;
using UnityEditor;
using System;

public class ExampleClass : EditorWindow
{

    [MenuItem("EditorScripts/Create Material")]
    static void CreateMaterial()
    {
        // Create a simple material asset
        GameObject[] objs = Selection.gameObjects;
        GameObject card = null;
        foreach (GameObject gameObject in objs)
        {           
            if (String.Equals(gameObject.name, "card"))
            {
                card = gameObject;
            }
        }
        foreach (GameObject gameObject in objs)
        {
            if (!String.Equals(gameObject.name, "card"))
            {
                var script = gameObject.GetComponent<SpriteRenderer>();
                if (script != null)
                {                  
                    var materialName = script.sprite.name;
                    Material material = new Material(Shader.Find("Standard"));
                    material.mainTexture = script.sprite.texture;
                    AssetDatabase.CreateAsset(material, "Assets/Materials/Cards/" + materialName + ".mat");
                    card.GetComponent<MeshRenderer>().material = material;
                    PrefabUtility.CreatePrefab("Assets/Prefabs/Cards/" + materialName + ".prefab", card.gameObject);
                }
            }
          
        }
      

        // Print the path of the created asset
       // Debug.Log(AssetDatabase.GetAssetPath(material));
    }
    //[MenuItem("EditorScripts/[Selected] Edit Serialized Field")]
    //static void EditField()
    //{
    //    GameObject[] objs = Selection.gameObjects;

    //    foreach (GameObject gameObject in objs)
    //    {
    //        var script = gameObject.GetComponent<AutoRotation>();
    //        if (script != null)
    //        {
    //            script.degPerSec = Random.Range(0, 20);
    //        }
    //    }
    //}

    //[MenuItem("EditorScripts/Check selected name and...")]
    //static void CheckName()
    //{
    //    GameObject[] objs = Selection.gameObjects;

    //    foreach (GameObject gameObject in objs)
    //    {
    //        if (gameObject.name.Contains("Asteroid3") )
    //        {
    //            gameObject.GetComponent<AsteroidExploder>().enabled = false;
    //            gameObject.GetComponent<DestroyableAsteroid>().enabled = false;
    //        }          
    //    }
    //}

    //[MenuItem("EditorScripts/Revert selected prefab")]
    //static void RevertPrefab()
    //{
    //    GameObject[] objs = Selection.gameObjects;

    //    foreach (GameObject gameObject in objs)
    //    {
    //        PrefabUtility.ReconnectToLastPrefab(gameObject);
    //        PropertyModification[] modifications = PrefabUtility.GetPropertyModifications(gameObject);
    //        PrefabUtility.RevertPrefabInstance(gameObject);
    //        PrefabUtility.SetPropertyModifications(gameObject, modifications);
    //    }
    //}
}