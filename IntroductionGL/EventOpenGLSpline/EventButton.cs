namespace IntroductionGL;

//: Обработчики Button
public partial class OpenGLSpline : Window {

    //: Обработчки кнопки переключения окон
    private void SwitchWindow_Click(object sender, RoutedEventArgs e) {
        MainWindow window = new MainWindow();
        window.Show();
        this.Close();
    }

    //: Обработчик кнопки изменения масштаба "+"
    private void IncreaseScale_Click(object sender, RoutedEventArgs e) {
        
        // Увеличение и уменьшение масштаба
        float value = Single.Parse(ValueScale.Text);
        if (Scale - value > 1e-7)
            Scale -= value;

        // Находится здесь потому что, если мы меняем масштаб, нужно пересчитывать экранные координаты
        ScreenPoint.Clear();
        for (int i = 0; i < ControlPoint.Count; i++)
            ScreenPoint.Add(ConvertCoordinateToScreen(ControlPoint[i]));

        // Пересчитываем сплайн
        CalculationSpline();
    }

    //: Обработчик кнопки изменения масштаба "-"
    private void DecreaseScale_Click(object sender, RoutedEventArgs e) {
        // Увеличение и уменьшение масштаба
        float value = Single.Parse(ValueScale.Text);
        Scale += value;

        // Находится здесь потому что, если мы меняем масштаб, нужно пересчитывать экранные координаты
        ScreenPoint.Clear();
        for (int i = 0; i < ControlPoint.Count; i++)
            ScreenPoint.Add(ConvertCoordinateToScreen(ControlPoint[i]));

        // Пересчитываем сплайн
        CalculationSpline();
    }

    //: Обработчик кнопки "Удлаить сплайн"
    private void DeleteSpline_Click(object sender, RoutedEventArgs e) {
        Spline.Clear();
        ControlPoint.Clear();
        ScreenPoint.Clear();
    }
}