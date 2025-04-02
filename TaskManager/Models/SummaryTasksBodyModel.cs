using System;

namespace TaskManager.Models;

public class SummaryTasksBodyModel
{
    public int TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime CompletionDate { get; set; }
    public string ResultNotes { get; set; }
    
    public AssignedUserData AssignedUser { get; set; }
    public AssignedGroupData AssignedGroup { get; set; }
}

public class AssignedUserData
{
    public int UserId { get; set; }
    public string FullName { get; set; }
}

public class AssignedGroupData
{
    public int GroupId { get; set; }
    public string GroupName { get; set; }
}