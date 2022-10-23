namespace IntroductionGL;

public partial class OpenGL3D : Window
{
    public OpenGL3D()
    {
        InitializeComponent();

        Load();
    }

    /* ----------------------- Переменные --------------------------------- */
    OpenGL gl3D; // Переменная OpenGl

    public Vector[] Section;   // Координаты 2D сечения
    public Vector[] Trajectory;   // Координаты траектории тиражирования
    public Vector[] ChangeParam;   // Параметры изменения сечения

    public Camera camera = new Camera(); // Переменная камеры

    public bool isLight = true;  // Включен свет?
    public bool isPerspective = true; // 
    /* ----------------------- Переменные --------------------------------- */

    // Начальное состояние OpenGl
    private void openGLControl3D_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args) {
        gl3D = args.OpenGL;
        //gl3D.ClearColor(1, 1, 1, 0);
    }

    private void openGLControl3D_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) {
        // Очиська буфера цвета и глубины
        gl3D.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        // Включаем буфер глубины
        gl3D.Enable(OpenGL.GL_DEPTH_TEST);

        // Включить или выключить свет
        if (isLight)
            gl3D.Enable(OpenGL.GL_LIGHTING);
        else
            gl3D.Disable(OpenGL.GL_LIGHTING);

        // Обнуляем матрицу (на всякий случай)
        gl3D.LoadIdentity();

        // Устанавливаем камеру
        gl3D.LookAt(camera.Position.x,    camera.Position.y,    camera.Position.z,
                    camera.Orientation.x, camera.Orientation.y, camera.Orientation.z,
                    camera.Rotation.x,    camera.Rotation.y,    camera.Rotation.z);

        // Установка освещения если включено естественно
        if (isLight)
            InstallLight();

        // Нарисовать сетку
        DrawGrid();

        // Нарисовать нашу фигуру

        // Перемещение в точку взгяда наблюдателя
        gl3D.Translate(camera.Orientation.x, 0, camera.Orientation.z);

    }

    private void openGLControl3D_Resized(object sender, OpenGLRoutedEventArgs args)
    {
        // Вычисляем соотношение между шириной и высотой
        float ratio = (float)(openGLControl3D.ActualWidth / openGLControl3D.Height);

        // Устанавливаем матрицу проекции / определяет объем сцены
        gl3D.MatrixMode(MatrixMode.Projection);

        // Единичная матрица
        gl3D.LoadIdentity();

        // Окно просмотра
        gl3D.Viewport(0, 0, (int)openGLControl3D.ActualWidth, (int)openGLControl3D.ActualHeight);

        // Если режим перспективы, иначе ортографическая проекция
        if (isPerspective)
            gl3D.Perspective(60, ratio, 0.1f, 100.0f);
        else {
            if (openGLControl3D.ActualWidth >= openGLControl3D.ActualHeight)
                gl3D.Ortho(-10 * ratio, 10 * ratio, -10, 10, -100, 100);
            else
                gl3D.Ortho(-10, 10, -10 / ratio, 10 / ratio, -100, 100);
        }

        // Возврат к матрице модели
        gl3D.MatrixMode(OpenGL.GL_MODELVIEW);
    }

    //: Загрузка всех вспомогательных ресурсов
    private void Load() {
        // Чтение входных сечений и траекторий 
        try {
            // Входные данные
            string json = File.ReadAllText("EventOpenGL3D/coordinates.json");
            Data data = JsonConvert.DeserializeObject<Data>(json)!;
            if (data is null) throw new FileNotFoundException("File uncorrected!");

            // Отформатируем входные данные
            (Section, Trajectory, ChangeParam) = data;
        }
        catch (FileNotFoundException ex) {
            Trace.WriteLine(ex.Message);
        }

        // Установка начального положения камеры
        Vector pos = new Vector(-18.0f, 0.5f, -3.0f);
        Vector ori = new Vector(-12.0f, 0.5f, -3.0f);
        Vector rot = new Vector(0.0f, 1.0f, 0.0f);

        camera.CameraPosition(pos, ori, rot);
    }

    //: Установка света
    private void InstallLight() {
        gl3D.Material(OpenGL.GL_FRONT, OpenGL.GL_DIFFUSE, new float[] { 0.1f, 0.1f, 0.1f, 1.0f });
        gl3D.Enable(OpenGL.GL_COLOR_MATERIAL);
        
        // Напрваленный источник света
        gl3D.Enable(OpenGL.GL_LIGHT0);
        gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, new float[] { 0.1f, 0.1f, 0.1f, 1.0f });
        gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, new float[] { 0.4f, 0.7f, 0.2f });
        gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, new float[] { -1.0f, 4.0f, -1.0f, 0.0f });
    }

    //: Рисовка сетки
    private void DrawGrid() {
        gl3D.Color(0, 255, 0);
        for (int i = -50; i <= 50; i++)
        {
            gl3D.Begin(BeginMode.Lines);
            gl3D.Vertex(-50, 0, i);
            gl3D.Vertex(50, 0, i);

            gl3D.Vertex(i, 0, -50);
            gl3D.Vertex(i, 0, 5);
            gl3D.End();
        }
    }
}

