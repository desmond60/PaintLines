using System.Runtime.CompilerServices;

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

    public bool isLight = true;
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
        if (isLight)
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
        if (isLight)
            InstallLight();

        // Рисование сетки
        DrawGrid();

        // Рисование фигуры
        //DrawFigure();

        // Рисование точки ориентира
        gl3D.PointSize(50);
        gl3D.Begin(BeginMode.Points);
        gl3D.Color((byte)0, (byte)0, (byte)255);
        gl3D.Vertex(camera.Orientation[0], camera.Orientation[1], camera.Orientation[2]);
        gl3D.End();

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
        float ratio = (float)(openGLControl3D.ActualWidth / openGLControl3D.ActualHeight);

        // Устанавливаем матрицу проекции / определяет объем сцены
        gl3D.MatrixMode(MatrixMode.Projection);

        // Единичная матрица
        gl3D.LoadIdentity();

        // Окно просмотра
        gl3D.Viewport(0, 0, (int)openGLControl3D.ActualWidth, (int)openGLControl3D.ActualHeight);

        // Если режим перспективы, иначе ортографическая проекция
        gl3D.Perspective(60, ratio, 0.01f, 50.0f);

        // Возврат к матрице модели GL_MODELVIEW
        gl3D.MatrixMode(OpenGL.GL_MODELVIEW);
    }

    //: Загрузка всех вспомогательных ресурсов
    private void Load()
    {
        // % ***** Чтение входных сечений и траекторий ***** %

        try
        {
            // Входные данные
            //string json = File.ReadAllText("EventOpenGL3D/coordinates.json");
            //EventOpenGL3D.Data data = JsonConvert.DeserializeObject<EventOpenGL3D.Data>(json)!;
            //if (data is null) throw new FileNotFoundException("File uncorrected!");

            // Отформатируем входные данные
            //(Section, Trajectory, ChangeParam, Percent, Angles) = data;
        }
        catch (FileNotFoundException ex)
        {
            Trace.WriteLine(ex.Message);
            return;
        }

        // % *****  Вычисление координат тиражированной фигуры ***** %

        
    }

    //: Установка света
    private void InstallLight()
    {
        // Включение режима двустороннего освещения
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_TWO_SIDE, OpenGL.GL_TRUE);

        // Значения глобальной фоновой составляющей
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new[] { 0.5f, 0.5f, 0.5f, 1f });

        // Задание материала
        gl3D.Enable(OpenGL.GL_COLOR_MATERIAL);

        switch (0)
        {
            case 0:
                // Точечный свет
                gl3D.Enable(OpenGL.GL_LIGHT0);
                gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, new[] { 0f, 1f, 0f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, new[] { 0.4f, 0.4f, 0.4f });
                gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, new[] { 0.5f, 0.5f, 0.5f, 1f });
                break;

            case 1:
                gl3D.Enable(OpenGL.GL_LIGHT1);
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, new[] { 0f, 5f, 0f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, new[] { 0.4f, 0.4f, 0.4f });
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, new[] { 0.5f, 0.5f, 0.5f, 1f });

                // Затухание
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_CONSTANT_ATTENUATION, 0);
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_LINEAR_ATTENUATION, 0.05f);
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_QUADRATIC_ATTENUATION, 0.05f);
                break;

            case 2:
                gl3D.Enable(OpenGL.GL_LIGHT2);
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_POSITION, new[] { 5f, 5f, -10f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_AMBIENT, new[] { 0.4f, 0.4f, 0.4f });
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_DIFFUSE, new[] { 0.5f, 0.5f, 0.5f, 1f });

                // Прожектор
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPOT_EXPONENT, 0);
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPOT_CUTOFF, 45);
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPOT_DIRECTION, new[] { -1f, 0f, 0f, 1f });
                break;

            default:
                break;
        }
    }
}