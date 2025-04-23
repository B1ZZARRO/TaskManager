using System.Collections.Generic;

namespace TaskManager.Models;

public class StorageItemsModel
{
    public List<StorageItemBodyModel> Body { get; set; }
    public string Message { get; set; }
}

public class StorageItemBodyModel
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string ItemType { get; set; }
    public int Quantity { get; set; }
}