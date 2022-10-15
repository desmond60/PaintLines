using System.Diagnostics;
using System.Xml.Linq;

namespace IntroductionGL;

public partial class MainWindow : Window
{
    public MainWindow() {
        InitializeComponent();
    }

    /* ----------------------- Переменные --------------------------------- */
    OpenGL gl; // Переменная OpenGl
    
    public List<Primitive> Primitives                = new List<Primitive>();                // Вершины примитивов всех примитивов набора
    public List<Point>     Points                    = new List<Point>();                    // Примитивы одного набора
    public List<CollectionPrimitives> CollPrimitives = new List<CollectionPrimitives>();     // Наборы примитивов

    public bool isEditingModeColPrim = false;  // Режим редактирования набора примитивов 
    public bool isEditingModePrim    = false;  // Режим редактирования примитива
    public bool isEditingModePoint   = false;  // Режим редактирования вершины примитива

    public string name_item_ComBox_CollPrim = string.Empty;  // Имя выбранного набора примитивов
    public string name_item_ComBox_Prim     = string.Empty;  // Имя выбранного примитива
    public string name_item_comBox_Point    = string.Empty;  // Имя выбранной вершины примитива

    public float lineWidth   = 1.0f;                       // Ширина линии (по умолчанию)
    public TypeLine typeLine = TypeLine.ORDINARY;          // Тип линии (по умолчанию)
    public Color curColor    = new Color(0, 0, 0);         // Текущий цвет (по умолчанию)
    public Color DefColor    = new Color(100, 100, 100);   // Цвет (по умолчанию)
    public Color SigColor    = new Color(255, 153, 0);     // Цвет выделения

    public int ScrollSize = 0; // Коэффициент изменения размеров окошка OpenGl
    /* ----------------------- Переменные --------------------------------- */

    // Начальное состояние OpenGl
    private void openGLControl1_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args) {
        gl = args.OpenGL;
        gl.ClearColor(1, 1, 1, 0);
    }

    private void openGLControl1_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) {
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        for (int i = 0; i < Points.Count(); i++) {
            gl.PointSize(Points[i].Size);
            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            gl.Begin(BeginMode.Points);
            gl.Color(Points[i].color.R, Points[i].color.G, Points[i].color.B);
            gl.Vertex(Points[i].X, Points[i].Y);
            gl.End();
        }

        // Для сглаживания
        gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
        gl.Enable(OpenGL.GL_LINE_SMOOTH);
        gl.Enable(OpenGL.GL_BLEND);
        gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

        for (int i = 0; i < Primitives.Count(); i++)
        {
            switch (Primitives[i].type) {
                case TypeLine.ORDINARY:
                    break;
                case TypeLine.POINT:
                    gl.Enable(OpenGL.GL_LINE_STIPPLE);
                    gl.LineStipple(1, 0x0101);
                    break;
                case TypeLine.DASHED:
                    gl.Enable(OpenGL.GL_LINE_STIPPLE);
                    gl.LineStipple(1, 0x00F0);
                    break;
                case TypeLine.DASHEDPOINT:
                    gl.Enable(OpenGL.GL_LINE_STIPPLE);
                    gl.LineStipple(1, 0x1C47);
                    break;
                default: break;
            }
            gl.LineWidth(Primitives[i].LineWidth);
            gl.Begin(BeginMode.Lines);
            gl.Color(Primitives[i].fPoint.color.R, Primitives[i].fPoint.color.G, Primitives[i].fPoint.color.B);
            gl.Vertex(Primitives[i].fPoint.X, Primitives[i].fPoint.Y);
            gl.Color(Primitives[i].sPoint.color.R, Primitives[i].sPoint.color.G, Primitives[i].sPoint.color.B);
            gl.Vertex(Primitives[i].sPoint.X, Primitives[i].sPoint.Y);
            gl.End();
            gl.Disable(OpenGL.GL_LINE_STIPPLE);
        }

        for (int i = 0; i < CollPrimitives.Count(); i++) {

            // Пропуск выбранного в combobox набора примитивов
            if (CollPrimitives[i].Name.Equals(name_item_ComBox_CollPrim) && isEditingModeColPrim) {
                continue;
            }

            for (int j = 0; j < CollPrimitives[i].Primitives.Count(); j++)
            {
                switch (CollPrimitives[i].Primitives[j].type)
                {
                    case TypeLine.ORDINARY:
                        break;
                    case TypeLine.POINT:
                        gl.Enable(OpenGL.GL_LINE_STIPPLE);
                        gl.LineStipple(1, 0x0101);
                        break;
                    case TypeLine.DASHED:
                        gl.Enable(OpenGL.GL_LINE_STIPPLE);
                        gl.LineStipple(1, 0x00F0);
                        break;
                    case TypeLine.DASHEDPOINT:
                        gl.Enable(OpenGL.GL_LINE_STIPPLE);
                        gl.LineStipple(1, 0x1C47);
                        break;
                    default: break;
                }
                gl.LineWidth(CollPrimitives[i].Primitives[j].LineWidth);
                gl.Begin(BeginMode.Lines);
                gl.Color(CollPrimitives[i].Primitives[j].fPoint.color.R, CollPrimitives[i].Primitives[j].fPoint.color.G, CollPrimitives[i].Primitives[j].fPoint.color.B);
                gl.Vertex(CollPrimitives[i].Primitives[j].fPoint.X, CollPrimitives[i].Primitives[j].fPoint.Y);
                gl.Color(CollPrimitives[i].Primitives[j].sPoint.color.R, CollPrimitives[i].Primitives[j].sPoint.color.G, CollPrimitives[i].Primitives[j].sPoint.color.B);
                gl.Vertex(CollPrimitives[i].Primitives[j].sPoint.X, CollPrimitives[i].Primitives[j].sPoint.Y);
                gl.End();
                gl.Disable(OpenGL.GL_LINE_STIPPLE);
            }
                
        }
            
    }

    private void openGLControl1_Resized(object sender, OpenGLRoutedEventArgs args) {

        gl.MatrixMode(MatrixMode.Projection);
        gl.LoadIdentity();
        gl.Ortho(0,openGLControl1.ActualWidth, openGLControl1.ActualHeight,0, 0,0);

        gl.MatrixMode(MatrixMode.Modelview);
    }

    private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e) {
        curColor = new Color((byte)ColorPicker.Color.RGB_R, (byte)ColorPicker.Color.RGB_G, (byte)ColorPicker.Color.RGB_B);

        if (isCreateColPrim && !isEditingModePrim && isEditingModeColPrim && RadioButtonColPrim.IsChecked!.Value)
        {
            for (int i = 0; i < Primitives.Count; i++)
                Primitives[i] = Primitives[i] with 
                {
                    fPoint = Primitives[i].fPoint with { color = curColor },
                    sPoint = Primitives[i].sPoint with { color = curColor }
                };
            return;
        }

        if (isEditingModePrim && isCreateColPrim && !isEditingModePoint)
        {
            Primitive temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim); ;
            int index = Primitives.IndexOf(temp_prim);
            Primitives[index] = Primitives[index] with 
            {
                fPoint = Primitives[index].fPoint with { color = curColor },
                sPoint = Primitives[index].sPoint with { color = curColor }
            };
            return;
        }

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
    }

    private void openGLControl1_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        //gl.Viewport((int)(openGLControl1.ActualWidth / 2.0) - 50, (int)(openGLControl1.ActualHeight / 2.0) - 50, 100, 100);
        //Point mouse = new Point((float)e.GetPosition(null).X, (float)e.GetPosition(null).Y);
        //gl.Viewport((int)mouse.X, (int)mouse.Y, 400, 400);

/*        if (e.Delta < 0)
        {
            ScrollSize -= 5;
            gl.Scale(0.9, 0.9, 1);
        }
        else {
            ScrollSize += 5;
            gl.Scale(1.1, 1.1, 1);
        }
*/
        //int new_width  = (int)(openGLControl1.ActualWidth + 20);
        //int new_height = (int)(openGLControl1.ActualHeight + 20);
        //gl.Viewport((int)mouse.X - new_width / 2, (int)mouse.Y - new_height / 2, (int)(openGLControl1.ActualWidth), (int)(openGLControl1.ActualHeight));

    }


}
