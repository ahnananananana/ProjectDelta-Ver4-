using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class hThumbNailStudio : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera;
    private RenderTexture m_renderTexture;
    [SerializeField]
    private int m_imageHeight;
    private string m_folderPath;
    [SerializeField]
    private hLevelSetter m_levelSetter;
    [SerializeField]
    private Transform m_skybox;

    private void Awake()
    {
        m_folderPath = Path.Combine(Application.persistentDataPath, "Thumbnails");
        if (!Directory.Exists(m_folderPath))
            Directory.CreateDirectory(m_folderPath);
        float screenRatio = Screen.width / (float)Screen.height;
        m_renderTexture = new RenderTexture((int)(m_imageHeight * screenRatio), m_imageHeight, 24, RenderTextureFormat.ARGB32);
        m_renderTexture.useMipMap = false;
        m_renderTexture.autoGenerateMips = false;
        m_camera.targetTexture = m_renderTexture;
    }
    public void SaveThumbnail(hLevel serialData)
    {
        m_skybox.gameObject.SetActive(true);
        hLevel level = m_levelSetter.CreateLevel(serialData);
        m_levelSetter.AdjustCamera(m_camera, level, m_skybox);

        m_camera.Render();
        RenderTexture.active = m_renderTexture;
        Texture2D tex = new Texture2D(m_renderTexture.width, m_renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), 0, 0, false);
        RenderTexture.active = null;
        byte[] bytes = tex.EncodeToPNG();

        string fileName = level.difficulty.ToString() + "_" + level.levelNum.ToString() + ".png";
        string path = Path.Combine(m_folderPath, fileName);
        File.WriteAllBytes(path, bytes);

        level.gameObject.SetActive(false);
        m_skybox.gameObject.SetActive(false);
        /*Thread thread = new Thread(new ThreadStart(() => File.WriteAllBytes(path, bytes)));
        thread.Start();*/
    }
    #region Binary serial
    /*public void SaveThumbnail(hLevel.SerialData serialData)
    {
        m_skybox.gameObject.SetActive(true);
        hLevel level = m_levelSetter.CreateLevel(serialData);
        m_levelSetter.AdjustCamera(m_camera, level, m_skybox);

        m_camera.Render();
        RenderTexture.active = m_renderTexture;
        Texture2D tex = new Texture2D(m_renderTexture.width, m_renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, m_renderTexture.width, m_renderTexture.height), 0, 0, false);
        RenderTexture.active = null;
        byte[] bytes = tex.EncodeToPNG();

        string fileName = level.difficulty.ToString() + "_" + level.levelNum.ToString() + ".png";
        string path = Path.Combine(m_folderPath, fileName);
        File.WriteAllBytes(path, bytes);

        level.gameObject.SetActive(false);
        m_skybox.gameObject.SetActive(false);
        *//*Thread thread = new Thread(new ThreadStart(() => File.WriteAllBytes(path, bytes)));
        thread.Start();*//*
    }*/
    #endregion
}
