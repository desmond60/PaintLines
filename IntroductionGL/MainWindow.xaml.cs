using System.Diagnostics;

namespace IntroductionGL;

public partial class MainWindow : Window
{
    public MainWindow() {
        InitializeComponent();
    }

    public List<Point> TempPoints = new List<Point>();
    public List<Primitive> Primitives = new List<Primitive>();
    public List<Point>     Points     = new List<Point>();
    public List<CollectionPrimitives> CollPrimitives = new List<CollectionPrimitives>();
    public bool isCreateColPrim = false;
    public bool isEditingModeColPrim   = false;
    public bool isEditingModePrim = false;
    public bool isEditingModePoint = false;
    public string name_item_ComBox_CollPrim = string.Empty;
    public string name_item_ComBox_Prim     = string.Empty;
    public string name_item_comBox_Point = string.Empty;


    public float lineWidth = 1.0f;
    public TypeLine typeLine = TypeLine.ORDINARY;
    public Color curColor = new Color(0, 0, 0);
    public Color DefColor = new Color(100, 100, 100);
    public Color SigColor = new Color(255, 153, 0);
    public Primitive colorPrim = new Primitive();

    public readonly Point speed_point = new Point(0.01f, 0.01f);

    private void OpenGLControl_MouseDown(object sender, MouseButtonEventArgs e) {

        if (!isEditingModePrim)
            if (e.LeftButton == MouseButtonState.Pressed) {
                Point point = new Point((float)e.GetPosition(this).X, (float)e.GetPosition(this).Y);
                point = Primitive_Coordinate(point, openGLControl1.ActualWidth, openGLControl1.ActualHeight);
                TempPoints.Add(point);
                Points.Add(point);
                isCreateColPrim = false;
                ComboBoxCollPrimitives.IsEnabled = false;
                if (TempPoints.Count == 1) colorPrim.fPoint = colorPrim.fPoint with { color = curColor };
                if (TempPoints.Count == 2) {

                    if (Primitives.Any())
                        Primitives.Add(new Primitive(TempPoints[0], TempPoints[1], $"Primitive_{Convert.ToInt32(Primitives[^1].Name.Split("_")[1]) + 1}"));
                    else
                        Primitives.Add(new Primitive(TempPoints[0], TempPoints[1], $"Primitive_{Primitives.Count() + 1}"));
                    Primitives[^1] = Primitives[^1] with
                    {
                        LineWidth = lineWidth,
                        type = typeLine,
                        fPoint = Primitives[^1].fPoint with { color = colorPrim.fPoint.color },
                        sPoint = Primitives[^1].sPoint with { color = curColor }
                    };
                    TempPoints.Clear();
                    isCreateColPrim = true;
                    ComboBoxCollPrimitives.IsEnabled = true;
                }
                return;
            }

        if (isEditingModePoint) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                Point point = new Point((float)e.GetPosition(this).X, (float)e.GetPosition(this).Y);
                point = Primitive_Coordinate(point, openGLControl1.ActualWidth, openGLControl1.ActualHeight);

                Primitive temp_prim = GetPrimByName(name_item_ComBox_Prim);
                int index = Primitives.IndexOf(temp_prim);
                Primitives[index] = name_item_comBox_Point switch
                {
                    "fpoint" => Primitives[index] with { fPoint = point with { color = temp_prim.fPoint.color} },
                    "spoint" => Primitives[index] with { sPoint = point with { color = temp_prim.sPoint.color} },
                    _ => Primitives[index]
                };
                Points.Clear();
                switch (name_item_comBox_Point)
                {
                    case "fpoint":
                        Points.Add(Primitives[index].fPoint with { color = SigColor });
                        Points.Add(Primitives[index].sPoint with { color = DefColor });
                        break;
                    case "spoint":
                        Points.Add(Primitives[index].fPoint with { color = DefColor });
                        Points.Add(Primitives[index].sPoint with { color = SigColor });
                        break;
                }

            }
        }

    }

    private void openGLControl1_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args) {
        var gl = args.OpenGL;
        gl.ClearColor(255, 255, 255, 0);
    }

    private void openGLControl1_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) {
        var gl = args.OpenGL;

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

    private void openGLControl1_Resized(object sender, OpenGLRoutedEventArgs args) { }

    private void Grid_KeyDown(object sender, KeyEventArgs args) {

        if (args.Key == Key.Space) {
            if (!isEditingModeColPrim)
            {
                if (Primitives.Count() > 0 && isCreateColPrim)
                {
                    CollectionPrimitives item_col_prim = new CollectionPrimitives(Primitives);
                    ComboBoxCollPrimitives.Items.Add(NameCollPrim(CollPrimitives, ref item_col_prim));
                    CollPrimitives.Add(item_col_prim);
                    Points.Clear();
                    Primitives.Clear();
                    isEditingModeColPrim = false;
                    isEditingModePrim = false;
                    isEditingModePoint = false;
                    DeleteCollPrimitives.IsEnabled = false;
                    SliderLineWidth.IsEnabled = true;
                    ComboBoxTypeLine.IsEnabled = true;
                }
            }
            else
            {
                if (Primitives.Count() > 0 && isCreateColPrim)
                {

                    CollectionPrimitives item_col_prim = CollPrimitives.Find(n => n.Name.Equals(name_item_ComBox_CollPrim));
                    CollPrimitives.Remove(item_col_prim);
                    item_col_prim = new CollectionPrimitives(Primitives);

                    ComboBoxCollPrimitives.Items.RemoveAt(ComboBoxCollPrimitives.SelectedIndex);

                    ComboBoxCollPrimitives.Items.Add(NameCollPrim(CollPrimitives, ref item_col_prim));

                    CollPrimitives.Add(item_col_prim);
                    Points.Clear();
                    Primitives.Clear();
                    isEditingModeColPrim = false;
                    isEditingModePrim = false;
                    isEditingModePoint = false;
                    ComboBoxPrimitives.Items.Clear();
                    ComboBoxPoints.Items.Clear();
                    ComboBoxPrimitives.IsEnabled = false;
                    ComboBoxPoints.IsEnabled = false;
                    DeleteCollPrimitives.IsEnabled = false;
                    SliderLineWidth.IsEnabled = true;
                    ComboBoxTypeLine.IsEnabled = true;
                }
            }
        }

        // Перемещение набора примитивов и одного примитива вверх
        if (args.Key == Key.W) {
            Points.Clear();
            if (!isEditingModePrim)
                for (int i = 0; i < Primitives.Count(); i++) {
                    Point new_fpoint = new Point(Primitives[i].fPoint.X, Primitives[i].fPoint.Y + speed_point.Y);
                    Point new_spoint = new Point(Primitives[i].sPoint.X, Primitives[i].sPoint.Y + speed_point.Y);
                    Primitives[i] = Primitives[i] with { fPoint = new_fpoint, sPoint = new_spoint };
                    Points.Add(Primitives[i].fPoint);
                    Points.Add(Primitives[i].sPoint);
                }
            else { // Перемещение примитива
                Points.Clear();
                Primitive tempPrim = GetPrimByName(name_item_ComBox_Prim);
                Point new_fpoint = new Point(tempPrim.fPoint.X, tempPrim.fPoint.Y + speed_point.Y);
                Point new_spoint = new Point(tempPrim.sPoint.X, tempPrim.sPoint.Y + speed_point.Y);

                int index_prim = Primitives.IndexOf(tempPrim);
                Primitives[index_prim] = Primitives[index_prim] with { fPoint = new_fpoint, sPoint = new_spoint };
                Points.Add(Primitives[index_prim].fPoint);
                Points.Add(Primitives[index_prim].sPoint);
            }
        }

        // Перемещение набора примитивов вниз
        if (args.Key == Key.S) {
            Points.Clear();
            if (!isEditingModePrim)
                for (int i = 0; i < Primitives.Count(); i++) {
                    Point new_fpoint = new Point(Primitives[i].fPoint.X, Primitives[i].fPoint.Y - speed_point.Y);
                    Point new_spoint = new Point(Primitives[i].sPoint.X, Primitives[i].sPoint.Y - speed_point.Y);
                    Primitives[i] = Primitives[i] with { fPoint = new_fpoint, sPoint = new_spoint };
                    Points.Add(Primitives[i].fPoint);
                    Points.Add(Primitives[i].sPoint);
                }
            else { // Перемещение примитива
                Points.Clear();
                Primitive tempPrim = GetPrimByName(name_item_ComBox_Prim);
                Point new_fpoint = new Point(tempPrim.fPoint.X, tempPrim.fPoint.Y - speed_point.Y);
                Point new_spoint = new Point(tempPrim.sPoint.X, tempPrim.sPoint.Y - speed_point.Y);

                int index_prim = Primitives.IndexOf(tempPrim);
                Primitives[index_prim] = Primitives[index_prim] with { fPoint = new_fpoint, sPoint = new_spoint };
                Points.Add(Primitives[index_prim].fPoint);
                Points.Add(Primitives[index_prim].sPoint);
            }
        }

        // Перемещение набора примитивов влево
        if (args.Key == Key.A) {
            Points.Clear();
            if (!isEditingModePrim)
                for (int i = 0; i < Primitives.Count(); i++) {
                    Point new_fpoint = new Point(Primitives[i].fPoint.X - speed_point.X, Primitives[i].fPoint.Y);
                    Point new_spoint = new Point(Primitives[i].sPoint.X - speed_point.X, Primitives[i].sPoint.Y);
                    Primitives[i] = Primitives[i] with { fPoint = new_fpoint, sPoint = new_spoint };
                    Points.Add(Primitives[i].fPoint);
                    Points.Add(Primitives[i].sPoint);
                }
            else { // Перемещение примтива
                Points.Clear();
                Primitive tempPrim = GetPrimByName(name_item_ComBox_Prim);
                Point new_fpoint = new Point(tempPrim.fPoint.X - speed_point.X, tempPrim.fPoint.Y);
                Point new_spoint = new Point(tempPrim.sPoint.X - speed_point.X, tempPrim.sPoint.Y);

                int index_prim = Primitives.IndexOf(tempPrim);
                Primitives[index_prim] = Primitives[index_prim] with { fPoint = new_fpoint, sPoint = new_spoint };
                Points.Add(Primitives[index_prim].fPoint);
                Points.Add(Primitives[index_prim].sPoint);
            }
        }

        // Перемещение набора примитивов вправо
        if (args.Key == Key.D) {
            Points.Clear();
            if (!isEditingModePrim)
                for (int i = 0; i < Primitives.Count(); i++) {
                    Point new_fpoint = new Point(Primitives[i].fPoint.X + speed_point.X, Primitives[i].fPoint.Y);
                    Point new_spoint = new Point(Primitives[i].sPoint.X + speed_point.X, Primitives[i].sPoint.Y);
                    Primitives[i] = Primitives[i] with { fPoint = new_fpoint, sPoint = new_spoint };
                    Points.Add(Primitives[i].fPoint);
                    Points.Add(Primitives[i].sPoint);
                }
            else { // Перемещение примитива
                Points.Clear();
                Primitive tempPrim = GetPrimByName(name_item_ComBox_Prim);
                Point new_fpoint = new Point(tempPrim.fPoint.X + speed_point.X, tempPrim.fPoint.Y);
                Point new_spoint = new Point(tempPrim.sPoint.X + speed_point.X, tempPrim.sPoint.Y);

                int index_prim = Primitives.IndexOf(tempPrim);
                Primitives[index_prim] = Primitives[index_prim] with { fPoint = new_fpoint, sPoint = new_spoint };
                Points.Add(Primitives[index_prim].fPoint);
                Points.Add(Primitives[index_prim].sPoint);
            }

        }

    }

    private CollectionPrimitives GetCollPrimByName(string name)
    {
        foreach (var item in CollPrimitives)
            if (item.Name.Equals(name))
                return item;
        return new CollectionPrimitives();
    }

    private Primitive GetPrimByName(string name) {
        foreach (var item in Primitives)
            if (item.Name.Equals(name))
                return item;
        return new Primitive();
    }

    private string GetTypeLine(TypeLine type) {
        return type switch
        {
            TypeLine.ORDINARY    => "Обычный",
            TypeLine.POINT       => "Точечный",
            TypeLine.DASHED      => "Штриховой",
            TypeLine.DASHEDPOINT => "Штрихпунктирный",
            _                    => "Обычный"
        };
    }

    private void ComboBoxCollPrimitives_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            
        if (!String.IsNullOrEmpty(ComboBoxCollPrimitives.SelectedValue?.ToString())) {
            name_item_ComBox_CollPrim = ComboBoxCollPrimitives.SelectedValue.ToString()!;
            isEditingModeColPrim = true;
            isEditingModePrim = false;
            isEditingModePoint = false;
            ComboBoxPrimitives.Items.Clear();
            ComboBoxPoints.Items.Clear();
            ComboBoxPrimitives.IsEnabled = true;
            ComboBoxPoints.IsEnabled = false;
            DeleteCollPrimitives.IsEnabled = true;
            SliderLineWidth.IsEnabled = true;
            ComboBoxTypeLine.IsEnabled = true;

            List<Primitive> tempPrims = GetCollPrimByName(name_item_ComBox_CollPrim).Primitives;
            Primitives = new List<Primitive>(tempPrims);
            Points = new List<Point>();
            foreach (var item in tempPrims) {
                ComboBoxPrimitives.Items.Add(item.Name);
                Points.Add(item.fPoint with { color = DefColor });
                Points.Add(item.sPoint with { color = DefColor });
            }
        }
    }

    private void ComboBoxPrimitives_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        if (!String.IsNullOrEmpty(ComboBoxPrimitives.SelectedValue?.ToString())) {
            name_item_ComBox_Prim = ComboBoxPrimitives.SelectedValue.ToString()!;

            isEditingModePrim = true;
            isEditingModePoint = false;
            ComboBoxPoints.Items.Clear();
            ComboBoxPoints.IsEnabled = true;
            SliderLineWidth.IsEnabled = true;
            ComboBoxTypeLine.IsEnabled = true;

            List<Primitive> tempPrims = GetCollPrimByName(name_item_ComBox_CollPrim).Primitives;
            Primitives = new List<Primitive>(tempPrims);
            Primitive tempPrim = GetPrimByName(name_item_ComBox_Prim);
            TextBoxLineWidth.Text = tempPrim.LineWidth.ToString("F1", CultureInfo.GetCultureInfo("en-US"));
            SliderLineWidth.Value = tempPrim.LineWidth;
            ComboBoxTypeLine.Text = GetTypeLine(tempPrim.type);
            Points = new List<Point>();
            Points.Add(tempPrim.fPoint with { color = DefColor });
            Points.Add(tempPrim.sPoint with { color = DefColor });

            ComboBoxPoints.Items.Add("fpoint");
            ComboBoxPoints.Items.Add("spoint");
        }
    }

    private void ComboBoxPoints_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!String.IsNullOrEmpty(ComboBoxPoints.SelectedValue?.ToString()))
        {
            name_item_comBox_Point = ComboBoxPoints.SelectedValue.ToString()!;

            isEditingModePoint = true;
            SliderLineWidth.IsEnabled = false;
            ComboBoxTypeLine.IsEnabled = false;

            switch (name_item_comBox_Point) {
                case "fpoint":
                    Points[0] = Points[0] with { color = SigColor };
                    Points[1] = Points[1] with { color = DefColor };
                    break;

                case "spoint":
                    Points[0] = Points[0] with { color = DefColor };
                    Points[1] = Points[1] with { color = SigColor };
                    break;
            }

        }
    }

    private void DeletePrimitive_Click(object sender, RoutedEventArgs e) {
        if (Primitives.Any()) {
            if (!isEditingModePrim && isCreateColPrim) {
                Primitives.Remove(Primitives[^1]);
                Points.Remove(Points[^1]);
                Points.Remove(Points[^1]);
            }
            else if (!String.IsNullOrEmpty(ComboBoxPrimitives.SelectedValue?.ToString())) {
                Primitive temp_prim = GetPrimByName(name_item_ComBox_Prim);
                Primitives.Remove(temp_prim);
                Points.Remove(Points[^1]);
                Points.Remove(Points[^1]);
                ComboBoxPrimitives.Items.RemoveAt(ComboBoxPrimitives.SelectedIndex);
                GetCollPrimByName(name_item_ComBox_CollPrim).Primitives.Remove(temp_prim);
                ComboBoxPoints.Items.Clear();
                isEditingModePrim = false;
                isEditingModePoint = false;
                SliderLineWidth.IsEnabled = true;
                ComboBoxTypeLine.IsEnabled = true;
            }
        }
        openGLControl1.Focus();
    }

    private void DeleteCollPrimitives_Click(object sender, RoutedEventArgs e) {
        if (CollPrimitives.Any()) {
            CollectionPrimitives coll_prim = GetCollPrimByName(name_item_ComBox_CollPrim);
            CollPrimitives.Remove(coll_prim);
            Primitives.Clear();
            Points.Clear();
            ComboBoxCollPrimitives.Items.RemoveAt(ComboBoxCollPrimitives.SelectedIndex);
            ComboBoxPrimitives.Items.Clear();
            ComboBoxPoints.Items.Clear();
            ComboBoxPoints.IsEnabled = false;
            DeleteCollPrimitives.IsEnabled = false;
            isEditingModeColPrim = false;
            isEditingModePrim = false;
            isEditingModePoint = false;
            SliderLineWidth.IsEnabled = true;
            ComboBoxTypeLine.IsEnabled = true;
        }
        openGLControl1.Focus();
    }

    private void SliderLineWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        if (TextBoxLineWidth is not null) { 
            string STRlineWidth = e.NewValue.ToString("F1", CultureInfo.GetCultureInfo("en-US"));
            TextBoxLineWidth.Text = STRlineWidth;
            lineWidth = Convert.ToSingle(STRlineWidth.Replace('.', ','));
        }

        if (isCreateColPrim && !isEditingModePrim && isEditingModeColPrim)
        {
            for (int i = 0; i < Primitives.Count; i++)
                Primitives[i] = Primitives[i] with { LineWidth = lineWidth };
            return;
        }

        if (isEditingModePrim && isCreateColPrim) {
            Primitive temp_prim = GetPrimByName(name_item_ComBox_Prim);
            int index = Primitives.IndexOf(temp_prim);
            Primitives[index] = Primitives[index] with { LineWidth = lineWidth };
            return;
        }


    }

    private void ComboBoxTypeLine_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (!String.IsNullOrEmpty(ComboBoxTypeLine.SelectedValue?.ToString()))
        {
            var strContent = ((ComboBoxItem)ComboBoxTypeLine.SelectedValue).Content.ToString();
            typeLine = strContent switch
            {
                "Обычный"         => TypeLine.ORDINARY,
                "Точечный"        => TypeLine.POINT,
                "Штриховой"       => TypeLine.DASHED,
                "Штрихпунктирный" => TypeLine.DASHEDPOINT,
                _                 => TypeLine.ORDINARY
            };
        }

        if (isCreateColPrim && !isEditingModePrim && isEditingModeColPrim) { 
            for (int i = 0; i < Primitives.Count; i++)
                Primitives[i] = Primitives[i] with { type = typeLine };
            return;
        }

        if (isEditingModePrim && isCreateColPrim)
        {
            Primitive temp_prim = GetPrimByName(name_item_ComBox_Prim);
            int index = Primitives.IndexOf(temp_prim);
            Primitives[index] = Primitives[index] with { type = typeLine };
            return;
        }

    }

    private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e) {
        curColor = new Color((byte)ColorPicker.Color.RGB_R, (byte)ColorPicker.Color.RGB_G, (byte)ColorPicker.Color.RGB_B);

        if (isCreateColPrim && !isEditingModePrim && isEditingModeColPrim)
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
            Primitive temp_prim = GetPrimByName(name_item_ComBox_Prim);
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
            Primitive temp_prim = GetPrimByName(name_item_ComBox_Prim);
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

}
