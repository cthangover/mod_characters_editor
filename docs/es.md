# Characters Editor — edita los personajes de un mod en un formulario

El Characters Editor edita los personajes de un mod mientras el juego está en marcha: el nombre en todos los idiomas del mod, el retrato, el sprite de paseo para los mapas RPG, las cifras de combate, la probabilidad de reclutamiento y las acciones de combate. **Guardar** escribe los archivos del mod y el juego usa el cambio al instante.

Se abre desde la caja de herramientas (el martillo de la barra) como **Personajes**.

## Una edición y su resultado en el juego

En RPG Plus, el héroe de la aldea es el personaje `rpg_hero` del mod `rpg_plus_game`. Elige ese mod arriba, selecciona **Héroe**, escribe un nombre nuevo en inglés y pulsa **Guardar** (Ctrl+S):

![La ventana Personajes sobre rpg_plus_game: la lista Aldric · rpg_hero, Limo · rpg_slime, Lobo · rpg_wolf; el formulario de rpg_hero con la clave del nombre rpg_demo/character/hero, texto (en) Aldric y los otros seis idiomas sin cambios, Retrato characters/village/hero con su imagen, Charset (RPG) vacío, Nivel 1, Salud 24, Ataque 6, Defensa 0, Puntos de acción 1, Experiencia 0, Probabilidad de reclutar 0. La línea de estado: «Guardado: 0 archivo(s), 1 texto(s) en el mod rpg_plus_game; el juego releyó sus datos».](assets/editor-saved.jpg)

Solo cambió la línea en inglés del `locale/en.properties` del mod. El miembro del grupo en el panel de personajes justo después:

![El panel de acciones del personaje: el miembro del grupo ahora se llama Aldric (rpg_hero, HP 42/42, AP 1, ATK 12, DEF 4) con Ataque básico, Defensa y Golpe poderoso.](assets/panel-after.jpg)

El panel muestra HP 42 y ATK 12 aunque el archivo dice 24 y 6: en RPG Plus la clase y los atributos del héroe (Stats Engine con Classic RPG Rules) calculan las cifras de combate y las escriben sobre los valores del archivo. En un juego sin atributos se usan los números del archivo.

## El formulario

- **Id** — el id del personaje; por él lo encuentran escenarios, grupos, combates y mapas.
- **Nombre** — la clave de traducción y una línea por idioma del mod.
- **Retrato** — la imagen en tarjetas y paneles; **…** abre una cuadrícula para elegir una.
- **Charset (RPG)** — el sprite de paseo en los mapas RPG (3 fotogramas × 4 orientaciones).
- **Nivel**, **Salud**, **Ataque**, **Defensa**, **Puntos de acción**, **Experiencia** — las cifras de combate del personaje.
- **Probabilidad de reclutar** — de 0 a 100: la probabilidad de capturar al personaje tras vencerlo (ver Recruiting).
- **Comportamiento** y **Acciones** — cómo pelea el personaje y qué acciones de combate tiene.
- **Campos de otros mods** — los campos que otros mods describen para los personajes en sus esquemas (por ejemplo la clase, los atributos y los rasgos de Stats Engine) aparecen en su propia sección.
- **Otros campos** — el resto del JSON del personaje, editado como JSON y conservado tal cual.

La barra: **Nueva** (Ctrl+N), **Copia** (Ctrl+D), **Borrar**, **Deshacer**/**Rehacer** (Ctrl+Z / Ctrl+Y), **Guardar** — solo se escriben los archivos cambiados; los textos van a los archivos de localización del mod.

## Personajes de otros mods

La lista muestra los personajes del mod elegido. Marque **Entries of other mods** para añadir los personajes de sus dependencias, marcados `↗ dueño`. Editar uno escribe un parche en su mod (`patches/character/<su mod>.json`) solo con los valores que cambió; el archivo del dueño queda idéntico byte a byte. **Reset to inherited** quita el parche, y **↺** junto a un campo quita su valor. Solo se puede escribir en mods instalados como carpeta.

## El bloque del grupo

Bajo los campos, el bloque del grupo muestra dos cosas:

- **Líder del grupo** — el personaje cuya ficha usa el jugador al caminar por los mapas RPG. El líder es el que nombra `stats.player` del juego; si no hay ninguno, el primer personaje del grupo de combate. El bloque dice *Party leader* o *Not the party leader* para el personaje abierto. Todavía no hay botón para elegir otro líder: es un ajuste de la colección y se cambia desde el gestor de mods.
- **Cifras de combate calculadas** — si el personaje tiene `Stats`, cada estadística con su valor y su cifra de combate. La pista indica que la salud, el ataque y la defensa vienen de las estadísticas, así que se editan estas y no los campos. Con `Stats`, **Health**, **Attack** y **Defence** son de solo lectura. Sin `Stats`, el bloque lo dice y las cifras de combate son los campos de arriba.
- **Se une al grupo al hablar con él** — márquelo para escribir el escenario de unión `scenarios/<su mod>/party/<id>.scenario`: pone el flag `<id>_joined` a 1 y añade el personaje al grupo (`action party.add`). Quitar la marca borra el archivo al instante. El líder nunca se une (la casilla está desactivada). Para que el personaje sea un compañero en el mapa, colóquelo desde la pestaña **Personajes** del editor de mapas RPG: hablar con el actor colocado lo une, el actor deja su casilla y el compañero camina detrás del héroe.

## Si algo sale mal

1. **Cambiar salud o ataque no tiene efecto.** El juego calcula las cifras a partir de los atributos, como hace RPG Plus con su grupo.
2. **El personaje no está en la paleta del editor de mapas RPG.** La pestaña **Personajes** solo muestra los personajes con hoja de movimiento (**Charset**); sin ella no se puede colocar. La pestaña **Actores** muestra los objetos `rpg_object`, no los personajes del juego.
3. **La lista de mods está vacía.** Los mods empaquetados no se pueden editar.
