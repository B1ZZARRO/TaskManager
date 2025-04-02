using System;
using System.Collections.Generic;

namespace TaskManager.Models;

public class TaskModel
{
    public List<TaskItemModel> Body { get; set; }
}

public class TaskItemModel
{
    public DateTime CreatedAt { get; set; }
    public DateTime Deadline { get; set; }
    public int AssignedUserId  { get; set; }
}