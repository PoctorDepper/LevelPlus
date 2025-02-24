using System.IO;
using LevelPlus.Players;
using Terraria;

namespace LevelPlus.Network;

public class GainExperiencePacket : Packet
{
    public long Amount { get; set; }

    protected override bool Forward => false;
    
    protected override void Write(BinaryWriter writer)
    {
        writer.Write(Amount);
    }

    protected override void Read(BinaryReader reader, int whoAmI)
    {
        Amount = reader.ReadInt64(); 
        
        Main.LocalPlayer.GetModPlayer<LevelPlayer>().GainExperience(Amount);
    }
}