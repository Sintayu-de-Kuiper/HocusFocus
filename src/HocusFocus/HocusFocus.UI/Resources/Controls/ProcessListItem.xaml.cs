using System.Windows.Controls;namespace HocusFocus.UI.Resources.Controls;public partial class ProcessListItem : UserControl{    public ProcessListItem()    {        InitializeComponent();        DataContext = this;    }    public string ProcessName { get; set; }}