namespace IntroductionGL;

//: Рисование фигуры в разных режимах
public partial class OpenGL3D_Rays : Window
{

    //: Главная функция рисование фигуры
    private void DrawFigure()
    {

    }

    //: Рисование сетки
    private void DrawGrid()
    {
        gl3D.Color((byte)255, (byte)0, (byte)255);
        for (int i = -30; i <= 30; i++)
        {
            gl3D.Begin(BeginMode.Lines);

            gl3D.Vertex(-30.0f, 0.0f, (float)i);
            gl3D.Vertex(30.0f, 0.0f, (float)i);

            gl3D.Vertex((float)i, 0.0f, -30.0f);
            gl3D.Vertex((float)i, 0.0f, 30.0f);

            gl3D.End();
        }
    }

}