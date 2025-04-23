using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RestSharp;
using TaskManager.API;
using TaskManager.Models;

namespace TaskManager;

public partial class StorageWindow : Window
{
    private RestClient apiClientUser = ServiceBuilder.GetInstance();
    
    public StorageWindow(int sId, string sName, Window parent)
    {
        InitializeComponent();
        
        var response = apiClientUser.Get<StorageItemsModel>(new RestRequest($"/Storage/GetAllItems"));
        List<StorageItemBodyModel> itemsList = new List<StorageItemBodyModel>();
        for (int i = 0; i < response.Body.Count; i++)
        {
            itemsList.Add(new StorageItemBodyModel()
            {
                ItemId = response.Body[i].ItemId,
                ItemName = response.Body[i].ItemName,
                ItemType = response.Body[i].ItemType,
                Quantity = response.Body[i].Quantity
            });
        }
        StorageDataGrid.ItemsSource = itemsList;
        
        DataGridTextColumn itemIdColumn = new DataGridTextColumn
        {
            Header = "ID", Binding = new Binding("ItemId")
        };
        StorageDataGrid.Columns.Add(itemIdColumn);

        DataGridTextColumn itemNameColumn = new DataGridTextColumn
        {
            Header = "Название", Binding = new Binding("ItemName")
        };
        StorageDataGrid.Columns.Add(itemNameColumn);

        DataGridTextColumn itemTypeColumn = new DataGridTextColumn
        {
            Header = "Тип", Binding = new Binding("ItemType")
        };
        StorageDataGrid.Columns.Add(itemTypeColumn);

        DataGridTextColumn quantityColumn = new DataGridTextColumn
        {
            Header = "Количество", Binding = new Binding("Quantity")
        };
        StorageDataGrid.Columns.Add(quantityColumn);
    }

    private void StorageWindow_OnClosed(object? sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }
}