namespace HocusFocus.UI.MVVM.ViewModels;public class SearchBarViewModel : Bindable{    private string _searchText;    public string SearchText    {        get => _searchText;        set        {            _searchText = value;            OnPropertyChanged(nameof(SearchText));        }    }}