using System.Collections.Generic;
using System.IO;
using System.Text;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using TerrariaApi.Server;
using TShockAPI;

namespace TDB;

public partial class TDBGetData
{
    static bool listenGetData = false;
    public static void ToggleGetData(TSPlayer op, TerrariaPlugin main)
    {
        if (listenGetData)
        {
            DisposeGetData(main);
            op.SendSuccessMessage("已关闭调试！");
        }
        else
        {
            listenGetData = true;
            ServerApi.Hooks.NetGetData.Register(main, GetData);
            op.SendSuccessMessage("已开启调试，请切换至控制台上查看调试信息！");
        }
    }

    public static void DisposeGetData(TerrariaPlugin main)
    {
        listenGetData = false;
        ServerApi.Hooks.NetGetData.Deregister(main, GetData);
    }


    static void GetData(GetDataEventArgs args)
    {
        // 调试备注
        // 参考：https://tshock.readme.io/edit/multiplayer-packet-structure
        // 泰拉1.4.4.x时改动了一些参数，具体请用 ILSpy 反编译一份泰拉的源码查看
        // 消息包的接收，参考：MessageBuffer.cs
        // 消息包的发送，参考：NetMessage.cs

        // MsgID值
        // tshock项目：https://github.com/Pryaxis/TSAPI/blob/general-devel/TerrariaServerAPI/TerrariaApi.Server/PacketTypes.cs
        // 泰拉源码：Terraria.ID.MessageID.cs

        switch (args.MsgID)
        {
            default: Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()); return;

            case PacketTypes.PlayerUpdate: PlayerUpdate(args); break;
            case PacketTypes.PlayerSlot: PlayerSlot(args); break;

            case PacketTypes.ProjectileNew: ProjectileNew(args); break;
            case PacketTypes.ProjectileDestroy: ProjectileDestroy(args); break;

            case PacketTypes.MassWireOperation: MassWireOperation(args); break;

            case PacketTypes.PlaceObject: PlaceObject(args); break;
            case PacketTypes.Tile: Tile(args); break;
            case PacketTypes.TileSendSquare: TileSendSquare(args); break;
            case PacketTypes.PlaceTileEntity: PlaceTileEntity(args); break;
            case PacketTypes.PlaceItemFrame: PlaceItemFrame(args); break;
            case PacketTypes.TileEntityHatRackItemSync: TileEntityHatRackItemSync(args); break;

            case PacketTypes.ChestGetContents: ChestGetContents(args); break;
            case PacketTypes.ChestItem: ChestItem(args); break;
            case PacketTypes.ChestOpen: ChestOpen(args); break;
            case PacketTypes.PlaceChest: PlaceChest(args); break;
        }
    }


    static void PlaceObject(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int x = reader.ReadInt16();
        int y = reader.ReadInt16();
        short tileType = reader.ReadInt16();
        int tileStyle = reader.ReadInt16();
        int size = reader.ReadByte();
        int random = reader.ReadSByte();
        int direction = (reader.ReadBoolean() ? 1 : (-1));
        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "x", x },
            { "y", y },
            { "tileType", tileType },
            { "style", tileStyle },
            { "size", size },
            { "random", random },
            { "direction", direction },
        });

    }


    static void MassWireOperation(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int startX = reader.ReadInt16();
        int startY = reader.ReadInt16();
        int endX = reader.ReadInt16();
        int endY = reader.ReadInt16();
        int ToolMode = reader.ReadByte();

        // ToolMode BitFlags: 1 = Red, 2 = Green, 4 = Blue, 8 = Yellow, 16 = Actuator, 32 = Cutter 33移除红电线 34移绿
        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "startX", startX },
            { "startY", startY },
            { "endX", endX },
            { "endY", endY },
            { "ToolMode", ToolMode },
        });
    }


    static void Tile(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        byte Action = reader.ReadByte();
        int tileX = reader.ReadInt16();
        int tileY = reader.ReadInt16();
        int flags1 = reader.ReadInt16();
        int flags2 = reader.ReadByte();

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "Action", Action },
            { "tileX", tileX },
            { "tileY", tileY },
            { "flags1", flags1 },
            { "flags2", flags2 },
        });
    }


    static void TileSendSquare(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int x = reader.ReadInt16();
        int y = reader.ReadInt16();
        ushort width = reader.ReadByte();
        ushort length = reader.ReadByte();
        byte b13 = reader.ReadByte();

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "x", x },
            { "y", y },
            { "width", width },
            { "length", length },
            { "tileChangeType", b13 },
        });
    }


    static void PlaceTileEntity(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int x = reader.ReadInt16();
        int y = reader.ReadInt16();
        ushort tileEntityType = reader.ReadByte();

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "x", x },
            { "y", y },
            { "tileEntityType", tileEntityType },
        });
    }


    static void PlaceItemFrame(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        short x = reader.ReadInt16();
        int y = reader.ReadInt16();
        int id = reader.ReadInt16();
        int prefix = reader.ReadByte();
        int stack = reader.ReadInt16();

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "x", x  },
            { "y", y  },
            { "id", id  },
            { "prefix", prefix  },
            { "stack", stack  },
        });
    }


    static void TileEntityHatRackItemSync(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int whoAmI = reader.ReadByte();
        int hatRackIndex = reader.ReadInt32();
        int itemIndex = reader.ReadByte();
        int id = 0;
        int stack = 0;
        int prefix = 0;
        //NetMessage.TrySendData(b, -1, whoAmI, null, whoAmI, tileEntityID, itemIndex, dye.ToInt());


        if (!TileEntity.ByID.TryGetValue(hatRackIndex, out var value4))
        {
            //reader.ReadInt32();
            //reader.ReadByte();
        }
        else
        {
            if (itemIndex >= 2)
            {
                value4 = null;
            }
            if (value4 is TEHatRack tEHatRack)
            {
                id = reader.ReadUInt16();
                stack = reader.ReadUInt16();
                prefix = reader.ReadByte();
            }
            else
            {
                //reader.ReadInt32();
                //reader.ReadByte();
            }
        }

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "whoAmI", whoAmI  },
            { "hatRackIndex", hatRackIndex  },
            { "itemIndex", itemIndex  },
            { "id", id==0?"":$"{id}"  },
            { "stack", stack==0?"":$"{stack}"  },
            { "prefix", prefix==0?"":$"{prefix}"  },
        });
    }





}
