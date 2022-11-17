namespace IntroductionGL;

//: Обработчики элементов Toggle
public partial class OpenGL3D_Rays : Window {

    //: Обработчик Toggle "Вкл./Выкл. трассировки"
    private void ToggleRayTracer_Click(object sender, RoutedEventArgs e) {
        IsRaysTracer = !IsRaysTracer;
    }
}
