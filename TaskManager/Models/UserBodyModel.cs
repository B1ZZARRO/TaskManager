namespace TaskManager.Models;

public class UserBodyModel
{
    public int UserId { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public int? RoleId { get; set; }
    public string RoleName { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; }
}