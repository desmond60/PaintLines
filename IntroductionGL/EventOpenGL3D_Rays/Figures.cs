namespace IntroductionGL;

//: Рисование фигуры в разных режимах
public partial class OpenGL3D_Rays : Window
{

    //: Главная функция рисование фигуры
    private void DrawFigures()
    {

    }

    //: Рисование сетки
    private void DrawSurface()
    {
        // Сохраняем текущую матрицу
        gl3D.PushMatrix();

        // Масштабируем квадрат
        gl3D.Scale(stage.Square.ScaleX, 1, stage.Square.ScaleZ);

        // Перемещаемся в центр квадрата
        gl3D.Translate(stage.Square.Center[0], stage.Square.Center[1], stage.Square.Center[1]);

        // Рисуем квадрат
        gl3D.Begin(BeginMode.Polygon);
        gl3D.Color(stage.Square.Color.R, stage.Square.Color.G, stage.Square.Color.B, stage.Square.Color.A);
        gl3D.Vertex(-1f, 0f, -1f);
        gl3D.Vertex(-1f, 0f, 1f);
        gl3D.Vertex(1f, 0f, 1f);
        gl3D.Vertex(1f, 0f, -1f);
        gl3D.End();

        // Восстанавливаем матрицу
        gl3D.PopMatrix();
    }

}