using UnityEditor;
using UnityEngine;

public static class hLevelSerializationOnce
{
    [MenuItem("Level/Serialize Levels At Once")] 
    static void SerializeLevelsAtOnce()
    {
        var normal = Resources.LoadAll<hLevel>("Levels/Normal");
        var hard = Resources.LoadAll<hLevel>("Levels/Hard");
        var extreme = Resources.LoadAll<hLevel>("Levels/Extreme");

        for (int i = 0; i < normal.Length; ++i)
        {
            var level = normal[i];
            level.difficulty = Difficulty.NORMAL;
            level.levelNum = i + 1;
            level.Save();
        }

        for (int i = 0; i < hard.Length; ++i)
        {
            var level = hard[i];
            level.difficulty = Difficulty.HARD;
            level.levelNum = i + 1;
            level.Save();
        }

        for (int i = 0; i < extreme.Length; ++i)
        {
            var level = extreme[i];
            level.difficulty = Difficulty.EXTREME;
            level.levelNum = i + 1;
            level.Save();
        }
    }
}
