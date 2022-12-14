namespace IntroductionGL;

//: Обработчики мыши
public partial class OpenGLSpline : Window
{

    //: Обработчик колесика мыши
    private void openGLControl3D_MouseWheel(object sender, MouseWheelEventArgs e) {

        if (IsEditModePoint) {
            // Увеличение и уменьшение веса
            if (e.Delta > 0) {
                if (ActiveWeightPoint - 1f > 1e-7)
                    ActiveWeightPoint -= 1f;
            }
            else
                ActiveWeightPoint += 1f;
            InitWeightsPoint.Text = ActiveWeightPoint.ToString("F1");
            return;
        }


        // Увеличение и уменьшение масштаба
        float value = Single.Parse(ValueScale.Text);
        if (e.Delta > 0) {
            if (Scale - value > 1e-7)
                Scale -= value;
        }
        else 
            Scale += value;

        // Находится здесь потому что, если мы меняем масштаб, нужно пересчитывать экранные координаты
        ScreenPoint.Clear();
        for (int i = 0; i < ControlPoint.Count; i++)
            ScreenPoint.Add(ConvertCoordinateToScreen(ControlPoint[i]));
        
        // Пересчитываем сплайн
        CalculationSpline();
    }

    //: Обработчик передвижения мыши
    private void openGLControl3D_MouseMove(object sender, MouseEventArgs e) {
        
        // Вычисление позиции мыши на окошке OpenGl
        MousePos = new Point(
            (float)e.GetPosition(openGLControl3D).X - 26, 
            (float)(openGLControl3D.ActualHeight - e.GetPosition(openGLControl3D).Y - 10)
        );

        if (IsEditModePoint) {

            // Показываем новые точки
            ControlPoint[ActivePointIndex] = ConvertCoordinateToReal(MousePos) with { w = ActiveWeightPoint };
            ScreenPoint[ActivePointIndex] = ConvertCoordinateToScreen(ControlPoint[ActivePointIndex]) with { w = ActiveWeightPoint };

            // Показываем новый сплайн
            CalculationSpline();
            return;
        }

        // Определяем стоим ли мы на точке
        (IsActivePoint, ActivePointIndex) = GetActivePoint(ConvertCoordinateToReal(MousePos));
    }

    //: Обработчик нажатия машки
    private void openGLControl3D_MouseDown(object sender, MouseButtonEventArgs e) {

        // Добавление контрольной точки и экранной координаты этой точки
        if (e.LeftButton == MouseButtonState.Pressed) {

            // Если это редактиование точки
            if (IsEditModePoint) {
                
                // Запоминаем новое место активной точки
                ControlPoint[ActivePointIndex] = ConvertCoordinateToReal(MousePos) with { w = ActiveWeightPoint };
                ScreenPoint[ActivePointIndex] = ConvertCoordinateToScreen(ControlPoint[ActivePointIndex]) with { w = ActiveWeightPoint };
                
                // Запоминаем новый сплайн
                CalculationSpline();

                // Сбрасываем на всякий случай
                ActivePointIndex = 0;
                IsActivePoint = false;
                IsEditModePoint = false;
                InitWeightsPoint.IsEnabled = false;
                InitWeightsPoint.Text = String.Empty;
                return;
            }

            // Если находимся на активной точке
            if (IsActivePoint) {
                InitWeightsPoint.IsEnabled = true;
                ActiveWeightPoint = ControlPoint[ActivePointIndex].w;
                InitWeightsPoint.Text = ControlPoint[ActivePointIndex].w.ToString("F1");
                IsEditModePoint = true;
                return;
            }

            // Добавялем новую точку
            ControlPoint.Add(ConvertCoordinateToReal(MousePos));
            ScreenPoint.Add(ConvertCoordinateToScreen(ControlPoint[^1]));
            
            // Строим сплайн
            CalculationSpline();
        }
    }

    //: Выявить находится ли мышка на какой-нибудь точке
    private (bool, int) GetActivePoint(Point mousePos) {

        int delta = 6; // Площадь попадания возле точки
        Point point = ConvertCoordinateToScreen(mousePos); // Точка для рассмотрения

        // Проходимся по всем экранным точкам
        for (int i = 0; i < ScreenPoint.Count; i++) {
            if (point.X < ScreenPoint[i].X + delta &&
                point.X > ScreenPoint[i].X - delta &&
                point.Y < ScreenPoint[i].Y + delta &&
                point.Y > ScreenPoint[i].Y - delta)
            {
                ScreenPoint[i] = ScreenPoint[i] with { color = new Color(255, 153, 0, 255) };
                return (true, i);
            }
            ScreenPoint[i] = ScreenPoint[i] with { color = new Color(100, 100, 100, 255) };
        }

        return (false, 0);
    }

}