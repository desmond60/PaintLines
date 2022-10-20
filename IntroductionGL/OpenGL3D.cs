namespace IntroductionGL;

//: Окошко OpenGL
public partial class MainWindow : Window
{

    /* ----------------------- Переменные --------------------------------- */
    //OpenGL gl3D; // Переменная OpenGl
    /* ----------------------- Переменные --------------------------------- */

    // Начальное состояние OpenGls
/*    private void openGLControl3D_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args) {
        gl3D = args.OpenGL;
        gl3D.ClearColor(1, 1, 1, 0);
    }

    private void openGLControl3D_OpenGLDraw(object sender, OpenGLRoutedEventArgs args) {
        gl3D.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        gl3D.PointSize(20);
        gl3D.Enable(OpenGL.GL_POINT_SMOOTH);
        gl3D.Begin(BeginMode.Points);
        gl3D.Color((byte)128, (byte)128, (byte)128, (byte)255);
        gl3D.Vertex(0, 0);
        gl3D.Vertex(1, 0);
        gl3D.Vertex(2, 0);
        gl3D.End();

    }

    private void openGLControl3D_Resized(object sender, OpenGLRoutedEventArgs args) {
        gl3D.MatrixMode(MatrixMode.Projection);
        gl3D.LoadIdentity();
        gl3D.Ortho(0, openGLControl3D.ActualWidth, openGLControl3D.ActualHeight, 0, 0, 0);

        gl3D.MatrixMode(MatrixMode.Modelview);
    }*/
}