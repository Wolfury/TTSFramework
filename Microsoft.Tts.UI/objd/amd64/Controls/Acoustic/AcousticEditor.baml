   M S B A M L   `   `   `  ����K  GMicrosoft.Tts.UI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:Microsoft.Tts.UI.Controls.Acoustic"Microsoft.Tts.UI.Controls.Acoustic  5      [2clr-namespace:Microsoft.Tts.UI.Controls.Trajectory$Microsoft.Tts.UI.Controls.Trajectory  5      7    1Microsoft.Tts.UI.Controls.Acoustic.AcousticEditor   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      EAcoustic0clr-namespace:Microsoft.Tts.UI.Controls.Acoustic     5      OcustomTrajectory2clr-namespace:Microsoft.Tts.UI.Controls.Trajectory     5      UserControl_1��$	��300��5      $	��400��6   .��5	      � 
Grid_1��  � ColumnDefinitions  5
   
   �� 5      ColumnDefinition_1��$��25 �6:   �� 5      -   5         ��Name$ 
_midColumn��5   :   ColumnDefinition_2���� 5      ColumnDefinition_3��$��50 �6:    � RowDefinitions 5   
   �� 5      RowDefinition_7��$��20 �64   �� 5      RowDefinition_8��$��20 �64   �� 5      RowDefinition_1��$	��50* �64   �� 5      RowDefinition_2��$��5 �64   �� 5      RowDefinition_3��$	��50* �64   �� 5      RowDefinition_4��$��5 �64   �� 5      RowDefinition_5��$	��50* �64   �� 5      RowDefinition_6��$��25 �64   .+�5   
   �� -   5        $��gridSplitter1��5   l   gridSplitter1��$��3��6-   $��3��6:   $��Stretch=�6N   $��Stretch=�6�   
�������
����ح�$��1q�6�    J� SnapsToDevicePixels . 6�   �� 5   
   -   5        $��gridSplitter2��5   l   gridSplitter2��$��5��6-   $��3��6:   $��Stretch=�6N   $��Stretch=�6�   
�������
����ح�$��1q�6�    . 6�   �� 5   
   -   5        $��
textBlock1��5   =   
textBlock1��$��0��6    $��2��60   $��F0��5      $��12(�5       J� RenderTransformOrigin$ 0.5,0.5&�5       J� IsHitTestVisible .  $��Center=�5!      $��Center=�5"      z�5#      �� 5$      RotateTransform_2�� �� Angle$	 -90Q�6<   5&      �� 5'   
   -   5        $��
textBlock2��5(   =   
textBlock2��$��0��6    $��4��60   $
��Gain��5)      $��12(�5*      $ 0.5,0.5&�5+       .  $��Center=�5-      $��Center=�5.      z�5/      �� 50      RotateTransform_3��$	 -90Q�6<   52      �� 53   
   -   5        $��
textBlock3��54   =   
textBlock3��$��0��6    $��6��60   $
��Wave��55      $��12(�56      $ 0.5,0.5&�57       .  $��Center=�59      $��Center=�5:      z�5;      �� 5<      RotateTransform_4��$	 -90Q�6<   5>      7   1Microsoft.Tts.UI.Controls.Trajectory.SegmentGraph  5?   
   -   5          Name$ _wordSegmentGraph��5A      _wordSegmentGraph��$��1��5B      $��0��6   $��20��5D      ��5C      ��  �� Path$ ActualWidth�	 �� ElementName$	 	_f0Editor��5D        5E   
   -   5        $ _phoneSegmentGraph��5G      _phoneSegmentGraph��$��1��5H      $��1��6   $��20��5J      ��5I      �� $ ActualWidth�$	 	_f0Editor��5J      1   +Microsoft.Tts.UI.Controls.Acoustic.F0Editor  5K   
   -	   5        
  Name$
 	_f0Editor��5M      	_f0Editor��$��2��5L      $��1��6,   5N      4   .Microsoft.Tts.UI.Controls.Trajectory.AxisRuler  5O   
   -
   5          Name$ _f0YAxis��5P      _f0YAxis��$��2��60   $��2��6=      IsBoxToValue .     IsRulerLeft .  5S      3   -Microsoft.Tts.UI.Controls.Acoustic.GainEditor  5T   
   -   5          Name$ _gainEditor��5V      _gainEditor��$��4��5U   !   $��1��6.   5W        5X   
   -   5        $ 
_gainYAxis��5Y       
_gainYAxis��$��4��64   $��2��6A    .   .  5\      7   1Microsoft.Tts.UI.Controls.Acoustic.WaveFormViewer  5]   
   -   5          Name$ 	_waveForm��5_      	_waveForm��$��6��5^      $��1��6,   5`        5a   
   -   5        $ _waveformYAxis��5b   $   _waveformYAxis��$��6��6<   $��2��6I    .   .  5e      8   2Microsoft.Tts.UI.Controls.Trajectory.DurationGraph  5f   
   -   5          Name$ 
_durations��5i      
_durations��$��1��5g       $��1��6-   $��6��5h      5j      <   6Microsoft.Tts.UI.Controls.Trajectory.TimeAxisScrollbar  5k   
   -   5          Name$ _timeAxisScallbar��5l   '   _timeAxisScallbar��$��25��5n      $��1��5o      $��8��5p      |�. ��5m      �� $ ActualWidth�$	 	_f0Editor��5r      6   0Microsoft.Tts.UI.Controls.Trajectory.ZoomControl  5s   
   -   5          Name$ _zoomControl��5s   w   _zoomControl��$��2��6<   $��7��6L   $��Stretch=�6Y   $��Stretch=�6�   5t      5u      