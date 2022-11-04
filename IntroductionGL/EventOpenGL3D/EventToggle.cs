namespace IntroductionGL;

//: Обработчики элементов Toggle
public partial class OpenGL3D : Window
{

    //: Обработчик Toggle "Вкл./Выкл. сетки"
    private void ToggleGrid_Click(object sender, RoutedEventArgs e) {
        isGrid = !isGrid;
    }

    //: Обработчик Toggle "Вкл./Выкл. свет"
    private void ToggleLight_Click(object sender, RoutedEventArgs e) {
        isLight = !isLight;
    }

    //: Обработчки Toggle "Вкл./Выкл. буфер глубины"
    private void ToggleDepth_Click(object sender, RoutedEventArgs e) {
        isDepth = !isDepth;
    }

    //: Обработчик Toggle "Вкл./Выкл. каркасного режима"
    private void ToggleSceleton_Click(object sender, RoutedEventArgs e) {
        isSceleton = !isSceleton;
    }

    //: Обработчки Toggle "Вкл./Выкл. демонстрацию нормалей"
    private void ToggleDrawNormal_Click(object sender, RoutedEventArgs e) {
        isDrawNormal = !isDrawNormal;
    }

    //: Обработчик Toggle "Вкл./Выкл. текстурирование фигуры"
    private void ToggleShowTexture_Click(object sender, RoutedEventArgs e) {
        isShowTexture = !isShowTexture;
    }

    //: Обработчик Toggle "Вкл./Выкл. сглаживание нормалей"
    private void ToggleSmoothNormal_Click(object sender, RoutedEventArgs e) {
        isSmoothNormal = !isSmoothNormal;
    }

    //: Обработчик Toggle "Вкл./Выкл. туман"
    private void ToggleFog_Click(object sender, RoutedEventArgs e) {
        isFog = !isFog;
    }
}
