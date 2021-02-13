using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

public struct hUserRecord
{
    public string id;
    public string userName;
    public float record;

    public hUserRecord(string id = "", string userName = "", float record = -1)
    {
        this.id = id;
        this.userName = userName;
        this.record = record;
    }
}

public class hDatabase : MonoBehaviour
{
    private static hDatabase instance;
    public static hDatabase current
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<hDatabase>();
                instance?.Init();
            }
            if(instance == null)
            {
                GameObject gob = new GameObject
                {
                    name = "Database"
                };
                instance = gob.AddComponent<hDatabase>();
                DontDestroyOnLoad(gob);
            }
            return instance;
        }
    }


    private Dictionary<System.Type, hLevelObject> levelObjectPrefabDic;
    //private string levelObjectsPrefabPath = "GameObjects/LevelObjects";
    private Dictionary<string, List<hUserRecord>> levelRecordDic;
    [SerializeField]
    private AudioClip _ballFlyClip;
    [SerializeField]
    private AudioMixerGroup _audioMixerGroup;
    private List<Material> _groundMats;

    private static bool isInit = false;
    public AudioClip ballFlyClip { get => _ballFlyClip; set => _ballFlyClip = value; }
    public AudioMixerGroup audioMixerGroup { get => _audioMixerGroup; set => _audioMixerGroup = value; }
    public List<Material> groundMats { get => _groundMats; set => _groundMats = value; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (isInit)
        {
            DestroyImmediate(gameObject);
            return;
        }
        isInit = true;
        instance = this;

        _groundMats = new List<Material>();

        levelObjectPrefabDic = new Dictionary<System.Type, hLevelObject>();

        //var levelObjects = Resources.LoadAll<hLevelObject>(levelObjectsPrefabPath);

        /*for (int i = 0; i < levelObjects.Length; ++i)
            levelObjectPrefabDic[levelObjects[i].GetType().UnderlyingSystemType] = levelObjects[i];*/

        levelRecordDic = new Dictionary<string, List<hUserRecord>>();
        DontDestroyOnLoad(gameObject);
    }

    public void AddRecord(string key, hUserRecord userRecord)
    {
        if (!levelRecordDic.ContainsKey(key))
            levelRecordDic[key] = new List<hUserRecord>();

        levelRecordDic[key].Add(userRecord);
        levelRecordDic[key].Sort((record1, record2) => { if (record1.record > record2.record) return 1; return -1; } );
    }

    public hLevelObject GetLevelObject(System.Type t) => levelObjectPrefabDic[t];

    public AsyncOperation GetLevelRecords(Difficulty difficulty, int levelNum, List<hUserRecord> userRecords)
    {
        List<hUserRecord> recordTemp;
        var key = ((int)difficulty).ToString() + levelNum;
        if (levelRecordDic.TryGetValue(key, out recordTemp))
        {
            for (int i = 0; i < recordTemp.Count; ++i)
                userRecords.Add(recordTemp[i]);
            return null;
        }
        else
        {
            var op = RequestLevelRecords(difficulty, levelNum);
            op.completed += (AsyncOperation _) =>
            {
                if (!levelRecordDic.ContainsKey(key))
                    return;
                recordTemp = levelRecordDic[key];
                for (int i = 0; i < recordTemp.Count; ++i)
                    userRecords.Add(recordTemp[i]);
            };
            return op;
        }
    }

    private AsyncOperation RequestLevelRecords(Difficulty difficulty, int levelNum)
    {
        UnityWebRequest www = UnityWebRequest.Get(hNetworkManager.serverURL + "/ranking/" + (int)difficulty + "/" + levelNum);
        var op = www.SendWebRequest();
        op.completed += (AsyncOperation _ ) =>
        {
            if (www.downloadHandler.data.Length == 0)
                return;
            string data = System.Text.Encoding.Default.GetString(www.downloadHandler.data);

            string[] userDataStrs = data.Split('|');
            for (int i = 0; i < userDataStrs.Length; ++i)
            {
                string[] userDataStr = userDataStrs[i].Split('?');
                string diff = userDataStr[0];
                string lvNum = userDataStr[1];
                string id = userDataStr[2];
                string userName = userDataStr[3];
                float record = float.Parse(userDataStr[4]);

                string key = diff + lvNum;
                AddRecord(key, new hUserRecord(id, userName, record));
            }
        };
        return op;
    }

}
