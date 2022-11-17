namespace IntroductionGL;

//: Обработчики Button
public partial class OpenGL3D_Rays : Window {

    //: Обработчки кнопки переключения окон
    private void SwitchWindow_Click(object sender, RoutedEventArgs e) {
        MainWindow window = new MainWindow();
        window.Show();
        this.Close();
    }
}