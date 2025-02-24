using System.IO;
using LevelPlus.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LevelPlus.Network;

public class OrePlacePacket : Packet
{
    public Point16 Position { get; set; }
    
    protected override bool Forward => false;
    protected override void Write(BinaryWriter writer)
    {
        writer.WriteVector2(Position.ToVector2());
    }

    protected override void Read(BinaryReader reader, int whoAmI)
    {
        if (Main.netMode != NetmodeID.Server) return;
        
        OreExperienceSystem.placedOres.Add(reader.ReadVector2().ToPoint16());
    }
}