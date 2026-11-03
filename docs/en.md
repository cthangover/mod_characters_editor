# Characters Editor — editing a mod's characters in a form

The Characters Editor edits the characters of a mod while the game runs: the name in every language of the mod, the portrait, the walking sprite for RPG maps, the battle numbers, the recruit chance and the battle actions. **Save** writes the mod's files, and the game uses the change at once.

It opens from the tool box (the hammer on the toolbar) as **Characters**.

## An edit and its result in the game

In RPG Plus, the hero of the village is the character `rpg_hero` of the mod `rpg_plus_game`. Choose that mod at the top, pick **Hero**, type a new English name and press **Save** (Ctrl+S):

![The Characters window on rpg_plus_game: the list Aldric · rpg_hero, Slime · rpg_slime, Wolf · rpg_wolf; the form of rpg_hero with the name key rpg_demo/character/hero, text (en) Aldric and the other six languages unchanged, Portrait characters/village/hero with its picture, Charset (RPG) empty, Level 1, Health 24, Attack 6, Defence 0, Action points 1, Experience 0, Recruit chance 0. The status line: "Saved: 0 file(s), 1 text(s) in mod rpg_plus_game; the game re-read its data".](assets/editor-saved.jpg)

Only the English line of the mod's `locale/en.properties` changed. The party member on the character panel right after:

![The Character actions panel: the party member is now called Aldric (rpg_hero, HP 42/42, AP 1, ATK 12, DEF 4) with Basic attack, Defence and Power strike.](assets/panel-after.jpg)

The panel shows HP 42 and ATK 12 while the file says 24 and 6: in RPG Plus the hero's class and stats (Stats Engine with Classic RPG Rules) compute the battle numbers and write them over the file's values. In a game without stats the file's numbers are the ones used.

## The form

- **Id** — the character's id; scenarios, parties, battles and maps find it by this id.
- **Name** — the translation key and a line per language of the mod.
- **Portrait** — the picture on cards and panels; **…** opens a grid to pick one.
- **Charset (RPG)** — the walking sprite on RPG maps (3 frames × 4 facings).
- **Level**, **Health**, **Attack**, **Defence**, **Action points**, **Experience** — the battle numbers of the character. When the character has `Stats` (from the Stats Engine), **Health**, **Attack** and **Defence** are read-only: the stats compute them, and the form shows the computed values under the fields (see the party block below).
- **Recruit chance** — 0 to 100: the chance to capture the character after beating it (see Recruiting).
- **Behaviour** and **Actions** — how the character fights and which battle actions it has.
- **Loot** — what drops when the character is defeated as an enemy: a list of rows, each with an item (a picker of the items the game knows), a minimum and a maximum count and a chance from 0 to 100.
- **Fields of other mods** — fields other mods describe for characters in their schemas (for example Stats Engine's class, stats and traits) appear in their own section.
- **Id** of a saved character is read-only; **Rename…** under it changes the id and the references to it (see below).
- **Other fields** — the rest of the character's JSON, edited as JSON and kept as it is.

The toolbar: **New** (Ctrl+N), **Copy** (Ctrl+D), **Delete**, **Undo**/**Redo** (Ctrl+Z / Ctrl+Y), **Save** — only changed files are written; texts go into the mod's locale files.

## Renaming an id

**Rename…** under the Id of a saved character asks for the new id and lists every file that names the character before anything is written: the character's own file, the scenarios that give it to a party or start a battle with it, the placements and maps that place it. Files of other mods that name it are only listed, and they are not changed. **Rename** saves your pending edits first; **Undo rename** in the toolbar puts every changed file back exactly as it was. Ids take letters, digits, `_`, `-` and `/` between their parts, and must be free in every mod.

## The party block

Below the fields, the party block shows two things:

- **Party leader** — the character whose sheet the player walks with on RPG maps. The leader is the character the game's `stats.player` names; without one, the first character of the battle party. The block says *Party leader* or *Not the party leader* for the open character. Choosing a new leader is not a button yet: it is a setting of the collection, changed through the mod manager.
- **Computed battle numbers** — when the character has `Stats`, every stat with its value and its battle number. The hint says that health, attack and defence come from the stats, so you edit the stats, not these fields. Without `Stats` the block says so, and the battle numbers are the fields above.
- **Joins the party when talked to** — tick it to write the join scenario `scenarios/<your mod>/party/<id>.scenario`: it sets the flag `<id>_joined` to 1 and adds the character to the party (`action party.add`). Unticking deletes the file at once. The leader never joins (the box is disabled). If the character's actor object already has a dialog (its **Interaction** opens a scenario or has its own lines), the join comes after that dialog: the dialog plays, then the character joins, and the dialog is not replaced. The map editor writes this when the character is chosen in its **Characters** tab. To make the character a companion on a map, place it from the **Characters** tab of the RPG Map Editor: talking to the placed actor joins it, the actor leaves its cell and the companion walks behind the hero.

## Characters of other mods

The list shows the characters of the chosen mod. Tick **Entries of other mods** to add the characters of its dependencies, marked `↗ owner`. Editing one writes a patch in your mod (`patches/character/<your mod>.json`) with only the values you changed; the owner's file stays byte for byte the same. **Reset to inherited** removes the patch, and **↺** next to a field removes your value of that field. Only mods installed as folders can be written to.

## If something goes wrong

1. **Changed health or attack has no effect.** The game computes the numbers from stats, as RPG Plus does for its party.
2. **The character is not in the RPG Map Editor's palette.** The palette's **Characters** tab lists the characters that have a walking sheet (**Charset**); a character without one cannot be placed. The **Actors** tab lists the actor objects of `rpg_object`, not the game's characters.
3. **The mod list is empty.** Packed mods cannot be edited.
