namespace IntroductionGL;

//: Обработчики клавиш
public partial class OpenGLSpline : Window
{

    //: Обработка нажатий клавиш
    private void Grid_KeyDown(object sender, KeyEventArgs e) {

        // Если нажата кнопка F11 окошко на весь экран
        if (e.Key == Key.F11) {
            window.WindowState = WindowState.Maximized;
            return;
        }

        // Перемещаемся по сетке вверх и вправо
        if (Keyboard.IsKeyDown(Key.W) && Keyboard.IsKeyDown(Key.D)) {
            Position = Position with { X = Position.X - 5f, Y = Position.Y - 5f };
            return;
        }

        // Перемещаемся по сетке вверх и влево
        if (Keyboard.IsKeyDown(Key.W) && Keyboard.IsKeyDown(Key.A)) {
            Position = Position with { X = Position.X + 5f, Y = Position.Y - 5f };
            return;
        }

        // Перемещаемся по сетке вниз и влево
        if (Keyboard.IsKeyDown(Key.S) && Keyboard.IsKeyDown(Key.A)) {
            Position = Position with { X = Position.X + 5f, Y = Position.Y + 5f };
            return;
        }

        // Перемещаемся по сетке вниз и вправо
        if (Keyboard.IsKeyDown(Key.S) && Keyboard.IsKeyDown(Key.D)) {
            Position = Position with { X = Position.X - 5f, Y = Position.Y + 5f };
            return;
        }

        // Перемещаемся по сетке вверх
        if (e.Key == Key.W) {
            Position = Position with { Y = Position.Y - 5f };
            return;
        }

        // Перемещаемся по сетке вниз
        if (e.Key == Key.S) {
            Position = Position with { Y = Position.Y + 5f };
            return;
        }

        // Перемещаемся по сетке вправо
        if (e.Key == Key.D) {
            Position = Position with { X = Position.X - 5f };
            return;
        }

        // Перемещаемся по сетке влево
        if (e.Key == Key.A) {
            Position = Position with { X = Position.X + 5f };
            return;
        }

    }
}