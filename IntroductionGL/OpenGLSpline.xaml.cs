namespace IntroductionGL;

//: Логика взаимодействия с окном OpenGLSpline
public partial class OpenGLSpline : Window
{
    public OpenGLSpline()
    {
        InitializeComponent();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
    }

    /* ----------------------- Переменные --------------------------------- */
    OpenGL  gl3D;                         // Переменная OpenGl
    Point   Position = new Point(0f, 0f); // Точка нахождения по сетке
    Point   MousePos = new Point(0f, 0f); // Позиция мыши
    List<Point> ControlPoint = new List<Point>(); // Контрольные точки
    List<Point> ScreenPoint  = new List<Point>(); // Экранные точки
    List<Point> Spline       = new List<Point>(); // Координаты сплайна

    int OrderBasicFunction = 3; // Порядок базисных функций 

    float Scale = 1f;               // Коэффициент масштабирования
    float ActiveWeightPoint = 1.0f; // Текущее значение веса активной точки
    int   ActivePointIndex = 0;     // Индекс активной точки
    
    
    bool IsActivePoint   = false; // Стоим ли мы на активной точке
    bool IsEditModePoint = false; // Режим редактирования точки
    bool IsViewWeights   = false; // Показать веса точек
    /* ----------------------- Переменные --------------------------------- */

    //: Начальное состояние OpenGl
    private void openGLControl3D_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args) {
        gl3D = args.OpenGL;
        gl3D.ClearColor(1f, 1f, 1f, 1f);
    }

    //: Функция отрисовки OpenGL
    private void openGLControl3D_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) {

        // Очистка буфера цвета и глубины
        gl3D.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        // Рисование сетки
        DrawGrid();

        // Рисование осей
        DrawAxes();

        // Рисование ломанной
        DrawLineStrip();

        // Рисование сплайна
        DrawSpline();

        // Рисование весов
        DrawWeights();

        // Рисование координат возле курсора
        DrawMouseCoordinate(MousePos);

        // (Не обазательно) !Но гарантирует, что программа ждет в этой точке пока OpenGL рисует
        gl3D.Finish();
    }

    //: Состояние окна OpenGL при изменении размеров окна
    private void openGLControl3D_Resized(object sender, OpenGLRoutedEventArgs args) {

        // Устанавливаем матрицу проекции / определяет объем сцены
        gl3D.MatrixMode(MatrixMode.Projection);

        // Единичная матрица
        gl3D.LoadIdentity();

        // Окно просмотра
        gl3D.Viewport(0, 0, (int)openGLControl3D.ActualWidth, (int)openGLControl3D.ActualHeight);

        // Ортографическая проекция
        gl3D.Ortho2D(0, openGLControl3D.ActualWidth, 0, openGLControl3D.ActualHeight);
    }

}

