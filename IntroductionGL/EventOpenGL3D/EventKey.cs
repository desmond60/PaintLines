namespace IntroductionGL;

//: Обработчики Key
public partial class OpenGL3D : Window {

    //: Обработка нажатий клавиш
    private void Grid_KeyDown(object sender, KeyEventArgs e) {
        
        // Если нажата клавиша W (движение камеры вперед)
        if (e.Key == Key.W && e.IsRepeat) {
            camera.CameraMove(0.02f);
            return;
        }

        // Если нажата клавиша S (движение камеры назад)
        if (e.Key == Key.S && e.IsRepeat) {
            camera.CameraMove(-0.02f);
            return;
        }

        // Если нажата клавиша A (поворот камеры влево)
        if (e.Key == Key.A && e.IsRepeat) {
            camera.CameraRotationObserver(-0.05f, new Vector<float>(new[] { 0.0f, 1.0f, 0.0f }));
            return;
        }

        // Если нажата клавиша D (поворот камеры вправо)
        if (e.Key == Key.D && e.IsRepeat) {
            camera.CameraRotationObserver(0.05f, new Vector<float>(new[] { 0.0f, 1.0f, 0.0f }));
            return;
        }

        // Если нажата клавиша Space (камера поднимается)
        if (e.Key == Key.Space && e.IsRepeat) {
            camera.CameraUpDown(0.08f);
            return;
        }

        // Если нажата клавиша Ctrl (камера отпускается)
        if (e.Key == Key.LeftCtrl && e.IsRepeat) {
            camera.CameraUpDown(-0.08f);
            return;
        }

    }
}

