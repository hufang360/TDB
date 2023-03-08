using System.Collections.Generic;
using System.IO;
using System.Text;
using Terraria;
using TerrariaApi.Server;

namespace TDB;

public partial class TDBGetData
{

    public static void PlayerUpdate(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int whoAmI = reader.ReadByte();
        Player player10 = Main.player[whoAmI];
        BitsByte bitsByte7 = reader.ReadByte();
        BitsByte bitsByte8 = reader.ReadByte();
        BitsByte bitsByte9 = reader.ReadByte();
        BitsByte bitsByte10 = reader.ReadByte();

        var s = "";
        if (bitsByte8[0])
        {
            s = $"pulley:true, pulleyDir:{(byte)((!bitsByte8[1]) ? 1u : 2u)}";
        }
        else
        {
            s = $"pulley:false";
        }

        var selectedItem = reader.ReadByte();
        var pos = reader.ReadVector2();
        //TShock.Log.ConsoleInfo($"whoAmI:{whoAmI}, pos:{player10.position.X / 16},{player10.position.Y / 16}");
        //TShock.Log.ConsoleInfo($"\ncontrolUp:{bitsByte7[0]}, controlDown:{bitsByte7[1]}, controlLeft:{bitsByte7[2]}, controlRight:{bitsByte7[3]}, controlJump:{bitsByte7[4]}, controlUseItem:{bitsByte7[5]}, direction:{(bitsByte7[6] ? 1 : (-1))}，vortexStealthActive:{bitsByte8[3]}, gravDir:{(bitsByte8[4] ? 1 : (-1))}, shouldGuard:{bitsByte8[5]}, ghost:{bitsByte8[6]}, selectedItem:{selectedItem}, {s}");

        s = "";
        if (bitsByte8[2])
        {
            s = $"velocity:{reader.ReadVector2()}";
        }
        else
        {
            //player10.velocity = Vector2.Zero;
            s = $"velocity:0";
        }
        if (bitsByte9[6])
        {
            //player10.PotionOfReturnOriginalUsePosition = reader.ReadVector2();
            //player10.PotionOfReturnHomePosition = reader.ReadVector2();
            reader.ReadVector2();
            reader.ReadVector2();
        }
        //TShock.Log.ConsoleInfo($"{s}, tryKeepingHoveringUp:{bitsByte9[0]}, IsVoidVaultEnabled:{bitsByte9[1]}, sitting.isSitting:{bitsByte9[2]}, downedDD2EventAnyDifficulty:{bitsByte9[3]}, isPettingAnimal:{bitsByte9[4]}, isTheAnimalBeingPetSmall:{bitsByte9[5]}, tryKeepingHoveringDown:{bitsByte9[7]}, autoReuseAllWeapons:{bitsByte10[1]}, controlDownHold:{bitsByte10[2]}, isOperatingAnotherEntity:{bitsByte10[3]}, controlUseTile:{bitsByte10[4]}, sleeping:{bitsByte10[0]}");
    }





    static void PlayerSlot(GetDataEventArgs args)
    {
        using MemoryStream ms = new(args.Msg.readBuffer, args.Index, args.Length);
        using BinaryReader reader = new(ms, Encoding.UTF8, true);

        int playerID = reader.ReadByte();
        int slotID = reader.ReadInt16();
        int stack = reader.ReadInt16();
        int prefix = reader.ReadByte();
        int netID = reader.ReadByte();

        Utils.LogGetData($"{args.MsgID}={(int)args.MsgID}", new Dictionary<object, object>()
        {
            { "playerID", playerID },
            { "slotID", slotID },
            { "stack", stack },
            { "prefix", prefix },
            { "netID", netID },
        });
    }



}
