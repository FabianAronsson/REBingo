namespace REBingo.Models;

public class Entry
{
    public string type { get; set; }
    public string name { get; set; }
    public List<Condition> conditions { get; set; }
}