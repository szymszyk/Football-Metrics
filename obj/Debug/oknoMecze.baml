   M S B A M L   `   `   `  ����J  FFootballMetrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FootballMetricsFootballMetrics  5   	       FootballMetrics.oknoMecze   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:FootballMetrics     5   	     � Title$  Mecze��5   	   $	��300��6   $	��700��6$   ��5
      �� 5   
    �� ImageSource$ obrazy/tło.jpg��6    j� Stretch$ UniformToFill=�62   .��5      �  � RowDefinitions 5      �� 5   
   $��* �6   �� 5   
   $
��Auto �6   .+�5      �� -   5         ��Name$ 
listaMeczy��5      $��0��5      $��5q�6   ��5   '   ��  �� View 5   
   �� 5       �� AllowsColumnReorder . .P�5      �� $��ID��6!    �� Width$
 Auto��6-    �� DisplayMemberBinding 6:   �� *ID+�� 5      $��
Gospodarze��6!   $
 Auto��65    6B   �� *
Nazwa_Gosp+�� 5      $��Bramki gospodarzy��6!   $
 Auto��6<    6I   �� *	Gole_Gosp+�� 5      $��Goście��6!   $
 Auto��61    6>   �� *
Nazwa_Gosc+�� 5      $��Bramki gości��6!   $
 Auto��67    6D   �� *
Gole_Gosci+�� 5      $
��Data��6!   $
 Auto��6/    6<   �� *Data+	 �� StringFormat$	 {0:dd-MM-yyyy}���� 5      $��Turniej��6!   $
 Auto��62    6?   �� *	Turniej+5      5       � 5!      $��1��6   .+�5"   
   �� -   5        
 ��Name$
 btnDodaj��5"      $��Dodaj��6#   $��5q�63   6>   �� 5#   
   -   5        $
 	btnEdytuj��5#      $��Edytuj��6$   $��5q�65   6@   �� 5$   
   -   5        $
 btnUsun��5$      $��Usuń��6"   $��5q�61   6<   5%      5(      5)      