namespace IntroductionGL;

//: Обработчкики клавиш
public partial class OpenGL2D : Window
{

    //: Обработка нажатий клавиш
    private void Grid_KeyDown(object sender, KeyEventArgs args) {

        // Если нажата клавиша Space
        if (args.Key == Key.Space) {
            // Если не режим редкатирования набора притива
            if (!isEditingModeColPrim) {
                // Если есть хотя бы один притив и примитив создан 
                if (Primitives.Count() > 0 && isCreateColPrim) {
                    // Добавление набора
                    CollectionPrimitives item_col_prim = new CollectionPrimitives(Primitives);
                    ComboBoxCollPrimitives.Items.Add(NameCollPrim(CollPrimitives, ref item_col_prim));
                    CollPrimitives.Add(item_col_prim);
                    InformationBlock.Text = $"Включен режим создание набора примитив (Создан набор \"{item_col_prim.Name}\")";

                    // Очищение листов текущего набора
                    Points.Clear();
                    Primitives.Clear();

                    /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
                    isEditingModeColPrim = false;
                    isEditingModePrim    = false;
                    isEditingModePoint   = false;

                    DeleteCollPrimitives.IsEnabled = false;

                    SliderLineWidth.IsEnabled    = true;
                    ComboBoxTypeLine.IsEnabled   = true;
                    ButtonEditColPrim.IsEnabled  = false;
                    ButtonEditPrim.IsEnabled     = true;

                    CanvasColPrim.Visibility = Visibility.Hidden;
                    CanvasPrim.Visibility    = Visibility.Visible;
                    /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
                }
            }
            else { // Если идет редактирование набора
                // Если есть хотя бы один притив и примитив создан
                if (Primitives.Count() > 0 && isCreateColPrim) {
                    // Удалаяем выбранный набор
                    CollectionPrimitives item_col_prim = CollPrimitives.Find(s => s.Name == name_item_ComBox_CollPrim);
                    CollPrimitives.Remove(item_col_prim);

                    // Создаем новый
                    item_col_prim = new CollectionPrimitives(Primitives);

                    // Удалаем старое название 
                    if (item_col_prim.Primitives.Count != int.Parse(ComboBoxCollPrimitives.Text.Split('_')[1])) {
                        ComboBoxCollPrimitives.Items.Add(NameCollPrim(CollPrimitives, ref item_col_prim));
                        CollPrimitives.Add(item_col_prim);
                    }
                    else { 
                        item_col_prim = item_col_prim with { Name = ComboBoxCollPrimitives.Text };
                        CollPrimitives.Add(item_col_prim);
                        ComboBoxCollPrimitives.Items.Add(item_col_prim.Name);
                    }
                    InformationBlock.Text = $"Включен режим создания набора примитивов (Изменен набор примитивов \"{item_col_prim.Name}\")";

                    // Очищение листов текущего набора (+ ComboBox)
                    Points.Clear();
                    Primitives.Clear();
                    ComboBoxPrimitives.Items.Clear();
                    ComboBoxPoints.Items.Clear();
                    ComboBoxCollPrimitives.Items.RemoveAt(ComboBoxCollPrimitives.SelectedIndex);

                    /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
                    isEditingModeColPrim = false;
                    isEditingModePrim    = false;
                    isEditingModePoint   = false;
                    
                    ComboBoxPrimitives.IsEnabled   = false;
                    ComboBoxPoints.IsEnabled       = false;
                    DeleteCollPrimitives.IsEnabled = false;
                    ButtonEditColPrim.IsEnabled    = false;

                    SliderLineWidth.IsEnabled    = true;
                    ComboBoxTypeLine.IsEnabled   = true;
                    ButtonEditPrim.IsEnabled     = true;

                    CanvasColPrim.Visibility = Visibility.Hidden;
                    CanvasPrim.Visibility    = Visibility.Visible;
                    /* ------------------ Откл. и Вкл. компонент приложения ----------------- */
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
                    Primitives[i] = Primitives[i] with 
                    { 
                        fPoint = new_fpoint with { color = Primitives[i].fPoint.color }, 
                        sPoint = new_spoint with { color = Primitives[i].sPoint.color }
                    };
                    Points.Add(Primitives[i].fPoint with { color = DefColor });
                    Points.Add(Primitives[i].sPoint with { color = DefColor });
                }
            else { // Перемещение примитива
                Points.Clear();
                Primitive tempPrim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
                Point new_fpoint = new Point(tempPrim.fPoint.X, tempPrim.fPoint.Y + speed_point.Y);
                Point new_spoint = new Point(tempPrim.sPoint.X, tempPrim.sPoint.Y + speed_point.Y);

                int index_prim = Primitives.IndexOf(tempPrim);
                Primitives[index_prim] = Primitives[index_prim] with 
                { 
                    fPoint = new_fpoint with { color = Primitives[index_prim].fPoint.color }, 
                    sPoint = new_spoint with { color = Primitives[index_prim].sPoint.color }
                };
                Points.Add(Primitives[index_prim].fPoint with { color = DefColor });
                Points.Add(Primitives[index_prim].sPoint with { color = DefColor });
            }
        }

        // Перемещение набора примитивов вниз
        if (args.Key == Key.S) {
            Points.Clear();
            if (!isEditingModePrim)
                for (int i = 0; i < Primitives.Count(); i++) {
                    Point new_fpoint = new Point(Primitives[i].fPoint.X, Primitives[i].fPoint.Y - speed_point.Y);
                    Point new_spoint = new Point(Primitives[i].sPoint.X, Primitives[i].sPoint.Y - speed_point.Y);
                    Primitives[i] = Primitives[i] with 
                    { 
                        fPoint = new_fpoint with { color = Primitives[i].fPoint.color }, 
                        sPoint = new_spoint with { color = Primitives[i].sPoint.color }
                    };
                    Points.Add(Primitives[i].fPoint with { color = DefColor });
                    Points.Add(Primitives[i].sPoint with { color = DefColor });
                }
            else { // Перемещение примитива
                Points.Clear();
                Primitive tempPrim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
                Point new_fpoint = new Point(tempPrim.fPoint.X, tempPrim.fPoint.Y - speed_point.Y);
                Point new_spoint = new Point(tempPrim.sPoint.X, tempPrim.sPoint.Y - speed_point.Y);

                int index_prim = Primitives.IndexOf(tempPrim);
                Primitives[index_prim] = Primitives[index_prim] with 
                { 
                    fPoint = new_fpoint with { color = Primitives[index_prim].fPoint.color }, 
                    sPoint = new_spoint with { color = Primitives[index_prim].sPoint.color }
                };
                Points.Add(Primitives[index_prim].fPoint with { color = DefColor });
                Points.Add(Primitives[index_prim].sPoint with { color = DefColor });
            }
        }

        // Перемещение набора примитивов влево
        if (args.Key == Key.A) {
            Points.Clear();
            if (!isEditingModePrim)
                for (int i = 0; i < Primitives.Count(); i++) {
                    Point new_fpoint = new Point(Primitives[i].fPoint.X - speed_point.X, Primitives[i].fPoint.Y);
                    Point new_spoint = new Point(Primitives[i].sPoint.X - speed_point.X, Primitives[i].sPoint.Y);
                    Primitives[i] = Primitives[i] with 
                    { 
                        fPoint = new_fpoint with { color = Primitives[i].fPoint.color }, 
                        sPoint = new_spoint with { color = Primitives[i].sPoint.color }
                    };
                    Points.Add(Primitives[i].fPoint with { color = DefColor });
                    Points.Add(Primitives[i].sPoint with { color = DefColor });
                }
            else { // Перемещение примтива
                Points.Clear();
                Primitive tempPrim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
                Point new_fpoint = new Point(tempPrim.fPoint.X - speed_point.X, tempPrim.fPoint.Y);
                Point new_spoint = new Point(tempPrim.sPoint.X - speed_point.X, tempPrim.sPoint.Y);

                int index_prim = Primitives.IndexOf(tempPrim);
                Primitives[index_prim] = Primitives[index_prim] with 
                { 
                    fPoint = new_fpoint with { color = Primitives[index_prim].fPoint.color }, 
                    sPoint = new_spoint with { color = Primitives[index_prim].sPoint.color }
                };
                Points.Add(Primitives[index_prim].fPoint with { color = DefColor });
                Points.Add(Primitives[index_prim].sPoint with { color = DefColor });
            }
        }

        // Перемещение набора примитивов вправо
        if (args.Key == Key.D) {
            Points.Clear();
            if (!isEditingModePrim)
                for (int i = 0; i < Primitives.Count(); i++) {
                    Point new_fpoint = new Point(Primitives[i].fPoint.X + speed_point.X, Primitives[i].fPoint.Y);
                    Point new_spoint = new Point(Primitives[i].sPoint.X + speed_point.X, Primitives[i].sPoint.Y);
                    Primitives[i] = Primitives[i] with 
                    { 
                        fPoint = new_fpoint with { color = Primitives[i].fPoint.color },
                        sPoint = new_spoint with { color = Primitives[i].sPoint.color }
                    };
                    Points.Add(Primitives[i].fPoint with { color = DefColor });
                    Points.Add(Primitives[i].sPoint with { color = DefColor });
                }
            else { // Перемещение примитива
                Points.Clear();
                Primitive tempPrim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
                Point new_fpoint = new Point(tempPrim.fPoint.X + speed_point.X, tempPrim.fPoint.Y);
                Point new_spoint = new Point(tempPrim.sPoint.X + speed_point.X, tempPrim.sPoint.Y);

                int index_prim = Primitives.IndexOf(tempPrim);
                Primitives[index_prim] = Primitives[index_prim] with 
                { 
                    fPoint = new_fpoint with { color = Primitives[index_prim].fPoint.color }, 
                    sPoint = new_spoint with { color = Primitives[index_prim].sPoint.color }
                };
                Points.Add(Primitives[index_prim].fPoint with { color = DefColor });
                Points.Add(Primitives[index_prim].sPoint with { color = DefColor });
            }

        }
    }
}


