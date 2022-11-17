namespace IntroductionGL;

//: Обработчики ComboBox
public partial class OpenGL3D : Window {

    //: Обработчик ComboBox "Projection"
    private void ComboBoxProjection_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (gl3D is null) return;

        if (!String.IsNullOrEmpty(ComboBoxProjection.SelectedValue?.ToString())) {

            // Вычисляем соотношение между шириной и высотой
            float ratio = (float)(openGLControl3D.ActualWidth / openGLControl3D.ActualHeight);

            // Устанавливаем матрицу проекции / определяет объем сцены
            gl3D.MatrixMode(MatrixMode.Projection);

            // Единичная матрица
            gl3D.LoadIdentity();

            // Окно просмотра
            gl3D.Viewport(0, 0, (int)openGLControl3D.ActualWidth, (int)openGLControl3D.ActualHeight);

            if (((ComboBoxItem)ComboBoxProjection.SelectedValue).Content.ToString() == "Перспективная")
            {
                isPerspective = true;
                gl3D.Perspective(60, ratio, 0.01f, 50.0f);
            }
            else {
                isPerspective = false;
                if (openGLControl3D.ActualWidth >= openGLControl3D.ActualHeight)
                    gl3D.Ortho(-10*ratio,10*ratio, -10,10, -100,100);
                else
                    gl3D.Ortho(-10,10, -10/ratio,10/ratio, -100,100);
            }

            // Возврат к матрице модели GL_MODELVIEW
            gl3D.MatrixMode(OpenGL.GL_MODELVIEW);
        }
    }

    //: Обработчки ComboBox "ViewLight"
    private void ComboBoxViewLight_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (!String.IsNullOrEmpty(ComboBoxViewLight.SelectedValue?.ToString()))
        {
            ViewLight = ((ComboBoxItem)ComboBoxViewLight.SelectedValue).Content.ToString() switch
            {
                "Точечный" => 0,
                "Затухание" => 1,
                "Прожектор" => 2,
                _ => 0
            };
        }
    }

    //: Обработчики ComboBox "ViewTexture"
    private void ComboBoxViewTexture_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (!String.IsNullOrEmpty(ComboBoxViewTexture.SelectedValue?.ToString())) { 
            var str = ((ComboBoxItem)ComboBoxViewTexture.SelectedValue).Content.ToString();
            int.TryParse(string.Join("", str!.Where(c => char.IsDigit(c))), out _texture);
        }
    }
}
