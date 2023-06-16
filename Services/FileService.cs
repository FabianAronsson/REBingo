﻿using System.Text.Json;
using REBingo.Models;

namespace REBingo.Services;

public class FileService
{
    public readonly string BaseSpeedPath = "./datapacks/speed_set/";
    public readonly string Advancements = "data/flytre/advancements/detection";
    public readonly string Clarify = "data/flytre/functions/clarify";
    public readonly string Detect = "data/flytre/functions/detect";
    public readonly string Structure = "data/flytre/functions/randomize_board/structure";
    public readonly string All0 = "data/flytre/functions/randomize_board/structure/all_0.mcfunction";
    public readonly string PickRandom = "data/flytre/functions/randomize_board/structure/pick_random.mcfunction";
    public readonly string ResetBoardColors = "data/flytre/functions/reset_board_colors.mcfunction";
    public readonly string AddItemObjectives = "./datapacks/base_generated/data/flytre/functions/add_item_objectives.mcfunction";
    public readonly string Predicates = "data/flytre/predicates";
    public readonly string LootTable = "data/flytre/loot_tables/bingo_item.json";

    public void CreateFolders(string itemName)
    {
        Directory.CreateDirectory(BaseSpeedPath + Predicates + itemName + "speed");
        Directory.CreateDirectory(BaseSpeedPath + Detect + itemName + "speed");
    }

    public void CreateAdvancementFile(string itemName)
    {
        var content = "{\"parent\":\"flytre:items/root\",\"criteria\": {\"" + itemName + "speed" +
                         "\": {\"conditions\": {\"items\": [{\"items\": [\"" + itemName +
                         "\"]}],\"player\": [{\"condition\":\"minecraft:value_check\",\"value\": {\"type\":\"minecraft:score\",\"target\": {\"type\": \"minecraft:fixed\",\"name\": \"" +
                         itemName + "speed" + "\" },\"score\": \"global\"},\"range\": {\"min\": 1}}]},\"trigger\": \"minecraft:inventory_changed\"}},\"requirements\": [[\"" +
                         itemName + "speed" + "\"]],\"rewards\": {\"function\": \"flytre:detect/" + itemName + "speed" + "/base\"}}";
        CreateFile(BaseSpeedPath + Advancements + itemName + "speed" + ".json", content);
    }

    public void UpdateClarifyFiles(string itemName)
    {
        var di = new DirectoryInfo(BaseSpeedPath + Clarify);
        var mcfunctions = di.GetFiles("*.mcfunction");

        foreach (var mcfunction in mcfunctions)
        {
            if (mcfunction.Name.Equals("base.mcfunction"))
                continue;
            var content =
                $"execute if score {itemName + "speed"} global matches {mcfunction.Name.Split('.')[0]} run tellraw @s [\"\",{{\"text\":\"The item in slot {mcfunction.Name.Split('.')[0]} is {itemName}\", \"color\":\"dark_purple\" }}]";
            var sw = mcfunction.AppendText();
            sw.WriteLine(content);
        }
    }

    public void CreateDetectFiles(string itemName)
    {
        var baseContent =
            $"scoreboard players set @s clear 0 execute unless score lockout stage matches 1..2 unless score red {itemName + "speed"} matches 1.. as @s[team=red] run scoreboard players set @s clear 1 execute unless score lockout stage matches 1 unless score yellow {itemName + "speed"} matches 1.. as @s[team=yellow] run scoreboard players set @s clear 1 execute unless score lockout stage matches 1 unless score green {itemName + "speed"} matches 1.. as @s[team=green] run scoreboard players set @s clear 1 execute unless score lockout stage matches 1 unless score blue {itemName + "speed"} matches 1.. as @s[team=blue] run scoreboard players set @s clear 1 execute if score lockout stage matches 1 unless score completed {itemName + "speed"} matches 1.. as @s[team=red] run scoreboard players set @s clear 1 execute if score lockout stage matches 1 unless score completed {itemName + "speed"} matches 1.. as @s[team=yellow] run scoreboard players set @s clear 1 execute if score lockout stage matches 1 unless score completed {itemName + "speed"} matches 1.. as @s[team=green] run scoreboard players set @s clear 1 execute if score lockout stage matches 1 unless score completed {itemName + "speed"} matches 1.. as @s[team=blue] run scoreboard players set @s clear 1 execute as @s[scores={{clear=1..}},team=red] run scoreboard players set red {itemName + "speed"} 1 execute as @s[scores={{clear=1..}},team=yellow] run scoreboard players set yellow {itemName + "speed"} 1 execute as @s[scores={{clear=1..}},team=green] run scoreboard players set green {itemName + "speed"} 1 execute as @s[scores={{clear=1..}},team=blue] run scoreboard players set blue {itemName + "speed"} 1 execute as @s[scores={{clear=1..}}] run scoreboard players set completed {itemName + "speed"} 1 execute as @s[scores={{clear=1..}}] run function flytre:detect/{itemName + "speed"}/gottenadvancement revoke @s only flytre:detection/{itemName + "speed"}";
        CreateFile(BaseSpeedPath + Detect + "base.mcfunction", baseContent);

        var gottenContent =
            $"execute as @s[team=red] run tellraw @a [\"\",{{\"text\":\"Red completed an item: \",\"color\":\"dark_red\"}}] execute as @s[team=yellow] run tellraw @a [\"\",{{\"text\":\"Yellow completed an item: \",\"color\":\"gold\"}}] execute as @s[team=green] run tellraw @a [\"\",{{\"text\":\"Green completed an item: \",\"color\":\"green\"}},] execute as @s[team=blue] run tellraw @a [\"\",{{\"text\":\"Blue completed an item: \",\"color\":\"dark_aqua\"}},] execute as @s[team=red] as @a[team=red] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1 execute as @s[team=yellow] as @a[team=yellow] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1 execute as @s[team=green] as @a[team=green] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1 execute as @s[team=blue] as @a[team=blue] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1 execute as @s[team=red] as @a[team=!red] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1 execute as @s[team=yellow] as @a[team=!yellow] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1 execute as @s[team=green] as @a[team=!green] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1 execute as @s[team=blue] as @a[team=!blue] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1  execute if score {itemName + "speed"} global matches 1 at @e[type=armor_stand,tag=bingo,tag=1] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 1 run scoreboard players operation red board_1 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 1 run scoreboard players operation yellow board_1 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 1 run scoreboard players operation green board_1 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 1 run scoreboard players operation blue board_1 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 1 run scoreboard players operation completed board_1 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 2 at @e[type=armor_stand,tag=bingo,tag=2] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 2 run scoreboard players operation red board_2 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 2 run scoreboard players operation yellow board_2 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 2 run scoreboard players operation green board_2 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 2 run scoreboard players operation blue board_2 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 2 run scoreboard players operation completed board_2 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 3 at @e[type=armor_stand,tag=bingo,tag=3] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 3 run scoreboard players operation red board_3 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 3 run scoreboard players operation yellow board_3 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 3 run scoreboard players operation green board_3 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 3 run scoreboard players operation blue board_3 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 3 run scoreboard players operation completed board_3 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 4 at @e[type=armor_stand,tag=bingo,tag=4] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 4 run scoreboard players operation red board_4 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 4 run scoreboard players operation yellow board_4 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 4 run scoreboard players operation green board_4 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 4 run scoreboard players operation blue board_4 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 4 run scoreboard players operation completed board_4 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 5 at @e[type=armor_stand,tag=bingo,tag=5] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 5 run scoreboard players operation red board_5 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 5 run scoreboard players operation yellow board_5 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 5 run scoreboard players operation green board_5 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 5 run scoreboard players operation blue board_5 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 5 run scoreboard players operation completed board_5 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 6 at @e[type=armor_stand,tag=bingo,tag=6] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 6 run scoreboard players operation red board_6 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 6 run scoreboard players operation yellow board_6 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 6 run scoreboard players operation green board_6 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 6 run scoreboard players operation blue board_6 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 6 run scoreboard players operation completed board_6 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 7 at @e[type=armor_stand,tag=bingo,tag=7] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 7 run scoreboard players operation red board_7 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 7 run scoreboard players operation yellow board_7 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 7 run scoreboard players operation green board_7 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 7 run scoreboard players operation blue board_7 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 7 run scoreboard players operation completed board_7 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 8 at @e[type=armor_stand,tag=bingo,tag=8] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 8 run scoreboard players operation red board_8 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 8 run scoreboard players operation yellow board_8 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 8 run scoreboard players operation green board_8 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 8 run scoreboard players operation blue board_8 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 8 run scoreboard players operation completed board_8 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 9 at @e[type=armor_stand,tag=bingo,tag=9] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 9 run scoreboard players operation red board_9 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 9 run scoreboard players operation yellow board_9 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 9 run scoreboard players operation green board_9 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 9 run scoreboard players operation blue board_9 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 9 run scoreboard players operation completed board_9 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 10 at @e[type=armor_stand,tag=bingo,tag=10] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 10 run scoreboard players operation red board_10 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 10 run scoreboard players operation yellow board_10 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 10 run scoreboard players operation green board_10 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 10 run scoreboard players operation blue board_10 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 10 run scoreboard players operation completed board_10 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 11 at @e[type=armor_stand,tag=bingo,tag=11] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 11 run scoreboard players operation red board_11 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 11 run scoreboard players operation yellow board_11 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 11 run scoreboard players operation green board_11 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 11 run scoreboard players operation blue board_11 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 11 run scoreboard players operation completed board_11 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 12 at @e[type=armor_stand,tag=bingo,tag=12] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 12 run scoreboard players operation red board_12 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 12 run scoreboard players operation yellow board_12 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 12 run scoreboard players operation green board_12 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 12 run scoreboard players operation blue board_12 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 12 run scoreboard players operation completed board_12 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 13 at @e[type=armor_stand,tag=bingo,tag=13] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 13 run scoreboard players operation red board_13 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 13 run scoreboard players operation yellow board_13 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 13 run scoreboard players operation green board_13 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 13 run scoreboard players operation blue board_13 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 13 run scoreboard players operation completed board_13 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 14 at @e[type=armor_stand,tag=bingo,tag=14] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 14 run scoreboard players operation red board_14 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 14 run scoreboard players operation yellow board_14 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 14 run scoreboard players operation green board_14 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 14 run scoreboard players operation blue board_14 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 14 run scoreboard players operation completed board_14 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 15 at @e[type=armor_stand,tag=bingo,tag=15] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 15 run scoreboard players operation red board_15 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 15 run scoreboard players operation yellow board_15 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 15 run scoreboard players operation green board_15 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 15 run scoreboard players operation blue board_15 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 15 run scoreboard players operation completed board_15 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 16 at @e[type=armor_stand,tag=bingo,tag=16] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 16 run scoreboard players operation red board_16 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 16 run scoreboard players operation yellow board_16 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 16 run scoreboard players operation green board_16 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 16 run scoreboard players operation blue board_16 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 16 run scoreboard players operation completed board_16 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 17 at @e[type=armor_stand,tag=bingo,tag=17] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 17 run scoreboard players operation red board_17 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 17 run scoreboard players operation yellow board_17 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 17 run scoreboard players operation green board_17 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 17 run scoreboard players operation blue board_17 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 17 run scoreboard players operation completed board_17 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 18 at @e[type=armor_stand,tag=bingo,tag=18] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 18 run scoreboard players operation red board_18 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 18 run scoreboard players operation yellow board_18 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 18 run scoreboard players operation green board_18 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 18 run scoreboard players operation blue board_18 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 18 run scoreboard players operation completed board_18 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 19 at @e[type=armor_stand,tag=bingo,tag=19] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 19 run scoreboard players operation red board_19 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 19 run scoreboard players operation yellow board_19 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 19 run scoreboard players operation green board_19 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 19 run scoreboard players operation blue board_19 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 19 run scoreboard players operation completed board_19 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 20 at @e[type=armor_stand,tag=bingo,tag=20] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 20 run scoreboard players operation red board_20 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 20 run scoreboard players operation yellow board_20 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 20 run scoreboard players operation green board_20 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 20 run scoreboard players operation blue board_20 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 20 run scoreboard players operation completed board_20 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 21 at @e[type=armor_stand,tag=bingo,tag=21] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 21 run scoreboard players operation red board_21 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 21 run scoreboard players operation yellow board_21 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 21 run scoreboard players operation green board_21 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 21 run scoreboard players operation blue board_21 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 21 run scoreboard players operation completed board_21 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 22 at @e[type=armor_stand,tag=bingo,tag=22] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 22 run scoreboard players operation red board_22 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 22 run scoreboard players operation yellow board_22 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 22 run scoreboard players operation green board_22 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 22 run scoreboard players operation blue board_22 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 22 run scoreboard players operation completed board_22 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 23 at @e[type=armor_stand,tag=bingo,tag=23] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 23 run scoreboard players operation red board_23 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 23 run scoreboard players operation yellow board_23 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 23 run scoreboard players operation green board_23 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 23 run scoreboard players operation blue board_23 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 23 run scoreboard players operation completed board_23 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 24 at @e[type=armor_stand,tag=bingo,tag=24] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 24 run scoreboard players operation red board_24 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 24 run scoreboard players operation yellow board_24 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 24 run scoreboard players operation green board_24 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 24 run scoreboard players operation blue board_24 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 24 run scoreboard players operation completed board_24 = completed {itemName + "speed"}  execute if score {itemName + "speed"} global matches 25 at @e[type=armor_stand,tag=bingo,tag=25] run function flytre:set_corner/base execute if score {itemName + "speed"} global matches 25 run scoreboard players operation red board_25 = red {itemName + "speed"} execute if score {itemName + "speed"} global matches 25 run scoreboard players operation yellow board_25 = yellow {itemName + "speed"} execute if score {itemName + "speed"} global matches 25 run scoreboard players operation green board_25 = green {itemName + "speed"} execute if score {itemName + "speed"} global matches 25 run scoreboard players operation blue board_25 = blue {itemName + "speed"} execute if score {itemName + "speed"} global matches 25 run scoreboard players operation completed board_25 = completed {itemName + "speed"}";
        CreateFile(BaseSpeedPath + Detect + "gotten.mcfunction", baseContent);
    }

    public void UpdateStructureFile(string itemName)
    {
        var di = new DirectoryInfo(BaseSpeedPath + Structure);
        var mcfunctions = di.GetFiles("*.mcfunction");

        foreach (var mcfunction in mcfunctions)
        {
            var content =
                $"execute if score {itemName + "speed"} global matches {mcfunction.Name.Split('.')[0]} as @e[type=armor_stand,tag=bingo,tag={mcfunction.Name.Split('.')[0]}] at @s run setblock ~ ~ ~ minecraft:structure_block[mode=load]{{metadata:\"\",mirror:\"NONE\",ignoreEntities:1b,powered:0b,rotation:\"NONE\",posX:0,mode:\"LOAD\",posY:-17,sizeX:16,posZ:0,name:\"flytre:{itemName + "speed" + "structure"}sizeY:32,sizeZ:16,showboundingbox:1b}}";

            var fileContent = new List<string>(File.ReadAllLines(BaseSpeedPath + Structure + mcfunction.Name));

            var temp = fileContent[^2];
            fileContent.Add(content);
            fileContent.Add(temp);
            File.WriteAllLines(BaseSpeedPath + Structure + mcfunction.Name, fileContent);
        }
    }

    public void UpdateAll0File(string itemName)
    {
        var content = $"scoreboard players set {itemName + "speed"} global 0";
        File.AppendAllText(All0, content);
    }

    public void UpdatePickRandomFile(string itemName)
    {
        var content = new List<string>
        {
            $"execute if score x rng matches 0 unless score {itemName + "speed"} global matches 0 run scoreboard players set failed global 1",
            $"execute if score x rng matches 0 if score {itemName + "speed"} global matches 0 run scoreboard players operation {itemName + "speed"} global = trial global"
        };
        var fileContent = new List<string>(File.ReadAllLines(BaseSpeedPath + PickRandom));
        var counter = 0;
        foreach (var s in fileContent)
        {
            if (s.Contains("execute"))
                counter += 1;
        }
        counter = (counter - 1) / 2;

        foreach (var s in fileContent)
        {
            if (s.Contains(counter.ToString()))
            {
                fileContent[fileContent.IndexOf(s)] = s.Replace(counter.ToString(), (counter + 1).ToString());
            }
        }
        var temp = fileContent[^2];
        fileContent.AddRange(content);
        fileContent.Add(temp);
        File.WriteAllLines(BaseSpeedPath + PickRandom, fileContent);
    }

    public void UpdateResetBoardColorsFile(string itemName)
    {
        var fileContent = new List<string>(File.ReadAllLines(BaseSpeedPath + ResetBoardColors));
        var content = new List<string>
        {
            $"scoreboard players reset red {itemName + "speed" }",
            $"scoreboard players reset yellow {itemName + "speed" }",
            $"scoreboard players reset green {itemName + "speed" }",
            $"scoreboard players reset blue {itemName + "speed" }",
            $"scoreboard players reset completed {itemName + "speed" }",
        };

        var temp = fileContent[^2];
        fileContent.AddRange(content);
        fileContent.Add(temp);
        File.WriteAllLines(BaseSpeedPath + PickRandom, fileContent);
    }

    public void UpdateAddObjectives(string itemName) => File.AppendAllText(BaseSpeedPath + AddItemObjectives, $"scoreboard objectives add {itemName + "speed"} dummy");

    public void UpdatePredicateFiles(string itemName)
    {
        var blue =
            $"[{{\"condition\": \"minecraft:entity_properties\",\"entity\": \"this\",\"predicate\": {{\"team\": \"blue\"}}}},{{\"condition\": \"minecraft:value_check\",\"value\": {{\"type\": \"minecraft:score\",\"target\": {{\"type\": \"minecraft:fixed\",\"name\": \"blue\"}},\"score\": \"{itemName + "speed"}\"}},\"range\": 0  }}]";
        var red =
            $"[{{\"condition\": \"minecraft:entity_properties\",\"entity\": \"this\",\"predicate\": {{\"team\": \"red\"}}}},{{\"condition\": \"minecraft:value_check\",\"value\": {{\"type\": \"minecraft:score\",\"target\": {{\"type\": \"minecraft:fixed\",\"name\": \"red\"}},\"score\": \"{itemName + "speed"}\"}},\"range\": 0  }}]";
        var yellow =
            $"[{{\"condition\": \"minecraft:entity_properties\",\"entity\": \"this\",\"predicate\": {{\"team\": \"yellow\"}}}},{{\"condition\": \"minecraft:value_check\",\"value\": {{\"type\": \"minecraft:score\",\"target\": {{\"type\": \"minecraft:fixed\",\"name\": \"yellow\"}},\"score\": \"{itemName + "speed"}\"}},\"range\": 0  }}]";
        var green =
            $"[{{\"condition\": \"minecraft:entity_properties\",\"entity\": \"this\",\"predicate\": {{\"team\": \"green\"}}}},{{\"condition\": \"minecraft:value_check\",\"value\": {{\"type\": \"minecraft:score\",\"target\": {{ \"type\": \"minecraft:fixed\",\"name\": \"green\"}},\"score\": \"{itemName + "speed"}\"}},\"range\": 0  }}]";
        var main =
            $"[{{\"condition\":\"minecraft:value_check\",\"value\":{{\"type\": \"minecraft:score\",\"target\":{{\"type\": \"minecraft:fixed\",\"name\":\"{itemName + "speed"}\"}},\"score\":\"global\"}},\"range\": {{\"min\": 1}}}},{{\"condition\":\"minecraft:alternative\",\"terms\":[{{\"condition\":\"minecraft:reference\",\"name\":\"flytre:{itemName + "speed"}/red\"}},{{\"condition\":\"minecraft:reference\",\"name\":\"flytre:{itemName + "speed"}/yellow\"}},{{\"condition\":\"minecraft:reference\",\"name\":\"flytre:{itemName + "speed"}/green\"}},{{\"condition\": \"minecraft:reference\",\"name\":\"flytre:{itemName + "speed"}/blue\"}}]}},{{\"condition\": \"minecraft:alternative\",\"terms\":[{{\"condition\": \"minecraft:inverted\",\"term\": {{\"condition\": \"minecraft:value_check\",\"value\": {{\"type\": \"minecraft:score\",\"target\": {{\"type\": \"minecraft:fixed\",\"name\": \"lockout\"}},\"score\": \"stage\"}},\"range\": 1}}}},{{\"condition\": \"minecraft:inverted\",\"term\": {{\"condition\": \"minecraft:value_check\",\"value\": {{\"type\": \"minecraft:score\",\"target\": {{\"type\": \"minecraft:fixed\",\"name\": \"completed\"}},\"score\": \"{itemName + "speed"}\"}},\"range\": {{\"min\": 1}}}}}}]}}]";

        CreateFile(BaseSpeedPath + Predicates + itemName + "speed.json", blue);
        CreateFile(BaseSpeedPath + Predicates + itemName + "speed.json", red);
        CreateFile(BaseSpeedPath + Predicates + itemName + "speed.json", yellow);
        CreateFile(BaseSpeedPath + Predicates + itemName + "speed.json", green);
        CreateFile(BaseSpeedPath + Predicates + itemName + "speed.json", main);
    }

    public void UpdateLootTable(string itemName)
    {
        var json = File.ReadAllText(BaseSpeedPath + LootTable);
        var lootTable = JsonSerializer.Deserialize<LootTable>(json);
        lootTable.pools[0].entries.Add(new Entry
        {
            conditions = new List<Condition>
            {
                new()
                {
                    condition = "minecraft:reference",
                    name = $"flytre:{itemName}/main"
                }
            }, name = itemName, type = "minecraft:item"
        });

        var content = JsonSerializer.Serialize(lootTable);
        CreateFile(BaseSpeedPath + LootTable, content);
    }

    public void CreateFile(string path, string content)
    {
        using StreamWriter sw = File.CreateText(path);
        sw.WriteLine(content);
        sw.Close();
    }
}
