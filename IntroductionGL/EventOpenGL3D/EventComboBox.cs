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

    //: Обработчики ComboBox "ViewTexture"
    private void ComboBoxViewTexture_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (!String.IsNullOrEmpty(ComboBoxViewTexture.SelectedValue?.ToString())) { 
            var str = ((ComboBoxItem)ComboBoxViewTexture.SelectedValue).Content.ToString();
            int.TryParse(string.Join("", str!.Where(c => char.IsDigit(c))), out _texture);
        }
    }

    //: Обработчик ComboBox "AddLight"
    private void ComboBoxAddLight_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        // Визуализируем окошко в зависимости от типа ИС
        if (!String.IsNullOrEmpty(ComboBoxAddLight.SelectedValue?.ToString()) && gl3D != null) {
            var str = ((ComboBoxItem)ComboBoxAddLight.SelectedValue).Content.ToString();
            switch (str)
            {
                case "Направленный":
                    DirectedLight.Visibility = Visibility.Visible;
                    PointLight.Visibility    = Visibility.Hidden;
                    SpotLight.Visibility     = Visibility.Hidden;
                    Attention.Visibility     = Visibility.Hidden;
                    AttentionSpot.Visibility = Visibility.Hidden;
                    return;

                case "Точечный":
                    DirectedLight.Visibility = Visibility.Hidden;
                    PointLight.Visibility    = Visibility.Visible;
                    SpotLight.Visibility     = Visibility.Hidden;
                    Attention.Visibility     = Visibility.Hidden;
                    AttentionSpot.Visibility = Visibility.Hidden;
                    return;

                case "Прожектор":
                    DirectedLight.Visibility = Visibility.Hidden;
                    PointLight.Visibility    = Visibility.Hidden;
                    SpotLight.Visibility     = Visibility.Visible;
                    Attention.Visibility     = Visibility.Hidden;
                    AttentionSpot.Visibility = Visibility.Hidden;
                    return;
            }

            // Иначе это ComboBox с изменением ИС
            DirectedLight.Visibility = Visibility.Hidden;
            PointLight.Visibility    = Visibility.Hidden;
            SpotLight.Visibility     = Visibility.Hidden;
            Attention.Visibility     = Visibility.Hidden;
            AttentionSpot.Visibility = Visibility.Hidden;
            var light = lights.Find(n => n.Name.Equals(str)); 
            str = str.Split('_')[0].ToString();
            FillFields(str, light);
        }
    }

    //: Заполенение полей выбранного цвета
    private void FillFields(string name, EventOpenGL3D.Light light) {

        AmbientX.Text = light.Ambient[0].ToString();
        AmbientY.Text = light.Ambient[1].ToString();
        AmbientZ.Text = light.Ambient[2].ToString();

        DiffuseX.Text = light.Diffuse[0].ToString();
        DiffuseY.Text = light.Diffuse[1].ToString();
        DiffuseZ.Text = light.Diffuse[2].ToString();

        SpecularX.Text = light.Specular[0].ToString();
        SpecularY.Text = light.Specular[1].ToString();
        SpecularZ.Text = light.Specular[2].ToString();

        ShowLight.IsChecked = light.IsShow;

        switch (name)
        {
            case "Directed":
                DirectedLight.Visibility = Visibility.Visible;

                OrientationX.Text = light.Direction[0].ToString();
                OrientationY.Text = light.Direction[1].ToString();
                OrientationZ.Text = light.Direction[2].ToString();
                return;

            case "Point":
                PointLight.Visibility = Visibility.Visible;
                DeleteAttenuationPointLight.Visibility = Visibility.Hidden;

                PositionX.Text = light.Position[0].ToString();
                PositionY.Text = light.Position[1].ToString();
                PositionZ.Text = light.Position[2].ToString();
                return;

            case "PointAtt":
                PointLight.Visibility = Visibility.Visible;
                Attention.Visibility = Visibility.Visible;
                DeleteAttenuationPointLight.Visibility = Visibility.Visible;

                PositionX.Text = light.Position[0].ToString();
                PositionY.Text = light.Position[1].ToString();
                PositionZ.Text = light.Position[2].ToString();

                Constant.Text  = light.Constant.ToString();
                Linear.Text    = light.Linear.ToString();
                Quadratic.Text = light.Quadratic.ToString();
                return;

            case "Spot":
                SpotLight.Visibility = Visibility.Visible;
                DeleteAttenuationSpotLight.Visibility = Visibility.Hidden;

                PositionSpotX.Text = light.Position[0].ToString();
                PositionSpotY.Text = light.Position[1].ToString();
                PositionSpotZ.Text = light.Position[2].ToString();
                DirectionX.Text    = light.Direction[0].ToString();
                DirectionY.Text    = light.Direction[1].ToString();
                DirectionZ.Text    = light.Direction[2].ToString();
                Cutoff.Text        = light.Cutoff.ToString();
                Exponent.Text      = light.Exponent.ToString();
                return;

            case "SpotAtt":
                SpotLight.Visibility = Visibility.Visible;
                AttentionSpot.Visibility = Visibility.Visible;
                DeleteAttenuationSpotLight.Visibility = Visibility.Visible;

                PositionSpotX.Text = light.Position[0].ToString();
                PositionSpotY.Text = light.Position[1].ToString();
                PositionSpotZ.Text = light.Position[2].ToString();
                DirectionX.Text    = light.Direction[0].ToString();
                DirectionY.Text    = light.Direction[1].ToString();
                DirectionZ.Text    = light.Direction[2].ToString();
                Cutoff.Text        = light.Cutoff.ToString();
                Exponent.Text      = light.Exponent.ToString();

                Constant.Text = light.Constant.ToString();
                Linear.Text = light.Linear.ToString();
                Quadratic.Text = light.Quadratic.ToString();
                return;
        }
    }

}