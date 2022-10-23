using System.Diagnostics;
using System.Drawing;

namespace IntroductionGL;

//: События ComboBox
public partial class OpenGL2D_2 : Window {

    //: Обработка comboBoxPrimitive
    private void ComboBoxPrimitives_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        // Если ComboBox не пуст
        if (!String.IsNullOrEmpty(ComboBoxPrimitives.SelectedValue?.ToString()))
        {
            InformationBlock.Text = $"Включен режим редактирования примитива (Выбран примитив \"{ComboBoxPrimitives.SelectedValue}\")";

            // Очистка других ComboBox + добавление названий вершин
            ComboBoxPointPrim.Items.Clear();
            for (int i = 0; i < 5; i++)
                ComboBoxPointPrim.Items.Add($"point{i+1}");

            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
            isEditingModePrim = true;
            isEditingModePoint = false;

            ComboBoxPointPrim.IsEnabled = true;
            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */

            // Выбранный примитив
            name_item_ComBox_Prim = ComboBoxPrimitives.SelectedValue.ToString()!;
            PrimitiveFiveRect tempPrim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);

            // Установление данных на панель конкретного примитива
            Points = new List<Point>();
            for (int i = 0; i < tempPrim.points.Count(); i++)
                Points.Add(tempPrim.points[i] with { color = DefColor });
        }

    }

    //: Обработчик ComboBox точек примитива
    private void ComboBoxPointPrim_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        // Если ComboBox не пуст
        if (!String.IsNullOrEmpty(ComboBoxPointPrim.SelectedValue?.ToString()))
        {
            InformationBlock.Text = $"Включен режим редактирования вершины примтива (Выбрана вершина \"{ComboBoxPointPrim.SelectedValue}\")";

            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
            isEditingModePoint = true;
            /* ------------------ Откл. и Вкл. компонент приложения ----------------- */

            // Выбранная вершина
            name_item_comBox_Point = ComboBoxPointPrim.SelectedValue.ToString()!;
            
            // Выделяем вершины
            for (int i = 0; i < 5; i++) {
                Points[i] = Points[i] with { color = DefColor };
                if (Convert.ToInt32(name_item_comBox_Point[^1].ToString()) == i + 1)
                    Points[i] = Points[i] with { color = SigColor };
            }
        }
    }
}