namespace IntroductionGL;

public partial class OpenGL2D : Window {

    //: Обработчк Slider изменения ширины линии
    private void SliderLineWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
        
        // Заменяем текст в TextBox
        if (TextBoxLineWidth is not null) {
            string STRlineWidth = e.NewValue.ToString("F1", CultureInfo.GetCultureInfo("en-US"));
            TextBoxLineWidth.Text = STRlineWidth;
            lineWidth = Convert.ToSingle(STRlineWidth.Replace('.', ','));
        }

        // Если вкл. режим редактирования набора примитивов
        if (isCreateColPrim && !isEditingModePrim && isEditingModeColPrim && CanvasColPrim.Visibility == Visibility.Visible) {
            for (int i = 0; i < Primitives.Count; i++)
                Primitives[i] = Primitives[i] with { LineWidth = lineWidth };
            return;
        }

        // Если вкл. режим редактирования примтива
        if (isEditingModePrim && isCreateColPrim) {
            Primitive temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim); ;
            int index = Primitives.IndexOf(temp_prim);
            Primitives[index] = Primitives[index] with { LineWidth = lineWidth };
            return;
        }

        // Изменение последнего нарисованного примитива
        if (Primitives.Any() && !isEditingModeColPrim) {
            Primitives[^1] = Primitives[^1] with { LineWidth = lineWidth };
            return;
        }
    }
}