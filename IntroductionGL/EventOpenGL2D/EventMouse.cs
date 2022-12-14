namespace IntroductionGL;

//: Обработчики Mouse
public partial class OpenGL2D : Window {

    public List<Point> TempPoints = new List<Point>();   // Точки созданного примитивов
    public bool isCreateColPrim = false;                 // Создан ли примитив?

    //: Обработчик нажатия мыши на окошко OpenGL
    private void OpenGLControl_MouseDown(object sender, MouseButtonEventArgs e) {

        // Если не включен режим редактирования
        if (!isEditingModePrim)
            if (e.LeftButton == MouseButtonState.Pressed) {
                // Вычисляем координаты точки нажати и вмещяем в диапозон (0,1)
                Point point = new Point((float)e.GetPosition(openGLControl2D).X, (float)e.GetPosition(openGLControl2D).Y);
                point = Primitive_Coordinate(point, openGLControl2D.ActualWidth, openGLControl2D.ActualHeight);

                // Добавление точки
                TempPoints.Add(point with { color = curColor });
                Points.Add(point);

                isCreateColPrim = false;                    // False: примитив не создан до конца
                ComboBoxCollPrimitives.IsEnabled = false;   // Откл. ComboBox кол. примтивов, т.к. примитив не создан до конца

                if (TempPoints.Count == 2) {

                    // Создание примитива
                    if (Primitives.Any()) // Если создан первый примитив
                        Primitives.Add(new Primitive(TempPoints[0], TempPoints[1], $"Primitive_{Convert.ToInt32(Primitives[^1].Name.Split("_")[1]) + 1}"));
                    else
                        Primitives.Add(new Primitive(TempPoints[0], TempPoints[1], $"Primitive_{Primitives.Count() + 1}"));
                    Primitives[^1] = Primitives[^1] with
                    {
                        LineWidth = lineWidth,
                        type = typeLine,
                    };

                    TempPoints.Clear();
                    isCreateColPrim                  = true;    // True: примитив создан
                    ComboBoxCollPrimitives.IsEnabled = true;    // Вкл. ComboBox кол. примитивов, т.к. примитив создан до конца

                    // Если включен режим редак. кол. примитивов и отключен режим редак. примитива
                    // Добавить новый примитив в сущ. коллекцию
                    if (isEditingModeColPrim && !isEditingModePrim) {
                        ComboBoxPrimitives.Items.Add(Primitives[^1].Name);
                        Points[^1] = Primitives[^1].fPoint with { color = DefColor };
                        Points[^2] = Primitives[^1].sPoint with { color = DefColor };
                        int index = CollPrimitives.IndexOf(CollPrimitives.Find(s => s.Name == name_item_ComBox_CollPrim));
                        CollPrimitives[index] = CollPrimitives[index] with { Primitives = Primitives };
                    }
                }
                return;
            }

        // Если включен режим редактирования точки
        if (isEditingModePoint) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                // Вычисляем координаты точки нажати и вмещяем в диапозон (0,1)
                Point point = new Point((float)e.GetPosition(openGLControl2D).X, (float)e.GetPosition(openGLControl2D).Y);
                point = Primitive_Coordinate(point, openGLControl2D.ActualWidth, openGLControl2D.ActualHeight);

                // Выбранный примитив
                Primitive temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
                int index = Primitives.IndexOf(temp_prim);
                Primitives[index] = name_item_comBox_Point switch
                {
                    "fpoint" => Primitives[index] with { fPoint = point with { color = temp_prim.fPoint.color } },
                    "spoint" => Primitives[index] with { sPoint = point with { color = temp_prim.sPoint.color } },
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

}
