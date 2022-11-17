namespace IntroductionGL;

//: Обработчики Mouse
public partial class OpenGL2D_2 : Window
{
    public List<Point> TempPoints = new List<Point>();   // Точки созданного примитивов
    public bool isCreateColPrim = false;                 // Создан ли примитив?.

    //: Обработчик мыши
    private void OpenGLControl_MouseDown(object sender, MouseButtonEventArgs e) {

        if (e.LeftButton == MouseButtonState.Pressed)
        {
            // Вычисляем координаты точки нажати и вмещяем в диапозон (0,1)
            Point point = new Point((float)e.GetPosition(openGLControl2D).X, (float)e.GetPosition(openGLControl2D).Y);
            point = Primitive_Coordinate(point, openGLControl2D.ActualWidth, openGLControl2D.ActualHeight);

            // Добавление точки
            TempPoints.Add(point);
            Points.Add(point);

            isCreateColPrim = false;                // False: примитив не создан до конца
            ComboBoxPrimitives.IsEnabled = false;   // Откл. ComboBox кол. примтивов, т.к. примитив не создан до конца

            // 1 точка - центр пятиугольника. 2 точка - конец радиуса описанной окружности
            if (TempPoints.Count == 2) {

            // Создание примитива
                Points.Clear();
                Point[] points = new Point[5];
                var R = Sqrt(Pow(TempPoints[1].X - TempPoints[0].X, 2) + Pow(TempPoints[1].Y - TempPoints[0].Y, 2));
                for (int i = 0; i < 5; i++) { 
                    points[i] = new Point((float)(TempPoints[0].X + R * Sin(i * (2 * PI) / 5.0)), (float)(TempPoints[0].Y + R * Cos(i * (2 * PI) / 5.0)));
                    points[i].color = curColor;
                }

                if (Primitives.Any()) // Если создан первый примитив
                    Primitives.Add(new PrimitiveFiveRect(points, $"Primitive_{Convert.ToInt32(Primitives[^1].Name.Split("_")[1]) + 1}"));
                else
                    Primitives.Add(new PrimitiveFiveRect(points, $"Primitive_{Primitives.Count() + 1}"));

                // Добавление созданного примитива в ComboBoxPrim
                ComboBoxPrimitives.Items.Add(Primitives[^1].Name);

                TempPoints.Clear();
                isCreateColPrim = true;                 // True: примитив создан
                ComboBoxPrimitives.IsEnabled = true;    // Вкл. ComboBox кол. примитивов, т.к. примитив создан до конца
            }
            return;
        }
    }
}

