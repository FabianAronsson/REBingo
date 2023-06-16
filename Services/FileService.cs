using System.IO;
using System.Runtime.InteropServices;

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

    public void CreateFolders(string itemName)
    {
        Directory.CreateDirectory(BaseSpeedPath + Predicates + itemName);
        Directory.CreateDirectory(BaseSpeedPath + Detect + itemName);
    }

    public void CreateAdvancementFile(string itemName)
    {
        var content = "{\"parent\":\"flytre:items/root\",\"criteria\": {\"" + itemName + "speed" +
                         "\": {\"conditions\": {\"items\": [{\"items\": [\"" + itemName +
                         "\"]}],\"player\": [{\"condition\":\"minecraft:value_check\",\"value\": {\"type\":\"minecraft:score\",\"target\": {\"type\": \"minecraft:fixed\",\"name\": \"" +
                         itemName + "speed" + "\" },\"score\": \"global\"},\"range\": {\"min\": 1}}]},\"trigger\": \"minecraft:inventory_changed\"}},\"requirements\": [[\"" +
                         itemName + "speed" + "\"]],\"rewards\": {\"function\": \"flytre:detect/bedSpeed/base\"}}";
        CreateFile(BaseSpeedPath + Advancements, content);
    }

    public void CreateClarifyFile(string itemName)
    {
        var di = new DirectoryInfo(BaseSpeedPath + Clarify);
        var mcfunctions = di.GetFiles("*.mcfunction");

        foreach (var mcfunction in mcfunctions)
        {
            if (mcfunction.Name.Equals("base.mcfunction"))
                continue;
            var content =
                $"execute if score {itemName + "speed"} global matches {mcfunction.Name.Split('.')[0]} run tellraw @s [\"\",{{\"text\":\"The item in slot {mcfunction.Name.Split('.')[0]} is {itemName}]";
            var sw = mcfunction.AppendText();
            sw.WriteLine(content);
        }
    }

    public void CreateFile(string path, string content)
    {
        using StreamWriter sw = File.CreateText(path);
        sw.WriteLine(content);
    }
}
