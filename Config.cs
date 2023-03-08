using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TDB;

public class Config
{
    public List<ConfigData> datas = new();

    public void Init()
    {
        datas.Add(new ConfigData("tdb", "TShock调试小助手",
            new List<string> { "tdb" },
            "E:\\Dev-tr\\TShockTDB\\bin\\Debug\\net6.0\\TDB.dll"
            ));

        datas.Add(new ConfigData("wm", "WorldModify",
            new List<string> {
                "worldmodify",
                "moonphase",
                "moonstyle",
                "bossmanage",
                "npcmanage",
                "igen",
                "worldinfo",
                "bossinfo",
                "relive",
                "cleartomb",
                },
            "E:\\Dev-tr\\TShockWorldModify\\WorldModify\\bin\\Debug\\net6.0\\WorldModify.dll"
            ));
    }

    /// <summary>
    /// 加载配置文件
    /// </summary>
    public static Config Load(string path)
    {
        if (File.Exists(path))
        {
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
        }
        else
        {
            var c = new Config();
            c.Init();
            File.WriteAllText(path, JsonConvert.SerializeObject(c, Formatting.Indented));
            return c;
        }
    }
}

public class ConfigData
{
    public string key = "";
    public string name = "";
    public List<string> cmds = new();
    public string fullPath = "";

    public ConfigData(string _key, string _name, List<string> _cmds, string _fullPath)
    {
        key = _key;
        name = _name;
        cmds = _cmds;
        fullPath = _fullPath;
    }
}