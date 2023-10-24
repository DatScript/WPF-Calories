using Syncfusion.Windows.Shared;
using System.Media;
using System.Windows;

namespace Common.MessageBox;
/// <summary>
/// Interaction logic for MessageCus.xaml
/// </summary>
public partial class MessageCus : ChromelessWindow
{
    public MessageBoxResult MessageBoxResult;
    public MessageCus(string Title_, string Message_)
    {
        InitializeComponent();
        MessageText.Text = Message_;
        Title = Title_;
        SystemSounds.Asterisk.Play();
    }

    public static MessageBoxResult Show(string Title_, string ErrorMessage_, MessageBoxButton messageBoxButton = MessageBoxButton.OK)
    {
        MessageCus winMessageBox = new MessageCus(Title_, ErrorMessage_);
        if (messageBoxButton == MessageBoxButton.OK)
        {
            winMessageBox.BtnCancel.Visibility = Visibility.Collapsed;
        }
        winMessageBox.ShowDialog();
        return winMessageBox.MessageBoxResult;
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult = MessageBoxResult.OK;
        this.Close();
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult = MessageBoxResult.Cancel;
        this.Close();
    }
}