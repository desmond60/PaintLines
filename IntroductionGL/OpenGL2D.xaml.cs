using SharpGL.WPF;

namespace IntroductionGL;

/// <summary>
/// Логика взаимодействия для OpenGL2D.xaml
/// </summary>
public partial class OpenGL2D : Window
{
    public OpenGL2D()
    {
        InitializeComponent();
    }

    /* ----------------------- Переменные --------------------------------- */
    OpenGL gl2D; // Переменная OpenGl

    public List<Primitive> Primitives = new List<Primitive>();                // Вершины примитивов всех примитивов набора
    public List<Point> Points = new List<Point>();                    // Примитивы одного набора
    public List<CollectionPrimitives> CollPrimitives = new List<CollectionPrimitives>();     // Наборы примитивов

    public bool isEditingModeColPrim = false;  // Режим редактирования набора примитивов 
    public bool isEditingModePrim = false;  // Режим редактирования примитива
    public bool isEditingModePoint = false;  // Режим редактирования вершины примитива

    public string name_item_ComBox_CollPrim = string.Empty;  // Имя выбранного набора примитивов
    public string name_item_ComBox_Prim = string.Empty;  // Имя выбранного примитива
    public string name_item_comBox_Point = string.Empty;  // Имя выбранной вершины примитива

    public float lineWidth = 1.0f;                            // Ширина линии (по умолчанию)
    public TypeLine typeLine = TypeLine.ORDINARY;               // Тип линии (по умолчанию)
    public Color curColor = new Color(0, 0, 0, 255);         // Текущий цвет (по умолчанию)
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

        for (int i = 0; i < Points.Count(); i++)
        {
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

        for (int i = 0; i < Primitives.Count(); i++)
        {
            switch (Primitives[i].type)
            {
                case TypeLine.ORDINARY:
                    break;
                case TypeLine.POINT:
                    gl2D.Enable(OpenGL.GL_LINE_STIPPLE);
                    gl2D.LineStipple(1, 0x0101);
                    break;
                case TypeLine.DASHED:
                    gl2D.Enable(OpenGL.GL_LINE_STIPPLE);
                    gl2D.LineStipple(1, 0x00F0);
                    break;
                case TypeLine.DASHEDPOINT:
                    gl2D.Enable(OpenGL.GL_LINE_STIPPLE);
                    gl2D.LineStipple(1, 0x1C47);
                    break;
                default: break;
            }
            gl2D.LineWidth(Primitives[i].LineWidth);
            gl2D.Begin(BeginMode.Lines);
            gl2D.Color(Primitives[i].fPoint.color.R, Primitives[i].fPoint.color.G, Primitives[i].fPoint.color.B, Primitives[i].fPoint.color.A);
            gl2D.Vertex(Primitives[i].fPoint.X, Primitives[i].fPoint.Y);
            gl2D.Color(Primitives[i].sPoint.color.R, Primitives[i].sPoint.color.G, Primitives[i].sPoint.color.B, Primitives[i].sPoint.color.A);
            gl2D.Vertex(Primitives[i].sPoint.X, Primitives[i].sPoint.Y);
            gl2D.End();
            gl2D.Disable(OpenGL.GL_LINE_STIPPLE);
        }

        for (int i = 0; i < CollPrimitives.Count(); i++)
        {

            // Пропуск выбранного в combobox набора примитивов
            if (CollPrimitives[i].Name.Equals(name_item_ComBox_CollPrim) && isEditingModeColPrim)
            {
                continue;
            }

            for (int j = 0; j < CollPrimitives[i].Primitives.Count(); j++)
            {
                switch (CollPrimitives[i].Primitives[j].type)
                {
                    case TypeLine.ORDINARY:
                        break;
                    case TypeLine.POINT:
                        gl2D.Enable(OpenGL.GL_LINE_STIPPLE);
                        gl2D.LineStipple(1, 0x0101);
                        break;
                    case TypeLine.DASHED:
                        gl2D.Enable(OpenGL.GL_LINE_STIPPLE);
                        gl2D.LineStipple(1, 0x00F0);
                        break;
                    case TypeLine.DASHEDPOINT:
                        gl2D.Enable(OpenGL.GL_LINE_STIPPLE);
                        gl2D.LineStipple(1, 0x1C47);
                        break;
                    default: break;
                }
                gl2D.LineWidth(CollPrimitives[i].Primitives[j].LineWidth);
                gl2D.Begin(BeginMode.Lines);
                gl2D.Color(CollPrimitives[i].Primitives[j].fPoint.color.R, CollPrimitives[i].Primitives[j].fPoint.color.G, CollPrimitives[i].Primitives[j].fPoint.color.B, CollPrimitives[i].Primitives[j].fPoint.color.A);
                gl2D.Vertex(CollPrimitives[i].Primitives[j].fPoint.X, CollPrimitives[i].Primitives[j].fPoint.Y);
                gl2D.Color(CollPrimitives[i].Primitives[j].sPoint.color.R, CollPrimitives[i].Primitives[j].sPoint.color.G, CollPrimitives[i].Primitives[j].sPoint.color.B, CollPrimitives[i].Primitives[j].sPoint.color.A);
                gl2D.Vertex(CollPrimitives[i].Primitives[j].sPoint.X, CollPrimitives[i].Primitives[j].sPoint.Y);
                gl2D.End();
                gl2D.Disable(OpenGL.GL_LINE_STIPPLE);
            }

        }

    }

    private void openGLControl2D_Resized(object sender, OpenGLRoutedEventArgs args)
    {

        gl2D.MatrixMode(MatrixMode.Projection);
        gl2D.LoadIdentity();
        gl2D.Ortho(0, openGLControl2D.ActualWidth, openGLControl2D.ActualHeight, 0, 0, 0);
        gl2D.MatrixMode(MatrixMode.Modelview);
    }

    private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
    {
        curColor = new Color((byte)ColorPicker.Color.RGB_R, (byte)ColorPicker.Color.RGB_G, (byte)ColorPicker.Color.RGB_B, (byte)ColorPicker.Color.A);

        // Фокус, чтобы когда нажимаем на стрелочку не терялся фокус на Opengl
        openGLControl2D.Focus();

        // Редактирование набора примитива
        if (isCreateColPrim && !isEditingModePrim && isEditingModeColPrim && CanvasColPrim.Visibility == Visibility.Visible)
        {
            for (int i = 0; i < Primitives.Count; i++)
                Primitives[i] = Primitives[i] with
                {
                    fPoint = Primitives[i].fPoint with { color = curColor },
                    sPoint = Primitives[i].sPoint with { color = curColor }
                };
            return;
        }

        // Если включен режим редактирования примитива
        if (isEditingModePrim && isCreateColPrim && !isEditingModePoint)
        {
            Primitive temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
            int index = Primitives.IndexOf(temp_prim);
            Primitives[index] = Primitives[index] with
            {
                fPoint = Primitives[index].fPoint with { color = curColor },
                sPoint = Primitives[index].sPoint with { color = curColor }
            };
            return;
        }

        // Если включен режим редактирования вершины примитива
        if (isEditingModePoint && isCreateColPrim)
        {
            Primitive temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
            int index = Primitives.IndexOf(temp_prim);
            Primitives[index] = name_item_comBox_Point switch
            {
                "fpoint" => Primitives[index] with { fPoint = Primitives[index].fPoint with { color = curColor } },
                "spoint" => Primitives[index] with { sPoint = Primitives[index].sPoint with { color = curColor } },
                _ => Primitives[index] with
                {
                    fPoint = Primitives[index].fPoint with { color = curColor },
                    sPoint = Primitives[index].sPoint with { color = curColor }
                }
            };
            return;
        }

        // Изменение последнего нарисованного примитива
        if (Primitives.Any())
        {
            Primitives[^1] = Primitives[^1] with
            {
                fPoint = Primitives[^1].fPoint with { color = curColor },
                sPoint = Primitives[^1].sPoint with { color = curColor }
            };
            return;
        }
    }

}

