   M S B A M L   `   `   `  ����C  ?Sportics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Sportics.ViewModelSportics.ViewModel  5   	       Sportics.View.AdminWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   b ^SharpVectors.Converters.Wpf, Version=1.8.4.2, Culture=neutral, PublicKeyToken=b532964b8548be770svg&http://sharpvectors.codeplex.com/svgc/  5   	   )vm clr-namespace:Sportics.ViewModel  5   	     � Title$  Sportics��5	   	   $	��500��5
   	   $	��800��6   $
��1250��5   	   $	��650��6    � WindowStartupLocation$ CenterScreen=�5   	    � DataContext 5      '   !Sportics.ViewModel.AdminViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   
�������$��60��64    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5       $
��Auto �6#   �� 5"      $��* �6#   .+�5'      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $
��40,0q�6M   $��0��6[   .+�5(      (  "SharpVectors.Converters.SvgViewbox     Source$$ /Resources/Images/dumbbell.svg?�6!   $��40��6I   $��40��6U   �� 5)      $��SPORTICS��6   $��24(�6+   $��Center=�69   $
��10,0q�6T   5*      �� 5-      $ 
Horizontal=�6   $��Center=�62   $��Center=�6O   $��1��6j   .+�5.      �� $��Абонементы��6   $��16(�6.   $��0,0,10,0q�6<   $	��150��6N   �� 5/      $��Расписание��6   $��16(�6.   $
��10,0q�6<   $	��150��6J   �� 50      $��Тренеры��6   $��16(�6+   $
��10,0q�69   $	��150��6G   51      �� 54      $ 
Horizontal=�6   $��Right=�62   $��Center=�6N   $��0,0,10,0q�6i   $��2��6{   .+�55      ��  � Cursor$
 Hand��6   $��10q�6'    J� InputBindings 56      d� 57      	 d� Gesture$	 	LeftClicka�6'   
 �� Command
 6;   �� *OpenLoginCommand+._�59        $( "/Resources/Images/bell-regular.svg?�6%   $��30��6Q   $��30��6]   5:      �� 5<      $
 Hand��6   $��10q�6'    5=      d� 5>      $	 	LeftClicka�6'   
 6;   �� *OpenLoginCommand+._�5@        $. (Resources\Images\circle-user-regular.svg?�6%   $��30��6W   $��30��6c   5A      5B      5C      �� 5F   
   $��1��6   $��0q�6$   .��5G      �� 
�������.+�5H      �� .@�5I      �� -   5        $��Memberships��5I      ��5J      �� 5K      $��Абонементы��6(   .��5N      &   System.Windows.Controls.DataGrid     AutoGenerateColumns .     CanUserAddRows .     Columns 5O      0  *System.Windows.Controls.DataGridTextColumn  5P   "   ,  &System.Windows.Controls.DataGridColumn   Header$ Id��65     5Q   "   $ Название��65     5R   "   $ Название��65     5S   "   $ Название��65     5T   "   $ Название��65     5U   "   $ Название��65   5W      5X      �� 5Z      -   5        $��Сlients��5Z      ��5[      �� 5\      $��Клиенты��6(   .��5_         .   .  ��6V   �� *Users+ 5`        5a   "   $ Id��65   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6A   �� *Id+  5b   "   $
 Name��65    6C   �� *Name+  5c   "   $ Email��65    6D   �� *Email+  5d   "   $ PhoneNumber��65    6J   �� *PhoneNumber+  5e   "   $ Password��65    6G   �� *
Password+  5f   "   $
 Role��65    6C   �� *Role+5h      5i      �� 5k      -   5        $��Coaches��5k      ��5l      �� 5m      $��Тренеры��6(   .��5p         .   .   5q        5r   "   $ Id��65     5s   "   $ Название��65     5t   "   $ Название��65     5u   "   $ Название��65     5v   "   $ Название��65     5w   "   $ Название��65   5y      5z      5{      5|      5}      �� 5�   
   $��2��6   
������� �� Padding$ 10q�66   ._�5�      �� $ 
Horizontal=�6   $��Right=�62   $��0,0,15,0q�6N   .+�5�        $ Resources\Images\Mail.svg?�6!   $��20��6D   $��20��6P   $��0,0,5,0q�6[   �� 5�      $��sportics@gmail.com��6   $��Center=�65     5�      $  Resources\Images\Phone.svg?�6!   $��20��6E   $��20��6Q   $��20,0,5,0q�6\   �� 5�      $��+375 (44) 573-04-02��6   $��Center=�66   5�      5�      5�      5�      