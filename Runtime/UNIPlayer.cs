using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UNIHper;
using Newtonsoft.Json;
using DNHper;
using UnityEditor;
using System.Xml.Serialization;

public class NetTask
{
    public string TaskName;
    public List<string> TaskParams;

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class NameMaping
{
    [XmlAttribute]
    public string Name;

    [XmlAttribute]
    public string videoKey;
}

[SerializedAt(AppPath.StreamingDir)]
public class UNIPlayer : UConfig
{
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    public static void AddToAssembly()
    {
        UNIHperSettings.AddAssemblyToSettingsIfNotExists(typeof(UNIPlayer).Assembly.GetName().Name);
        AssetDatabase.Refresh();
    }
#endif

    [XmlAttribute]
    public string PlayerIP = "127.0.0.1";

    [XmlAttribute]
    public int PlayerPort = 22222;

    private string NetPlayerKey => $"{PlayerIP}_{PlayerPort}";

    [XmlElement("VideoMap")]
    public List<NameMaping> NameMapings = new List<NameMaping>();

    public void PlayVideo(string name)
    {
        var videoKey = NameMapings.Find(x => x.Name == name)?.videoKey;

        if (videoKey == null)
        {
            videoKey = name;
        }

        Debug.Log($"Play video {videoKey}");
        var task = new NetTask
        {
            TaskName = "ExecuteScript",
            TaskParams = new List<string> { $"PlayVideo({videoKey})", }
        };

        Managements.Network.Send2UdpServer(task.ToJson().ToBytes(), NetPlayerKey);
    }

    public void Back2Idle()
    {
        var task = new NetTask
        {
            TaskName = "ExecuteScript",
            TaskParams = new List<string> { "Back2Idle()" }
        };
        Managements.Network.Send2UdpServer(task.ToJson().ToBytes(), NetPlayerKey);
    }

    // Write your comments here
    protected override string Comment()
    {
        return @"

        <VideoMap Name=""陶庙镇"" Key=""v1"" />
        ";
    }

    // Called once after the config data is loaded
    protected override void OnLoaded()
    {
        Managements.Network.BuildUdpClient(PlayerIP, PlayerPort);
    }

    protected override void OnUnloaded()
    {
        Managements.Network.Dispose();
    }
}
