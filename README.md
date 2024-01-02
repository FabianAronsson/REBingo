# REBingo

A patcher for Flytre's Minecraft bingo datapack.

This simple program adds new items to the "speed" pack, but in the future it should be possible to add to any pack.

In the [items](items.md) file, it is possible to see what fields needs to be changed and what ideas we had when creating new difficulties. 

# How to use:
- Put an original folder of the latest Flytre's bingo pack in the root folder of the program.
- Put a json file with the blocks/items you want to add. You need to use the official ids for this.
  - For example
  ```js
  [
    "block.minecraft.polished_andesite",
    "block.minecraft.coal_block",
    "block.minecraft.coarse_dirt",
    "block.minecraft.yellow_stained_glass",
    "block.minecraft.red_bed",
    "block.minecraft.tuff",
    "block.minecraft.red_banner",
    "block.minecraft.blue_carpet",
    "block.minecraft.note_block",
    "block.minecraft.polished_granite",
    "block.minecraft.light_weighted_pressure_plate",
    "block.minecraft.oxeye_daisy",
    "block.minecraft.chiseled_bookshelf",
    "block.minecraft.oak_hanging_sign",
    "block.minecraft.spruce_stairs"
  ]
  ```
- Name the file to items2.json. This will be changed in a future release.
- Run the program.

If successful, a message will be displayed and the modpack will have been modified.

Now you can import the datapack into Minecraft and enjoy the new items. 

Keep in mind that this does not create any icons as they need to be created separetley. The modpack will work fine, but it might be difficult to know what items you are trying to get ;)


# Future Plans
- Create a graphical interface for easy creation of sets and difficulties
- Automatic generation of NBT-Icon files
