   M S B A M L   `   `   `  ����J  FImageServiceGUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:ImageServiceGUI.ControlsImageServiceGUI.Controls  5      '    !ImageServiceGUI.Controls.Settings   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:ImageServiceGUI.Controls     5      .��5	      �� .+�5
   
   �� $��Center=�6   $	��Top=�6-   .��6B   
Settings6L   � 5   
   $��14(�6     � RowDefinitions  5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6     � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $��* �6#   .+�5      ��  �� Padding$ 3q�6   $��3q�6$   �� 60   .�65   Output Directory:6H   6O   �� 5      -   5         ��Name$ OutPutDirectoryTxt��5      $��1��64   $ 3q�6D   $��3q�6P   ��6[   �� *VM_OutputDirectory+6�   �� 5      $��1��6   $ 3q�6%   $��3q�61   �� 6=   .�6B   Source Name:6P   6W   �� 5      -   5        $ SourceNameTxt��5      $��1��6/   $��1��6<   $ 3q�6L   $��3q�6X   ��6c   �� *VM_SourceName+6�   �� 5      $ 3q�6   $��3q�6$   $��2��6/   �� 6=   .�6B   	Log Name:6M   6T   �� 5      -   5        $ 
LogNameTxt��5      $��2��6,   $��1��69   $ 3q�6I   $��3q�6U   ��6`   �� *
VM_LogName+6~   �� 5      $ 3q�6   $��3q�6$   $��3��6/   �� 6=   .�6B   Thumbnail Size:6S   6Z   �� 5      -   5        $ ThumbnailSizeTxt��5      $��3��62   $��1��6?   $ 3q�6O   $��3q�6[   ��6f   �� *VM_ThumbnailSize+6�   5      �� 5    
   $��3q�6   $��3q�6   $��Center=�6(   $��Stretch=�6L   ��6j   �� *	SubRemove+.��6�   Remove6�   �� 5!   
   -   5        $��	pathsList��5!   B   $��3q�6   $
��Auto��6    �� SelectionMode$ Single=�6+   $��Stretch=�6S   ��6x   �� *VM_DirectoryHandlerPaths+ �� SelectedItem 6�   �� *VM_SelectedDirectoryPath+5"      5#      5$      