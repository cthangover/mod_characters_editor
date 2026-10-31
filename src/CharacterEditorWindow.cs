using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.Json.Nodes;
using Cthangover.Core.Characters;
using Cthangover.Core.Games;
using Cthangover.Core.Mods.Resolvers;
using Cthangover.Core.Settings;
using Cthangover.Mods.Stats;
using Cthangover.Tools.Common;
using Cthangover.Tools.EntityEditor;
using Godot;

namespace Cthangover.Tools.CharactersEditor
{
    /// <summary>
    /// Editor of characters (<c>"Kind": "character"</c>, <see cref="CharacterInfo"/>):
    /// heroes, NPCs and enemies alike. Name in every language of the mod, the
    /// portrait (<c>characters</c> group, shown in dialogs and battles), the RPG
    /// map look (<c>Charset</c> in the <c>charsets</c> group: the RPG map editor
    /// places the character with it; a novel leaves it empty), battle stats and the
    /// battle actions offered from the loaded <c>action</c> entities. Loot and
    /// other mods' fields are "other fields" of the base window. New characters go
    /// to <c>characters/&lt;mod&gt;.json</c>.
    /// <para>
    /// Plan 12.5: the panel under the form marks the party leader (the character
    /// whose <c>Charset</c> the player walks with on RPG maps: the game setting
    /// <c>stats.player</c>, else the first battle-set member, as <c>RpgPlayerLook</c>
    /// reads it) and shows the values a character's <c>Stats</c> compute. A character
    /// with <c>Stats</c> gets its health, attack and defence from them, so those three
    /// fields are read-only here (validator rule <c>STAT_FIELD_OVERRIDDEN</c>, plan 6.4).
    /// Setting the leader is the game settings' job (mod manager), not a button here.
    /// </para>
    /// </summary>
    public partial class CharacterEditorWindow : EntityEditorWindow
    {
        /// <summary>Builds the window and opens the first mod that has characters.</summary>
        public CharacterEditorWindow() : base("tools/characters_editor/title", "character", "characters", Fields())
        {
        }

        private static List<EntityField> Fields() => new()
        {
            new EntityField { Key = "Name", LabelKey = "tools/characters_editor/name", Kind = EntityFieldKind.LocaleText, KeyPattern = "characters/{0}/name" },
            new EntityField { Key = "Image", LabelKey = "tools/characters_editor/image", TooltipKey = "tools/characters_editor/image_tip", Kind = EntityFieldKind.Picture, Group = "characters" },
            new EntityField { Key = "Charset", LabelKey = "tools/characters_editor/charset", TooltipKey = "tools/characters_editor/charset_tip", Kind = EntityFieldKind.Picture, Group = "charsets" },
            new EntityField { Key = "Level", LabelKey = "tools/characters_editor/level", Kind = EntityFieldKind.Integer },
            new EntityField { Key = "Health", LabelKey = "tools/characters_editor/health", Kind = EntityFieldKind.Integer, Locked = HasStats },
            new EntityField { Key = "Attack", LabelKey = "tools/characters_editor/attack", Kind = EntityFieldKind.Integer, Locked = HasStats },
            new EntityField { Key = "Defence", LabelKey = "tools/characters_editor/defence", Kind = EntityFieldKind.Integer, Locked = HasStats },
            new EntityField { Key = "Point", LabelKey = "tools/characters_editor/point", TooltipKey = "tools/characters_editor/point_tip", Kind = EntityFieldKind.Integer },
            new EntityField { Key = "Exp", LabelKey = "tools/characters_editor/exp", TooltipKey = "tools/characters_editor/exp_tip", Kind = EntityFieldKind.Integer },
            new EntityField { Key = "RecruitmentChance", LabelKey = "tools/characters_editor/recruitment", TooltipKey = "tools/characters_editor/recruitment_tip", Kind = EntityFieldKind.Integer },
            new EntityField { Key = "Behaviour", LabelKey = "tools/characters_editor/behaviour", TooltipKey = "tools/characters_editor/behaviour_tip", Kind = EntityFieldKind.Text },
            new EntityField { Key = "Actions", LabelKey = "tools/characters_editor/actions", TooltipKey = "tools/characters_editor/actions_tip", Kind = EntityFieldKind.List, Options = Actions }
        };

        private static IEnumerable<string> Actions()
            => (Entities.Collect<ActionCharacterInfo>()?.Keys ?? Enumerable.Empty<string>()).OrderBy(id => id, StringComparer.Ordinal);

        /// <summary>A character with <c>Stats</c> gets its bound battle numbers from the stats (see the class summary).</summary>
        private static bool HasStats(JsonObject data) => data["Stats"] != null;

        /// <summary>The party leader's id, as <c>RpgPlayerLook.LeaderId</c> reads it; null without one.</summary>
        public static string LeaderId()
        {
            var setting = ActiveGame.GetSetting(PlayerSetting);
            if (!string.IsNullOrWhiteSpace(setting))
                return setting.Trim();
            var roster = GameData.Instance?.Runtime?.CharacterData;
            return roster?.Characters.Keys.FirstOrDefault(id => roster.BattleSet.Contains(id));
        }

        private const string PlayerSetting = "stats.player";

        // Created by CreatePreviewBlock, which the base window calls while it builds (before this class's constructor body): no initialisers.
        private Label leaderLabel;
        private VBoxContainer computedBox;
        private CheckBox joinBox;
        private string shownId;

        /// <summary>The party section and the computed battle numbers, below the fields.</summary>
        protected override Control CreatePreviewBlock()
        {
            var block = new VBoxContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
            block.AddChild(ToolUi.Section("tools/characters_editor/section_party"));
            leaderLabel = ToolUi.Hint(string.Empty);
            leaderLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            block.AddChild(leaderLabel);
            joinBox = new CheckBox { Name = "JoinsParty", Text = ToolUi.Tr("tools/characters_editor/join_check"), TooltipText = ToolUi.Tr("tools/characters_editor/join_tip") };
            joinBox.Toggled += OnJoinToggled;
            block.AddChild(joinBox);
            var joinHint = ToolUi.Hint(ToolUi.Tr("tools/characters_editor/join_hint"));
            joinHint.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            block.AddChild(joinHint);
            block.AddChild(ToolUi.Section("tools/characters_editor/section_computed"));
            block.AddChild(ToolUi.Hint(ToolUi.Tr("tools/characters_editor/computed_hint")));
            computedBox = new VBoxContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
            block.AddChild(computedBox);
            return block;
        }

        /// <summary>Refreshes the leader mark and the computed numbers for the shown character.</summary>
        protected override void OnSelectionChanged(EntityRecord record)
        {
            if (leaderLabel == null || computedBox == null)
                return;
            shownId = record?.Id;
            leaderLabel.Text = record == null ? string.Empty
                : ToolUi.Tr(record.Id == LeaderId() ? "tools/characters_editor/leader_yes" : "tools/characters_editor/leader_no");
            if (joinBox != null)
            {
                // The leader is the protagonist (stats.player): it never joins, and the check box says so.
                joinBox.SetPressedNoSignal(record != null && CompanionScenario.Exists(ModId, record.Id));
                joinBox.Disabled = record == null || record.Id == LeaderId();
            }
            foreach (var child in computedBox.GetChildren())
            {
                computedBox.RemoveChild(child);
                child.QueueFree();
            }
            if (record == null || !HasStats(record.Data))
            {
                computedBox.AddChild(ToolUi.Hint(ToolUi.Tr("tools/characters_editor/computed_none")));
                return;
            }
            foreach (var definition in StatsService.Definitions.Where(d => !string.IsNullOrEmpty(d.Binds)))
            {
                var row = string.Format(CultureInfo.InvariantCulture, ToolUi.Tr("tools/characters_editor/computed_row"),
                    ToolUi.Tr(definition.Name), StatValue(record.Id, definition.ID), definition.Binds);
                computedBox.AddChild(ToolUi.Hint(row));
            }
        }

        /// <summary>
        /// Writes or removes the join scenario of the shown character (plan 14.G7) at once: the file is the whole change, the
        /// map actor reads it the next time the character is chosen in the RPG map editor.
        /// </summary>
        private void OnJoinToggled(bool on)
        {
            if (string.IsNullOrEmpty(shownId) || shownId == LeaderId())
                return;
            var written = on ? CompanionScenario.Write(ModId, shownId) : CompanionScenario.Remove(ModId, shownId);
            if (!written)
            {
                SetStatus(ToolUi.Tr("tools/common/readonly"), Cthangover.Mods.Interface.Tool.ToolStatusKind.Error);
                joinBox.SetPressedNoSignal(!on);
                return;
            }
            Cthangover.Core.Mods.ModManager.Instance.GetResolver<ScenarioResolver>()?.ClearCache();
            SetStatus(string.Format(ToolUi.Tr(on ? "tools/characters_editor/join_on" : "tools/characters_editor/join_off"), shownId),
                Cthangover.Mods.Interface.Tool.ToolStatusKind.Ok);
        }

        /// <summary>The character's sheet value of <paramref name="stat"/> (integer, invariant), or "-" when no sheet can be made.</summary>
        private static string StatValue(string characterId, string stat)
        {
            try
            {
                var sheet = StatsService.Sheet(characterId);
                return sheet != null && sheet.Has(stat) ? sheet.Rounded(stat).ToString(CultureInfo.InvariantCulture) : "-";
            }
            catch (System.Exception)
            {
                return "-";
            }
        }

        /// <summary>A level 1 fighter with a plain attack; the portrait is the author's to pick.</summary>
        protected override JsonObject CreateEntity(string id) => new()
        {
            ["Id"] = id,
            ["Behaviour"] = "base",
            ["Level"] = 1,
            ["Point"] = 1,
            ["Health"] = 10,
            ["Defence"] = 0,
            ["Attack"] = 2,
            ["Exp"] = 1,
            ["RecruitmentChance"] = 0,
            ["Actions"] = new JsonArray("physics/attack")
        };

        /// <summary><c>&lt;mod&gt;_new_character</c>: character ids are flat.</summary>
        protected override string NewIdBase(string mod) => mod + "_new_character";
    }
}
