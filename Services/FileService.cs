using System.Text.Json;
using REBingo.Models;

namespace REBingo.Services;

public static class FileService
{
    private static readonly string BaseSpeedPath = "./datapacks/speed_set/";
    private static readonly string Advancements = "data/flytre/advancements/detection/";
    private static readonly string Clarify = "data/flytre/functions/clarify/";
    private static readonly string Detect = "data/flytre/functions/detect/";
    private static readonly string Structure = "data/flytre/functions/randomize_board/structure/";
    private static readonly string All0 = "data/flytre/functions/randomize_board/all_0.mcfunction";
    private static readonly string PickRandom = "data/flytre/functions/randomize_board/pick_random.mcfunction";
    private static readonly string ResetBoardColors = "data/flytre/functions/reset_board_colors.mcfunction";
    private static readonly string AddItemObjectives = "./datapacks/base_generated/data/flytre/functions/add_item_objectives.mcfunction";
    private static readonly string Predicates = "data/flytre/predicates/";
    private static readonly string LootTable = "data/flytre/loot_tables/bingo_item.json";


    public static void Build(string itemName)
    {
        CreateFolders(itemName.Split('.')[2]);
        CreateAdvancementFile(itemName.Split('.')[2]);
        UpdateAddObjectives(itemName.Split('.')[2]);
        UpdateClarifyFiles(itemName);
        CreateBaseDetectFiles(itemName.Split('.')[2]);
        CreateGottenDetectFiles(itemName);
        UpdateStructureFile(itemName.Split('.')[2]);
        UpdateAll0File(itemName.Split('.')[2]);
        UpdatePickRandomFile(itemName.Split('.')[2]);
        UpdateResetBoardColorsFile(itemName.Split('.')[2]);
        UpdatePredicateFiles(itemName.Split('.')[2]);
        UpdateLootTable(itemName.Split('.')[2]);
    }

    private static void CreateFolders(string itemName)
    {
        Directory.CreateDirectory(BaseSpeedPath + Predicates + itemName + "speed");
        Directory.CreateDirectory(BaseSpeedPath + Detect + itemName + "speed");
    }

    private static void CreateAdvancementFile(string itemName)
    {
        var content = "{\"parent\":\"flytre:items/root\",\"criteria\": {\"" + itemName + "speed" +
                         "\": {\"conditions\": {\"items\": [{\"items\": [\"" + itemName +
                         "\"]}],\"player\": [{\"condition\":\"minecraft:value_check\",\"value\": {\"type\":\"minecraft:score\",\"target\": {\"type\": \"minecraft:fixed\",\"name\": \"" +
                         itemName + "speed" + "\" },\"score\": \"global\"},\"range\": {\"min\": 1}}]},\"trigger\": \"minecraft:inventory_changed\"}},\"requirements\": [[\"" +
                         itemName + "speed" + "\"]],\"rewards\": {\"function\": \"flytre:detect/" + itemName + "speed" + "/base\"}}";
        CreateFile(BaseSpeedPath + Advancements + itemName + "speed" + ".json", content);
    }

    private static void UpdateClarifyFiles(string itemName)
    {
        var di = new DirectoryInfo(BaseSpeedPath + Clarify);
        var mcfunctions = di.GetFiles("*.mcfunction");

        foreach (var mcfunction in mcfunctions)
        {
            if (mcfunction.Name.Equals("base.mcfunction"))
                continue;
            var content =
                $"execute if score {itemName.Split('.')[2] + "speed"} global matches {mcfunction.Name.Split('.')[0]} run tellraw @s [\"\",{{\"text\":\"The item in slot {mcfunction.Name.Split('.')[0]} is \", \"color\":\"dark_purple\" }},{{\"translate\":\"{itemName}\",\"color\":\"dark_purple\"}}]";
            var sw = mcfunction.AppendText();
            sw.WriteLine(content);
            sw.Close();
        }
    }

    private static void CreateBaseDetectFiles(string itemName)
    {
        var baseContent = $"scoreboard players set @s clear 0\r\nexecute unless score lockout stage matches 1..2 unless score red {itemName + "speed"} matches 1.. as @s[team=red] run scoreboard players set @s clear 1\r\nexecute unless score lockout stage matches 1 unless score yellow {itemName + "speed"} matches 1.. as @s[team=yellow] run scoreboard players set @s clear 1 \r\nexecute unless score lockout stage matches 1 unless score green {itemName + "speed"} matches 1.. as @s[team=green] run scoreboard players set @s clear 1\r\nexecute unless score lockout stage matches 1 unless score blue {itemName + "speed"} matches 1.. as @s[team=blue] run scoreboard players set @s clear 1\r\nexecute if score lockout stage matches 1 unless score completed {itemName + "speed"} matches 1.. as @s[team=red] run scoreboard players set @s clear 1\r\nexecute if score lockout stage matches 1 unless score completed {itemName + "speed"} matches 1.. as @s[team=yellow] run scoreboard players set @s clear 1\r\nexecute if score lockout stage matches 1 unless score completed {itemName + "speed"} matches 1.. as @s[team=green] run scoreboard players set @s clear 1\r\nexecute if score lockout stage matches 1 unless score completed {itemName + "speed"} matches 1.. as @s[team=blue] run scoreboard players set @s clear 1\r\nexecute as @s[scores={{clear=1..}},team=red] run scoreboard players set red {itemName + "speed"} 1\r\nexecute as @s[scores={{clear=1..}},team=yellow] run scoreboard players set yellow {itemName + "speed"} 1\r\nexecute as @s[scores={{clear=1..}},team=green] run scoreboard players set green {itemName + "speed"} 1\r\nexecute as @s[scores={{clear=1..}},team=blue] run scoreboard players set blue {itemName + "speed"} 1\r\nexecute as @s[scores={{clear=1..}}] run scoreboard players set completed {itemName + "speed"} 1\r\nexecute as @s[scores={{clear=1..}}] run function flytre:detect/{itemName + "speed"}/gotten\r\nadvancement revoke @s only flytre:detection/{itemName + "speed"}";
        CreateFile(BaseSpeedPath + Detect + itemName + "speed/" + "base.mcfunction", baseContent);
    }
    private static void CreateGottenDetectFiles(string itemName)
    {
        Console.WriteLine(itemName.Split('.')[2]);
        var gottenContent =
           $"execute as @s[team=red] run tellraw @a [\"\",{{\"text\":\"Red completed an item: \",\"color\":\"dark_red\"}} ,{{\"translate\":\"{itemName}\",\"color\":\"dark_red\"}}]\r\nexecute as @s[team=yellow] run tellraw @a [\"\",{{\"text\":\"Yellow completed an item: \",\"color\":\"gold\"}},{{\"translate\":\"{itemName}\",\"color\":\"gold\"}}]\r\nexecute as @s[team=green] run tellraw @a [\"\",{{\"text\":\"Green completed an item: \",\"color\":\"green\"}},{{\"translate\":\"{itemName}\",\"color\":\"green\"}}]\r\nexecute as @s[team=blue] run tellraw @a [\"\",{{\"text\":\"Blue completed an item: \",\"color\":\"dark_aqua\"}},{{\"translate\":\"{itemName}\",\"color\":\"dark_aqua\"}}]\r\nexecute as @s[team=red] as @a[team=red] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1\r\nexecute as @s[team=yellow] as @a[team=yellow] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1\r\nexecute as @s[team=green] as @a[team=green] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1\r\nexecute as @s[team=blue] as @a[team=blue] at @s run playsound minecraft:entity.experience_orb.pickup player @s ~ ~ ~ 1 0.1\r\nexecute as @s[team=red] as @a[team=!red] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1\r\nexecute as @s[team=yellow] as @a[team=!yellow] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1\r\nexecute as @s[team=green] as @a[team=!green] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1\r\nexecute as @s[team=blue] as @a[team=!blue] at @s run playsound minecraft:entity.guardian.death player @s ~ ~ ~ 1 1\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 1 at @e[type=armor_stand,tag=bingo,tag=1] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 1 run scoreboard players operation red board_1 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 1 run scoreboard players operation yellow board_1 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 1 run scoreboard players operation green board_1 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 1 run scoreboard players operation blue board_1 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 1 run scoreboard players operation completed board_1 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 2 at @e[type=armor_stand,tag=bingo,tag=2] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 2 run scoreboard players operation red board_2 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 2 run scoreboard players operation yellow board_2 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 2 run scoreboard players operation green board_2 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 2 run scoreboard players operation blue board_2 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 2 run scoreboard players operation completed board_2 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 3 at @e[type=armor_stand,tag=bingo,tag=3] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 3 run scoreboard players operation red board_3 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 3 run scoreboard players operation yellow board_3 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 3 run scoreboard players operation green board_3 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 3 run scoreboard players operation blue board_3 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 3 run scoreboard players operation completed board_3 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 4 at @e[type=armor_stand,tag=bingo,tag=4] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 4 run scoreboard players operation red board_4 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 4 run scoreboard players operation yellow board_4 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 4 run scoreboard players operation green board_4 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 4 run scoreboard players operation blue board_4 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 4 run scoreboard players operation completed board_4 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 5 at @e[type=armor_stand,tag=bingo,tag=5] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 5 run scoreboard players operation red board_5 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 5 run scoreboard players operation yellow board_5 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 5 run scoreboard players operation green board_5 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 5 run scoreboard players operation blue board_5 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 5 run scoreboard players operation completed board_5 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 6 at @e[type=armor_stand,tag=bingo,tag=6] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 6 run scoreboard players operation red board_6 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 6 run scoreboard players operation yellow board_6 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 6 run scoreboard players operation green board_6 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 6 run scoreboard players operation blue board_6 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 6 run scoreboard players operation completed board_6 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 7 at @e[type=armor_stand,tag=bingo,tag=7] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 7 run scoreboard players operation red board_7 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 7 run scoreboard players operation yellow board_7 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 7 run scoreboard players operation green board_7 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 7 run scoreboard players operation blue board_7 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 7 run scoreboard players operation completed board_7 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 8 at @e[type=armor_stand,tag=bingo,tag=8] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 8 run scoreboard players operation red board_8 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 8 run scoreboard players operation yellow board_8 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 8 run scoreboard players operation green board_8 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 8 run scoreboard players operation blue board_8 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 8 run scoreboard players operation completed board_8 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 9 at @e[type=armor_stand,tag=bingo,tag=9] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 9 run scoreboard players operation red board_9 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 9 run scoreboard players operation yellow board_9 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 9 run scoreboard players operation green board_9 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 9 run scoreboard players operation blue board_9 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 9 run scoreboard players operation completed board_9 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 10 at @e[type=armor_stand,tag=bingo,tag=10] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 10 run scoreboard players operation red board_10 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 10 run scoreboard players operation yellow board_10 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 10 run scoreboard players operation green board_10 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 10 run scoreboard players operation blue board_10 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 10 run scoreboard players operation completed board_10 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 11 at @e[type=armor_stand,tag=bingo,tag=11] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 11 run scoreboard players operation red board_11 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 11 run scoreboard players operation yellow board_11 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 11 run scoreboard players operation green board_11 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 11 run scoreboard players operation blue board_11 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 11 run scoreboard players operation completed board_11 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 12 at @e[type=armor_stand,tag=bingo,tag=12] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 12 run scoreboard players operation red board_12 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 12 run scoreboard players operation yellow board_12 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 12 run scoreboard players operation green board_12 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 12 run scoreboard players operation blue board_12 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 12 run scoreboard players operation completed board_12 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 13 at @e[type=armor_stand,tag=bingo,tag=13] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 13 run scoreboard players operation red board_13 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 13 run scoreboard players operation yellow board_13 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 13 run scoreboard players operation green board_13 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 13 run scoreboard players operation blue board_13 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 13 run scoreboard players operation completed board_13 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 14 at @e[type=armor_stand,tag=bingo,tag=14] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 14 run scoreboard players operation red board_14 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 14 run scoreboard players operation yellow board_14 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 14 run scoreboard players operation green board_14 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 14 run scoreboard players operation blue board_14 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 14 run scoreboard players operation completed board_14 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 15 at @e[type=armor_stand,tag=bingo,tag=15] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 15 run scoreboard players operation red board_15 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 15 run scoreboard players operation yellow board_15 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 15 run scoreboard players operation green board_15 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 15 run scoreboard players operation blue board_15 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 15 run scoreboard players operation completed board_15 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 16 at @e[type=armor_stand,tag=bingo,tag=16] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 16 run scoreboard players operation red board_16 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 16 run scoreboard players operation yellow board_16 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 16 run scoreboard players operation green board_16 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 16 run scoreboard players operation blue board_16 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 16 run scoreboard players operation completed board_16 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 17 at @e[type=armor_stand,tag=bingo,tag=17] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 17 run scoreboard players operation red board_17 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 17 run scoreboard players operation yellow board_17 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 17 run scoreboard players operation green board_17 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 17 run scoreboard players operation blue board_17 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 17 run scoreboard players operation completed board_17 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 18 at @e[type=armor_stand,tag=bingo,tag=18] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 18 run scoreboard players operation red board_18 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 18 run scoreboard players operation yellow board_18 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 18 run scoreboard players operation green board_18 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 18 run scoreboard players operation blue board_18 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 18 run scoreboard players operation completed board_18 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 19 at @e[type=armor_stand,tag=bingo,tag=19] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 19 run scoreboard players operation red board_19 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 19 run scoreboard players operation yellow board_19 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 19 run scoreboard players operation green board_19 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 19 run scoreboard players operation blue board_19 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 19 run scoreboard players operation completed board_19 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 20 at @e[type=armor_stand,tag=bingo,tag=20] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 20 run scoreboard players operation red board_20 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 20 run scoreboard players operation yellow board_20 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 20 run scoreboard players operation green board_20 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 20 run scoreboard players operation blue board_20 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 20 run scoreboard players operation completed board_20 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 21 at @e[type=armor_stand,tag=bingo,tag=21] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 21 run scoreboard players operation red board_21 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 21 run scoreboard players operation yellow board_21 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 21 run scoreboard players operation green board_21 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 21 run scoreboard players operation blue board_21 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 21 run scoreboard players operation completed board_21 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 22 at @e[type=armor_stand,tag=bingo,tag=22] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 22 run scoreboard players operation red board_22 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 22 run scoreboard players operation yellow board_22 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 22 run scoreboard players operation green board_22 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 22 run scoreboard players operation blue board_22 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 22 run scoreboard players operation completed board_22 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 23 at @e[type=armor_stand,tag=bingo,tag=23] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 23 run scoreboard players operation red board_23 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 23 run scoreboard players operation yellow board_23 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 23 run scoreboard players operation green board_23 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 23 run scoreboard players operation blue board_23 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 23 run scoreboard players operation completed board_23 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 24 at @e[type=armor_stand,tag=bingo,tag=24] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 24 run scoreboard players operation red board_24 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 24 run scoreboard players operation yellow board_24 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 24 run scoreboard players operation green board_24 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 24 run scoreboard players operation blue board_24 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 24 run scoreboard players operation completed board_24 = completed {itemName.Split('.')[2] + "speed"}\r\n\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 25 at @e[type=armor_stand,tag=bingo,tag=25] run function flytre:set_corner/base\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 25 run scoreboard players operation red board_25 = red {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 25 run scoreboard players operation yellow board_25 = yellow {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 25 run scoreboard players operation green board_25 = green {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 25 run scoreboard players operation blue board_25 = blue {itemName.Split('.')[2] + "speed"}\r\nexecute if score {itemName.Split('.')[2] + "speed"} global matches 25 run scoreboard players operation completed board_25 = completed {itemName.Split('.')[2] + "speed"}";
        CreateFile(BaseSpeedPath + Detect + itemName.Split('.')[2] + "speed/" + "gotten.mcfunction", gottenContent);
    }

    private static void UpdateStructureFile(string itemName)
    {
        var di = new DirectoryInfo(BaseSpeedPath + Structure);
        var mcfunctions = di.GetFiles("*.mcfunction");

        foreach (var mcfunction in mcfunctions)
        {
            var content =
                $"execute if score {itemName + "speed"} global matches {mcfunction.Name.Split('.')[0]} as @e[type=armor_stand,tag=bingo,tag={mcfunction.Name.Split('.')[0]}] at @s run setblock ~ ~ ~ minecraft:structure_block[mode=load]{{metadata:\"\",mirror:\"NONE\",ignoreEntities:1b,powered:0b,rotation:\"NONE\",posX:0,mode:\"LOAD\",posY:-17,sizeX:16,posZ:0,name:\"flytre:{itemName + "speed" + "structure"}\", sizeY:32,sizeZ:16,showboundingbox:1b}}";

            var fileContent = new List<string>(File.ReadAllLines(BaseSpeedPath + Structure + mcfunction.Name));

            var temp = fileContent[^1];
            fileContent[^1] = content;
            fileContent.Add(temp);
            File.WriteAllLines(BaseSpeedPath + Structure + mcfunction.Name, fileContent);
        }
    }

    private static void UpdateAll0File(string itemName)
    {
        var content = $"\nscoreboard players set {itemName + "speed"} global 0";
        File.AppendAllText(BaseSpeedPath + All0, content);
    }

    private static void UpdatePickRandomFile(string itemName)
    {

        var fileContent = new List<string>(File.ReadAllLines(BaseSpeedPath + PickRandom));
        var counter = 0;
        foreach (var s in fileContent)
        {
            if (s.Contains("execute"))
                counter += 1;
        }
        counter = (counter - 1) / 2;

        foreach (var s in fileContent.ToList())
        {
            if (s.Contains(counter.ToString()))
            {
                fileContent[fileContent.IndexOf(s)] = s.Replace(counter.ToString(), (counter + 1).ToString());
            }
        }

        var content = new List<string>
        {
            $"execute if score x rng matches { counter } unless score {itemName + "speed"} global matches 0 run scoreboard players set failed global 1",
            $"execute if score x rng matches { counter } if score {itemName + "speed"} global matches 0 run scoreboard players operation {itemName + "speed"} global = trial global"
        };

        var temp = fileContent[^1];
        fileContent[^1] = content[0];
        fileContent.Add(content[1]);
        fileContent.Add(temp);

        File.WriteAllLines(BaseSpeedPath + PickRandom, fileContent);
    }

    private static void UpdateResetBoardColorsFile(string itemName)
    {
        var fileContent = new List<string>(File.ReadAllLines(BaseSpeedPath + ResetBoardColors));
        var baseContent = $"scoreboard players reset red {itemName + "speed"}";
        var content = new List<string>
        {
            $"scoreboard players reset yellow {itemName + "speed" }",
            $"scoreboard players reset green {itemName + "speed" }",
            $"scoreboard players reset blue {itemName + "speed" }",
            $"scoreboard players reset completed {itemName + "speed" }",
        };

        var temp = fileContent[^1];
        fileContent[^1] = baseContent;
        fileContent.AddRange(content);
        fileContent.Add(temp);
        File.WriteAllLines(BaseSpeedPath + ResetBoardColors, fileContent);
    }

    private static void UpdateAddObjectives(string itemName) => File.AppendAllText(AddItemObjectives, $"\nscoreboard objectives add {itemName + "speed"} dummy");

    private static void UpdatePredicateFiles(string itemName)
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

        CreateFile(BaseSpeedPath + Predicates + itemName + "speed/" + "blue.json", blue);
        CreateFile(BaseSpeedPath + Predicates + itemName + "speed/" + "red.json", red);
        CreateFile(BaseSpeedPath + Predicates + itemName + "speed/" + "yellow.json", yellow);
        CreateFile(BaseSpeedPath + Predicates + itemName + "speed/" + "green.json", green);
        CreateFile(BaseSpeedPath + Predicates + itemName + "speed/" + "main.json", main);
    }

    private static void UpdateLootTable(string itemName)
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
                    name = $"flytre:{itemName + "speed"}/main"
                }
            },
            name = itemName,
            type = "minecraft:item"
        });

        var content = JsonSerializer.Serialize(lootTable);
        CreateFile(BaseSpeedPath + LootTable, content);
    }

    private static void CreateFile(string path, string content)
    {
        using StreamWriter sw = File.CreateText(path);
        sw.WriteLine(content);
        sw.Close();
    }
}
