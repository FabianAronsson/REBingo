﻿using System.Text.Json;
using REBingo.Services;

Console.WriteLine("Welcome to REBingo");

var json = File.ReadAllText("./items2.json");
var items = JsonSerializer.Deserialize<List<string>>(json);

foreach (var item in items)
{
    FileService.Build(item);
}
Console.WriteLine("Successfully added items");

Console.ReadKey();