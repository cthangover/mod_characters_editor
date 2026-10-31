namespace Cthangover.Tools.CharactersEditor
{
    using Cthangover.Tools.Services;

    /// <summary>
    /// The join scenario of a character (plan 14.G7): <c>scenarios/&lt;mod&gt;/party/&lt;id&gt;.scenario</c>, which sets the join
    /// flag and runs <c>action party.add</c>. The "Joins the party" block of this editor writes or removes it. The rpg
    /// editor reads it when it makes the character's map actor (<c>RpgCharacterActors</c> in <c>rpg_editor</c>), which
    /// takes the scenario key and the flag from the same convention: change both together.
    /// </summary>
    public static class CompanionScenario
    {
        /// <summary>The file of the join scenario, relative to the mod.</summary>
        public static string Path(string mod, string characterId) => "scenarios/" + Key(mod, characterId) + ".scenario";

        /// <summary>The scenario path in the <c>scenarios</c> group, without the extension.</summary>
        public static string Key(string mod, string characterId) => mod + "/party/" + characterId;

        /// <summary>The story flag the join sets to 1; the map actor of the character hides then.</summary>
        public static string Flag(string characterId) => characterId + "_joined";

        /// <summary>The text of the scenario: the flag, then the party change.</summary>
        public static string Text(string characterId)
            => "flag " + Flag(characterId) + " 1\naction party.add id=" + characterId + "\nend\n";

        /// <summary>True when the mod has the join scenario of the character.</summary>
        public static bool Exists(string mod, string characterId)
            => !string.IsNullOrEmpty(mod) && !string.IsNullOrEmpty(characterId) && ModFileService.ReadText(mod, Path(mod, characterId)) != null;

        /// <summary>Writes the join scenario; false when the mod is not writable.</summary>
        public static bool Write(string mod, string characterId) => ModFileService.WriteText(mod, Path(mod, characterId), Text(characterId));

        /// <summary>Removes the join scenario; false when the mod is not writable.</summary>
        public static bool Remove(string mod, string characterId) => !Exists(mod, characterId) || ModFileService.DeleteFile(mod, Path(mod, characterId));
    }
}
