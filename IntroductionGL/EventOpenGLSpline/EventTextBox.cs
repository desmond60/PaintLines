namespace IntroductionGL;

//: Обработчики TextBox
public partial class OpenGLSpline : Window
{

    //: TextBox только цифры и точка и минус
    private void LightPreviewTextInput(object sender, TextCompositionEventArgs e) {
        // Добавляем регулярное выражение
        var regex = new Regex("[^0-9.-]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    //: TextBox только цифры
    private void LightPreviewTextInputInt(object sender, TextCompositionEventArgs e) {
        // Добавляем регулярное выражение
        var regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    //: TextBox на изменение количества требуемых точек
    private void CountControlPoint_TextChanged(object sender, TextChangedEventArgs e) {

        // Пересчитываем сплайн если порядок базисных функций меняется

        // Если TextBox пуст, (по умолчанию порядок 3)
        if (CountControlPoint.Text == String.Empty) {
            OrderBasicFunction = 3;
            CalculationSpline();
            return;
        }

        // Если TextBox не пуст
        OrderBasicFunction = int.Parse(CountControlPoint.Text);
        CalculationSpline();
    }

    //: Обработчик изменения веса активной точки
    private void InitWeightsPoint_TextChanged(object sender, TextChangedEventArgs e) {

        // Если TextBox не пуст пересчитываем сплайн
        if (InitWeightsPoint.Text != String.Empty) {
            ControlPoint[ActivePointIndex] = ControlPoint[ActivePointIndex] with { w = ActiveWeightPoint };
            ScreenPoint[ActivePointIndex] = ScreenPoint[ActivePointIndex] with { w = ActiveWeightPoint };
            CalculationSpline();
        }
    }
}

