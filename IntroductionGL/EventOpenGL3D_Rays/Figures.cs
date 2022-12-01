namespace IntroductionGL;

//: Рисование фигур и установка света
public partial class OpenGL3D_Rays : Window {

    //: Главная функция рисования фигур
    private void DrawFigures()
    {
        // Рисуем сферы
        for (int i = 0; i < stage.Spheres.Length; i++)
            if (stage.Spheres[i].isShow)
                DrawSphere(i);
        
        // Рисуем тетраэдры
        for (int i = 0; i < stage.Tetrahedrons.Length; i++)
            if (stage.Tetrahedrons[i].isShow)
                DrawTetrahedron(i);

        // Рисуем плоскость
        DrawSurface();
    }

    //: Установка света
    private void InstallLight() {

        // Включение режима двустороннего освещения
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_TWO_SIDE, OpenGL.GL_TRUE);

        // Значения глобальной фоновой составляющей
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new[] { 0.5f, 0.5f, 0.5f, 1f });

        // Задание материала
        gl3D.Enable(OpenGL.GL_COLOR_MATERIAL);

        // Точечный свет
        gl3D.Enable(OpenGL.GL_LIGHT0);
        gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, new[] { 0f, 1f, 0f, 1f });
        gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, new[] { 0.4f, 0.4f, 0.4f });
        gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, new[] { 0.5f, 0.5f, 0.5f, 1f });
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

    //: Рисование плоскости
    private void DrawSurface()
    {
        // Сохраняем текущую матрицу
        gl3D.PushMatrix();

        // Масштабируем плоскость
        gl3D.Scale(stage.Square.ScaleX, 1, stage.Square.ScaleZ);

        // Перемещяем плоскость в координаты центра
        gl3D.Translate(stage.Square.Center[0], stage.Square.Center[1], stage.Square.Center[2]);

        // Рисуем плоскость
        gl3D.Begin(BeginMode.Polygon);
        gl3D.Color(stage.Square.Color.R, stage.Square.Color.G, stage.Square.Color.B, stage.Square.Color.A);
        gl3D.Vertex(2f, 0f, 2f);
        gl3D.Vertex(-2f, 0f, 2);
        gl3D.Vertex(-2f, 0f, -2f);
        gl3D.Vertex(2f, 0f, -2);
        gl3D.End();

        // Восстанавливаем матрицу
        gl3D.PopMatrix();
    }

    //: Рисование сферы
    private void DrawSphere(int index) 
    {
        // Сохраняем текущую матрицу
        gl3D.PushMatrix();

        // Перемещаем сферу в координаты центра
        gl3D.Translate(stage.Spheres[index].Center[0], stage.Spheres[index].Center[1], stage.Spheres[index].Center[2]);

        // Задаем цвет сфере
        gl3D.Color(stage.Spheres[index].Color.R, stage.Spheres[index].Color.G, stage.Spheres[index].Color.B, stage.Spheres[index].Color.A);

        var obj = gl3D.NewQuadric();

        //?
        gl3D.QuadricDrawStyle(obj, OpenGL.GLU_FILL);
        
        // Рисуем сферу
        gl3D.Sphere(obj, stage.Spheres[index].R, 10, 10);

        // Восстанавливаем матрицу
        gl3D.PopMatrix();
    }

    //: Рисование тетраэдра
    private void DrawTetrahedron(int index)
    {
        // Сохраняем текущую матрицу
        gl3D.PushMatrix();

        // Перемещаем тетраэдр в координаты центра
        gl3D.Translate(stage.Tetrahedrons[index].Center[0], stage.Tetrahedrons[index].Center[1], stage.Tetrahedrons[index].Center[2]);

        // Задаем цвет тетраэдра
        gl3D.Color(stage.Spheres[index].Color.R, stage.Tetrahedrons[index].Color.G, stage.Tetrahedrons[index].Color.B, stage.Tetrahedrons[index].Color.A);

        // Рисуем тетраэдр
        gl3D.Begin(BeginMode.Polygon);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[0][0], stage.Tetrahedrons[index].Node[0][1], stage.Tetrahedrons[index].Node[0][2]);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[1][0], stage.Tetrahedrons[index].Node[1][1], stage.Tetrahedrons[index].Node[1][2]);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[2][0], stage.Tetrahedrons[index].Node[2][1], stage.Tetrahedrons[index].Node[2][2]);
        gl3D.End();

        gl3D.Begin(BeginMode.Polygon);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[1][0], stage.Tetrahedrons[index].Node[1][1], stage.Tetrahedrons[index].Node[1][2]);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[3][0], stage.Tetrahedrons[index].Node[3][1], stage.Tetrahedrons[index].Node[3][2]);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[2][0], stage.Tetrahedrons[index].Node[2][1], stage.Tetrahedrons[index].Node[2][2]);
        gl3D.End();

        gl3D.Begin(BeginMode.Polygon);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[3][0], stage.Tetrahedrons[index].Node[3][1], stage.Tetrahedrons[index].Node[3][2]);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[0][0], stage.Tetrahedrons[index].Node[0][1], stage.Tetrahedrons[index].Node[0][2]);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[2][0], stage.Tetrahedrons[index].Node[2][1], stage.Tetrahedrons[index].Node[2][2]);
        gl3D.End();

        gl3D.Begin(BeginMode.Polygon);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[1][0], stage.Tetrahedrons[index].Node[1][1], stage.Tetrahedrons[index].Node[1][2]);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[0][0], stage.Tetrahedrons[index].Node[0][1], stage.Tetrahedrons[index].Node[0][2]);
            gl3D.Vertex(stage.Tetrahedrons[index].Node[3][0], stage.Tetrahedrons[index].Node[3][1], stage.Tetrahedrons[index].Node[3][2]);
        gl3D.End();

        // Восстанавливаем матрицу
        gl3D.PopMatrix();
    }
}