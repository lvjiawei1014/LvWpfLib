﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "FEB3164520D8EAAF470299C018C6D29D9E3238CDA8D5B9E742197CF67BB9AF7B"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using LvWpfDemo;
using Microsoft.Windows.Themes;
using Ncer;
using Ncer.UI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace LvWpfDemo {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 209 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEdit;
        
        #line default
        #line hidden
        
        
        #line 214 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Ncer.UI.ImageView imageView;
        
        #line default
        #line hidden
        
        
        #line 216 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDrawRect;
        
        #line default
        #line hidden
        
        
        #line 217 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDrawEllipse;
        
        #line default
        #line hidden
        
        
        #line 218 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDrawCircle;
        
        #line default
        #line hidden
        
        
        #line 223 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Ncer.UI.ColormapBar colorBar;
        
        #line default
        #line hidden
        
        
        #line 224 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnJet;
        
        #line default
        #line hidden
        
        
        #line 225 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGray;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/LvWpfDemo;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\MainWindow.xaml"
            ((LvWpfDemo.MainWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnEdit = ((System.Windows.Controls.Button)(target));
            
            #line 209 "..\..\MainWindow.xaml"
            this.btnEdit.Click += new System.Windows.RoutedEventHandler(this.BtnEdit_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.imageView = ((Ncer.UI.ImageView)(target));
            return;
            case 4:
            
            #line 215 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btn2_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnDrawRect = ((System.Windows.Controls.Button)(target));
            
            #line 216 "..\..\MainWindow.xaml"
            this.btnDrawRect.Click += new System.Windows.RoutedEventHandler(this.BtnDrawRect_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnDrawEllipse = ((System.Windows.Controls.Button)(target));
            
            #line 217 "..\..\MainWindow.xaml"
            this.btnDrawEllipse.Click += new System.Windows.RoutedEventHandler(this.btnDrawEllipse_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnDrawCircle = ((System.Windows.Controls.Button)(target));
            
            #line 218 "..\..\MainWindow.xaml"
            this.btnDrawCircle.Click += new System.Windows.RoutedEventHandler(this.btnDrawCircle_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.colorBar = ((Ncer.UI.ColormapBar)(target));
            return;
            case 9:
            this.btnJet = ((System.Windows.Controls.Button)(target));
            
            #line 224 "..\..\MainWindow.xaml"
            this.btnJet.Click += new System.Windows.RoutedEventHandler(this.btnJet_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnGray = ((System.Windows.Controls.Button)(target));
            
            #line 225 "..\..\MainWindow.xaml"
            this.btnGray.Click += new System.Windows.RoutedEventHandler(this.btnGray_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

