using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public class StorageMovementModel
{
    public List<StorageMovementBodyModel> Body { get; set; }
    public string Message { get; set; }
}

public class StorageMovementBodyModel
{
    public string ItemName { get; set; }
    public string MovementType { get; set; }
    public int Quantity { get; set; }
    public DateTime MovementDate { get; set; }
    public string RelatedTaskTitle { get; set; }
}