﻿#pragma checksum "..\..\..\MainPages\AdminShowUsersPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6ACCCCC3EDCCE11C9620276CFC4FFB9512A601F2398C4B5A8EF2641E6695B7B4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
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
using WpfPractice.MainPages;


namespace WpfPractice.MainPages {
    
    
    /// <summary>
    /// AdminShowUsersPage
    /// </summary>
    public partial class AdminShowUsersPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\MainPages\AdminShowUsersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TbFinder1;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\MainPages\AdminShowUsersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TbFinder;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\MainPages\AdminShowUsersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView LbUser;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\MainPages\AdminShowUsersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TbFindeUsers;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\MainPages\AdminShowUsersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnAdd;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\MainPages\AdminShowUsersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnChangeToUserTable;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\MainPages\AdminShowUsersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnChangeToWarehouse;
        
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
            System.Uri resourceLocater = new System.Uri("/WpfPractice;component/mainpages/adminshowuserspage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainPages\AdminShowUsersPage.xaml"
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
            
            #line 9 "..\..\..\MainPages\AdminShowUsersPage.xaml"
            ((WpfPractice.MainPages.AdminShowUsersPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TbFinder1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.TbFinder = ((System.Windows.Controls.TextBox)(target));
            
            #line 27 "..\..\..\MainPages\AdminShowUsersPage.xaml"
            this.TbFinder.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TbFinder_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.LbUser = ((System.Windows.Controls.ListView)(target));
            
            #line 31 "..\..\..\MainPages\AdminShowUsersPage.xaml"
            this.LbUser.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.LbUser_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.TbFindeUsers = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.BtnAdd = ((System.Windows.Controls.Button)(target));
            
            #line 100 "..\..\..\MainPages\AdminShowUsersPage.xaml"
            this.BtnAdd.Click += new System.Windows.RoutedEventHandler(this.BtnAdd_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BtnChangeToUserTable = ((System.Windows.Controls.Button)(target));
            
            #line 102 "..\..\..\MainPages\AdminShowUsersPage.xaml"
            this.BtnChangeToUserTable.Click += new System.Windows.RoutedEventHandler(this.BtnChangeToUserTable_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.BtnChangeToWarehouse = ((System.Windows.Controls.Button)(target));
            
            #line 104 "..\..\..\MainPages\AdminShowUsersPage.xaml"
            this.BtnChangeToWarehouse.Click += new System.Windows.RoutedEventHandler(this.BtnChangeToWarehouse_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
