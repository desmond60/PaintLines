namespace IntroductionGL;


public partial class MainWindow : Window {

    //: Обработчки кнопки "Удаление примитива"
    private void DeletePrimitive_Click(object sender, RoutedEventArgs e) {
        
        // Если есть хотя бы один примитив
        if (Primitives.Any()) {

            // Если ComboBox не пуст
            if (!String.IsNullOrEmpty(ComboBoxPrimitives.SelectedValue?.ToString())) {
                
                // Удаление примитива
                Primitive temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
                Primitives.Remove(temp_prim);
                Points.Remove(Points[^1]);
                Points.Remove(Points[^1]);
                ComboBoxPrimitives.Items.RemoveAt(ComboBoxPrimitives.SelectedIndex);
                CollPrimitives.Find(s => s.Name == name_item_ComBox_CollPrim).Primitives.Remove(temp_prim);
                
                // Очистка ComboBox
                ComboBoxPoints.Items.Clear();

                /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
                isEditingModePrim         = false;
                isEditingModePoint        = false;
                SliderLineWidth.IsEnabled = true;

                ComboBoxTypeLine.IsEnabled = true;
                /* ------------------ Откл. и Вкл. компонент приложения ----------------- */

                // Отображение точек примитива, т.к. вкл. режим редактирования набора
                foreach (var item in Primitives) {
                    Points.Add(item.fPoint with { color = DefColor });
                    Points.Add(item.sPoint with { color = DefColor });
                }
            }
            // Удаление последнего приитива при создании или редактировании набора
            else if (!isEditingModePrim && isCreateColPrim && isEditingModeColPrim) {
                ComboBoxPrimitives.Items.Remove(Primitives[^1].Name);
                Primitives.Remove(Primitives[^1]);
                Points.Remove(Points[^1]);
                Points.Remove(Points[^1]);
                int index = CollPrimitives.IndexOf(CollPrimitives.Find(s => s.Name == name_item_ComBox_CollPrim));
                CollPrimitives[index] = CollPrimitives[index] with { Primitives = Primitives };
            }
            else if (!isEditingModePrim && isCreateColPrim && !isEditingModeColPrim) {
                Primitives.Remove(Primitives[^1]);
                Points.Remove(Points[^1]);
                Points.Remove(Points[^1]);
            }
        }
    }

    //: Обработчки кнопки "Удаление набора примитивов"
    private void DeleteCollPrimitives_Click(object sender, RoutedEventArgs e) {
        
        // Если есть хотя бы один набор
        if (CollPrimitives.Any()) {
            
            // Удаление набора
            CollectionPrimitives coll_prim = CollPrimitives.Find(s => s.Name == name_item_ComBox_CollPrim);
            CollPrimitives.Remove(coll_prim);
            Primitives.Clear();
            Points.Clear();
            ComboBoxCollPrimitives.Items.RemoveAt(ComboBoxCollPrimitives.SelectedIndex);
            
            // Очистка ComboBox
            ComboBoxPrimitives.Items.Clear();
            ComboBoxPoints.Items.Clear();

            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
            isEditingModeColPrim = false;
            isEditingModePrim    = false;
            isEditingModePoint   = false;

            ComboBoxPoints.IsEnabled       = false;
            DeleteCollPrimitives.IsEnabled = false;

            SliderLineWidth.IsEnabled  = true;
            ComboBoxTypeLine.IsEnabled = true;
            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
        }
    }

}