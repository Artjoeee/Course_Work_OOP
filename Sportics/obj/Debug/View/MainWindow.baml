   M S B A M L   `   `   `  ����C  ?Sportics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Sportics.ViewModelSportics.ViewModel  5   	       Sportics.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   b ^SharpVectors.Converters.Wpf, Version=1.8.4.2, Culture=neutral, PublicKeyToken=b532964b8548be770svg&http://sharpvectors.codeplex.com/svgc/  5   	   )vm clr-namespace:Sportics.ViewModel  5   	     � Title$  Sportics��5	   	   $	��500��5
   	   $	��800��6   $
��1250��5   	   $	��650��6    � WindowStartupLocation$ CenterScreen=�5   	    � DataContext 5      &    Sportics.ViewModel.MainViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   
�������$��60��64    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5!      $
��Auto �6#   �� 5#      $��* �6#   .+�5(      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $
��40,0q�6M   $��0��6[   .+�5)      (  "SharpVectors.Converters.SvgViewbox     Source$$ /Resources/Images/dumbbell.svg?�6!   $��40��6I   $��40��6U   �� 5*      $��SPORTICS��6   $��24(�6+   $��Center=�69   $
��10,0q�6T   5+      �� 5.      $ 
Horizontal=�6   $��Center=�62   $��Center=�6O   $��1��6j   .+�5/      �� $��Абонементы��6   $��16(�6.   $��0,0,10,0q�6<   $	��150��6N   �� 50      $��Расписание��6   $��16(�6.   $
��10,0q�6<   $	��150��6J   �� 51      $��Тренеры��6   $��16(�6+   $
��10,0q�69   $	��150��6G   52      �� 55      $ 
Horizontal=�6   $��Right=�62   $��Center=�6N   $��0,0,10,0q�6i   $��2��6{   .+�56      ��  � Cursor$
 Hand��6   $��10q�6'    J� InputBindings 57      d� 58      	 d� Gesture$	 	LeftClicka�6'   
 �� Command
 6;   �� *OpenLoginCommand+._�5:        $( "/Resources/Images/bell-regular.svg?�6%   $��30��6Q   $��30��6]   5;      �� 5=      $
 Hand��6   $��10q�6'    5>      d� 5?      $	 	LeftClicka�6'   
 6;   �� *OpenLoginCommand+._�5A        $. (Resources\Images\circle-user-regular.svg?�6%   $��30��6W   $��30��6c   5B      5C      5D      �� 5G   
   $��1��6   $��0q�6$   .��5H      �� 
�������.+�5I      �� $)��#Предложения недели��6   $��32(�65   $��Center=�6C   $
��0,20q�6`   � 5K      $��Center=�6   $
��0,20q�69    � 	ItemWidth$	 270��6G    � 
ItemHeight$
 Auto��6W   .+�5M      �� $	��250��6   
�������$��0,0,0,0q�6A   ._�5N      �� $��10q�6%   .+�5O      � $	��120��6#   
�������$��5q�6G   �� 5P      $��Месяц (24/7)��6(   $��16(�6<   $��SemiBold"�6J    �� TextAlignment$ Center=�6`   $
��0,10q�6w   �� 5Q      $��
90 руб.��6(   $��16(�67   $
��Bold"�6E   $ Center=�6W   $	��0,5q�6n   �� 5R      $��Подробнее��6%   $��Center=�69   $
��0,10q�6V   5S      5T      �� 5W      $	��250��6   
�������$��0,0,0,0q�6A   ._�5X      �� $��10q�6%   .+�5Y      � $	��120��6#   
�������$��5q�6G   �� 5Z      $��Месяц (8:00 - 16:00)��6(   $��16(�6D   $��SemiBold"�6R   $ Center=�6h   $
��0,10q�6   �� 5[      $��
50 руб.��6(   $��16(�67   $
��Bold"�6E   $ Center=�6W   $	��0,5q�6n   �� 5\      $��Подробнее��6%   $��Center=�69   $
��0,10q�6V   5]      5^      5_      5`      5a      �� 5d   
   $��2��6   
������� �� Padding$ 10q�66   ._�5e      �� $ 
Horizontal=�6   $��Right=�62   $��0,0,15,0q�6N   .+�5f        $ Resources\Images\Mail.svg?�6!   $��20��6D   $��20��6P   $��0,0,5,0q�6[   �� 5g      $��sportics@gmail.com��6   $��Center=�65     5h      $  Resources\Images\Phone.svg?�6!   $��20��6E   $��20��6Q   $��20,0,5,0q�6\   �� 5i      $��+375 (44) 573-04-02��6   $��Center=�66   5j      5k      5l      5m      