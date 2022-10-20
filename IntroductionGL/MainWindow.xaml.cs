using System.Diagnostics;

namespace IntroductionGL;

public partial class MainWindow : Window
{
    public MainWindow() {
        InitializeComponent();
    }

    private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e) {
        curColor = new Color((byte)ColorPicker.Color.RGB_R, (byte)ColorPicker.Color.RGB_G, (byte)ColorPicker.Color.RGB_B, (byte)ColorPicker.Color.A);
        
        // Фокус, чтобы когда нажимаем на стрелочку не терялся фокус на Opengl
        openGLControl2D.Focus();
        
        // Редактирование набора примитива
        if (isCreateColPrim && !isEditingModePrim && isEditingModeColPrim && RadioButtonColPrim.IsChecked!.Value)
        {
            for (int i = 0; i < Primitives.Count; i++)
                Primitives[i] = Primitives[i] with 
                {
                    fPoint = Primitives[i].fPoint with { color = curColor },
                    sPoint = Primitives[i].sPoint with { color = curColor }
                };
            return;
        }

        // Если включен режим редактирования примитива
        if (isEditingModePrim && isCreateColPrim && !isEditingModePoint)
        {
            Primitive temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
            int index = Primitives.IndexOf(temp_prim);
            Primitives[index] = Primitives[index] with 
            {
                fPoint = Primitives[index].fPoint with { color = curColor },
                sPoint = Primitives[index].sPoint with { color = curColor }
            };
            return;
        }

        // Если включен режим редактирования вершины примитива
        if (isEditingModePoint && isCreateColPrim)
        {
            Primitive temp_prim = Primitives.Find(s => s.Name == name_item_ComBox_Prim);
            int index = Primitives.IndexOf(temp_prim);
            Primitives[index] = name_item_comBox_Point switch
            {
                "fpoint" => Primitives[index] with { fPoint = Primitives[index].fPoint with { color = curColor } },
                "spoint" => Primitives[index] with { sPoint = Primitives[index].sPoint with { color = curColor } },
                _ => Primitives[index] with 
                {
                    fPoint = Primitives[index].fPoint with { color = curColor },
                    sPoint = Primitives[index].sPoint with { color = curColor }
                }
            };
            return;
        }

        // Изменение последнего нарисованного примитива
        if (Primitives.Any() && !isEditingModeColPrim)
        {
            Primitives[^1] = Primitives[^1] with
            {
                fPoint = Primitives[^1].fPoint with { color = curColor },
                sPoint = Primitives[^1].sPoint with { color = curColor }
            };
            return;
        }
    }

}
