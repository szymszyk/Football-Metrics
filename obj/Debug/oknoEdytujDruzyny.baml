   M S B A M L   `   `   `  ����J  FFootballMetrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FootballMetricsFootballMetrics  5   	   '    !FootballMetrics.oknoEdytujDruzyny   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FootballMetrics     5   	     � Title$  Drużyna��5   	   $	��300��6   $	��350��6&   ��5	      �� 5
   
    �� ImageSource$ obrazy/tło.jpg��6    j� Stretch$ UniformToFill=�62   .��5      �  � RowDefinitions 5   
   �� 5      $
��0.7* �6   �� 5      $
��0.3* �6   .+�5   
   �� $��0��6   .+�5      �� $��Nazwa drużyny:��6   $��25��6-   $	��200��69   $��0,25,0,0q�6E   �� 5      -   5         ��Name$ txtBox_NazwaDruzyny��5      $��20��63   $	��200��6?   $��0,5,0,0q�6L   �� 5      $��
Kontynent:��6   $��25��6)   $	��200��65   $��0,10,0,0q�6A   �� 5      -   5        $ txtBox_Kontynent��5      $��20��60   $	��200��6<   $��0,5,0,0q�6H   5      �� 5   
    �� Orientation$ 
Horizontal=�6   $��1��6.   $��Center=�6;   .+�5      �� -   5        $��
Zatwierdź��5      $��20��6)   $��80��65   $��0,0,20,0q�6[   �� 5      -   5        $��Anuluj��5      $��20��6&   $��80��62   $��20,0,0,0q�6U   5      5      5      