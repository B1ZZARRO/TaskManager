using System;
using System.Runtime.InteropServices.JavaScript;

namespace TaskManager.Models;

public class UserTasksBodyModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Deadline { get; set; }
    public string PriorityName { get; set; }
    public string StatusName { get; set; }
}