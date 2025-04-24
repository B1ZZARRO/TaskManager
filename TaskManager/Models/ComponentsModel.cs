using System.Collections.Generic;

namespace TaskManager.Models;

public class ComponentsModel
{
    public List<ComponentsBodyModel> Body { get; set; }
}

public class ComponentsBodyModel
{
    public int ComponentId { get; set; }
    public string ComponentName { get; set; }
    public int AssemblyTimeMinutes { get; set; }
    public List<ComponentItemsModel> Items { get; set; }
}

public class ComponentItemsModel
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemType { get; set; }
    public int QuantityOnStorage { get; set; }
    public int QuantityNeeded { get; set; }
}