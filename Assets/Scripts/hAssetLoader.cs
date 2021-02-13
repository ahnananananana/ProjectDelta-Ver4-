using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class hAssetLoader : MonoBehaviour
{
    private static bool _isStart = true;
    [SerializeField]
    private hLoading _loading;
    [SerializeField]
    private hThumbNailStudio m_thumbnailStudio;
    [SerializeField]
    private bool isDev;

    [SerializeField] private hLevel[] levels;

    void Start()
    {
        if (!_isStart)
        {
            Destroy(gameObject);
            return;
        }

        hSharedData.Initialize(levels);
        _isStart = false;
        _loading.FirstLoading();
        /*if (isDev)
        {
            hNetworkManager.serverURL = "http://127.0.0.1";
        }

        

        int length = Mathf.Min(SystemInfo.deviceUniqueIdentifier.Length, 10);
        hSharedData.userId = SystemInfo.deviceUniqueIdentifier.Substring(0, length);
        hSharedData.userName = PlayerPrefs.GetString("userName", "Player" + hSharedData.userId);*/
        /*if (_isStart)
        {
            _isStart = false;
            //StartCoroutine(GetAssetBundle());
        }*/
    }

    private IEnumerator GetAssetBundle()
    {
        /*//if current version is lower
        var version = PlayerPrefs.GetString("Version", "99");
#if UNITY_EDITOR
        PlayerPrefs.DeleteAll();
        version = "99";
#endif

        UnityWebRequest www = UnityWebRequest.Get(hNetworkManager.serverURL + "/levelasset/" + version);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Application.Quit();
        }
        else
        {
            string assetBundleDirectory = Application.persistentDataPath + "/AssetBundle";
            string serialDataPath = Application.persistentDataPath + "/SerialData";

            // 에셋 번들을 저장할 경로의 폴더가 존재하지 않는다면 생성시킨다.
            if (!Directory.Exists(assetBundleDirectory))
                Directory.CreateDirectory(assetBundleDirectory);
            if (!Directory.Exists(serialDataPath))
                Directory.CreateDirectory(serialDataPath);*/

            AssetBundle assetBundle;
            hLevel[] levelDatas;
            GameObject[] assets;
            Material[] mats;

        /*if (www.downloadHandler.data.Length == 0)
        {*/
#if UNITY_EDITOR
        assetBundle = AssetBundle.LoadFromFile("C:/Users/HURJM/Desktop/work/Unity/ProjectDelta(URP)/AssetBundles/StandaloneWindows/levels.unity3d");
#else
        assetBundle = AssetBundle.LoadFromFile("levels.unity3d");
#endif
        if (assetBundle == null)
                {
                    //Debug.Log("Failed to load AssetBundle!");
                    yield break;
                }

                mats = assetBundle.LoadAllAssets<Material>();
                for (int i = 0; i < mats.Length; ++i)
                {
                    var mat = mats[i];
                    mat.shader = Shader.Find(mat.shader.name);
                    if (mat.name.Contains("Ground"))
                        hDatabase.current.groundMats.Add(mat);
                }

                //binary 파일은 textasset으로 저장하고 불러옴
                assets = assetBundle.LoadAllAssets<GameObject>();
                levelDatas = new hLevel[assets.Length];
                for (int i = 0; i < assets.Length; ++i)
                    levelDatas[i] = assets[i].GetComponent<hLevel>();

                hSharedData.Initialize(levelDatas);
                _loading.FirstLoading();
                yield break;
            /*}

            var studio = Instantiate(m_thumbnailStudio);

            string lastVersion = "";
            int head = 0;
            for (; head < www.downloadHandler.data.Length; ++head)
            {
                if ((char)www.downloadHandler.data[head] == '?')
                {
                    PlayerPrefs.SetString("Version", lastVersion);
                    ++head;
                    break;
                }
                lastVersion += (char)www.downloadHandler.data[head];
            }

            byte[] data = new byte[www.downloadHandler.data.Length - head];
            System.Array.Copy(www.downloadHandler.data, head, data, 0, data.Length);

            // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
            File.WriteAllBytes(Path.Combine(assetBundleDirectory, "levels.unity3d"), data);

            assetBundle = AssetBundle.LoadFromFile(Path.Combine(assetBundleDirectory, "levels.unity3d"));

            if (assetBundle == null)
            {
                //Debug.Log("Failed to load AssetBundle!");
                yield break;
            }

            //binary 파일은 textasset으로 저장하고 불러옴
            mats = assetBundle.LoadAllAssets<Material>();
            for (int i = 0; i < mats.Length; ++i)
            {
                var mat = mats[i];
                mat.shader = Shader.Find(mat.shader.name);
                if (mat.name.Contains("Ground"))
                    hDatabase.current.groundMats.Add(mat);
            }

            assets = assetBundle.LoadAllAssets<GameObject>();
            levelDatas = new hLevel[assets.Length];
            for (int i = 0; i < assets.Length; ++i)
                levelDatas[i] = assets[i].GetComponent<hLevel>();

            var lastThemeColor = hColorManager.current.curColor;
            for (int i = 0; i < levelDatas.Length; ++i)
                studio.SaveThumbnail(levelDatas[i]);

            hColorManager.current.SetTheme(lastThemeColor, true);
            hSharedData.Initialize(levelDatas);
            _loading.FirstLoading();*/

#region Binary Asset
            /*TextAsset[] textAssets;
            AssetBundle assetBundle;
            hLevel.SerialData[] serialDatas;
            MemoryStream memoryStream;
            BinaryFormatter formatter;

            if (www.downloadHandler.data.Length == 0)
            {
                assetBundle = AssetBundle.LoadFromFile(Path.Combine(assetBundleDirectory, "levels.unity3d"));
                if (assetBundle == null)
                {
                    //Debug.Log("Failed to load AssetBundle!");
                    yield break;
                }
                else
                    //Debug.Log("Successed to load AssetBundle!");

                //binary 파일은 textasset으로 저장하고 불러옴
                textAssets = assetBundle.LoadAllAssets<TextAsset>();
                serialDatas = new hLevel.SerialData[textAssets.Length];

                memoryStream = new MemoryStream();
                formatter = new BinaryFormatter();
                for (int i = 0; i < textAssets.Length; ++i)
                {
                    memoryStream.Write(textAssets[i].bytes, 0, textAssets[i].bytes.Length);
                    memoryStream.Position = 0;
                    serialDatas[i] = (hLevel.SerialData)formatter.Deserialize(memoryStream);
                    memoryStream.SetLength(0);
                }
                memoryStream.Close();

                hSharedData.Initialize(serialDatas);
                _loading.FirstLoading();
                yield break;
            }

            var studio = Instantiate(m_thumbnailStudio);

            string lastVersion = "";
            int head = 0;
            for (; head < www.downloadHandler.data.Length; ++head)
            {
                if((char)www.downloadHandler.data[head] == '?')
                {
                    PlayerPrefs.SetString("Version", lastVersion);
                    ++head;
                    break;
                }
                lastVersion += (char)www.downloadHandler.data[head];
            }

            byte[] data = new byte[www.downloadHandler.data.Length - head];
            System.Array.Copy(www.downloadHandler.data, head, data, 0, data.Length);

            // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
            File.WriteAllBytes(Path.Combine(assetBundleDirectory , "levels.unity3d"), data);

            assetBundle = AssetBundle.LoadFromFile(Path.Combine(assetBundleDirectory, "levels.unity3d"));

            if (assetBundle == null)
            {
                //Debug.Log("Failed to load AssetBundle!");
                yield break;
            }
            else
                //Debug.Log("Successed to load AssetBundle!");

            //binary 파일은 textasset으로 저장하고 불러옴
            textAssets = assetBundle.LoadAllAssets<TextAsset>();

            serialDatas = new hLevel.SerialData[textAssets.Length];

            memoryStream = new MemoryStream();
            formatter = new BinaryFormatter();
            for (int i = 0; i < textAssets.Length; ++i)
            {
                memoryStream.Write(textAssets[i].bytes, 0, textAssets[i].bytes.Length);
                memoryStream.Position = 0;
                serialDatas[i] = (hLevel.SerialData)formatter.Deserialize(memoryStream);
                memoryStream.SetLength(0);
            }
            memoryStream.Close();

            var lastThemeColor = hColorManager.current.curColor;
            for (int i = 0; i < serialDatas.Length; ++i)
                studio.SaveThumbnail(serialDatas[i]);
            hColorManager.current.SetTheme(lastThemeColor, true);
            hSharedData.Initialize(serialDatas);
            _loading.FirstLoading();*/
#endregion
        }
    //}
}
