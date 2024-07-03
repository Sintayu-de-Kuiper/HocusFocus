using System.Collections.ObjectModel;using System.ComponentModel;using HocusFocus.Core;namespace HocusFocus.UI.MVVM.ViewModels;public class MainViewModel : Bindable{    public MainViewModel()    {        ProcessItems = new List<ProcessViewModel>();        AppManager = new AppManager();        ConfigurationManager = new ConfigurationManager();        ProcessController = new ProcessController(ConfigurationManager);        BlacklistConfig = ConfigurationManager.LoadConfiguration();        AppManager.GetAllInstalledApps().ForEach(process =>            ProcessItems.Add(                new ProcessViewModel(process.Name,                    BlacklistConfig.BlockedApps.Contains(process.Name))            ));        foreach (var app in ProcessItems) app.PropertyChanged += OnCheckboxChecked;        FilteredProcessItems = new ObservableCollection<ProcessViewModel>(ProcessItems);        SearchBarViewModel = new SearchBarViewModel();        SearchBarViewModel.PropertyChanged += OnTextChanged;        FocusButtonViewModel = new FocusButtonViewModel(ProcessController);    }    public FocusButtonViewModel FocusButtonViewModel { get; }    public SearchBarViewModel SearchBarViewModel { get; }    public List<ProcessViewModel> ProcessItems { get; set; }    public ObservableCollection<ProcessViewModel> FilteredProcessItems { get; set; }    private AppManager AppManager { get; }    private BlacklistConfig BlacklistConfig { get; }    private ConfigurationManager ConfigurationManager { get; }    private ProcessController ProcessController { get; }    public void OnTextChanged(object? sender, PropertyChangedEventArgs e)    {        var search = SearchBarViewModel.SearchText;        if (string.IsNullOrEmpty(search))        {            FilteredProcessItems.Clear();            ProcessItems.ForEach(process => FilteredProcessItems.Add(process));        }        else        {            FilterProcesses(search);        }    }    public void FilterProcesses(string filter)    {        FilteredProcessItems.Clear();        foreach (var process in ProcessItems)            if (process.Name.ToLower().Contains(filter.ToLower()))                FilteredProcessItems.Add(process);    }    private void OnCheckboxChecked(object? sender, EventArgs e)    {        var processViewModel = (ProcessViewModel)sender;        if (processViewModel.IsChecked)            BlacklistConfig.BlockedApps.Add(processViewModel.Name);        else            BlacklistConfig.BlockedApps.Remove(processViewModel.Name);        ConfigurationManager.SaveConfiguration(BlacklistConfig);    }}