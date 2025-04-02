using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RestSharp;
using TaskManager.API;
using TaskManager.Models;

namespace TaskManager;

public partial class AdminWindow : Window
{
    private RestClient apiClientUser = ServiceBuilder.GetInstance();

    public AdminWindow(int sId, string sName, Window parent)
    {
        InitializeComponent();
        UsersDG();
        TasksDG();
        Setting();
    }

    private void AdminWindow_OnClosed(object? sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void UsersDG()
    {
        var response = apiClientUser.Get<UsersModel>(new RestRequest($"/GetAllUsers"));
        List<UserBodyModel> usersList = new List<UserBodyModel>();
        for (int i = 0; i < response.Body.Count; i++)
        {
            usersList.Add(new UserBodyModel()
            {
                UserId = response.Body[i].UserId,
                LastName = response.Body[i].LastName,
                Name = response.Body[i].Name,
                Surname = response.Body[i].Surname,
                Login = response.Body[i].Login,
                Password = response.Body[i].Password,
                RoleId = response.Body[i].RoleId,
                RoleName = response.Body[i].RoleName,
                GroupId = response.Body[i].GroupId,
                GroupName = response.Body[i].GroupName
            });
        }
        
        var responseRoles = apiClientUser.Get<RoleModel>(new RestRequest($"/GetAllRoles"));
        List<String> roleList = new List<String>();
        for (int i = 0; i < responseRoles.Body.Count; i++)
        {
            roleList.Add(responseRoles.Body[i].RoleName);
        }

        var responseGroups = apiClientUser.Get<GroupModel>(new RestRequest($"/GetAllGroups"));
        List<String> groupList = new List<String>();
        for (int i = 0; i < responseGroups.Body.Count; i++)
        {
            groupList.Add(responseGroups.Body[i].GroupName);
        }
        
        DataGridTextColumn userIdColumn = new DataGridTextColumn
        {
            Header = "ID", Binding = new Binding("UserId")
        };
        UsersDataGrid.Columns.Add(userIdColumn);

        DataGridTextColumn lastNameColumn = new DataGridTextColumn
        {
            Header = "Фамилия", Binding = new Binding("LastName")
        };
        UsersDataGrid.Columns.Add(lastNameColumn);

        DataGridTextColumn nameColumn = new DataGridTextColumn
        {
            Header = "Имя", Binding = new Binding("Name")
        };
        UsersDataGrid.Columns.Add(nameColumn);

        DataGridTextColumn surnameColumn = new DataGridTextColumn
        {
            Header = "Отчество", Binding = new Binding("Surname")
        };
        UsersDataGrid.Columns.Add(surnameColumn);

        DataGridTextColumn loginColumn = new DataGridTextColumn
        {
            Header = "Логин", Binding = new Binding("Login")
        };
        UsersDataGrid.Columns.Add(loginColumn);

        DataGridTextColumn passwordColumn = new DataGridTextColumn
        {
            Header = "Пароль", Binding = new Binding("Password")
        };
        UsersDataGrid.Columns.Add(passwordColumn);
        
        DataTemplate roleTemplate = new DataTemplate();
        FrameworkElementFactory roleFactory = new FrameworkElementFactory(typeof(ComboBox));
        roleFactory.SetValue(ComboBox.ItemsSourceProperty, roleList);
        roleFactory.SetBinding(ComboBox.SelectedItemProperty, new Binding("RoleName"));
        roleTemplate.VisualTree = roleFactory;

        DataGridTemplateColumn roleNameColumn = new DataGridTemplateColumn
        {
            Header = "Роль", CellTemplate = roleTemplate
        };
        UsersDataGrid.Columns.Add(roleNameColumn);

        DataTemplate groupTemplate = new DataTemplate();
        FrameworkElementFactory groupFactory = new FrameworkElementFactory(typeof(ComboBox));
        groupFactory.SetValue(ComboBox.ItemsSourceProperty, groupList);
        groupFactory.SetBinding(ComboBox.SelectedItemProperty, new Binding("GroupName"));
        groupTemplate.VisualTree = groupFactory;

        DataGridTemplateColumn groupNameColumn = new DataGridTemplateColumn
        {
            Header = "Группа", CellTemplate = groupTemplate
        };
        UsersDataGrid.Columns.Add(groupNameColumn);

        UsersDataGrid.ItemsSource = usersList;
    }

    private void TasksDG()
    {
        // Получаем список сотрудников
        var usersResponse = apiClientUser.Get<UsersModel>(new RestRequest("/GetAllUsers"));
        var usersList = usersResponse.Body;

        // Список для хранения всех задач
        var allTasks = new List<TaskItemModel>();

        // Запрашиваем задачи каждого сотрудника
        foreach (var user in usersList)
        {
            var tasksResponse =
                apiClientUser.Get<TaskModel>(new RestRequest($"/tasks/user/{user.UserId}"));
            allTasks.AddRange(tasksResponse.Body);
        }

        // Определяем диапазон дат
        var minDate = allTasks.Min(t => t.CreatedAt.Date);
        var maxDate = allTasks.Max(t => t.Deadline.Date);
        var dateRange = Enumerable.Range(0, (maxDate - minDate).Days + 1)
            .Select(offset => minDate.AddDays(offset))
            .ToList();

        // Группируем задачи по сотрудникам
        var data = usersList.Select(user => new
        {
            FullName = $"{user.LastName} {user.Name} {user.Surname}",
            TaskCounts = dateRange.ToDictionary(
                date => date,
                date => allTasks.Count(task =>
                    task.AssignedUserId == user.UserId &&
                    task.CreatedAt.Date <= date &&
                    task.Deadline.Date >= date)
            )
        }).ToList();

        // Очищаем DataGrid
        TasksDataGrid.Columns.Clear();

        // Добавляем столбец для ФИО сотрудников
        TasksDataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = "ФИО",
            Binding = new Binding("FullName")
        });

        // Добавляем динамические столбцы для каждой даты
        foreach (var date in dateRange)
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                TasksDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = date.ToString($"{date:yyyy-MM-dd}\n{date:ddd}"),
                    Binding = new Binding($"TaskCounts[{date:yyyy-MM-dd}]")
                });
            }
        }

        // Привязываем данные
        TasksDataGrid.ItemsSource = data;
    }

    private void Setting()
    {
        var responseRoles = apiClientUser.Get<RoleModel>(new RestRequest($"/GetAllRoles"));
        List<String> roleList = new List<String>();
        for (int i = 0; i < responseRoles.Body.Count; i++)
        {
            roleList.Add(responseRoles.Body[i].RoleName);
        }

        var responseGroups = apiClientUser.Get<GroupModel>(new RestRequest($"/GetAllGroups"));
        List<String> groupList = new List<String>();
        for (int i = 0; i < responseGroups.Body.Count; i++)
        {
            groupList.Add(responseGroups.Body[i].GroupName);
        }

        lb_roles.ItemsSource = roleList;
        lb_group.ItemsSource = groupList;
    }
}