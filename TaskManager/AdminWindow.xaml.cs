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
        Device();
        Setting();
    }

    private void AdminWindow_OnClosed(object? sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void UsersDG()
    {
        var response = apiClientUser.Get<UsersModel>(new RestRequest($"/User/GetAllUsers"));
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
        
        var responseRoles = apiClientUser.Get<RoleModel>(new RestRequest($"/User/GetAllRoles"));
        List<String> roleList = new List<String>();
        for (int i = 0; i < responseRoles.Body.Count; i++)
        {
            roleList.Add(responseRoles.Body[i].RoleName);
        }

        var responseGroups = apiClientUser.Get<GroupModel>(new RestRequest($"/User/GetAllGroups"));
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
        var usersResponse = apiClientUser.Get<UsersModel>(new RestRequest("/User/GetAllUsers"));
        var usersList = usersResponse.Body;

        // Список для хранения всех задач
        var allTasks = new List<TaskItemModel>();

        // Запрашиваем задачи каждого сотрудника
        foreach (var user in usersList)
        {
            var tasksResponse =
                apiClientUser.Get<TaskModel>(new RestRequest($"/Task/tasks/user/{user.UserId}"));
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

    private void Device()
    {
        var responseDevices = apiClientUser.Get<DeviceModel>(new RestRequest($"/Device/GetAllDevices"));
        List<String> devicesList = new List<String>();
        for (int i = 0; i < responseDevices.Body.Count; i++)
        {
            devicesList.Add(responseDevices.Body[i].DeviceName + " " + responseDevices.Body[i].DeviceModel);
        }
        cb_devices.ItemsSource = devicesList;
        
        var responseItems = apiClientUser.Get<ComponentsModel>(new RestRequest($"/Device/GetDeviceComponents/{5}"));
        List<ComponentsBodyModel> itemsList = new List<ComponentsBodyModel>();
        for (int i = 0; i < responseItems.Body.Count; i++)
        {
            itemsList.Add(new ComponentsBodyModel()
            {
                ComponentName = responseItems.Body[i].ComponentName,
                AssemblyTimeMinutes = responseItems.Body[i].AssemblyTimeMinutes,
                Items = responseItems.Body[i].Items
            });
        }
        
        var response = apiClientUser.Get<ComponentsModel>(new RestRequest($"/Device/GetDeviceComponents/{5}"));
        List<ComponentsBodyModel> compList = new List<ComponentsBodyModel>();
        for (int i = 0; i < response.Body.Count; i++)
        {
            compList.Add(new ComponentsBodyModel()
            {
                ComponentName = response.Body[i].ComponentName,
                AssemblyTimeMinutes = response.Body[i].AssemblyTimeMinutes,
                Items = response.Body[i].Items.ToList()
            });
        }
        ComponentsDataGrid.ItemsSource = compList;

        /*DataGridTextColumn itemNameColumn = new DataGridTextColumn
        {
            Header = "Название", Binding = new Binding("ItemName")
        };
        ComponentsDataGrid.Columns.Add(itemNameColumn);

        DataGridTextColumn movementTypeColumn = new DataGridTextColumn
        {
            Header = "Тип", Binding = new Binding("MovementType")
        };
        ComponentsDataGrid.Columns.Add(movementTypeColumn);

        DataGridTextColumn quantityColumn = new DataGridTextColumn
        {
            Header = "Количество", Binding = new Binding("Quantity")
        };
        ComponentsDataGrid.Columns.Add(quantityColumn);
        
        DataGridTextColumn movementDateColumn = new DataGridTextColumn
        {
            Header = "Дата", Binding = new Binding("MovementDate")
        };
        ComponentsDataGrid.Columns.Add(movementDateColumn);
        
        DataGridTextColumn relatedTaskTitleColumn = new DataGridTextColumn
        {
            Header = "Задача", Binding = new Binding("RelatedTaskTitle")
        };
        ComponentsDataGrid.Columns.Add(relatedTaskTitleColumn);*/
    }

    private void Setting()
    {
        var responseRoles = apiClientUser.Get<RoleModel>(new RestRequest($"/User/GetAllRoles"));
        List<String> roleList = new List<String>();
        for (int i = 0; i < responseRoles.Body.Count; i++)
        {
            roleList.Add(responseRoles.Body[i].RoleName);
        }

        var responseGroups = apiClientUser.Get<GroupModel>(new RestRequest($"/User/GetAllGroups"));
        List<String> groupList = new List<String>();
        for (int i = 0; i < responseGroups.Body.Count; i++)
        {
            groupList.Add(responseGroups.Body[i].GroupName);
        }

        lb_roles.ItemsSource = roleList;
        lb_group.ItemsSource = groupList;
    }

    /*private async void LoadDeviceComponents(int deviceId)
    {
        var request = new RestRequest($"/Device/GetDeviceComponents?deviceId={deviceId}");
        var response = await apiClientUser.GetAsync<ComponentsModel>(request);

        if (response != null && response.Body != null)
        {
            ComponentsDataGrid.Columns.Clear();

            // Настраиваем колонки DataGrid
            ComponentsDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Название компонента",
                Binding = new Binding("ComponentName"),
                Width = new DataGridLength(2, DataGridLengthUnitType.Star)
            });

            ComponentsDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Время сборки (мин.)",
                Binding = new Binding("AssemblyTimeMinutes"),
                Width = 150
            });

            ComponentsDataGrid.Columns.Add(new DataGridTemplateColumn
            {
                Header = "Детали",
                CellTemplate = (DataTemplate)FindResource("ComponentItemsTemplate"),
                Width = 300
            });

            ComponentsDataGrid.ItemsSource = response.Body;
        }
    }*/
    
    private void Cb_devices_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        /*if (cb_devices.SelectedItem is DeviceModel selectedDevice)
        {
            int selectedDeviceId = selectedDevice.DeviceId;

            // Теперь у тебя есть выбранный ID, можно использовать дальше:
            MessageBox.Show($"Выбран ID прибора: {selectedDeviceId}");

            // Например, вызвать загрузку компонентов:
            LoadDeviceComponents(selectedDeviceId);
        }*/
    }
}