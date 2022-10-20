namespace IntroductionGL;

public partial class MainWindow : Window
{
    public MainWindow() {
        InitializeComponent();
    }

    private void Button2D_Click(object sender, RoutedEventArgs e)
    {
        OpenGL2D window_openGL2D = new OpenGL2D();
        window_openGL2D.Show();
        this.Close();
    }

    private void Button3D_Click(object sender, RoutedEventArgs e)
    {

    }
}
