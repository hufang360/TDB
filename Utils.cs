using System.Collections.Generic;
using TShockAPI;

namespace TDB;

public class Utils
{
    /// <summary>
    /// 输出日志
    /// </summary>
    public static void Log(object obj) { TShock.Log.ConsoleInfo($"[tdb]{obj}"); }

    public static void LogDict(Dictionary<object, object> dict)
    {
        Log(DictToString(dict));
    }
    public static void LogGetData(string title, Dictionary<object, object> dict)
    {
        TShock.Log.ConsoleInfo($"{title}, {DictToString(dict)}");
    }

    static string DictToString(Dictionary<object, object> dict, bool ignoreEmpty = true)
    {
        var li = new List<string>();
        foreach (var ele in dict)
        {
            var s = ele.Value.ToString();
            if (ignoreEmpty && string.IsNullOrEmpty(s))
            {
                continue;
            }
            li.Add($"{ele.Key}: {s}");
        }
        return string.Join(", ", li);
    }
}
