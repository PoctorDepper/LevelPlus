using System.IO;
using LevelPlus.Players;
using Terraria;

namespace LevelPlus.Network;

public class TeamSharePacket : Packet
{
    public long Amount { get; set; }
    public int Team { get; set; }

    protected override bool Forward => true;
    
    protected override void Write(BinaryWriter writer)
    {
        writer.Write(Amount);
        writer.Write(Team);
    }

    protected override void Read(BinaryReader reader, int whoAmI)
    {
        Amount = reader.ReadInt64(); 
        Team = reader.ReadInt32();
        
        var player = Main.LocalPlayer;
        
        if (player.team == Team) player.GetModPlayer<LevelPlayer>().GainExperience(Amount, true);
    }
}