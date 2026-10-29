# Characters Editor — editing a mod's characters in a form

The Characters Editor edits the characters of a mod while the game runs: the name in every language of the mod, the portrait, the walking sprite for RPG maps, the battle numbers, the recruit chance and the battle actions. **Save** writes the mod's files, and the game uses the change at once.

It opens from the tool box (the hammer on the toolbar) as **Characters**.

## An edit and its result in the game

In RPG Plus, the hero of the village is the character `rpg_hero` of the mod `rpg_demo_game`. Choose that mod at the top, pick **Hero**, type a new English name and press **Save** (Ctrl+S):

![The Characters window on rpg_demo_game: the list Aldric · rpg_hero, Slime · rpg_slime, Wolf · rpg_wolf; the form of rpg_hero with the name key rpg_demo/character/hero, text (en) Aldric and the other six languages unchanged, Portrait characters/village/hero with its picture, Charset (RPG) empty, Level 1, Health 24, Attack 6, Defence 0, Action points 1, Experience 0, Recruit chance 0. The status line: "Saved: 0 file(s), 1 text(s) in mod rpg_demo_game; the game re-read its data".](assets/editor-saved.jpg)

Only the English line of the mod's `locale/en.properties` changed. The party member on the character panel right after:

![The Character actions panel: the party member is now called Aldric (rpg_hero, HP 42/42, AP 1, ATK 12, DEF 4) with Basic attack, Defence and Power strike.](assets/panel-after.jpg)

The panel shows HP 42 and ATK 12 while the file says 24 and 6: in RPG Plus the hero's class and stats (Stats Engine with Classic RPG Rules) compute the battle numbers and write them over the file's values. In a game without stats the file's numbers are the ones used.

## The form

- **Id** — the character's id; scenarios, parties, battles and maps find it by this id.
- **Name** — the translation key and a line per language of the mod.
- **Portrait** — the picture on cards and panels; **…** opens a grid to pick one.
- **Charset (RPG)** — the walking sprite on RPG maps (3 frames × 4 facings). The RPG Map Editor offers characters with their charset for placing.
- **Level**, **Health**, **Attack**, **Defence**, **Action points**, **Experience** — the battle numbers of the character.
- **Recruit chance** — 0 to 100: the chance to capture the character after beating it (see Recruiting).
- **Behaviour** and **Actions** — how the character fights and which battle actions it has.
- **Fields of other mods** — fields other mods describe for characters in their schemas (for example Stats Engine's class, stats and traits) appear in their own section.
- **Other fields** — the rest of the character's JSON, edited as JSON and kept as it is.

The toolbar: **New** (Ctrl+N), **Copy** (Ctrl+D), **Delete**, **Undo**/**Redo** (Ctrl+Z / Ctrl+Y), **Save** — only changed files are written; texts go into the mod's locale files.

## Which characters can be edited

The list holds the characters in the selected mod's own files; only mods installed as folders are offered. Characters of other mods are not in the list and their files are never written. To change one of them, ship the same id in your own mod with a dependency on the owner, or add fields with a patch.

## If something goes wrong

1. **Changed health or attack has no effect.** The game computes the numbers from stats, as RPG Plus does for its party.
2. **The character is missing from the RPG Map Editor's Characters tab.** The tab lists characters with combat stats; any sprite can still be placed from the Appearance tab.
3. **The mod list is empty.** Packed mods cannot be edited.
