using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class hSharedData
{
    private static int _nextSceneNum = 1;
    private static Dictionary<Difficulty, List<hLevel>> _levelDic;
    private static hLevel _curLevel;
    private static Difficulty _curDifficulty;
    private static string _userId = "", _userName = "";

    public static int nextSceneNum { get => _nextSceneNum; set => _nextSceneNum = value; }
    public static Difficulty curDifficulty => _curDifficulty;
    public static hLevel curLevel
    {
        get => _curLevel;
        set
        {
            _curLevel = value;
            for (int i = 0; i < (int)Difficulty.count; ++i)
            {
                var difficulty = (Difficulty)i;
                if (_levelDic[difficulty].Contains(_curLevel))
                    _curDifficulty = difficulty;
            }
        }
    }

    public static string userId { get => _userId; set => _userId = value; }
    public static string userName
    {
        get => _userName; 
        set
        {
            _userName = value;
            PlayerPrefs.SetString("userName", _userName);
        }
    }



    #region Binary level
    /*public static void Initialize(hLevel.SerialData[] serialDatas)
    {
        _levelDic = new Dictionary<Difficulty, List<hLevel.SerialData>>();
        _levelDic[Difficulty.NORMAL] = new List<hLevel.SerialData>();
        _levelDic[Difficulty.HARD] = new List<hLevel.SerialData>(); 
        _levelDic[Difficulty.EXTREME] = new List<hLevel.SerialData>();
        var serializedlevels = serialDatas;

        for (int i = 0; i < serializedlevels.Length; ++i)
        {
            var level = serializedlevels[i];
            _levelDic[(Difficulty)level.difficulty].Add(level);
        }
        foreach (var list in _levelDic)
            list.Value.Sort();
    }*/
    #endregion

    public static void Initialize(hLevel[] serialDatas)
    {
        _levelDic = new Dictionary<Difficulty, List<hLevel>>();
        _levelDic[Difficulty.NORMAL] = new List<hLevel>();
        _levelDic[Difficulty.HARD] = new List<hLevel>(); 
        _levelDic[Difficulty.EXTREME] = new List<hLevel>();
        var serializedlevels = serialDatas;

        for (int i = 0; i < serializedlevels.Length; ++i)
        {
            var level = serializedlevels[i];
            _levelDic[level.difficulty].Add(level);
        }

        foreach (var list in _levelDic)
            list.Value.Sort();
    }

    public static List<hLevel> GetLevels(Difficulty difficulty) => _levelDic[difficulty];

    public static bool GetNextLevel(ref hLevel nextLevel)
    {
        List<hLevel> levels = _levelDic[_curDifficulty];
        int index = levels.FindIndex(level => level.Equals(_curLevel));
        if (++index == levels.Count)
        {
            return false;
        }
        else
        {
            nextLevel = levels[index];
            return true;
        }
    }
}
