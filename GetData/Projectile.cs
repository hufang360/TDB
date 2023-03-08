using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Terraria;
using TerrariaApi.Server;

namespace TDB;

public partial class TDBGetData
{
    static void ProjectileNew(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int proID = reader.ReadInt16();
        Vector2 position = reader.ReadVector2();
        Vector2 velocity2 = reader.ReadVector2();
        int owner = reader.ReadByte();
        int type = reader.ReadInt16();
        BitsByte ai0 = reader.ReadByte();
        BitsByte ai1 = (byte)(ai0[2] ? reader.ReadByte() : 0);
        //float[] projFlags = new float[2];
        //projFlags[0] = (ai0[0] ? reader.ReadSingle() : 0f);
        //projFlags[1] = (ai0[1] ? reader.ReadSingle() : 0f);
        var v0 = (ai0[0] ? reader.ReadSingle() : 0f);
        var v1 = (ai0[1] ? reader.ReadSingle() : 0f);
        int bannerIdToRespondTo = (ai0[3] ? reader.ReadUInt16() : 0);
        int damage2 = (ai0[4] ? reader.ReadInt16() : 0);
        float knockBack2 = (ai0[5] ? reader.ReadSingle() : 0f);
        int originalDamage = (ai0[6] ? reader.ReadInt16() : 0);
        int num37 = (ai0[7] ? reader.ReadInt16() : (-1));
        if (num37 >= 1000)
        {
            num37 = -1;
        }
        var v2 = (ai1[0] ? reader.ReadSingle() : 0f);
        if (Main.netMode == 2)
        {
            if (type == 949)
            {
                owner = 255;
            }
            else
            {

            }
        }

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "projIndex", proID  },
            { "position", $"{position.X},{position.Y}" },
            { "owner", owner},
            { "projID", $"{type}" },
        });

    }


    static void ProjectileDestroy(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int projID = reader.ReadInt16();
        int owner = reader.ReadByte();

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "projID", projID  },
            { "owner", owner},
        });
    }

}
