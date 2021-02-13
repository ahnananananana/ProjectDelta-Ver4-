using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MemoryStream stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        byte[] a = { 1, 2, 3, 4 };

        stream.Write(a, 0, a.Length);
        hLevel.SerialData s = (hLevel.SerialData)formatter.Deserialize(stream);

    }

}
