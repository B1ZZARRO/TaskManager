   M S B A M L   `   `   `  ����F  BTaskManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:TaskManagerTaskManager  5   	       TaskManager.AuthWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   +localclr-namespace:TaskManager     5   	     � Title$  
AuthWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $	��140 �6   �� 5      $��20 �6   �� 5      $��20 �6   �� 5      $��20 �6   �� 5      $��10 �6   �� 5      $��20 �6   �� 5      $��10 �6   �� 5      $��20 �6   �� 5      $��10 �6   �� 5      $��20 �6   �� 5      $��20 �6   �� 5      $��30 �6   �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      $	��250 �6   �� 5      .+�5   
   �� -   5        $��tb_login��5   D   $��1��6   $��1��6   $�� 10 0 10  0q�6/   6V   G� 5!   
   -   5        $��tb_password��5!   H   $��3��6   $��1��6#   $�� 10 0 10  0q�63   6^   �� 5#   
   -   5        $��cb_remember��5#   E   $��5��6   $��1��6    $�� 10 0 10  0q�60   $!��Запомнить меня��6X   $��Center=�6q   �� 5%   
   -   5        $��btn_auth��5%   Q   $��7��6   $��1��6   $��	50 0 50 0q�6.   $��
Войти��6A   6a   �� 5'   
   -   5        $��	lbl_error��5'   .   $��11��6   $��1��6   
���  ��$��Center=�6P   5(      5)      