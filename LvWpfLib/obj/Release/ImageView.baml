   M S B A M L   `   `   `  ���� C  ?LvWpfLib, Version=0.0.1.0, Culture=neutral, PublicKeyToken=null#clr-namespace:LvWpfLibLvWpfLib  $    LvWpfLib.LvImageView.ImageView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     (localclr-namespace:LvWpfLib     -      ContextMenu  � ContextMenu#  �    � 	Resources   TileBack j� TileMode j� Viewbox j� Viewport j� ViewportUnits j� ViewboxUnits L� Drawing � Brush	 � Geometry  Rect

 � Pen >� Brush  Ellipse @� Center @� RadiusX @� RadiusY  Polygon m�Name  FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray ! RadioButton.Static.Background �� Color  RadioButton.Static.Border  OptionMarkFocusVisual $	  RadioButton.MouseOver.Background  
 RadioButton.MouseOver.Border  RadioButton.MouseOver.Glyph # RadioButton.Disabled.Background  RadioButton.Disabled.Border  RadioButton.Disabled.Glyph " RadioButton.Pressed.Background  RadioButton.Pressed.Border  RadioButton.Pressed.Glyph  RadioButton.Static.Glyph  RadioButtonStyleColor �� 
TargetType �� 
TargetType �Name ��Name �� CornerRadius ��Name J� Opacity �� Triggers P� Property P� Value �� 
TargetName��%5
  &	       &	     &	 s    &	 �    &	  0    &	 �    &	 �    &	     &	 "    &		 w    &	
 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	 
    &	     &	 4    &	 I    7  7  7  7  7 	 7 
 7  7  7  7  L� $
 Tile=�$ 	0,0,16,16�$ 	0,0,16,16�$ Absolute=�$ Absolute=� I� .���  �#333333+	 ��        �     $� � $�  $� � $�     	�  �#222222+	 ��        �     z � z  z � z     	�  �#222222+	 �� z  z � z  $� � $�  $� � $�  z 	L� $
 None=� � +	 �� 	=  	= � 	=  -1� -1 -1� -1 	= 	
 >� 
 �����L� $
 None=� � 	 @� $ 12,12&�$ 6Q�$ 6Q�
 >� 
 �����L� $
 None=� � =	 ����j  z ���-  � �@KL @f�@f $� � -1��� � $� ��[ 	
 >� 
 ������� .@�m� $ EditMode��-   $��编辑模式���� m� $ DrawRect��-   $��绘制矩形��m� $ DrawLine��-   $��绘制线段���� m� $ AutoFit��-   $��自适应窗口��m� $ DisableAutoFit��-   $��取消适应窗口���� m� $ DeleteSelectedElement��-   $��删除所选图形��m� $ DeleteAllElement��-   $��删除全部图形���� .���  � �� �� .R��� $��2q� . #��� ��$��1��$	 1 2R��� $ 	#FFFFFFFF���� $ 	#FF707070���� .���  � �� �� .R��� $��14,0,0,0q� . #��� ��$��1��$	 1 2R��� $ 	#FFF3F9FF���� $ 	#FF5593FF���� $ 	#FF212121���� $ 	#FFE6E6E6���� $ 	#FFBCBCBC���� $ 	#FF707070���� $ 	#FFD9ECFF���� $ 	#FF3C77DD���� $ 	#FF212121���� $ 	#FF212121���� " 	�.���  �@�FocusVisualStyle8   ��  � ��8  ��  � ��8  ��  � ��# � ����  � ��$ 1q���  � �� �� " 	�.R�� $ templateRoot��
������  . .+��� $ radioButtonBorder��#��z��$��0q�#��z��$ 2��#��z��#��z��$��20��$��20��._�� $ markGrid��$��0q�$
��Auto��$
��Auto��.+��� $ 
optionMark��#��z��$��6��$��6��$ 0Q�$
��Auto��$
��Auto��$��2�� P�  � �� . . ��� 8   �@�FocusVisualStyle��  � ��$ 4,-1,0,0q�P�  �@J�IsMouseOver . . ��� $ radioButtonBorder��8   � ���� $ radioButtonBorder��8   � ��P�  � {� .  . ��� $ radioButtonBorder��8   � ���� $ radioButtonBorder��8   � ��P�  � �� . . ��� $ radioButtonBorder��8   � ���� $ radioButtonBorder��8 	  � ��P�  �@d�	IsChecked$
 trueY�. ��� $ 
optionMark�� �@J�Opacity$ 1Q�P�  Z�  �@d�	IsChecked. ��� $ 
optionMark�� �@J�Opacity$
 0.56Q�.���   � ColumnDefinitions  �� $��40 ��� $
��717* ��� $	��43* �! � RowDefinitions! �� $��32 ��� $	��33* ��� $��5* �.+��� $��Center=�$��3��$��36��$��74�����	#660097FF���	#FF0097FF$��1q�$��Center=�" �� Padding$
" 0,10q�._��� .+��� -	   # ��Name$# ibRect��$��24��$��24��#���  $	��0,2q���Z� �� -
   $# 	ibPolygon��$��24��$��24��#���  $	��0,2q���Z� �� -   $# 	ibEllipse��$��24��$��24��#���  $	��0,2q�$y�	Collapsed=���Z� �� $�� ��$��Center=�$��Center=�$��2q�$��10(�	� -   $ 	�Name$$ rbColorGreen��% d� 	IsChecked$
% TrueY����	#FF19FF02$��24��$��24��$��Center=�$��Center=�$��0q�$��Center=�$��Center=�#���  
�������$��0q�	� -   $$ 
rbColorRed��$��Center=�$��24��$��Center=�$��Center=�$��Center=�$��24��#���  
���  ��
�������	� -   $$ rbColorBlue��#���  $��Center=�$��Center=�$��24��$��24��$��Center=�$��Center=����	#FF0097FF
�������	� -   $$ rbColorYellow��#���  $��Center=�$��Center=�$��24��$��24��$��Center=�$��Center=�
��� ���
�������	� -   $$ rbColorWhite��#���  $��Center=�$��Center=�$��24��$��24��$��Center=�$��Center=�
�������
���   �	� -   $$ rbColorPurple��#���  $��Center=�$��Center=�$��24��$��24��$��Center=�$��Center=����	#FFC500FF
�������