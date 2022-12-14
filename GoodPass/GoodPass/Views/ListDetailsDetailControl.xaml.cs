using GoodPass.Models;
using GoodPass.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;

namespace GoodPass.Views;

public sealed partial class ListDetailsDetailControl : UserControl
{
    public ListDetailsViewModel ViewModel
    {
        get;
    }

    public GPData? ListDetailsMenuItem
    {

        get => GetValue(ListDetailsMenuItemProperty) as GPData;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }
    public static readonly DependencyProperty ListDetailsMenuItemProperty = DependencyProperty.Register("ListDetailsMenuItem", typeof(GPData), typeof(ListDetailsDetailControl), new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));

    public ListDetailsDetailControl()
    {
        ViewModel = App.GetService<ListDetailsViewModel>();
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ListDetailsDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }

    private void ListDetailsDetailControl_PasswordCopyButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var password = ListDetailsDetailControl_PasswordBox.Password;
        var passwordDatapackage = new DataPackage();
        passwordDatapackage.SetText(password);
        Clipboard.SetContent(passwordDatapackage);
        CopiedTipforPasswordCopyButton.IsOpen = true;
    }

    private void ListDetailsDetailControl_AcconutNameCopyButton_Click(object sender, RoutedEventArgs e)
    {
        var accountName = ListDetailsDetailControl_AccountNameText.Text;
        var dataPackage = new DataPackage();
        dataPackage.SetText(accountName);
        Clipboard.SetContent(dataPackage);
        CopiedTipforAcconutNameCopyButton.IsOpen = true;
    }

    private void PasswordRevealButton_Click(object sender, RoutedEventArgs e)
    {
        if (PasswordRevealButton.IsChecked == true)
            ListDetailsDetailControl_PasswordBox.PasswordRevealMode = PasswordRevealMode.Visible;
        else
            ListDetailsDetailControl_PasswordBox.PasswordRevealMode = PasswordRevealMode.Hidden;

    }

    private void ListDetailsDetailControl_EditButton_Click(object sender, RoutedEventArgs e)
    {
        //Todo：添加编辑模式代码
    }

    private async void ListDetailsDetailControl_DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        //弹窗提示用户确认
        var dialog = new GPDialog2();
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = App.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "删除确认";
        dialog.Content = "您确定要删除这组数据吗？";
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            var tarPlatform = ListDetailsMenuItem.PlatformName;
            var tarAccountName = ListDetailsMenuItem.AccountName;
            App.DataManager.DeleteData(tarPlatform, tarAccountName);
            //Todo:刷新页面
            //可参考资料 https://blog.csdn.net/timewaitfornoone/article/details/104442371
            //可参考资料 https://stackoverflow.com/questions/52710086/how-to-refresh-a-page-in-uwp
            //Todo:访问父级资源
            //可参考资料 https://stackoverflow.com/questions/52828684/how-to-get-parent-page-from-usercontrol-in-uwp
        }
    }
}
