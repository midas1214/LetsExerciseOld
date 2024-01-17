using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

[InitializeOnLoad]
public class AutoSave
{
    static AutoSave()
    {
        EditorApplication.playModeStateChanged += SaveOnPlay;
    }

    private static void SaveOnPlay(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            Debug.Log("Auto Saving---");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}