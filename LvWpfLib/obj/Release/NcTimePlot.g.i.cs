﻿#pragma checksum "..\..\NcTimePlot.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D7D01933E45D520194D185CB4FC43C7B6BF97922322B11572F9B5FFDC91FF3EF"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using LvWpfLib;
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


namespace LvWpfLib {
    
    
    /// <summary>
    /// NcTimePlot
    /// </summary>
    public partial class NcTimePlot : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\NcTimePlot.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LvWpfLib.NcTimePlot timePlot;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\NcTimePlot.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid root;
        
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
            System.Uri resourceLocater = new System.Uri("/LvWpfLib;component/nctimeplot.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\NcTimePlot.xaml"
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
            this.timePlot = ((LvWpfLib.NcTimePlot)(target));
            
            #line 8 "..\..\NcTimePlot.xaml"
            this.timePlot.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControl_MouseDown);
            
            #line default
            #line hidden
            
            #line 8 "..\..\NcTimePlot.xaml"
            this.timePlot.MouseMove += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseMove);
            
            #line default
            #line hidden
            
            #line 8 "..\..\NcTimePlot.xaml"
            this.timePlot.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(this.UserControl_MouseWheel);
            
            #line default
            #line hidden
            
            #line 8 "..\..\NcTimePlot.xaml"
            this.timePlot.Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\NcTimePlot.xaml"
            this.timePlot.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.timePlot_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 8 "..\..\NcTimePlot.xaml"
            this.timePlot.SizeChanged += new System.Windows.SizeChangedEventHandler(this.UserControl_SizeChanged);
            
            #line default
            #line hidden
            
            #line 9 "..\..\NcTimePlot.xaml"
            this.timePlot.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.timePlot_MouseUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.root = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

