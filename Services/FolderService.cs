namespace REBingo.Services;

public class FolderService
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
}