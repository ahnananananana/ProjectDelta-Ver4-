using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class hNetworkManager
{
    private static string s_serverURL = "http://ec2-3-34-181-106.ap-northeast-2.compute.amazonaws.com";
    static hNetworkManager()
    {

    }

    public static string serverURL { get => s_serverURL; set => s_serverURL = value; }
}
