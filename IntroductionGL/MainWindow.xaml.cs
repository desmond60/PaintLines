namespace IntroductionGL;

public partial class MainWindow : Window
{
    public MainWindow() {
        InitializeComponent();
    }

    // Запуск окошка с рисовкой линий
    private void ButtonLab1_Click(object sender, RoutedEventArgs e)
    {
        OpenGL2D window_openGL2D = new OpenGL2D();
        window_openGL2D.Show();
        this.Close();
    }

    // Запуск окошка с переливанием цветов
    private void ButtonLab2_Click(object sender, RoutedEventArgs e)
    {
        OpenGL2D_2 window_openGL2D_2 = new OpenGL2D_2();
        window_openGL2D_2.Show();
        this.Close();
    }

    // Запуск окошка с трезмерной визуализацией
    private void ButtonLab3_Click(object sender, RoutedEventArgs e)
    {
        OpenGL3D window_openGL3D = new OpenGL3D();
        window_openGL3D.Show();
        this.Close();
    }
}
