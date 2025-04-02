using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RestSharp;
using TaskManager.API;
using TaskManager.Models;

namespace TaskManager;

public partial class UserWindow : Window
{
    private RestClient apiClientUser = ServiceBuilder.GetInstance();
    
    public UserWindow(int sId, string sName, Window parent)
    {
        InitializeComponent();
        
        var response = apiClientUser.Get<UserTasksModel>(new RestRequest($"/tasks/user/{sId}"));
        List<UserTasksBodyModel> userTasksList = new List<UserTasksBodyModel>();
        for (int i = 0; i < response.Body.Count; i++)
        {
            userTasksList.Add(new UserTasksBodyModel()
            {
                Title = response.Body[i].Title,
                Description = response.Body[i].Description,
                CreatedAt = response.Body[i].CreatedAt,
                Deadline = response.Body[i].Deadline,
                PriorityName = response.Body[i].PriorityName,
                StatusName = response.Body[i].StatusName
            });
        }
        UserTasksDataGrid.ItemsSource = userTasksList;
        DataGridTextColumn titleColumn = new DataGridTextColumn
        {
            Header = "Задача",
            Binding = new Binding("Title")
        };
        UserTasksDataGrid.Columns.Add(titleColumn);
        
        DataGridTextColumn descriptionColumn = new DataGridTextColumn
        {
            Header = "Описание",
            Binding = new Binding("Description")
        };
        UserTasksDataGrid.Columns.Add(descriptionColumn);
        
        DataGridTextColumn createdAtColumn = new DataGridTextColumn
        {
            Header = "Назначено",
            Binding = new Binding("CreatedAt")
        };
        UserTasksDataGrid.Columns.Add(createdAtColumn);
        
        DataGridTextColumn deadlineColumn = new DataGridTextColumn
        {
            Header = "Дедлайн",
            Binding = new Binding("Deadline")
        };
        UserTasksDataGrid.Columns.Add(deadlineColumn);
        
        DataGridTextColumn priorityNameColumn = new DataGridTextColumn
        {
            Header = "Приоритет",
            Binding = new Binding("PriorityName")
        };
        UserTasksDataGrid.Columns.Add(priorityNameColumn);
        
        DataTemplate dataTemplate = new DataTemplate();
        FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
        textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("StatusName")); // Указываем привязку к данным
        dataTemplate.VisualTree = textBlockFactory;
        
        DataGridTemplateColumn statusNameColumn = new DataGridTemplateColumn
        {
            Header = "Статус",
            CellTemplate = dataTemplate
        };
        UserTasksDataGrid.Columns.Add(statusNameColumn);
    }
    
    private void UserWindow_OnClosed(object? sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    
}