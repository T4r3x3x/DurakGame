using MsBox.Avalonia;

using System.Threading.Tasks;

namespace DurakClient.MessageBoxes
{
    internal class MessageBoxHelper
    {
        internal static async Task ShowErrorMessageBoxAsync(string message)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error", message,
                       MsBox.Avalonia.Enums.ButtonEnum.Ok);
            await messageBox.ShowAsync();
        }
    }
}
