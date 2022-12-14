namespace IntroductionGL;

//: Логика взаимодействия с окном MainWindow
public partial class MainWindow : Window
{
    public MainWindow() {
        InitializeComponent();
    }

    //: Открытие окна с 2D-графикой (Рисование линий)
    private void ButtonLab1_Click(object sender, RoutedEventArgs e)
    {
        OpenGL2D window_openGL2D = new OpenGL2D();
        window_openGL2D.Show();
        this.Close();
    }

    //: Открытие окна с 2D-графикой (Смешивание цветов)
    private void ButtonLab2_Click(object sender, RoutedEventArgs e)
    {
        OpenGL2D_2 window_openGL2D_2 = new OpenGL2D_2();
        window_openGL2D_2.Show();
        this.Close();
    }

    //: Открытие окна с 3D-графикой
    private void ButtonLab3_Click(object sender, RoutedEventArgs e)
    {
        OpenGL3D window_openGL3D = new OpenGL3D();
        window_openGL3D.Show();
        this.Close();
    }

    //: Открытие окна с 3D-графиокй и трассировкой лучей
    private void ButtonLab4_Click(object sender, RoutedEventArgs e)
    {
        OpenGL3D_Rays window_openGL3D_rays = new OpenGL3D_Rays();
        window_openGL3D_rays.Show();
        this.Close();
    }

    //: Открытие окна с Spline 
    private void ButtonSpline_Click(object sender, RoutedEventArgs e)
    {
        OpenGLSpline window_openGLSpline = new OpenGLSpline();
        window_openGLSpline.Show();
        this.Close();
    }
}
