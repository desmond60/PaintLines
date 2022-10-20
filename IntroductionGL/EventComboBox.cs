namespace IntroductionGL;

//: Обработчики ComboBox
public partial class MainWindow : Window {

    //: Обработка выбора в ComboBox набора примитивов
    private void ComboBoxCollPrimitives_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        
        // Если ComboBox не пуст
        if (!String.IsNullOrEmpty(ComboBoxCollPrimitives.SelectedValue?.ToString())) {
            InformationBlock.Text = $"Включен режим редактирования набора примитивов (Выбран набор примитивов \"{ComboBoxCollPrimitives.SelectedValue}\")";

            // Очистка других ComboBox
            ComboBoxPrimitives.Items.Clear();
            ComboBoxPoints.Items.Clear();

            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
            isEditingModeColPrim = true;
            isEditingModePrim    = false;
            isEditingModePoint   = false;

            ComboBoxPrimitives.IsEnabled   = true;
            DeleteCollPrimitives.IsEnabled = true;
            SliderLineWidth.IsEnabled      = true;
            ComboBoxTypeLine.IsEnabled     = true;
            RadioButtonColPrim.IsEnabled   = true;
            RadioButtonPrim.IsEnabled      = true;

            ComboBoxPoints.IsEnabled       = false;
            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */

            // Выбранный набор
            name_item_ComBox_CollPrim = ComboBoxCollPrimitives.SelectedValue.ToString()!;
            List<Primitive> tempPrims = CollPrimitives.Find(s => s.Name == name_item_ComBox_CollPrim).Primitives;
            Primitives = new List<Primitive>(tempPrims);
            Points = new List<Point>();
            foreach (var item in tempPrims) {
                ComboBoxPrimitives.Items.Add(item.Name);
                Points.Add(item.fPoint with { color = DefColor });
                Points.Add(item.sPoint with { color = DefColor });
            }
        }
    }

    //: Обработка выбора в ComboBox примитивов конкретного набора
    private void ComboBoxPrimitives_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        // Если ComboBox не пуст
        if (!String.IsNullOrEmpty(ComboBoxPrimitives.SelectedValue?.ToString())) {
            InformationBlock.Text = $"Включен режим редактирования примитива (Выбран примитив \"{ComboBoxPrimitives.SelectedValue}\")";

            // Очистка других ComboBox + добавление названий вершин
            ComboBoxPoints.Items.Clear();
            ComboBoxPoints.Items.Add("fpoint");
            ComboBoxPoints.Items.Add("spoint");

            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
            isEditingModePrim  = true;
            isEditingModePoint = false;

            ComboBoxPoints.IsEnabled     = true;
            SliderLineWidth.IsEnabled    = true;
            ComboBoxTypeLine.IsEnabled   = true;

            RadioButtonColPrim.IsEnabled = false;
            RadioButtonPrim.IsEnabled    = false;
            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */

            // Выбранный примитив
            name_item_ComBox_Prim = ComboBoxPrimitives.SelectedValue.ToString()!;
            List<Primitive> tempPrims = CollPrimitives.Find(s => s.Name == name_item_ComBox_CollPrim).Primitives;
            Primitives = new List<Primitive>(tempPrims);
            Primitive tempPrim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
            
            // Установление данных на панель конкретного примитива
            TextBoxLineWidth.Text = tempPrim.LineWidth.ToString("F1", CultureInfo.GetCultureInfo("en-US"));
            SliderLineWidth.Value = tempPrim.LineWidth;
            ComboBoxTypeLine.Text = GetTypeLine(tempPrim.type);
            Points = new List<Point>();
            Points.Add(tempPrim.fPoint with { color = DefColor });
            Points.Add(tempPrim.sPoint with { color = DefColor });
        }
    }

    //: Обработка выбора в ComboBox вершины конкретного примитива
    private void ComboBoxPoints_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        // Если ComboBox не пуст
        if (!String.IsNullOrEmpty(ComboBoxPoints.SelectedValue?.ToString())) {
            InformationBlock.Text = $"Включен режим редактирования вершины примтива (Выбрана вершина \"{ComboBoxPoints.SelectedValue}\")";

            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
            isEditingModePoint = true;
            SliderLineWidth.IsEnabled  = false;
            ComboBoxTypeLine.IsEnabled = false;
            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */

            // Выбранная вершина
            name_item_comBox_Point = ComboBoxPoints.SelectedValue.ToString()!;
            switch (name_item_comBox_Point)
            {
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

    //: Обработка выбора в ComboBox типа линии
    private void ComboBoxTypeLine_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        
        // Если ComboBox не пуст
        if (!String.IsNullOrEmpty(ComboBoxTypeLine.SelectedValue?.ToString())) {
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

        // Если вкл. режим редактирования набора примитива
        if (isCreateColPrim && !isEditingModePrim && isEditingModeColPrim && RadioButtonColPrim.IsChecked!.Value) {
            for (int i = 0; i < Primitives.Count; i++)
                Primitives[i] = Primitives[i] with { type = typeLine };
            return;
        }

        // Если вкл. режим редактирования примтитива
        if (isEditingModePrim && isCreateColPrim) {
            Primitive temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim); ;
            int index = Primitives.IndexOf(temp_prim);
            Primitives[index] = Primitives[index] with { type = typeLine };
            return;
        }

        // Изменение последнего нарисованного примитива
        if (Primitives.Any() && !isEditingModeColPrim)
        {
            Primitives[^1] = Primitives[^1] with { type = typeLine };
            return;
        }
    }
}