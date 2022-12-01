namespace IntroductionGL;

//: Логика взаимодействия с окном OpenGL3D
public partial class OpenGL3D : Window
{
    public OpenGL3D()
    {
        InitializeComponent();
        Load(); // Чтение и подготовка 3D-фигуры
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
    }

    /* ----------------------- Переменные --------------------------------- */
    OpenGL gl3D; // Переменная OpenGl

    public List<Triangle> Figure            = new List<Triangle>();            // Треугольники
    public List<Vector<float>> Normals      = new List<Vector<float>>();       // Нормали
    public List<Vector<float>> SmoothNormal = new List<Vector<float>>();       // Сглаженные нормали
    public List<EventOpenGL3D.Light> lights = new List<EventOpenGL3D.Light>(); // Источники света

    public Camera camera   = new Camera();    // Камера
    public Texture texture = new Texture();   // Текстура

    public Color colorFog = new Color(153, 149, 140, 255); // Цвет тумана

    public bool isGrid         = true;  // Вкл./Выкл. сетки
    public bool isDepth        = true;  // Вкл./Выкл. буфера глубины
    public bool isLight        = false; // Вкл./Выкл. света
    public bool isSceleton     = false; // Вкл./Выкл. каркасного режима
    public bool isDrawNormal   = false; // Вкл./Выкл. демонстрации нормалей
    public bool isShowTexture  = false; // Вкл./Выкл. текстурирование фигуры
    public bool isSmoothNormal = false; // Вкл./Выкл. сглаживание нормалей
    public bool isFog          = false; // Вкл./Выкл. туман

    public bool isPerspective = true; // Если перспективный режим

    public int  _texture  = 1; // Хранит текстуру
    /* ----------------------- Переменные --------------------------------- */

    //: Начальное состояние OpenGl
    private void openGLControl3D_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args) {
        gl3D = args.OpenGL;
        gl3D.ClearColor(0f, 0f, 0f, 1f);
    }

    //: Функция отрисовки OpenGL
    private void openGLControl3D_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) {

        // Очистка буфера цвета и глубины
        gl3D.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        // Включаем/выключаем буфер глубины
        if (isDepth)
            gl3D.Enable(OpenGL.GL_DEPTH_TEST);
        else
            gl3D.Disable(OpenGL.GL_DEPTH_TEST);

        // Включаем освещение
        gl3D.Enable(OpenGL.GL_LIGHTING);

        // Обнуляем матрицу (на всякий случай)
        gl3D.LoadIdentity();

        // Устанавливаем камеру
        gl3D.LookAt(camera.Position[0],    camera.Position[1],    camera.Position[2],
                    camera.Orientation[0], camera.Orientation[1], camera.Orientation[2],
                    camera.Rotation[0],    camera.Rotation[1],    camera.Rotation[2]);

        // Установка освещения
        if (isLight)
            InstallLight();
        else {
            gl3D.ClearColor(0f, 0f, 0f, 1f);
            gl3D.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_DIFFUSE, new float[] { 0f, 0f, 0f, 1f });
            gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new[] { 0f, 0f, 0f, 0f });
            gl3D.Disable(OpenGL.GL_COLOR_MATERIAL);
            gl3D.Disable(OpenGL.GL_FOG);
            gl3D.Disable(OpenGL.GL_LIGHT0);
            gl3D.Disable(OpenGL.GL_LIGHT1);
            gl3D.Disable(OpenGL.GL_LIGHT2);
            gl3D.Disable(OpenGL.GL_LIGHT3);
            gl3D.Disable(OpenGL.GL_LIGHT4);
            gl3D.Disable(OpenGL.GL_LIGHT5);
            gl3D.Disable(OpenGL.GL_LIGHT6);
            gl3D.Disable(OpenGL.GL_LIGHT7);
        }

        // Рисование сетки
        if (isGrid)
            DrawGrid();

        // Рисование фигуры
        DrawFigure();

        // Рисование точки ориентира
        DrawCube(camera.Orientation, new Color(0, 0, 255));

        // Перемещение в точку взгяда наблюдателя
        gl3D.Translate(camera.Orientation[0], 0, camera.Orientation[2]);

        // отключение источников света
        gl3D.Disable(OpenGL.GL_FOG);
        gl3D.Disable(OpenGL.GL_LIGHT0);
        gl3D.Disable(OpenGL.GL_LIGHT1);
        gl3D.Disable(OpenGL.GL_LIGHT2);
        gl3D.Disable(OpenGL.GL_LIGHT3);
        gl3D.Disable(OpenGL.GL_LIGHT4);
        gl3D.Disable(OpenGL.GL_LIGHT5);
        gl3D.Disable(OpenGL.GL_LIGHT6);
        gl3D.Disable(OpenGL.GL_LIGHT7);

        // (Не обазательно) !Но гарантирует, что программа ждет в этой точке пока OpenGL рисует
        gl3D.Finish();
    }

    //: Состояние окна OpenGL при изменении размеров окна
    private void openGLControl3D_Resized(object sender, OpenGLRoutedEventArgs args) {

        // Вычисляем соотношение между шириной и высотой
        float aspect = (float)(openGLControl3D.ActualWidth / openGLControl3D.ActualHeight);

        // Устанавливаем матрицу проекции / определяет объем сцены
        gl3D.MatrixMode(MatrixMode.Projection);

        // Единичная матрица
        gl3D.LoadIdentity();

        // Окно просмотра
        gl3D.Viewport(0, 0, (int)openGLControl3D.ActualWidth, (int)openGLControl3D.ActualHeight);

        // Если режим перспективы, иначе ортографическая проекция
        if (isPerspective)
            gl3D.Perspective(60, aspect, 0.01f, 50.0f);
        else {
            if (openGLControl3D.ActualWidth >= openGLControl3D.ActualHeight)
                gl3D.Ortho(-10 * aspect, 10 * aspect, -10, 10, -100, 100);
            else
                gl3D.Ortho(-10, 10, -10 / aspect, 10 / aspect, -100, 100);
        }

        // Возврат к матрице модели GL_MODELVIEW
        gl3D.MatrixMode(OpenGL.GL_MODELVIEW);
    }

    //: Загрузка всех вспомогательных ресурсов
    private void Load() {

        // % ***** Чтение входных сечений и траекторий ***** %
        Vector<float>[] Section;      // Координаты 2D сечения
        Vector<float>[] Trajectory;   // Координаты траектории тиражирования
        Vector<float>[] ChangeParam;  // Параметры изменения сечения
        Vector<float> Percent;      // Проценты изменения сечения
        Vector<float> Angles;       // Углы поворота треугольника
        try
        {
            // Входные данные
            string json = File.ReadAllText("EventOpenGL3D/coordinates.json");
            EventOpenGL3D.Data data = JsonConvert.DeserializeObject<EventOpenGL3D.Data>(json)!;
            if (data is null) throw new FileNotFoundException("File uncorrected!");

            // Отформатируем входные данные
            (Section, Trajectory, ChangeParam, Percent, Angles) = data;
        }
        catch (FileNotFoundException ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        // % *****  Вычисление координат тиражированной фигуры ***** %

        // Шаблонное направление изначального треугольника
        Vector<float> patternTriangle = Vector<float>.GetVectorPolygon(Section[0], Section[1], Section[2]);

        // Вычисление длины пути
        float trajectoryLenght = 0;
        for (int i = 0; i < Trajectory.Length - 1; i++)
            trajectoryLenght += Vector<float>.Norm(Trajectory[i + 1] - Trajectory[i]);

        // Проходимся по всем изменениям
        for (int i = 0; i < ChangeParam.Length; i++) {

            // Считаем расстояние до точки тиражирования (заданный процент от длины пути)
            float tiradeLenght = (float)(trajectoryLenght * (Percent[i] / 100.0));

            // Ищем отрезок в который попадает точка тиражирования
            float curLenght = 0;
            float predLenght = 0;
            int index = 0;
            for (int j = 0; j < Trajectory.Length - 1; j++) {
                curLenght += Vector<float>.Norm(Trajectory[j + 1] - Trajectory[j]);

                // Если мы нашли отрезок, которому принадлежит точка тиражирования. Сохр индекс начала отрезка
                if (tiradeLenght >= predLenght && tiradeLenght <= curLenght) {
                    index = j;
                    if (index < 0) index = 0;
                    break;
                }
                else
                    predLenght = curLenght;
            }

            // Расстояние от начала отрезка до точки тиражирования и норма направления
            float localPath = tiradeLenght - predLenght;
            Vector<float> orientation = Trajectory[index + 1] - Trajectory[index];
            float localNorm = Vector<float>.Norm(orientation);
            float ratio = localPath / localNorm; // Отношение расстояния к норме направления

            // Вектор сдвига
            Vector<float> shift = new Vector<float>(new[] {
                Trajectory[index][0] + orientation[0] * ratio,
                Trajectory[index][1] + orientation[1] * ratio,
                Trajectory[index][2] + orientation[2] * ratio });

            // Угол (можно так вичислять угол, но как-то он фигово вычисляется)
            /*float scalar = Vector<float>.Scalar(orientation, patternTriangle);
            float angle = (float)(Acos(scalar / (Vector<float>.Norm(orientation) * Vector<float>.Norm(patternTriangle))) * 180.0 / PI);
            angle = angle < 0 ? (180 - angle) : angle - 180;
            if (angle == 180) angle = 0;*/

            // Создаем треугольник
            Vector<float> section1 = new Vector<float>(new[] { Section[0][0], Section[0][1], Section[0][2], 1f });
            Vector<float> section2 = new Vector<float>(new[] { Section[1][0], Section[1][1], Section[1][2], 1f });
            Vector<float> section3 = new Vector<float>(new[] { Section[2][0], Section[2][1], Section[2][2], 1f });

            // Масштабируем треугольник
            Matrix<float> matrixScale = new Matrix<float>(new float[4, 4]{
                {ChangeParam[i][0], 0f, 0f, 0f},
                {0f, ChangeParam[i][1], 0f, 0f},
                {0f, 0f, ChangeParam[i][2], 0f},
                {0f, 0f, 0f,                1f}
            });
            section1 = matrixScale * section1;
            section2 = matrixScale * section2;
            section3 = matrixScale * section3;

            // Ось (отвечающая за координату по которой крутить)
            Vector<float> axe = Vector<float>.GetVector(orientation, patternTriangle);

            // Повернуть треугольник, если angle != 0
            if (Angles[i] != 0)
            {
                float c = (float)(Cos(Angles[i] * PI / 180.0));
                float s = (float)(Sin(Angles[i] * PI / 180.0));
                Vector<float> v = Vector<float>.Normalize(axe);

                Matrix<float> matrixRotation = new Matrix<float>(new float[4, 4]{
                    {c + v[0]*v[0]*(1 - c)     ,   v[0]*v[1]*(1 - c) - v[2]*s,   v[0]*v[2]*(1 - c) + v[1]*s, 0f},
                    {v[1]*v[0]*(1 - c) + v[2]*s,   c + v[1]*v[1]*(1 - c)     ,   v[1]*v[2]*(1 - c) - v[0]*s, 0f},
                    {v[2]*v[0]*(1 - c) - v[1]*s,   v[2]*v[1]*(1 - c) + v[0]*s,   c + v[2]*v[2]*(1 - c)     , 0f},
                    {0f                        ,   0f                        ,   0f                        , 1f}
                });
                section1 = matrixRotation * section1;
                section2 = matrixRotation * section2;
                section3 = matrixRotation * section3;
            }

            // Сдвигаем треугольник
            Matrix<float> matrixShift = new Matrix<float>(new float[4, 4]{
                {1f, 0f, 0f, shift[0]},
                {0f, 1f, 0f, shift[1]},
                {0f, 0f, 1f, shift[2]},
                {0f, 0f, 0f, 1f}
            });
            section1 = matrixShift * section1;
            section2 = matrixShift * section2;
            section3 = matrixShift * section3;

            // Добавление треугольника
            Figure.Add(new Triangle(section1, section2, section3));
        }

        // Вычисление нормалей к плоскостям
        Normals.Add(Vector<float>.GetVectorPolygon(Figure[0].section1, Figure[0].section2, Figure[0].section3));
        for (int i = 0; i < Figure.Count - 1; i++) {
            Normals.Add(Vector<float>.GetVectorPolygon(Figure[i].section2, Figure[i].section1, Figure[i + 1].section1));
            Normals.Add(Vector<float>.GetVectorPolygon(Figure[i].section1, Figure[i].section3, Figure[i + 1].section3));
            Normals.Add(Vector<float>.GetVectorPolygon(Figure[i].section3, Figure[i].section2, Figure[i + 1].section2));
        }
        Normals.Add(Vector<float>.GetVectorPolygon(Figure[^1].section1, Figure[^1].section3, Figure[^1].section2));

        // Инвертируем нормали
        for (int i = 0; i < Normals.Count; i++)
            Normals[i] = (Vector<float>)(-Normals[i]).Clone();

        // Вычисление сглаженных нормале
        var smoothNor = (Normals[0] + Normals[1] + Normals[2]) / 3.0f;
        SmoothNormal.Add(smoothNor);
        SmoothNormal.Add(smoothNor);
        SmoothNormal.Add(smoothNor);
        for (int i = 1, numNor = 1; i < Figure.Count - 1; i++, numNor += 3)
        {
            smoothNor = (Normals[numNor] + Normals[numNor + 1] + Normals[numNor + 3] + Normals[numNor + 4]) / 4.0f;
            SmoothNormal.Add(smoothNor);
            smoothNor = (Normals[numNor] + Normals[numNor + 2] + Normals[numNor + 3] + Normals[numNor + 5]) / 4.0f;
            SmoothNormal.Add(smoothNor);
            smoothNor = (Normals[numNor + 1] + Normals[numNor + 2] + Normals[numNor + 4] + Normals[numNor + 5]) / 4.0f;
            SmoothNormal.Add(smoothNor);
        }
        smoothNor = (Normals[^1] + Normals[^3] + Normals[^4]) / 3.0f;
        SmoothNormal.Add(smoothNor);
        smoothNor = (Normals[^1] + Normals[^2] + Normals[^4]) / 3.0f;
        SmoothNormal.Add(smoothNor);
        smoothNor = (Normals[^1] + Normals[^2] + Normals[^3]) / 3.0f;
        SmoothNormal.Add(smoothNor);
    }
}



