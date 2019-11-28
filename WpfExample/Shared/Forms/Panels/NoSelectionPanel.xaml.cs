﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shared.Forms.Panels
{
    /// <summary>
    /// Логика взаимодействия для NoSelectionPanel.xaml
    /// </summary>
    public partial class NoSelectionPanel : UserControl
    {
        public static DependencyProperty TextProperty;
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public NoSelectionPanel()
        {
            InitializeComponent();
        }

        static NoSelectionPanel()
        {
            FrameworkPropertyMetadata metadataText = new FrameworkPropertyMetadata(global::Resources.Properties.Resources.txtForViewingDataSelectRecord);
            TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(NoSelectionPanel), metadataText); 
        }
    }
}
