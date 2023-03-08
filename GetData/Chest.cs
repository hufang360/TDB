using System.Collections.Generic;
using System.IO;
using System.Text;
using TerrariaApi.Server;

namespace TDB;

public partial class TDBGetData
{
    public static void ChestGetContents(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int x = reader.ReadInt16();
        int y = reader.ReadInt16();

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "x", x  },
            { "y", y  },
        });
    }


    public static void ChestItem(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int chestID = reader.ReadInt16();
        int itemSlot = reader.ReadByte();
        int stack = reader.ReadInt16();
        int prefix = reader.ReadByte();
        int itemID = reader.ReadInt16();

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "chestID", chestID  },
            { "itemSlot", itemSlot},
            { "stack", stack},
            { "prefix", prefix},
            { "itemID", itemID},
        });
    }


    public static void ChestOpen(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int chestID = reader.ReadInt16();
        int x = reader.ReadInt16();
        int y = reader.ReadInt16();
        int nameLength = reader.ReadByte();
        string name = "";
        if (nameLength != 0)
        {
            if (nameLength <= 20)
            {
                name = reader.ReadString();
            }
            else if (nameLength != 255)
            {
                nameLength = 0;
            }
        }

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "chestID", chestID  },
            { "x", x},
            { "y", y},
            { "nameLength", nameLength},
            { "name", name}
        });
    }


    public static void PlaceChest(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        byte action = reader.ReadByte();
        int x = reader.ReadInt16();
        int y = reader.ReadInt16();
        int style = reader.ReadInt16();
        int chestID = reader.ReadInt16();

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "chestID", chestID  },
            { "x", x},
            { "y", y},
            { "style", style},
            { "chestID", chestID  },
        });
    }



}
