namespace IntroductionGL;

//: Обработчики Button
public partial class OpenGL3D : Window {

    //: Обработчик нажатия кнопки "LoadTexture"
    private void BtnLoadTexture_Click(object sender, RoutedEventArgs e) {

        // Создание OpenFileDialog
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

        // Фильтры файлов
        dlg.DefaultExt = ".bmp";
        dlg.Filter = "BMP Files (*.bmp)|*.bmp";

        // Вызов окошка
        bool? result = dlg.ShowDialog();

        // Проверка на успешность
        if (result == true)
        {
            // Запоминаем текстуру (.bmp)
            foreach (var item in dictTexture) {
                if (item.Value == dlg.FileName)
                    return;
            }
            dictTexture.Add(dictTexture.Count + 1, dlg.FileName);
            ComboBoxViewTexture.Items.Add(new ComboBoxItem() { Content = $"texture{dictTexture.Count}" });
            _texture = dictTexture.Count;
            ComboBoxViewTexture.SelectedIndex = dictTexture.Count - 1;
        }
    }

    //: Обработчик нажатия кнопки переключения окон
    private void SwitchWindow_Click(object sender, RoutedEventArgs e) {
        MainWindow window = new MainWindow();
        window.Show();
        this.Close();
    }

    //: Обработчик нажатия закладки "Изменение ИС"
    private void EditLight_Click(object sender, RoutedEventArgs e) {
        
        // Изменяем визуализацию Canvas
        AddEditLight.Content = "Изменить";
        DeleteLight.Visibility = Visibility.Visible;
        ShowLight.Visibility = Visibility.Visible;

        // Заполнение ComboBox
        ComboBoxAddLight.Items.Clear();
        foreach (var item in lights)
            ComboBoxAddLight.Items.Add(new ComboBoxItem() { Content = $"{item.Name}" });
        ComboBoxAddLight.SelectedIndex = 0;

        // Заполнить поля первого ИС в ComboBox
        var str = ((ComboBoxItem)ComboBoxAddLight.SelectedValue).Content.ToString();
        var light = lights.Find(n => n.Name.Equals(str));
        str = str.Split('_')[0].ToString();
        FillFields(str, light);
    }

    //: Обработчик нажатия кнопки "Добавление источника света"
    private void AddLight_Click(object sender, RoutedEventArgs e) {
        
        // Изменяем визуализацию Canvas
        AddEditLight.Content = "Добавить";
        DeleteLight.Visibility = Visibility.Hidden;
        ShowLight.Visibility = Visibility.Hidden;

        // Заполнение ComboBox
        ComboBoxAddLight.Items.Clear();
        ComboBoxAddLight.Items.Add(new ComboBoxItem() { Content = $"Направленный" });
        ComboBoxAddLight.Items.Add(new ComboBoxItem() { Content = $"Точечный" });
        ComboBoxAddLight.Items.Add(new ComboBoxItem() { Content = $"Прожектор" });
        ComboBoxAddLight.SelectedIndex = 0;
    }

    //: Обработчик нажатия кнопки "Добавить затухание ИС Точечный"
    private void AddAttenuationPointLight_Click(object sender, RoutedEventArgs e) {
        
        // Визуализируем Canvas
        Attention.Visibility                   = Visibility.Visible;
        DeleteAttenuationPointLight.Visibility = Visibility.Visible;

        // Если это просто добавление окошка
        string name = ((ComboBoxItem)ComboBoxAddLight.SelectedValue).Content.ToString()!;
        if (name == "Направленный" || name == "Точечный" || name == "Прожектор")
            return;
        
        // Если это изменение ИС
        var light = lights.Find(n => n.Name.Equals(name));
        lights.Remove(light!);
        ComboBoxAddLight.Items.Remove((ComboBoxItem)ComboBoxAddLight.SelectedItem);
        light.IsAttenuation = true;
        light.Type = light.Type switch
        {
            TypeLight.POINT => TypeLight.POINT_ATTENUATION,
            TypeLight.SPOT => TypeLight.SPOT_ATTENUATION
        };
        EditLight(ref light!);
        lights.Add(light);

        // Заполнение ComboBox
        ComboBoxAddLight.Items.Add(new ComboBoxItem() { Content = $"{light.Name}" });
        ComboBoxAddLight.SelectedIndex = ComboBoxAddLight.Items.Count - 1;
    }

    //: Обработчик нажатия кнопки "Добавить затухание ИС Прожектор"
    private void AddAttenuationSpotLight_Click(object sender, RoutedEventArgs e) {
        
        // Визуализируем Canvas
        AttentionSpot.Visibility = Visibility.Visible;
        DeleteAttenuationSpotLight.Visibility = Visibility.Visible;

        // Если это просто добавление ИС
        string name = ((ComboBoxItem)ComboBoxAddLight.SelectedValue).Content.ToString()!;
        if (name == "Направленный" || name == "Точечный" || name == "Прожектор")
            return;

        // Если это изменение ИС
        var light = lights.Find(n => n.Name.Equals(name));
        lights.Remove(light!);
        ComboBoxAddLight.Items.Remove((ComboBoxItem)ComboBoxAddLight.SelectedItem);
        light.IsAttenuation = true;
        light.Type = light.Type switch
        {
            TypeLight.POINT => TypeLight.POINT_ATTENUATION,
            TypeLight.SPOT=> TypeLight.SPOT_ATTENUATION
        };
        EditLight(ref light!);
        lights.Add(light);

        // Заполнение ComboBox
        ComboBoxAddLight.Items.Add(new ComboBoxItem() { Content = $"{light.Name}" });
        ComboBoxAddLight.SelectedIndex = ComboBoxAddLight.Items.Count - 1;
    }

    //: Обработчик нажатия кнопки "Добавить или изменить ИС"
    private void AddEditLight_Click(object sender, RoutedEventArgs e) {

        // Если кнопка "Добавить"
        if (AddEditLight.Content.ToString() == "Добавить") {
            var light = new EventOpenGL3D.Light();
            EditLight(ref light);
            lights.Add(light);
            EditingLight.IsEnabled = true;
        }
        else {
            var light = lights.Find(n => n.Name.Equals(((ComboBoxItem)ComboBoxAddLight.SelectedValue).Content));
            lights.Remove(light!);
            EditLight(ref light!);
            lights.Add(light);
        }
    }

    //: Обработчик нажатия кнопки "Удалить ИС"
    private void DeleteLight_Click(object sender, RoutedEventArgs e) {

        // Имя источника света
        var str = ((ComboBoxItem)ComboBoxAddLight.SelectedValue).Content.ToString();

        // Удаление источника света
        lights.Remove(lights.Find(n => n.Name.Equals(str))!);
        ComboBoxAddLight.Items.Remove((ComboBoxItem)ComboBoxAddLight.SelectedItem);

        // Если удалены все источники света
        if (lights.Count == 0) {
            AddLight_Click(sender, e);
            EditingLight.IsEnabled = false;
        }
        else {
            ComboBoxAddLight.SelectedIndex = 0;

            // Заполнение полей первого ИС
            str = ((ComboBoxItem)ComboBoxAddLight.SelectedValue).Content.ToString();
            var light = lights.Find(n => n.Name.Equals(str));
            str = str.Split('_')[0].ToString();
            FillFields(str, light);
        }
    }

    //: Обработчик нажатия кнопки "Убрать затухание"
    private void DeleteAttenuationLight_Click(object sender, RoutedEventArgs e) {
        
        // Визуализируем окошко
        Attention.Visibility        = Visibility.Hidden;
        AttentionSpot.Visibility    = Visibility.Hidden;
        ((Button)sender).Visibility = Visibility.Hidden;

        // Если это просто добавление ИС
        string name = ((ComboBoxItem)ComboBoxAddLight.SelectedValue).Content.ToString()!;
        if (name == "Направленный" || name == "Точечный" || name == "Прожектор")
            return;

        // Если изменяем ИС
        var light = lights.Find(n => n.Name.Equals(name));
        lights.Remove(light!);
        ComboBoxAddLight.Items.Remove((ComboBoxItem)ComboBoxAddLight.SelectedItem);
        light.IsAttenuation = false;
        light.Type = light.Type switch
        {
            TypeLight.POINT_ATTENUATION => TypeLight.POINT,
            TypeLight.SPOT_ATTENUATION => TypeLight.SPOT
        };
        EditLight(ref light!);
        lights.Add(light);

        // Заполнение ComboBox
        ComboBoxAddLight.Items.Add(new ComboBoxItem() { Content = $"{light.Name}" });
        ComboBoxAddLight.SelectedIndex = ComboBoxAddLight.Items.Count - 1;
    }

    //: Функция изменить параметры ИС
    private void EditLight(ref EventOpenGL3D.Light light) {

        // Определяем тип источника света
        if (AddEditLight.Content.Equals("Добавить")) {
            light.Type = ComboBoxAddLight.Text switch
            {
                "Направленный" => TypeLight.DIRECTED,
                "Точечный" when Attention.Visibility == Visibility.Visible => TypeLight.POINT_ATTENUATION,
                "Точечный" => TypeLight.POINT,
                "Прожектор" when AttentionSpot.Visibility == Visibility.Visible => TypeLight.SPOT_ATTENUATION,
                "Прожектор" => TypeLight.SPOT,
                _ => light.Type
            };

        }

        // Определяем имя ИС
        light.Name = GetNameLight(light.Type, lights);

        // Определяем показывать ИС
        light.IsShow = ShowLight.IsChecked.Value;
        
        // Сначала присвоем все три составляющии
        light.Ambient[0] = Single.Parse(AmbientX.Text);
        light.Ambient[1] = Single.Parse(AmbientY.Text);
        light.Ambient[2] = Single.Parse(AmbientZ.Text);

        light.Diffuse[0] = Single.Parse(DiffuseX.Text);
        light.Diffuse[1] = Single.Parse(DiffuseY.Text);
        light.Diffuse[2] = Single.Parse(DiffuseZ.Text);

        light.Specular[0] = Single.Parse(SpecularX.Text);
        light.Specular[1] = Single.Parse(SpecularY.Text);
        light.Specular[2] = Single.Parse(SpecularZ.Text);

        // Смотрим если добавленно затухение точечного 
        if (light.Type == TypeLight.POINT_ATTENUATION)
        {
            light.IsAttenuation = true;
            light.Constant = Single.Parse(Constant.Text);
            light.Linear = Single.Parse(Linear.Text);
            light.Quadratic = Single.Parse(Quadratic.Text);
        }
        else if (light.Type == TypeLight.SPOT_ATTENUATION)
        {
            light.IsAttenuation = true;
            light.Constant = Single.Parse(ConstantSpot.Text);
            light.Linear = Single.Parse(LinearSpot.Text);
            light.Quadratic = Single.Parse(QuadraticSpot.Text);
        }

        // Смотрим какой источник света и добавляем соответствующие ему компоненты
        switch (light.Name.Split('_')[0])
        {
            case "Directed":
                light.Direction[0] = Single.Parse(OrientationX.Text);
                light.Direction[1] = Single.Parse(OrientationY.Text);
                light.Direction[2] = Single.Parse(OrientationZ.Text);
                break;

            case "Point" or "PointAtt":
                light.Position[0] = Single.Parse(PositionX.Text);
                light.Position[1] = Single.Parse(PositionY.Text);
                light.Position[2] = Single.Parse(PositionZ.Text);
                break;

            case "Spot" or "SpotAtt":
                light.Position[0] = Single.Parse(PositionSpotX.Text);
                light.Position[1] = Single.Parse(PositionSpotY.Text);
                light.Position[2] = Single.Parse(PositionSpotZ.Text);

                light.Direction[0] = Single.Parse(DirectionX.Text);
                light.Direction[1] = Single.Parse(DirectionY.Text);
                light.Direction[2] = Single.Parse(DirectionZ.Text);

                light.Cutoff = Single.Parse(Cutoff.Text);
                light.Exponent = Single.Parse(Exponent.Text);
                break;
        }
    }

    //: TextBox только цифры и точка
    private void LightPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Добавляем регулярное выражение
        var regex = new Regex("[^0-9.-]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    //: Обработчик CheckBox "Показывать ИС?"
    private void ShowLight_Click(object sender, RoutedEventArgs e)
    {
        var light = lights.Find(n => n.Name.Equals(((ComboBoxItem)ComboBoxAddLight.SelectedValue).Content));
        lights.Remove(light!);
        EditLight(ref light!);
        lights.Add(light);
    }
}
