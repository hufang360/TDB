using System.Collections.Generic;
using TShockAPI;

namespace TDB;

public partial class TDBHandler
{
    static bool listenHandler = false;
    public static void ToggleHandler(TSPlayer op)
    {
        if (listenHandler)
        {
            DisposeHandler();
            op.SendSuccessMessage("已关闭handler调试！");
        }
        else
        {
            listenHandler = true;
            GetDataHandlers.SendTileRect += OnTileRect;
            GetDataHandlers.PlaceTileEntity += OnPlaceTileEntity;
            op.SendSuccessMessage("已开启handler调试，请切换至控制台上查看调试信息！");
        }
    }

    public static void DisposeHandler()
    {
        listenHandler = false;
        GetDataHandlers.SendTileRect -= OnTileRect;
        GetDataHandlers.PlaceTileEntity -= OnPlaceTileEntity;
    }


    static void OnTileRect(object sender, GetDataHandlers.SendTileRectEventArgs e)
    {
        //var index = e.Player.Index;
        Utils.LogDict(new Dictionary<object, object>() {
            {$"OnTileRect/{PacketTypes.TileSendSquare}", (int)PacketTypes.TileSendSquare },
            {"TileX",e.TileX },
            {"TileY",e.TileY },
            {"Width",e.Width },
            {"Length",e.Length },
            {"ChangeType",e.ChangeType },
        });
    }

    static void OnPlaceTileEntity(object sender, GetDataHandlers.PlaceTileEntityEventArgs e)
    {
        Utils.LogDict(new Dictionary<object, object>() {
            {$"OnPlaceTileEntity/{PacketTypes.PlaceTileEntity}", (int)PacketTypes.PlaceTileEntity },
            {"X",e.X },
            {"Y",e.Y },
            {"Type",e.Type },
        });
    }


}
