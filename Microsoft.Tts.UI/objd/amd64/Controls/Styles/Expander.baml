   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   ExpanderToggleButton �� 
TargetType �� CornerRadius  NormalBorderBrush  
GlyphBrush �� Triggers P� Property P� Value �� 
TargetName  	DarkBrush �� Value	 �� Property  PressedBrush  DisabledBackgroundBrush  DisabledBorderBrush  DisabledForegroundBrush  ExpanderStyle
 �� 
TargetType � RowDefinitions �Name 	 
LightBrush � ColumnDefinitions � OverridesDefaultStyle 
 NormalBrush d� 	IsChecked �� Path �� Mode �� RelativeSource  WindowBackgroundBrush  SolidBorderBrush �� ElementName�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     6    :x,http://schemas.microsoft.com/winfx/2006/xaml      5      ResourceDictionary_1��  5      �� 5      ResourceDictionary_2��$ Shared.xaml?�66   %3
  &	        7  7  7  7  7  7  7  7  &	 $    7 	 7  7   7 
 7  7  7  7  7  �� 5
      ControlTemplate_1��" d�6K   .R�5      �� $��Border��5      
Border��$ 2,0,0,0��5      
������ 8��  $��0,0,1,0q�5      ._�5      F� $��Arrow��5      	Arrow��8�� $��Center=�5   	   $��Center=�5   	   "����        � 	=  	= � z     	5   	   5       5      P� 5      	Trigger_1�� �@J�IsMouseOver . . �5   
   �� Setter_1��$ Border��6"   8  	 � ��5      5   	   P� 5      	Trigger_2�� � �� . . �5   
   �� Setter_2��$ Border��6"   8  	 � ��5      5    	   P� 5!      	Trigger_3�� �@d�	IsChecked$
 trueY�67   . �5"   
   �� Setter_3��$ Arrow��6"   	 � ��" ��     	= � 	=     � z  	= 	5#      5$   	   P� 5%      	Trigger_4�� � {� .  . �5&   
   �� Setter_4��$ Border��6"   8  	 � ��5'      �� 5(   
   Setter_5��$ Border��6"   8  	 � ��5)      �� 5*   
   Setter_6��8  	 � ��5+      �� 5,   
   Setter_7��$ Arrow��6"   8  	 � ��5-      5.   	   50      �� 52      Style_1��"
 9�60   .�53      �� Setter_8��	 � �� 54      �� 55   
   ControlTemplate_2��" 9�64   .R�56      � 
Grid_1�� 57      �� 58      RowDefinition_1��$
��Auto �66   �� 59      $ 
ContentRow��61   
ContentRow��$��0 �6C   .+�5;      �� $��Border��5<       Border_1��$��0��5=      8��  8�� $��1q�5@      $ 2,2,0,0��5A      ._�5B      � 
Grid_2�� 5C      �� 5D      ColumnDefinition_1��$��20 �6@   �� 5E      ColumnDefinition_2��$��* �6@   .+�5G      d� ToggleButton_1�� . 8�� 8��  5H   *   �� $ 
IsExpanded�$ TwoWay=� �� *TemplatedParent+5L      �� 5M      ContentPresenter_1��$��1��5N   .   $��4q�5O      $��Header��5P      ��. 5Q      5R      5S      �� 5T      $��Content��5U      Content��$��1��5V      8�� 8�� $��1,0,1,1q�5Y      $ 0,0,2,2��5Z      ._�5[      �� ContentPresenter_2��#��z��6<   5\      5]       5^      P� 5_      	Trigger_5�� �@9�
IsExpanded . . �5`      �� Setter_9��$ 
ContentRow��6(    5a      �� $ Content��$ DesiredHeight�	 � ��5b      P� 5c      	Trigger_6�� � {� .  . �5d      �� 	Setter_10��$ Border��6)   8  	 � ��5e      �� 5f      	Setter_11��$ Border��6)   8  	 � ��5g      �� 5h      	Setter_12��8  	 � ��5i      5j      5m      5o      5p      5r      