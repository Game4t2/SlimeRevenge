using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class StageEditor : EditorWindow
{
    [SerializeField]
    public EnemyWave waveFile;

    [MenuItem("Tool/StageEditor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        StageEditor window = (StageEditor)EditorWindow.GetWindow(typeof(StageEditor));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        waveFile = EditorGUILayout.ObjectField(waveFile, typeof(EnemyWave), false) as EnemyWave;
        if (GUILayout.Button("New", GUILayout.Width(50)))
        {
            waveFile = CreateScriptableObject<EnemyWave>() as EnemyWave;
        }
        if (waveFile != null)
        {
            //EditorGUILayout.PropertyField(waveFile);
        }
        GUILayout.EndHorizontal();
    }

    public static Object CreateScriptableObject<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = "Assets/Resources/Stages";

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        return asset;
    }

}
