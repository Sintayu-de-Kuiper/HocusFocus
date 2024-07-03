using System.Windows;
using HocusFocus.UI.MVVM.ViewModels;

namespace HocusFocus.UI.MVVM.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();

        // ConfigurationManager configurationManager = new();
        // ProcessController processController = new(configurationManager);
        // processController.StartMonitoring();
    }
}