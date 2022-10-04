﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5121D182557831644369A439055EB7A1243E068A"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using ColorPicker;
using SharpGL.WPF;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace IntroductionGL {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SharpGL.WPF.OpenGLControl openGLControl1;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas canvas;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxCollPrimitives;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxPrimitives;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeletePrimitive;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteCollPrimitives;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider SliderLineWidth;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TextBoxLineWidth;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxTypeLine;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ColorPicker.StandardColorPicker ColorPicker;
        
        #line default
        #line hidden
        
        
        #line 131 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxPoints;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/IntroductionGL;V1.0.0.0;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 13 "..\..\..\MainWindow.xaml"
            ((IntroductionGL.MainWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.Grid_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.openGLControl1 = ((SharpGL.WPF.OpenGLControl)(target));
            
            #line 22 "..\..\..\MainWindow.xaml"
            this.openGLControl1.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.OpenGLControl_MouseDown);
            
            #line default
            #line hidden
            
            #line 25 "..\..\..\MainWindow.xaml"
            this.openGLControl1.OpenGLDraw += new SharpGL.WPF.OpenGLRoutedEventHandler(this.openGLControl1_OpenGLDraw);
            
            #line default
            #line hidden
            
            #line 26 "..\..\..\MainWindow.xaml"
            this.openGLControl1.Resized += new SharpGL.WPF.OpenGLRoutedEventHandler(this.openGLControl1_Resized);
            
            #line default
            #line hidden
            
            #line 27 "..\..\..\MainWindow.xaml"
            this.openGLControl1.OpenGLInitialized += new SharpGL.WPF.OpenGLRoutedEventHandler(this.openGLControl1_OpenGLInitialized);
            
            #line default
            #line hidden
            return;
            case 3:
            this.canvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 4:
            this.ComboBoxCollPrimitives = ((System.Windows.Controls.ComboBox)(target));
            
            #line 33 "..\..\..\MainWindow.xaml"
            this.ComboBoxCollPrimitives.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBoxCollPrimitives_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ComboBoxPrimitives = ((System.Windows.Controls.ComboBox)(target));
            
            #line 40 "..\..\..\MainWindow.xaml"
            this.ComboBoxPrimitives.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBoxPrimitives_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.DeletePrimitive = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\MainWindow.xaml"
            this.DeletePrimitive.Click += new System.Windows.RoutedEventHandler(this.DeletePrimitive_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.DeleteCollPrimitives = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\MainWindow.xaml"
            this.DeleteCollPrimitives.Click += new System.Windows.RoutedEventHandler(this.DeleteCollPrimitives_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.SliderLineWidth = ((System.Windows.Controls.Slider)(target));
            
            #line 99 "..\..\..\MainWindow.xaml"
            this.SliderLineWidth.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.SliderLineWidth_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.TextBoxLineWidth = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.ComboBoxTypeLine = ((System.Windows.Controls.ComboBox)(target));
            
            #line 113 "..\..\..\MainWindow.xaml"
            this.ComboBoxTypeLine.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBoxTypeLine_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.ColorPicker = ((ColorPicker.StandardColorPicker)(target));
            
            #line 124 "..\..\..\MainWindow.xaml"
            this.ColorPicker.ColorChanged += new System.Windows.RoutedEventHandler(this.ColorPicker_ColorChanged);
            
            #line default
            #line hidden
            return;
            case 12:
            this.ComboBoxPoints = ((System.Windows.Controls.ComboBox)(target));
            
            #line 132 "..\..\..\MainWindow.xaml"
            this.ComboBoxPoints.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ComboBoxPoints_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

