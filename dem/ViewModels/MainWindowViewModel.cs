using dem.ViewModels;
using dem;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.ComponentModel;

namespace dem.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ClientWebSocketHandler _clientWebSocketHandler = new ClientWebSocketHandler();



    [ObservableProperty]
    public string _currentMessage;
                

    public ObservableCollection<string> Messages { get; set; } = new();

    public MainWindowViewModel()
    {
        _clientWebSocketHandler.OnReceiveMessage += DisplayMessage;
    }

    private void DisplayMessage(string message)
    {
        Messages.Add(message);
    }

    public async Task SendMessage(string message)
    {
        Messages.Add(message);
        await _clientWebSocketHandler.SendMessage(message);

    }
}