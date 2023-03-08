using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace TDB;

public class LoadPlugin
{
    public static Main main;

    public LoadPlugin()
    {
    }

    /// <summary>
    /// 重载插件（测试用）
    /// </summary>
    public static void Reload(TSPlayer op, string Name, List<string> cmds, string rawDllPath)
    {
        var dllName = rawDllPath.Split("\\").Last();
        string dllpath = $"ServerPlugins/{dllName}";
        string newDllPath = Environment.OSVersion.Platform == PlatformID.Win32NT ? rawDllPath : dllName;
        if (!File.Exists(newDllPath))
        {
            Console.WriteLine($"指定位置未能找到插件文件：{newDllPath}，重载操作已取消！");
            return;
        }

        // 释放
        try
        {
            List<PluginContainer> li = new();
            li.AddRange(ServerApi.Plugins);

            // 释放插件（似乎无法完全释放）
            foreach (var val in li.Where(p => p.Plugin.Name == Name))
            {
                if (val.Initialized)
                {
                    val.DeInitialize();
                }
                val.Dispose();
            }
            li.Clear();

            // 移除指令（未能完全释放，先把旧插件的部分注册指令移除）
            foreach (Command c in Commands.ChatCommands)
            {
                //    Log($"{string.Join(",",c.Names)} c.Name:{c.Name}, c.DoLog:{c.DoLog}");
                foreach (var ckey in cmds)
                {
                    if (c.Names.Contains(ckey))
                        c.Names.Remove(ckey);
                }
            }
        }
        catch (Exception arg)
        {
            op.SendErrorMessage($"插件:{Name}在卸载时出错:{arg}");
        }

        // 加载
        File.Copy(newDllPath, dllpath, true);
        Load(op, dllpath);
    }

    public static void Load(TSPlayer op, string dllpath)
    {
        try
        {
            Assembly assembly = Assembly.Load(File.ReadAllBytes(dllpath));
            foreach (Type type in assembly.GetExportedTypes())
            {
                if (type.IsSubclassOf(typeof(TerrariaPlugin)) && type.IsPublic && !type.IsAbstract)
                {
                    object[] customAttributes = type.GetCustomAttributes(typeof(ApiVersionAttribute), false);
                    if (customAttributes.Length != 0)
                    {
                        TerrariaPlugin pluginInstance = (TerrariaPlugin)Activator.CreateInstance(type, main);
                        PluginContainer plg = new(pluginInstance);
                        plg.Initialize();
                        ServerApi.Plugins.Append(plg);
                        string md5 = GetMD5HashFromFile(dllpath);
                        if (md5.Length > 4)
                            md5 = md5[..4];
                        op.SendInfoMessage($"{plg.Plugin.Name} v{plg.Plugin.Version}（{md5}）已重载！");
                    }
                }
            }
        }
        catch (Exception value)
        {
            Console.WriteLine(value);
        }
    }

    static TSPlayer GetPlayer(int whoAmI)
    {
        return whoAmI == -1 ? TSPlayer.Server : TShock.Players[whoAmI];
    }

    private static string GetDllPath(string dllName)
    {
        if (dllName.ToLowerInvariant().EndsWith(".dll"))
            return $"ServerPlugins/{dllName}";
        else
            return $"ServerPlugins/{dllName}.dll";
    }

    /// <summary>
    /// 计算文件的md5
    /// </summary>
    /// <returns></returns>
    public static string GetMD5HashFromFile(string filePath)
    {
        FileStream file = new(filePath, FileMode.Open);
        MD5 md5 = MD5.Create();
        byte[] hashBytes = md5.ComputeHash(file);
        file.Close();
        return Convert.ToHexString(hashBytes); // .NET 5 +

        // Convert the byte array to hexadecimal string prior to .NET 5
        // StringBuilder sb = new System.Text.StringBuilder();
        // for (int i = 0; i < hashBytes.Length; i++)
        // {
        //     sb.Append(hashBytes[i].ToString("X2"));
        // }
        // return sb.ToString();

    }


}