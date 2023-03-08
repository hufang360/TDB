using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace TDB;

[ApiVersion(2, 1)]
public partial class TDB : TerrariaPlugin
{
    public override string Name => "TShock调试小助手";
    public override string Author => "hufang360";
    public override string Description => "获取调试信息";
    public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

    Config Con;
    readonly string ConDir = Path.Combine(TShock.SavePath, "TDB");

    public TDB(Main game) : base(game)
    {
        LoadPlugin.main = game;
    }

    public override void Initialize()
    {
        Commands.ChatCommands.Add(new Command("tdb", Manage, "tdb") { HelpText = "TShock调试小助手" });

        if (!Directory.Exists(ConDir))
        {
            Directory.CreateDirectory(ConDir);
        }
        Con = Config.Load(Path.Combine(ConDir, "config.json"));
    }

    void Manage(CommandArgs args)
    {

        void Help()
        {
            var li = new List<string>()
            {
                "/tdb, 调试开关",
                "/tdb handler, handler调试开关（目前仅写了几条调试项）",
                "/tdb debug, 临时打开tshock的debug模式",
                "/tdb clearlog, 清空tshock日志文件",
                "/tdb load <xx.dll>, 加载插件目录下的插件",
                "/tdb rp <key>, 重载指定插件",
                "/tdb rp list, 查询插件的key（来自配置文件）",
                "/tdb reload, 重载配置文件",
            };
            args.Player.SendInfoMessage(string.Join("\n", li));
        }
        if (args.Parameters.Count == 0)
        {
            TDBGetData.ToggleGetData(args.Player, this);
            return;
        }

        string fileName;
        switch (args.Parameters[0].ToLowerInvariant())
        {
            case "handler":
                TDBHandler.ToggleHandler(args.Player);
                return;

            // 清空日志
            case "clearlog":
                ILog log = TShock.Log;
                fileName = log.FileName;
                TShock.Log = new TextLog(Path.Combine(TShock.Config.Settings.LogPath, "tdb.log"), false);
                log.Dispose();
                File.Delete(fileName);

                log = TShock.Log;
                TShock.Log = new TextLog(fileName, false);
                log.Dispose();
                args.Player.SendSuccessMessage("已清空日志文件");
                break;

            // 临时开启tshockdebug（测试用）
            case "debug":
                TShock.Config.Settings.DebugLogs = !TShock.Config.Settings.DebugLogs;
                args.Player.SendInfoMessage($"tshock debug模式:{TShock.Config.Settings.DebugLogs}");
                return;


            // 重载配置文件
            case "reload":
                Con = Config.Load(Path.Combine(ConDir, "config.json"));
                args.Player.SendSuccessMessage("[tdb]已重载配置文件");
                break;

            // 重载插件
            case "rp":
            case "reloadplugin":
                if (args.Parameters.Count == 1)
                {
                    args.Player.SendInfoMessage("请输入要重载的插件，例如 /tdb rp <key>，key值可以输入/tdb rp list获得");
                }
                else if (args.Parameters[1].ToLowerInvariant() == "list")
                {
                    foreach (var d in Con.datas)
                    {
                        args.Player.SendInfoMessage(d.key);
                    }
                }
                else
                {
                    var flag = false;
                    foreach (var d in Con.datas.Where(d => d.key == args.Parameters[1].ToLowerInvariant()))
                    {
                        LoadPlugin.Reload(args.Player, d.name, d.cmds, d.fullPath);
                        flag = true;
                    }
                    if (!flag)
                    {
                        args.Player.SendErrorMessage("key不存在！");
                    }
                }
                break;

            case "load":
                if (args.Parameters.Count == 1)
                {
                    args.Player.SendInfoMessage("请输入dll文件名，例如 /tdb load tdb.dll");
                }
                else
                {
                    fileName = args.Parameters[1];
                    if (!fileName.EndsWith(".dll"))
                        fileName += ".dll";
                    string dllpath = $"ServerPlugins/{fileName}";
                    if (!File.Exists(dllpath))
                    {
                        Console.WriteLine($"未能找到 {dllpath}");
                        return;
                    }
                    else
                    {
                        LoadPlugin.Load(args.Player, dllpath);
                    }
                }
                break;


            case "help":
            case "h":
                Help();
                return;

            default:
                args.Player.SendErrorMessage("输入 /tdb help 查看指令用法");
                break;
        }

    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TDBGetData.DisposeGetData(this);
            TDBHandler.DisposeHandler();
        }
        base.Dispose(disposing);
    }
}