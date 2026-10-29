using Cthangover.Core.UI.Tool;
using Godot;

namespace Cthangover.Tools.CharactersEditor
{
    /// <summary>
    /// Registers the character editor window opened from the in-game toolbar
    /// through <see cref="CharacterEditorToolBoxButton"/>.
    /// </summary>
    public class CharacterEditorToolProvider : IToolProvider
    {
        /// <summary>Tool id.</summary>
        public string Id => "characters_editor";

        /// <summary>Display name key.</summary>
        public string LocaleKey => "tools/characters_editor/title";

        /// <summary>Creates the window.</summary>
        public Window CreateWindow() => new CharacterEditorWindow();
    }
}
