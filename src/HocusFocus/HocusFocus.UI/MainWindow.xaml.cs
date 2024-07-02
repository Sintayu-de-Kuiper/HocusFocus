using System.Windows;
using HocusFocus.UI.MVVM.ViewModels;

namespace HocusFocus.UI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}