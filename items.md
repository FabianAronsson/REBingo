## Items

- **Advancment**

```lua
{
  "parent": "flytre:items/root",
  "criteria": {
    "bedSpeed": { --change
      "conditions": {
        "items": [
          {
            "items": [
              "brick"
            ]
          }
        ],
        "player": [
          {
            "condition": "minecraft:value_check",
            "value": {
              "type": "minecraft:score",
              "target": {
                "type": "minecraft:fixed",
                "name": "bedSpeed" --change
              },
              "score": "global"
            },
            "range": {
              "min": 1
            }
          }
        ]
      },
      "trigger": "minecraft:inventory_changed"
    }
  },
  "requirements": [
    [
      "bedSpeed" --change
    ]
  ],
  "rewards": {
    "function": "flytre:detect/bedSpeed/base" --change
  }
}
```

- **Clarify**
  - In each 25 mcfunctions

```lua
--Create anywhere in each file
execute if score bedSpeed global matches 1 run tellraw @s ["",{"text":"The item in slot 1 is: ","color":"dark_purple"},{"translate":"item.minecraft.leather_boots","color":"dark_purple"}]
```

- **Detect**
  - base

```lua
--Create a mcfunction file in "detect/NAME OF ITEM/base.mcfunction"
scoreboard players set @s clear 0
execute unless score lockout stage matches 1..2 unless score red bedSpeed matches 1.. as @s[team=red] run scoreboard players set @s clear 1
execute unless score lockout stage matches 1 unless score yellow bedSpeed matches 1.. as @s[team=yellow] run scoreboard players set @s clear 1
execute unless score lockout stage matches 1 unless score green bedSpeed matches 1.. as @s[team=green] run scoreboard players set @s clear 1
execute unless score lockout stage matches 1 unless score blue bedSpeed matches 1.. as @s[team=blue] run scoreboard players set @s clear 1
execute if score lockout stage matches 1 unless score completed bedSpeed matches 1.. as @s[team=red] run scoreboard players set @s clear 1
execute if score lockout stage matches 1 unless score completed bedSpeed matches 1.. as @s[team=yellow] run scoreboard players set @s clear 1
execute if score lockout stage matches 1 unless score completed bedSpeed matches 1.. as @s[team=green] run scoreboard players set @s clear 1
execute if score lockout stage matches 1 unless score completed bedSpeed matches 1.. as @s[team=blue] run scoreboard players set @s clear 1
execute as @s[scores={clear=1..},team=red] run scoreboard players set red bedSpeed 1
execute as @s[scores={clear=1..},team=yellow] run scoreboard players set yellow bedSpeed 1
execute as @s[scores={clear=1..},team=green] run scoreboard players set green bedSpeed 1
execute as @s[scores={clear=1..},team=blue] run scoreboard players set blue bedSpeed 1
execute as @s[scores={clear=1..}] run scoreboard players set completed bedSpeed 1
execute as @s[scores={clear=1..}] run function flytre:detect/bedSpeed/gotten
advancement revoke @s only flytre:detection/bedSpeed
```

- gotten

```lua
--same as above create the file with the correct name.
execute as @s[team=red] run tellraw @a ["",{"text":"Red completed an item: ","color":"dark_red"},{"translate":"item.minecraft.brick","color":"dark_red"}]
execute as @s[team=yellow] run tellraw @a ["",{"text":"Yellow completed an item: ","color":"gold"},{"translate":"item.minecraft.brick","color":"gold"}]
execute as @s[team=green] run tellraw @a ["",{"text":"Green completed an item: ","color":"green"},{"translate":"item.minecraft.brick","color":"green"}]
execute as @s[team=blue] run tellraw @a ["",{"text":"Blue completed an item: ","color":"dark_aqua"},{"translate":"item.minecraft.brick","color":"dark_aqua"}]
execute as @s[team=red] as @a[team=red] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1
execute as @s[team=yellow] as @a[team=yellow] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1
execute as @s[team=green] as @a[team=green] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1
execute as @s[team=blue] as @a[team=blue] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1
execute as @s[team=red] as @a[team=!red] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1
execute as @s[team=yellow] as @a[team=!yellow] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1
execute as @s[team=green] as @a[team=!green] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1
execute as @s[team=blue] as @a[team=!blue] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1

execute if score bedSpeed global matches 1 at @e[type=armor_stand,tag=bingo,tag=1] run function flytre:set_corner/base
execute if score bedSpeed global matches 1 run scoreboard players operation red board_1 = red bedSpeed
execute if score bedSpeed global matches 1 run scoreboard players operation yellow board_1 = yellow bedSpeed
execute if score bedSpeed global matches 1 run scoreboard players operation green board_1 = green bedSpeed
execute if score bedSpeed global matches 1 run scoreboard players operation blue board_1 = blue bedSpeed
execute if score bedSpeed global matches 1 run scoreboard players operation completed board_1 = completed bedSpeed

execute if score bedSpeed global matches 2 at @e[type=armor_stand,tag=bingo,tag=2] run function flytre:set_corner/base
execute if score bedSpeed global matches 2 run scoreboard players operation red board_2 = red bedSpeed
execute if score bedSpeed global matches 2 run scoreboard players operation yellow board_2 = yellow bedSpeed
execute if score bedSpeed global matches 2 run scoreboard players operation green board_2 = green bedSpeed
execute if score bedSpeed global matches 2 run scoreboard players operation blue board_2 = blue bedSpeed
execute if score bedSpeed global matches 2 run scoreboard players operation completed board_2 = completed bedSpeed

execute if score bedSpeed global matches 3 at @e[type=armor_stand,tag=bingo,tag=3] run function flytre:set_corner/base
execute if score bedSpeed global matches 3 run scoreboard players operation red board_3 = red bedSpeed
execute if score bedSpeed global matches 3 run scoreboard players operation yellow board_3 = yellow bedSpeed
execute if score bedSpeed global matches 3 run scoreboard players operation green board_3 = green bedSpeed
execute if score bedSpeed global matches 3 run scoreboard players operation blue board_3 = blue bedSpeed
execute if score bedSpeed global matches 3 run scoreboard players operation completed board_3 = completed bedSpeed

execute if score bedSpeed global matches 4 at @e[type=armor_stand,tag=bingo,tag=4] run function flytre:set_corner/base
execute if score bedSpeed global matches 4 run scoreboard players operation red board_4 = red bedSpeed
execute if score bedSpeed global matches 4 run scoreboard players operation yellow board_4 = yellow bedSpeed
execute if score bedSpeed global matches 4 run scoreboard players operation green board_4 = green bedSpeed
execute if score bedSpeed global matches 4 run scoreboard players operation blue board_4 = blue bedSpeed
execute if score bedSpeed global matches 4 run scoreboard players operation completed board_4 = completed bedSpeed

execute if score bedSpeed global matches 5 at @e[type=armor_stand,tag=bingo,tag=5] run function flytre:set_corner/base
execute if score bedSpeed global matches 5 run scoreboard players operation red board_5 = red bedSpeed
execute if score bedSpeed global matches 5 run scoreboard players operation yellow board_5 = yellow bedSpeed
execute if score bedSpeed global matches 5 run scoreboard players operation green board_5 = green bedSpeed
execute if score bedSpeed global matches 5 run scoreboard players operation blue board_5 = blue bedSpeed
execute if score bedSpeed global matches 5 run scoreboard players operation completed board_5 = completed bedSpeed

execute if score bedSpeed global matches 6 at @e[type=armor_stand,tag=bingo,tag=6] run function flytre:set_corner/base
execute if score bedSpeed global matches 6 run scoreboard players operation red board_6 = red bedSpeed
execute if score bedSpeed global matches 6 run scoreboard players operation yellow board_6 = yellow bedSpeed
execute if score bedSpeed global matches 6 run scoreboard players operation green board_6 = green bedSpeed
execute if score bedSpeed global matches 6 run scoreboard players operation blue board_6 = blue bedSpeed
execute if score bedSpeed global matches 6 run scoreboard players operation completed board_6 = completed bedSpeed

execute if score bedSpeed global matches 7 at @e[type=armor_stand,tag=bingo,tag=7] run function flytre:set_corner/base
execute if score bedSpeed global matches 7 run scoreboard players operation red board_7 = red bedSpeed
execute if score bedSpeed global matches 7 run scoreboard players operation yellow board_7 = yellow bedSpeed
execute if score bedSpeed global matches 7 run scoreboard players operation green board_7 = green bedSpeed
execute if score bedSpeed global matches 7 run scoreboard players operation blue board_7 = blue bedSpeed
execute if score bedSpeed global matches 7 run scoreboard players operation completed board_7 = completed bedSpeed

execute if score bedSpeed global matches 8 at @e[type=armor_stand,tag=bingo,tag=8] run function flytre:set_corner/base
execute if score bedSpeed global matches 8 run scoreboard players operation red board_8 = red bedSpeed
execute if score bedSpeed global matches 8 run scoreboard players operation yellow board_8 = yellow bedSpeed
execute if score bedSpeed global matches 8 run scoreboard players operation green board_8 = green bedSpeed
execute if score bedSpeed global matches 8 run scoreboard players operation blue board_8 = blue bedSpeed
execute if score bedSpeed global matches 8 run scoreboard players operation completed board_8 = completed bedSpeed

execute if score bedSpeed global matches 9 at @e[type=armor_stand,tag=bingo,tag=9] run function flytre:set_corner/base
execute if score bedSpeed global matches 9 run scoreboard players operation red board_9 = red bedSpeed
execute if score bedSpeed global matches 9 run scoreboard players operation yellow board_9 = yellow bedSpeed
execute if score bedSpeed global matches 9 run scoreboard players operation green board_9 = green bedSpeed
execute if score bedSpeed global matches 9 run scoreboard players operation blue board_9 = blue bedSpeed
execute if score bedSpeed global matches 9 run scoreboard players operation completed board_9 = completed bedSpeed

execute if score bedSpeed global matches 10 at @e[type=armor_stand,tag=bingo,tag=10] run function flytre:set_corner/base
execute if score bedSpeed global matches 10 run scoreboard players operation red board_10 = red bedSpeed
execute if score bedSpeed global matches 10 run scoreboard players operation yellow board_10 = yellow bedSpeed
execute if score bedSpeed global matches 10 run scoreboard players operation green board_10 = green bedSpeed
execute if score bedSpeed global matches 10 run scoreboard players operation blue board_10 = blue bedSpeed
execute if score bedSpeed global matches 10 run scoreboard players operation completed board_10 = completed bedSpeed

execute if score bedSpeed global matches 11 at @e[type=armor_stand,tag=bingo,tag=11] run function flytre:set_corner/base
execute if score bedSpeed global matches 11 run scoreboard players operation red board_11 = red bedSpeed
execute if score bedSpeed global matches 11 run scoreboard players operation yellow board_11 = yellow bedSpeed
execute if score bedSpeed global matches 11 run scoreboard players operation green board_11 = green bedSpeed
execute if score bedSpeed global matches 11 run scoreboard players operation blue board_11 = blue bedSpeed
execute if score bedSpeed global matches 11 run scoreboard players operation completed board_11 = completed bedSpeed

execute if score bedSpeed global matches 12 at @e[type=armor_stand,tag=bingo,tag=12] run function flytre:set_corner/base
execute if score bedSpeed global matches 12 run scoreboard players operation red board_12 = red bedSpeed
execute if score bedSpeed global matches 12 run scoreboard players operation yellow board_12 = yellow bedSpeed
execute if score bedSpeed global matches 12 run scoreboard players operation green board_12 = green bedSpeed
execute if score bedSpeed global matches 12 run scoreboard players operation blue board_12 = blue bedSpeed
execute if score bedSpeed global matches 12 run scoreboard players operation completed board_12 = completed bedSpeed

execute if score bedSpeed global matches 13 at @e[type=armor_stand,tag=bingo,tag=13] run function flytre:set_corner/base
execute if score bedSpeed global matches 13 run scoreboard players operation red board_13 = red bedSpeed
execute if score bedSpeed global matches 13 run scoreboard players operation yellow board_13 = yellow bedSpeed
execute if score bedSpeed global matches 13 run scoreboard players operation green board_13 = green bedSpeed
execute if score bedSpeed global matches 13 run scoreboard players operation blue board_13 = blue bedSpeed
execute if score bedSpeed global matches 13 run scoreboard players operation completed board_13 = completed bedSpeed

execute if score bedSpeed global matches 14 at @e[type=armor_stand,tag=bingo,tag=14] run function flytre:set_corner/base
execute if score bedSpeed global matches 14 run scoreboard players operation red board_14 = red bedSpeed
execute if score bedSpeed global matches 14 run scoreboard players operation yellow board_14 = yellow bedSpeed
execute if score bedSpeed global matches 14 run scoreboard players operation green board_14 = green bedSpeed
execute if score bedSpeed global matches 14 run scoreboard players operation blue board_14 = blue bedSpeed
execute if score bedSpeed global matches 14 run scoreboard players operation completed board_14 = completed bedSpeed

execute if score bedSpeed global matches 15 at @e[type=armor_stand,tag=bingo,tag=15] run function flytre:set_corner/base
execute if score bedSpeed global matches 15 run scoreboard players operation red board_15 = red bedSpeed
execute if score bedSpeed global matches 15 run scoreboard players operation yellow board_15 = yellow bedSpeed
execute if score bedSpeed global matches 15 run scoreboard players operation green board_15 = green bedSpeed
execute if score bedSpeed global matches 15 run scoreboard players operation blue board_15 = blue bedSpeed
execute if score bedSpeed global matches 15 run scoreboard players operation completed board_15 = completed bedSpeed

execute if score bedSpeed global matches 16 at @e[type=armor_stand,tag=bingo,tag=16] run function flytre:set_corner/base
execute if score bedSpeed global matches 16 run scoreboard players operation red board_16 = red bedSpeed
execute if score bedSpeed global matches 16 run scoreboard players operation yellow board_16 = yellow bedSpeed
execute if score bedSpeed global matches 16 run scoreboard players operation green board_16 = green bedSpeed
execute if score bedSpeed global matches 16 run scoreboard players operation blue board_16 = blue bedSpeed
execute if score bedSpeed global matches 16 run scoreboard players operation completed board_16 = completed bedSpeed

execute if score bedSpeed global matches 17 at @e[type=armor_stand,tag=bingo,tag=17] run function flytre:set_corner/base
execute if score bedSpeed global matches 17 run scoreboard players operation red board_17 = red bedSpeed
execute if score bedSpeed global matches 17 run scoreboard players operation yellow board_17 = yellow bedSpeed
execute if score bedSpeed global matches 17 run scoreboard players operation green board_17 = green bedSpeed
execute if score bedSpeed global matches 17 run scoreboard players operation blue board_17 = blue bedSpeed
execute if score bedSpeed global matches 17 run scoreboard players operation completed board_17 = completed bedSpeed

execute if score bedSpeed global matches 18 at @e[type=armor_stand,tag=bingo,tag=18] run function flytre:set_corner/base
execute if score bedSpeed global matches 18 run scoreboard players operation red board_18 = red bedSpeed
execute if score bedSpeed global matches 18 run scoreboard players operation yellow board_18 = yellow bedSpeed
execute if score bedSpeed global matches 18 run scoreboard players operation green board_18 = green bedSpeed
execute if score bedSpeed global matches 18 run scoreboard players operation blue board_18 = blue bedSpeed
execute if score bedSpeed global matches 18 run scoreboard players operation completed board_18 = completed bedSpeed

execute if score bedSpeed global matches 19 at @e[type=armor_stand,tag=bingo,tag=19] run function flytre:set_corner/base
execute if score bedSpeed global matches 19 run scoreboard players operation red board_19 = red bedSpeed
execute if score bedSpeed global matches 19 run scoreboard players operation yellow board_19 = yellow bedSpeed
execute if score bedSpeed global matches 19 run scoreboard players operation green board_19 = green bedSpeed
execute if score bedSpeed global matches 19 run scoreboard players operation blue board_19 = blue bedSpeed
execute if score bedSpeed global matches 19 run scoreboard players operation completed board_19 = completed bedSpeed

execute if score bedSpeed global matches 20 at @e[type=armor_stand,tag=bingo,tag=20] run function flytre:set_corner/base
execute if score bedSpeed global matches 20 run scoreboard players operation red board_20 = red bedSpeed
execute if score bedSpeed global matches 20 run scoreboard players operation yellow board_20 = yellow bedSpeed
execute if score bedSpeed global matches 20 run scoreboard players operation green board_20 = green bedSpeed
execute if score bedSpeed global matches 20 run scoreboard players operation blue board_20 = blue bedSpeed
execute if score bedSpeed global matches 20 run scoreboard players operation completed board_20 = completed bedSpeed

execute if score bedSpeed global matches 21 at @e[type=armor_stand,tag=bingo,tag=21] run function flytre:set_corner/base
execute if score bedSpeed global matches 21 run scoreboard players operation red board_21 = red bedSpeed
execute if score bedSpeed global matches 21 run scoreboard players operation yellow board_21 = yellow bedSpeed
execute if score bedSpeed global matches 21 run scoreboard players operation green board_21 = green bedSpeed
execute if score bedSpeed global matches 21 run scoreboard players operation blue board_21 = blue bedSpeed
execute if score bedSpeed global matches 21 run scoreboard players operation completed board_21 = completed bedSpeed

execute if score bedSpeed global matches 22 at @e[type=armor_stand,tag=bingo,tag=22] run function flytre:set_corner/base
execute if score bedSpeed global matches 22 run scoreboard players operation red board_22 = red bedSpeed
execute if score bedSpeed global matches 22 run scoreboard players operation yellow board_22 = yellow bedSpeed
execute if score bedSpeed global matches 22 run scoreboard players operation green board_22 = green bedSpeed
execute if score bedSpeed global matches 22 run scoreboard players operation blue board_22 = blue bedSpeed
execute if score bedSpeed global matches 22 run scoreboard players operation completed board_22 = completed bedSpeed

execute if score bedSpeed global matches 23 at @e[type=armor_stand,tag=bingo,tag=23] run function flytre:set_corner/base
execute if score bedSpeed global matches 23 run scoreboard players operation red board_23 = red bedSpeed
execute if score bedSpeed global matches 23 run scoreboard players operation yellow board_23 = yellow bedSpeed
execute if score bedSpeed global matches 23 run scoreboard players operation green board_23 = green bedSpeed
execute if score bedSpeed global matches 23 run scoreboard players operation blue board_23 = blue bedSpeed
execute if score bedSpeed global matches 23 run scoreboard players operation completed board_23 = completed bedSpeed

execute if score bedSpeed global matches 24 at @e[type=armor_stand,tag=bingo,tag=24] run function flytre:set_corner/base
execute if score bedSpeed global matches 24 run scoreboard players operation red board_24 = red bedSpeed
execute if score bedSpeed global matches 24 run scoreboard players operation yellow board_24 = yellow bedSpeed
execute if score bedSpeed global matches 24 run scoreboard players operation green board_24 = green bedSpeed
execute if score bedSpeed global matches 24 run scoreboard players operation blue board_24 = blue bedSpeed
execute if score bedSpeed global matches 24 run scoreboard players operation completed board_24 = completed bedSpeed

execute if score bedSpeed global matches 25 at @e[type=armor_stand,tag=bingo,tag=25] run function flytre:set_corner/base
execute if score bedSpeed global matches 25 run scoreboard players operation red board_25 = red bedSpeed
execute if score bedSpeed global matches 25 run scoreboard players operation yellow board_25 = yellow bedSpeed
execute if score bedSpeed global matches 25 run scoreboard players operation green board_25 = green bedSpeed
execute if score bedSpeed global matches 25 run scoreboard players operation blue board_25 = blue bedSpeed
execute if score bedSpeed global matches 25 run scoreboard players operation completed board_25 = completed bedSpeed
```

- **Structure**
  - In each 25 mcfunctions

```lua
  execute if score bedSpeed global matches 1 as @e[type=armor_stand,tag=bingo,tag=1] at @s run setblock ~ ~ ~ minecraft:structure_block[mode=load]{metadata:"",mirror:"NONE",ignoreEntities:1b,powered:0b,rotation:"NONE",posX:0,mode:"LOAD",posY:-17,sizeX:16,posZ:0,name:"flytre:STRUCTURE NAME HERE",sizeY:32,sizeZ:16,showboundingbox:1b}
-- BEFORE THIS LINE
execute at @e[type=armor_stand,tag=bingo,tag=0] positioned ~ ~-1 ~ run setblock ~ ~ ~ redstone_block

-- CREATE STRUCTURE OBJECT IN  "/base_generated/data/flytre/structures/YOUR FILE HERE"
```

- **all_0**
  - Add item to list

```lua
scoreboard players set bedSpeed global 0 -- insert this anywhere in the file
```

- **pick_random**
  - Add item to list

```lua
function flytre:rng
scoreboard players operation x rng = num rng
scoreboard players set 119 global 119 -- increment for each item added!
scoreboard players operation x rng %= 119 global -- here as well
scoreboard players set failed global 0
-- the match should be the total + 1, in this case 120
execute if score x rng matches 0 unless score bedSpeed global matches 0 run scoreboard players set failed global 1
execute if score x rng matches 0 if score bedSpeed global matches 0 run scoreboard players operation bedSpeed global = trial global
```

- **reset_board_colors**
  - Add item to list

```lua
scoreboard players reset red bedSpeed
scoreboard players reset yellow bedSpeed
scoreboard players reset green bedSpeed
scoreboard players reset blue bedSpeed
scoreboard players reset completed
```

- add_items_objectives
  - Add item to list

```lua
scoreboard objectives add bedSpeed dummy
```

- predicates
  - Create a folder with same name as the item
  - Create a file for each team and a main file. all being json

```lua
-- All teams are the same, so change the score only
[
  {
    "condition": "minecraft:entity_properties",
    "entity": "this",
    "predicate": {
      "team": "blue"
    }
  },
  {
    "condition": "minecraft:value_check",
    "value": {
      "type": "minecraft:score",
      "target": {
        "type": "minecraft:fixed",
        "name": "blue"
      },
      "score": "bedSpeed" --change
    },
    "range": 0
  }
]
```

also change the main file:

```lua
[
  {
    "condition": "minecraft:value_check",
    "value": {
      "type": "minecraft:score",
      "target": {
        "type": "minecraft:fixed",
        "name": "bedSpeed" --Change
      },
      "score": "global"
    },
    "range": {
      "min": 1
    }
  },
  {
    "condition": "minecraft:alternative",
    "terms": [
      {
        "condition": "minecraft:reference",
        "name": "flytre:bedSpeed/red" --Change
      },
      {
        "condition": "minecraft:reference",
        "name": "flytre:bedSpeed/yellow" --Change
      },
      {
        "condition": "minecraft:reference",
        "name": "flytre:bedSpeed/green" --Change
      },
      {
        "condition": "minecraft:reference",
        "name": "flytre:bedSpeed/blue" --Change
      }
    ]
  },
  {
    "condition": "minecraft:alternative",
    "terms": [
      {
        "condition": "minecraft:inverted",
        "term": {
          "condition": "minecraft:value_check",
          "value": {
            "type": "minecraft:score",
            "target": {
              "type": "minecraft:fixed",
              "name": "lockout"
            },
            "score": "stage"
          },
          "range": 1
        }
      },
      {
        "condition": "minecraft:inverted",
        "term": {
          "condition": "minecraft:value_check",
          "value": {
            "type": "minecraft:score",
            "target": {
              "type": "minecraft:fixed",
              "name": "completed"
            },
            "score": "bedSpeed" --Change
          },
          "range": {
            "min": 1
          }
        }
      }
    ]
  }
]
```

- **Remember to update the loot table aswell!**

### Ultra Speed

- Button
- Glass
- Oak Leaves
- Daisy
- Lever
- Nugget
- Cooked Chicken
- Terracotta
- Stair
- Fence
- Fence Gate
- Trapdoor
- Cobblestone walls
- Crafting Table
- Wood
- Stripped Wood
- Button
- Flint
- Coarse Dirt
- Sandstone
- Smooth Sandstone
- Stone pressure plate
- Bamboo

### Speed 2.0

- Granite
- Andestite
- Coal Block
- Coarse Dirt
- Colored Glass **trans**
- Bed
- Tuff
- Banner
- Carpet
- Leaves **trans**
- Daisy
- Glow Licen **trans**
- Rose
- Peony
- Sea Grass
- Jack o' lantern **separat**
- Stonecutter **separat**
- Cartographer's table
- Loom
- Grinding stone
- Ladder
- Lightning Rod
- Chiseled Book Case **separat**
- Hanging sign
- Lectern **separat**
- Shears
- Shield **separat**
- Leather Horse Armor
- Nugget
- Cooked Chicken
- Pink Dye
- Magenta Dye
- Light blue Dye
- light Gray Dye
- Paper
- yellow Glazed Terracotta
- black Glazed Terracotta
- Terracotta
- Concrete powder (yellow)
- Concrete (red)
- bone block
- Fence Gate **separat**
- Stair **separat**
- Golden Pressure plate
- Dripleaf
- Cookie
- Golden Pickaxe
- Noteblock
- Bone block

### Normal 2.0

- Bamboo raft
- Sea Lantern
- Moss
- Bone Block
- Block of raw ores
- Flowering Azalea Leaves
- Shroomlight
- Soul Sand
- Nether Wart
- Jack o' lantern
- Daylight Sensor
- Furnace in a minecart
- Cod in a bucket
- Salmon in a bucket
- Anvil
- Empty map
- Spectral Arrow
- Brewing Stand
- Golden carrot
- Pink Dye
- Magenta Dye
- Cyan Dye
- Gray Dye
- Glistening Melon
- Cherry grove log
- Warped tree
- Crimson tree
- Dripleaf
- Jukebox
- Scaffolding

### Water Set

- Sea Lantern

### Current Speed

- leather_boots
- water_bucket
- feather
- purple_dye
- crossbow
- egg
- polished_diorite
- baked_potato
- mossy_cobblestone
- green_dye
- lily_pad **REMOVE**
- fern
- rabbit
- slime_ball **REMOVE**
- firework_rocket
- copper_block
- cobweb
- allium
- gunpowder
- leather
- minecart
- flint_and_steel
- snowball
- tnt
- blast_furnace
- dead_bush
- pointed_dripstone
- glass_bottle
- milk_bucket
- chest_minecart
- chiseled_stone_bricks
- iron_boots
- grass
- ender_pearl
- lapis_lazuli
- iron_helmet
- lava_bucket
- barrel
- bread
- rotten_flesh
- piston
- dried_kelp
- melon_slice
- lantern
- smithing_table
- cornflower
- charcoal
- repeater
- cooked_porkchop
- golden_sword
- spruce_sapling
- birch_door
- saddle
- arrow
- chiseled_sandstone
- cooked_beef
- hay_block
- brown_mushroom
- iron_axe
- mud_bricks
- smoker
- iron_trapdoor
- armor_stand
- rabbit_hide
- campfire
- vine
- iron_bars
- composter
- carved_pumpkin
- wheat_seeds
- redstone_torch
- glow_berries
- rail
- salmon
- clock
- acacia_boat
- item_frame
- golden_hoe
- compass
- mushroom_stew
- birch_sapling
- flint
- sugar
- emerald
- fletching_table
- iron_hoe
- dark_oak_boat
- cooked_mutton
- book
- tripwire_hook
- ink_sac
- sweet_berries
- bow
- cracked_stone_bricks
- beehive **REMOVE**
- orange_dye
- mangrove_chest_boat **REMOVE**
- apple
- lilac
- spider_eye
- deepslate_bricks
- cooked_cod
- lily_of_the_valley
- brick
- fishing_rod
- activator_rail
- flower_pot
- raw_gold
- bone
- dried_kelp_block
- beetroot
- gray_dye
- smooth_basalt
- painting
- red_mushroom
- magma_block
- cauldron
- hopper
- carrot
