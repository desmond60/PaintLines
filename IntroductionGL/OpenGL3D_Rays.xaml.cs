namespace IntroductionGL;

//: Логика взаимодействия с окном OpenGL3D_Rays
public partial class OpenGL3D_Rays : Window
{
    public OpenGL3D_Rays()
    {
        InitializeComponent();
        Load(); // Чтение и подготовка 3D-фигуры
    }

    /* ----------------------- Переменные --------------------------------- */
    OpenGL gl3D; // Переменная OpenGl

    public Camera camera = new Camera(); // Камера
    public EventOpenGL3D_Rays.Stage stage;

    public bool IsLight = true; // Вкл./Выкл. света
    public bool IsRaysTracer = false; // Вкл./Выкл. трассировку
    public bool IsOnOffFigure = false; // Вкл./Выкл. режима включения/исключения фигур
    public bool IsSurface = true; // Вкл./Выкл. плоскости
    public bool IsGrid = false;
    /* ----------------------- Переменные --------------------------------- */

    //: Начальное состояние OpenGl
    private void openGLControl3D_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args) {
        gl3D = args.OpenGL;
    }

    //: Функция отрисовки OpenGL
    private void openGLControl3D_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        // Очистка буфера цвета и глубины
        gl3D.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        // Включаем буфер глубины
        gl3D.Enable(OpenGL.GL_DEPTH_TEST);

        // Включение/Выключение света
        if (IsLight)
            gl3D.Enable(OpenGL.GL_LIGHTING);
        else
            gl3D.Disable(OpenGL.GL_LIGHTING);

        // Обнуляем матрицу (на всякий случай)
        gl3D.LoadIdentity();

        // Устанавливаем камеру
        gl3D.LookAt(camera.Position[0], camera.Position[1], camera.Position[2],
                    camera.Orientation[0], camera.Orientation[1], camera.Orientation[2],
                    camera.Rotation[0], camera.Rotation[1], camera.Rotation[2]);

        // Установка освещения
        if (IsLight)
            InstallLight();

        // Установка сетки
        if (IsGrid)
            DrawGrid();

        // Отрисовка фигур
        DrawFigures();

        // Запускаем трасировку 
        if (IsRaysTracer)
            RayTracing();

        // Рисование точки ориентира
/*        if (!IsRaysTracer) {
            gl3D.PointSize(50);
            gl3D.Begin(BeginMode.Points);
            gl3D.Color((byte)0, (byte)0, (byte)255);
            gl3D.Vertex(camera.Orientation[0], camera.Orientation[1], camera.Orientation[2]);
            gl3D.End();
        }*/

        // Перемещение в точку взгяда наблюдателя
        gl3D.Translate(camera.Orientation[0], 0, camera.Orientation[2]);

        // Выключение источников света
        gl3D.Disable(OpenGL.GL_LIGHT0);
        gl3D.Disable(OpenGL.GL_LIGHT1);
        gl3D.Disable(OpenGL.GL_LIGHT2);
        gl3D.Disable(OpenGL.GL_LIGHT3);
        gl3D.Disable(OpenGL.GL_FOG);

        // (Не обазательно) !Но гарантирует, что программа ждет в этой точке пока OpenGL рисует
        gl3D.Finish();
    }

    //: Состояние окна OpenGL при изменении размеров окна
    private void openGLControl3D_Resized(object sender, OpenGLRoutedEventArgs args)
    {
        // Вычисляем соотношение между шириной и высотой
        float aspect = (float)(openGLControl3D.ActualWidth / openGLControl3D.ActualHeight);
 
        // Устанавливаем матрицу проекции / определяет объем сцены
        gl3D.MatrixMode(MatrixMode.Projection);

        // Единичная матрица
        gl3D.LoadIdentity();

        // Окно просмотра
        gl3D.Viewport(0, 0, (int)openGLControl3D.ActualWidth, (int)openGLControl3D.ActualHeight);

        // Если режим перспективы, иначе ортографическая проекция
        gl3D.Perspective(60, aspect, 0.1f, 100.0f);

        // Возврат к матрице модели GL_MODELVIEW
        gl3D.MatrixMode(OpenGL.GL_MODELVIEW);
    }

    //: Загрузка всех вспомогательных ресурсов
    private void Load()
    {
        // % ***** Чтение входных сечений и траекторий ***** %
        Sphere[] spheres;           // Сферы
        Tetrahedron[] tetrahedrons; // Тетраэдры
        Square square;              // Плоскость
        try
        {
            // Входные данные
            string json = File.ReadAllText("EventOpenGL3D_Rays/objects.json");
            EventOpenGL3D_Rays.Data data = JsonConvert.DeserializeObject<EventOpenGL3D_Rays.Data>(json)!;
            if (data is null) throw new FileNotFoundException("File uncorrected!");

            // Отформатируем входные данные
            (spheres, tetrahedrons, square) = data;
        }
        catch (FileNotFoundException ex)
        {
            Trace.WriteLine(ex.Message);
            return;
        }

        // % ***** Инициализация плоскости и источника света ***** %
        Light light1 = new Light(new Vector<float>(new[] { -5f, 2f, -5f }), new ColorF(0.4f, 0.4f, 0.4f));
        Light light2 = new Light(new Vector<float>(new[] { -13f, 1f, 8f }), new ColorF(0.1f, 0.2f, 0.5f));

        for (int i = 0; i < spheres.Length; i++)
            spheres[i].isShow = true;

        for (int i = 0; i < tetrahedrons.Length; i++)
            tetrahedrons[i].isShow = true;

        stage = new EventOpenGL3D_Rays.Stage(square, spheres, tetrahedrons, new Light[] { light1, light2 });
    }

}