namespace Cthangover.Tools.CharactersEditor.Tests
{
    using Cthangover.Core.Mods.Testing;

    /// <summary>
    /// The join scenario of a character (plan 14.G7) as text and path: the file the "Joins the party" block writes, and the flag
    /// and <c>party.add</c> line it holds. The rpg editor reads the same names (<c>RpgCharacterActors</c>): both sides change together.
    /// </summary>
    public class CompanionScenarioTests
    {
        [ModTest("companion/path_and_key_are_the_party_folder")]
        static void PathAndKey(ModTestContext ctx)
        {
            ModAssert.Equal("scenarios/rpg_plus_game/party/rpg_companion.scenario", CompanionScenario.Path("rpg_plus_game", "rpg_companion"), "the file");
            ModAssert.Equal("rpg_plus_game/party/rpg_companion", CompanionScenario.Key("rpg_plus_game", "rpg_companion"), "the scenario key");
        }

        [ModTest("companion/flag_is_the_id_with_joined")]
        static void Flag(ModTestContext ctx)
        {
            ModAssert.Equal("rpg_companion_joined", CompanionScenario.Flag("rpg_companion"), "the join flag");
        }

        [ModTest("companion/text_sets_the_flag_then_adds_the_party_member")]
        static void Text(ModTestContext ctx)
        {
            ModAssert.Equal("flag rpg_companion_joined 1\naction party.add id=rpg_companion\nend\n", CompanionScenario.Text("rpg_companion"),
                "the text is LF, the flag first, then party.add");
        }
    }
}
