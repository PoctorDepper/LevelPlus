using System.Collections.Generic;
using System.Linq;
using LevelPlus.Configs;
using LevelPlus.Players;
using Terraria;
using Terraria.ModLoader;

namespace LevelPlus.Systems;

public class StatSystem : ModSystem
{
    // Base instances of Stat implementations, should not modify these directly
    private SortedList<string, ModPlayer> stats;
    
    public void ValidateStats(Terraria.Player player)
    {
        var maxPoints = player.GetModPlayer<LevelPlayer>().Level * PlayConfiguration.Instance.Level.Points +
                        PlayConfiguration.Instance.StartingPoints;
        var playerStats = GetStats(player.whoAmI);

        while (playerStats.Sum(s => s.Value) > maxPoints)
        {
            playerStats.ForEach(s => s.Value -= s.Value > 0 ? 1 : 0);
        }

        player.GetModPlayer<LevelPlayer>().Points = maxPoints - playerStats.Sum(s => s.Value);
    }

    public Stat GetStat(int player, string id)
    {
        if (!stats.TryGetValue(id, out var statPlayer)) return null;

        return (Stat)Main.player[player].GetModPlayer(statPlayer);
    }

    public List<Stat> GetStats(int player)
    {
        return stats.Values
            .Select(p => (Stat)Main.player[player].GetModPlayer(p))
            .ToList();
    }

    public override void Load()
    {
        stats = new SortedList<string, ModPlayer>();
    }

    public override void Unload()
    {
        stats.Clear();
        stats = null;
    }

    public override void OnModLoad()
    {
        foreach (var player in ModContent.GetContent<ModPlayer>())
        {
            if (player as Stat is not { } stat) continue;

            stats.Add(stat.Id, player);
        }
    }
}