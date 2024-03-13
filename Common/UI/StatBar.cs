// Copyright (c) Bitwiser.
// Licensed under the Apache License, Version 2.0.

using System.Linq;
using LevelPlus.Common.Players;
using LevelPlus.Common.Players.Stats;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace LevelPlus.Common.UI;

public class StatBar : UIElement
{
  private const float StatIconWidth = 20f;
  private const float StatIconHeight = 20f;
  private const float StatBarWidth = 100f;
  private const float StatBarHeight = 20f;
  private const float ButtonAddWidth = 20f;
  private const float ButtonAddHeight = 20f;
  private const float HintWidth = 20f;
  private const float HintHeight = 20f;

  private UITexture buttonAdd;
  private UITexture hint;
  private UITexture icon;
  private UITexture statBar;
  private UIText statText;
  private BaseStat stat;

  public StatBar(BaseStat stat)
  {
    this.stat = stat;
  }

  public override void OnInitialize()
  {
    base.OnInitialize();

    Width.Set(StatIconWidth + StatBarWidth + ButtonAddWidth + HintWidth, 0f);
    Height.Set(new[] { StatIconHeight, StatBarHeight, ButtonAddHeight, HintHeight }.Max(), 0f);

    icon = new UITexture(stat.IconPath);
    icon.Width.Set(StatIconWidth, 0f);
    icon.Height.Set(StatIconHeight, 0f);
    icon.Left.Set(0f, 0f);
    icon.Top.Set(0, 0.5f);
    icon.color = stat.UIColor;

    statBar = new UITexture("LevelPlus/Assets/Textures/UI/StatBar");
    statBar.Width.Set(StatBarWidth, 0f);
    statBar.Height.Set(StatBarHeight, 0f);
    statBar.Left.Set(0f, 0f);
    statBar.Top.Set(0f, 0.5f);
    statBar.color = stat.UIColor;

    statText = new UIText(stat.Value.ToString());
    statText.Width.Set(0, 1f);
    statText.Height.Set(0, 1f);
    statText.TextOriginY = 0.5f;
    statText.TextColor = stat.UIColor;

    statBar.Append(statText);

    buttonAdd = new UITexture("LevelPlus/Assets/Textures/UI/StatAdd", true);
    buttonAdd.Width.Set(ButtonAddWidth, 0f);
    buttonAdd.Height.Set(ButtonAddHeight, 0f);
    buttonAdd.Left.Set(StatBarWidth, 0f);
    buttonAdd.Top.Set(0f, 0f);
    buttonAdd.color = stat.UIColor;
    buttonAdd.OnLeftClick += delegate { Main.LocalPlayer.GetModPlayer<StatPlayer>().AddStat(stat.Id); };

    hint = new UITexture("LevelPlus/Assets/Textures/UI/Hint", true);
    hint.Width.Set(HintWidth, 0f);
    hint.Height.Set(HintHeight, 0f);
    hint.Left.Set(StatIconWidth + StatBarWidth + ButtonAddWidth, 0f);
    hint.Top.Set(0, 0.5f);

    Append(icon);
    Append(statBar);
    Append(buttonAdd);
    Append(hint);
  }

  public override void OnDeactivate()
  {
    base.OnDeactivate();

    icon = null;
    statBar = null;
    buttonAdd = null;
    hint = null;
  }

  public override void Update(GameTime time)
  {
    base.Update(time);

    statText.SetText(stat.Value.ToString());

    if (hint.IsMouseHovering) Main.instance.MouseText(stat.Description.Value);
  }
}