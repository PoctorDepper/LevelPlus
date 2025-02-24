using System.IO;
using LevelPlus.Players;
using LevelPlus.Systems;
using Terraria;
using Terraria.ModLoader;

namespace LevelPlus.Network;

public class StatPacket : Packet
{
    public string Id { get; set; }
    
    public int Value { get; set; }

    protected override bool Forward => true;

    public void SetStat(Stat stat)
    {
        Id = stat.Id;
        Value = stat.Value;
    }
    
    protected override void Write(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(Value);
    }

    protected override void Read(BinaryReader reader, int whoAmI)
    {
        Id = reader.ReadString();
        Value = reader.ReadInt32();

        if (Id == "level")
        {
            Main.player[whoAmI].GetModPlayer<LevelPlayer>().Level = Value;
        }
        else
        {
            ModContent.GetInstance<StatSystem>().GetStat(whoAmI, Id).Value = Value;
        }
    }
}