   M S B A M L   `   `   `  ���� J  FFootballMetrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FootballMetricsFootballMetrics      FootballMetrics.oknoMecze   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:FootballMetrics       � Title$  Mecze��$	��300��$	��700��.���  � RowDefinitions �� $��* ��� $
��Auto �.+��� -    ��Name$ 
listaMeczy��$��0��$��5q�����  �� View ��  �� AllowsColumnReorder . .P��� $��ID�� �� Width$
 Auto�� �� DisplayMemberBinding �� *ID+�� $��
Gospodarze��$
 Auto�� �� *
Nazwa_Gosp+�� $��Bramki gospodarzy��$
 Auto�� �� *	Gole_Gosp+�� $��Goście��$
 Auto�� �� *
Nazwa_Gosc+�� $��Bramki gości��$
 Auto�� �� *
Gole_Gosci+�� $
��Data��$
 Auto�� �� *Data+ �� StringFormat$ {0:dd-MM-yyyy}���� $��Turniej��$
 Auto�� �� *	Turniej+� $��1��.+��� -    ��Name$ btnDodaj��$��Dodaj��$��5q��� -   $ 	btnEdytuj��$��Edytuj��$��5q��� -   $ btnUsun��$��Usuń��$��5q�