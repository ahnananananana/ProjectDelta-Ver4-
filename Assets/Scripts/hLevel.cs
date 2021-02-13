using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public enum Difficulty
{
    NORMAL,
    HARD,
    EXTREME,
    count,
}


public class hLevel : MonoBehaviour, System.IComparable<hLevel>, System.IEquatable<hLevel>
{
    #region Serialization
    [System.Serializable]
    public struct SerialData : System.IEquatable<SerialData>, System.IComparable<SerialData>
    {
        public int levelNum;
        public int difficulty;
        public hLevelObject.SerialData[] levelObjects;

        public SerialData(int levelNum = -1, int difficulty = -1, hLevelObject.SerialData[] levelObjects = null)
        {
            this.levelNum = levelNum;
            this.difficulty = difficulty;
            this.levelObjects = levelObjects;
        }

        public hLevel DeSerialize(Transform parent)
        {
            GameObject gob = new GameObject();
            gob.name = "Level";
            gob.transform.SetParent(parent);
            gob.transform.localPosition = Vector3.zero;
            gob.transform.localRotation = Quaternion.Euler(Vector3.zero);
            gob.transform.localScale = Vector3.one;

            var collider = gob.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            var level = gob.AddComponent<hLevel>();
            level.levelNum = levelNum;
            level.difficulty = (Difficulty)difficulty;

            List<hPortal> portals = new List<hPortal>();
            hLevelObject[] levelObjects = new hLevelObject[this.levelObjects.Length];
            for (int i = 0; i < levelObjects.Length; ++i)
            {
                levelObjects[i] = this.levelObjects[i].DeSerialize(gob.transform);
                if (levelObjects[i].GetType() == typeof(hPortal))
                    portals.Add((hPortal)levelObjects[i]);
            }

            while(portals.Count > 0)
            {
                var portal1 = portals[0];
                portals.RemoveAt(0);
                for (int i = 0; i < portals.Count; ++i)
                {
                    var portal2 = portals[i];
                    if (portal1.nextPortalId == portal2.id)
                    {
                        portal1.nextPortal = portal2;
                        if (portal2.nextPortalId == portal1.id)
                        {
                            portal2.nextPortal = portal1;
                            portals.RemoveAt(i);
                        }
                        break;
                    }
                }
            }

            level.Initialize(levelObjects);
            Physics.SyncTransforms();

            return level;
        }

        public bool Equals(SerialData other)
        {
            if (levelNum == other.levelNum && difficulty == other.difficulty)
                return true;
            return false;
        }
        public int CompareTo(SerialData other)
        {
            if (levelNum > other.levelNum)
                return 1;
            else
                return -1;
        }
    }

    public SerialData Serialize()
    {
        hWayPoint.totalNum = 0;
        hPortal.totalNum = 0;
        SerialData serialData = new SerialData();
        serialData.levelNum = _levelNum;
        serialData.difficulty = (int)_difficulty;
        _levelObjects = GetComponentsInChildren<hLevelObject>();

        serialData.levelObjects = new hLevelObject.SerialData[_levelObjects.Length];
        for (int i = 0, j = 0; i < _levelObjects.Length; ++i , ++j)
            serialData.levelObjects[j] = _levelObjects[i].Serialize();

        return serialData;
    }
    #endregion

    private static hLevel _instance;

    private hGameBall _gameBall;
    [SerializeField]
    private Difficulty _difficulty;
    [SerializeField]
    private int _levelNum;

    private hLevelObject[] _levelObjects;
    private List<hWayPoint> _wayPointList;
    private List<hWinKey> _winKeyList;

    [SerializeField] private Sprite thumbnail;

    public hGameBall gameBall { get => _gameBall; set => _gameBall = value; }

    public static hLevel current => _instance;
    public int levelNum { get => _levelNum; set => _levelNum = value; }
    public Difficulty difficulty { get => _difficulty; set => _difficulty = value; }
    public List<hWinKey> winKeyList  => _winKeyList;

    public List<hWayPoint> wayPointList { get => _wayPointList; set => _wayPointList = value; }
    public Sprite Thumbnail => thumbnail; 

    private void Awake()
    {
        _instance = this;
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<hGameBall>()?.Die();
        other.GetComponent<hWinZone>()?.Lose();
    }

    public void Initialize(hLevelObject[] levelObjects = null/*, Camera camera = null*/)
    {
        _gameBall = GetComponentInChildren<hGameBall>();
        _levelObjects = levelObjects;
    }

    public void AddWayPoint(hWayPoint wayPoint)
    {
        if (_wayPointList == null)
            _wayPointList = new List<hWayPoint>();
        _wayPointList.Add(wayPoint);
    }

    public void AddWinKey(hWinKey winkey)
    {
        if (_winKeyList == null)
            _winKeyList = new List<hWinKey>();
        _winKeyList.Add(winkey);
    }

    public Vector3 GetWayPointPos(int index) => _wayPointList[index].transform.position;

    public void Save()
    {
        var serialData = Serialize();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Application.dataPath);
        stringBuilder.Append("/SerializedLevels/");
        stringBuilder.Append(_difficulty);
        stringBuilder.Append("_");
        stringBuilder.Append(_levelNum);
        stringBuilder.Append(".bytes");
        //Debug.Log(stringBuilder.ToString());
        FileStream stream = new FileStream(stringBuilder.ToString(), FileMode.Create, FileAccess.Write);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, serialData);
        stream.Close();
    }

    public int CompareTo(hLevel other)
    {
        if (_levelNum > other._levelNum)
            return 1;
        else
            return -1;
    }

    public bool Equals(hLevel other)
    {
        if (_levelNum == other._levelNum && _difficulty == other._difficulty)
            return true;
        return false;
    }
}
