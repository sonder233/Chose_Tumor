   M S B A M L   `   `   `  ����E  AChoseTumor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:ChoseTumor
ChoseTumor  5   	       ChoseTumor.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:ChoseTumor     5   	     � Title$  
MainWindow��5   	   $	��734��6   $
��1000��6)   .��5	      � $��0,0,-0.4,36.4q�6    � ColumnDefinitions 5
   
   �� 5      $��2* �6   �� 5      $
��395* �6   .+�5   
   �� -   5        $��imageListBox��5      $��20.2,10,9.6,82q�5   v   $��1��6�   ��5   &   �� ��5      ��  �� ElementName$ listBoxShowImages�� �� Path$ Width� �� Mode$ OneWay=���5      �� 5      .R�5      A�  A� Columns$ 6��6"   5      ��5      �� 5      .R�5      ��  �� Orientation$ Vertical=�6!   .+�5      �� $��3q�6$   $	��100��6E   ��6/   �� *Name+6E   �� 5      $��3q�6    $	��100��6C   $	��100��6O   ��6+   �� *Path+6O   5      5      5      �� 5   
   -   5         ��Name$ 
maskButton��5      $��遮罩��6%   $��1��62   $
��Left=�6B   $��477.2,641,0,0q�6]   $	��Top=�6t   $��75��6�   $��27��6�   �� 5   
   -   5        $��下一个人��5      $��1��6    $
��Left=�60   $��577.2,640,0,0q�6K   $	��Top=�6b   $��75��6z   6�   �� 5    
   -   5         ��Name$ comboBox��5       $��1��6%   $
��Left=�65   $��10.2,590,0,0q�6P   $	��Top=�6f   $	��581��6~   	 �� SelectedIndex$	 0��6�   �� 5!   
   -   5        $��Button��5!      $��1��6"   $
��Left=�62   $��904.2,598,0,0q�6M   $	��Top=�6d   $��75��6|   6�   �� 5"   
   -   5        
 ��Name$
 
curPatient��5"      $��Label��6$   $��1��64   $
��Left=�6D   $��0.2,612,0,0q�6_   $	��Top=�6t   $	��524��6�   �� 5#   
   -   5        $��10张��5#      $��1��6   $
��Left=�6/   $��368.2,641,0,0.4q�6J   $��75��6c   6n   5$      5%      