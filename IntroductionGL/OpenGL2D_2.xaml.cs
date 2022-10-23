namespace IntroductionGL;

public partial class OpenGL2D_2 : Window
{
    public OpenGL2D_2()
    {
        InitializeComponent();
    }

    /* ----------------------- Переменные --------------------------------- */
    OpenGL gl2D; // Переменная OpenGl

    public List<PrimitiveFiveRect> Primitives = new List<PrimitiveFiveRect>(); // Все созданные примитивы
    public List<Point> Points = new List<Point>();                             // Точки одного примитива

    public string name_item_ComBox_Prim = string.Empty;  // Имя выбранного примитива
    public string name_item_comBox_Point = string.Empty;  // Имя выбранной вершины примитива

    public bool isEditingModePrim = false;  // Режим редактирования примитива
    public bool isEditingModePoint = false;  // Режим редактирования вершины примитива
    public bool isTexture = false;

    public Texture texture = new Texture();

    public Color curColor = new Color(128, 128, 128, 255);         // Текущий цвет (по умолчанию)
    public Color DefColor = new Color(100, 100, 100, 255);   // Цвет (по умолчанию)
    public Color SigColor = new Color(255, 153, 0, 255);     // Цвет выделения
    /* ----------------------- Переменные --------------------------------- */

    // Начальное состояние OpenGls
    private void openGLControl2D_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        gl2D = args.OpenGL;
        gl2D.ClearColor(1, 1, 1, 0);
    }

    private void openGLControl2D_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) {
        // Очиська буфера цвета и глубины
        gl2D.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        // Отрисовка точек выбранного примитива
        for (int i = 0; i < Points.Count(); i++) {
            gl2D.PointSize(Points[i].Size);
            gl2D.Enable(OpenGL.GL_POINT_SMOOTH);
            gl2D.Begin(BeginMode.Points);
            gl2D.Color(Points[i].color.R, Points[i].color.G, Points[i].color.B, Points[i].color.A);
            gl2D.Vertex(Points[i].X, Points[i].Y);
            gl2D.End();
        }

        // Для сглаживания
        gl2D.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
        gl2D.Enable(OpenGL.GL_LINE_SMOOTH);
        gl2D.Enable(OpenGL.GL_BLEND);
        gl2D.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

        if (isTexture) { 
            gl2D.Enable(OpenGL.GL_TEXTURE_2D);
            texture.Bind(gl2D);
        }

        for (int i = 0; i < Primitives.Count; i++) {
            gl2D.Begin(BeginMode.TriangleFan);
            for (int k = 0; k < Primitives[i].points.Count(); k++) {
                gl2D.Color(Primitives[i].points[k].color.R, Primitives[i].points[k].color.G, Primitives[i].points[k].color.B, Primitives[i].points[k].color.A);
                gl2D.TexCoord(Primitives[i].points[k].X, Primitives[i].points[k].Y);
                gl2D.Vertex(Primitives[i].points[k].X, Primitives[i].points[k].Y);
            }
            gl2D.End();
        }

    }

    private void openGLControl2D_Resized(object sender, OpenGLRoutedEventArgs args) {
        gl2D.MatrixMode(MatrixMode.Projection);
        gl2D.LoadIdentity();
        gl2D.Ortho(0, openGLControl2D.ActualWidth, openGLControl2D.ActualHeight, 0, 0, 0);
        gl2D.MatrixMode(MatrixMode.Modelview);
    }

    private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e) {
        curColor = new Color((byte)ColorPicker.Color.RGB_R, (byte)ColorPicker.Color.RGB_G, (byte)ColorPicker.Color.RGB_B, (byte)ColorPicker.Color.A);

        // Фокус, чтобы когда нажимаем на стрелочку не терялся фокус на Opengl
        openGLControl2D.Focus();

        // Если выбран режим редактирования примитива
        if (isEditingModePrim && !isEditingModePoint) {
            PrimitiveFiveRect temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
            int index_prim = Primitives.IndexOf(temp_prim);
            Point[] newpoints = new Point[5];
            for (int i = 0; i < newpoints.Length; i++)
                newpoints[i] = temp_prim.points[i] with { color = curColor };
            Primitives[index_prim] = Primitives[index_prim] with { points = newpoints };
            return;
        }

        // Если выбран режим редактирования точки
        if (isEditingModePoint) {
            PrimitiveFiveRect temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
            int index_prim = Primitives.IndexOf(temp_prim);
            int index_point = Convert.ToInt32(name_item_comBox_Point[^1].ToString());
            Point[] newpoints = new Point[5];
            for (int i = 0; i < newpoints.Length; i++) {
                newpoints[i] = temp_prim.points[i];
                if (Convert.ToInt32(name_item_comBox_Point[^1].ToString()) == i+1)
                    newpoints[i] = newpoints[i] with { color = curColor };
            }      
            Primitives[index_prim] = Primitives[index_prim] with { points = newpoints };
            return;
        }

        if (Primitives.Any()) {
            for (int i = 0; i < Primitives[^1].points.Count(); i++)
                Primitives[^1].points[i] = Primitives[^1].points[i] with { color = curColor };
            return;
        }
    }

    private void ButtonRotationClockWise_Click(object sender, RoutedEventArgs e)
    {
        gl2D.Rotate(2*PI / 5.0, 0, 0, 1);
    }

    private void ButtonRotationNotClockWise_Click(object sender, RoutedEventArgs e)
    {
        gl2D.Rotate(-2 * PI / 5.0, 0, 0, 1);
    }

    private void ButtonZoomIn_Click(object sender, RoutedEventArgs e)
    {
        gl2D.Scale(1.5, 1.5, 1);
    }

    private void ButtonZoomOut_Click(object sender, RoutedEventArgs e)
    {
        gl2D.Scale(0.5, 0.5, 1);

    }

    private void ButtonLoadTexture_Click(object sender, RoutedEventArgs e)
    {
        isTexture = true;
        texture.Create(gl2D, @"D:\Хрень\avatar.jpg");
    }
}

