﻿#pragma checksum "..\..\UpdateBookModal.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9C2A928F6B1BBE231A32DDB2543D5C38BB83524A014A558980B53DF64089DD54"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using _RemoteDbAccess;


namespace _RemoteDbAccess {
    
    
    /// <summary>
    /// UpdateBookModal
    /// </summary>
    public partial class UpdateBookModal : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\UpdateBookModal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox title;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\UpdateBookModal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox description;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\UpdateBookModal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button contentchoose;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\UpdateBookModal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button coverchoose;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\UpdateBookModal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox authors;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\UpdateBookModal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tempauthor;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\UpdateBookModal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addauthor;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\UpdateBookModal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button delauthor;
        
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
            System.Uri resourceLocater = new System.Uri("/_RemoteDbAccess;component/updatebookmodal.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\UpdateBookModal.xaml"
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
            
            #line 27 "..\..\UpdateBookModal.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 28 "..\..\UpdateBookModal.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.title = ((System.Windows.Controls.TextBox)(target));
            
            #line 30 "..\..\UpdateBookModal.xaml"
            this.title.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.title_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.description = ((System.Windows.Controls.TextBox)(target));
            
            #line 32 "..\..\UpdateBookModal.xaml"
            this.description.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.description_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.contentchoose = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\UpdateBookModal.xaml"
            this.contentchoose.Click += new System.Windows.RoutedEventHandler(this.contentchoose_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.coverchoose = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\UpdateBookModal.xaml"
            this.coverchoose.Click += new System.Windows.RoutedEventHandler(this.coverchoose_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.authors = ((System.Windows.Controls.ListBox)(target));
            return;
            case 8:
            this.tempauthor = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.addauthor = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\UpdateBookModal.xaml"
            this.addauthor.Click += new System.Windows.RoutedEventHandler(this.addauthor_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.delauthor = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\UpdateBookModal.xaml"
            this.delauthor.Click += new System.Windows.RoutedEventHandler(this.delauthor_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

