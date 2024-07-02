﻿using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HocusFocus.Core;

namespace HocusFocus.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ConfigurationManager configurationManager = new ConfigurationManager();
            ProcessController processController = new(configurationManager);

            processController.StartMonitoring();
            InitializeComponent();
        }
    }
}