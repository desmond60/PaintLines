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
        Nullable<bool> result = dlg.ShowDialog();

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
}
