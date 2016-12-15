using UnityEngine;
using UnityEditor;
using System.Collections;

public static class Global
{
    public static string EnemyWavePath(string name = "")
    {
        string path = "";
       // path = System.IO.Path.Combine(AssetDatabase.GetAssetPath(), "Resources/Stages");
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        path = System.IO.Path.Combine(path, name);
        return path;
    }

}

public enum WinLose { win, lose, equal }
public enum Element { Normal = 0, Fire = 1, Water = 2, Grass = 3, Electric = 4, Soil = 5 }
public enum EnemyUnitType { NONE = 0, Sword, Pike, Mualer, Gunner, Mage, Cannon };
public enum SlimeUnitType { NONE = 0, Sword, Pike, Cannon, Shield, HERO }