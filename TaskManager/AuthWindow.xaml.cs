using System;
using System.Windows;
using RestSharp;
using TaskManager.API;
using TaskManager.Models;

namespace TaskManager;

public partial class AuthWindow : Window
{
    private RestClient apiClientUser = ServiceBuilder.GetInstance();
    private RememberMe _rememberMe = null;
    
    public AuthWindow()
    {
        InitializeComponent();
        _rememberMe = RememberMe.GetRememberMe();
        _initControlls();
        
    }
    
    private void _initControlls()
    {
        tb_login.Text = _rememberMe.Login;
        tb_password.Password = _rememberMe.Password;
        if (_rememberMe.Check == true) cb_remember.IsChecked = true;
    }

    private void Btn_auth_OnClick(object sender, RoutedEventArgs e)
    {
        if (cb_remember.IsChecked == true)
        {
            _rememberMe.Login = tb_login.Text;
            _rememberMe.Password = tb_password.Password;
            _rememberMe.Check = true;
            _rememberMe.Save();
        }
        else
        {
            _rememberMe.Login = "";
            _rememberMe.Password = "";
            _rememberMe.Check = false;
            _rememberMe.Save();
        }
        Auth();
    }
    
    private void Auth()
    {
        try
        {
            var response = apiClientUser.Post<UserModel>(new RestRequest("/User/auth")
                .AddJsonBody(new
                {
                    login = tb_login.Text,
                    password = tb_password.Password
                }));
            if (response?.Message == "Ok" && response.Body.RoleId == 1)
            {
                Hide();
                new AdminWindow(response.Body.UserId, response.Body.Name, this).Show();
            }
            else if (response?.Message == "Ok" && response.Body.RoleId == 2)
            {
                Hide();
                new UserWindow(response.Body.UserId, response.Body.Name, this).Show();
            }
            else if (response?.Message == "Ok" && response.Body.RoleId == 3)
            {
                Hide();
                new AdminWindow(response.Body.UserId, response.Body.Name, this).Show();
            }
            else if (response?.Message == "Ok" && response.Body.RoleId == 4)
            {
                Hide();
                new StorageWindow(response.Body.UserId, response.Body.Name, this).Show();
            }
            //else if (response?.Message == "Неверный логин или пароль") lbl_error.Content = "Неверный логин или пароль";

        }
        catch (Exception e)
        {
            lbl_error.Content = "Неверный логин или пароль";
        }
    }
}