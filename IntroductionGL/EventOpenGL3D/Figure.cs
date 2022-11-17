namespace IntroductionGL;

//: Рисование фигуры в разных режимах и установка света
public partial class OpenGL3D : Window {

    //: Главная функция рисование фигуры
    private void DrawFigure() {
        
        // Каркасные или некаркасный режим
        if (!isSceleton) {
            // Сглаженные нормали или несглаженные
            if (!isSmoothNormal)
                // С текстурированием или без
                if (!isShowTexture)
                    DrawNotSceleton(Normals);
                else
                    DrawTexture(Normals);
            else
                // С текстурированием или без
                if (!isShowTexture)
                    DrawNotSceleton(SmoothNormal);
                else
                    DrawTexture(SmoothNormal);
        }
        else {
            DrawSceleton();
        }

        // Рисовать нормали
        if (isDrawNormal) {
            // Сглаженные или не сглаженные
            if (!isSmoothNormal)
                DrawNotSmoothNormal();
            else
                DrawSmoothNormal();
        }

    }

    //: Установка света
    private void InstallLight() {

        // Включение режима двустороннего освещения
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_TWO_SIDE, OpenGL.GL_TRUE);

        // Значения глобальной фоновой составляющей
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new[] { 0.5f, 0.5f, 0.5f, 1f });

        // Задание материала
        gl3D.Enable(OpenGL.GL_COLOR_MATERIAL);

        // Если туман включен
        if (isFog) {
            gl3D.Enable(OpenGL.GL_FOG);
            gl3D.Fog(OpenGL.GL_FOG_MODE, OpenGL.GL_EXP2);
            gl3D.Fog(OpenGL.GL_FOG_COLOR, new[] { 1f, 1f, 0.5f, 1f });
            gl3D.Fog(OpenGL.GL_FOG_DENSITY, 0.05f);
        }

        switch (ViewLight)
        {
            case 0:
                // Точечный свет
                gl3D.Enable(OpenGL.GL_LIGHT0);
                gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, new[] { 200f, 200f, 300f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, new[] { 0f, 0f, 0f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, new[] { 1f, 1f, 1f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, new[] { 1f, 1f, 1f, 1f });

                break;

            case 1:
                gl3D.Enable(OpenGL.GL_LIGHT1);
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_POSITION, new[] { 200f, 200f, 300f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_AMBIENT, new[] { 0f, 0f, 0f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_DIFFUSE, new[] { 1f, 1f, 1f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_SPECULAR, new[] { 1f, 1f, 1f, 1f });

                // Затухание
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_CONSTANT_ATTENUATION, 0f);
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_LINEAR_ATTENUATION, 0.005f);
                gl3D.Light(OpenGL.GL_LIGHT1, OpenGL.GL_QUADRATIC_ATTENUATION, 0f);
                break;

            case 2:
                gl3D.Enable(OpenGL.GL_LIGHT2);
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_POSITION, new[] { 5f, 5f, -10f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_AMBIENT, new[] { 0f, 0f, 0f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_DIFFUSE, new[] { 1f, 1f, 1f, 1f });
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPECULAR, new[] { 1f, 1f, 1f, 1f });

                // Прожектор
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPOT_EXPONENT, 0);
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPOT_CUTOFF, 45);
                gl3D.Light(OpenGL.GL_LIGHT2, OpenGL.GL_SPOT_DIRECTION, new[] { -1f, 0f, 0f, 1f });
                break;

            default:
                break;
        }
    }

    //: Рисование сетки
    private void DrawGrid() {
        gl3D.Color((byte)255, (byte)0, (byte)255);
        for (int i = -30; i <= 30; i++) {
            gl3D.Begin(BeginMode.Lines);

            gl3D.Vertex(-30.0f, 0.0f, (float)i);
            gl3D.Vertex(30.0f, 0.0f, (float)i);

            gl3D.Vertex((float)i, 0.0f, -30.0f);
            gl3D.Vertex((float)i, 0.0f, 30.0f);

            gl3D.End();
        }
    }

    //: Рисование фигуры в не каркасном виде
    private void DrawNotSceleton(List<Vector<float>> normals) {
        int numNor = 0;

        // Установка цвета
        gl3D.Color((byte)0, (byte)255, (byte)(255));

        gl3D.Begin(OpenGL.GL_TRIANGLES);
            gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
            numNor++;
            gl3D.Vertex(Figure[0].section1[0], Figure[0].section1[1], Figure[0].section1[2]);
            gl3D.Vertex(Figure[0].section2[0], Figure[0].section2[1], Figure[0].section2[2]);
            gl3D.Vertex(Figure[0].section3[0], Figure[0].section3[1], Figure[0].section3[2]);
        gl3D.End();

        for (int i = 0; i < Figure.Count - 1; i++) {
            gl3D.Begin(BeginMode.Polygon);
                gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
                numNor++;
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
            gl3D.End();

            gl3D.Begin(BeginMode.Polygon);
                gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
                numNor++;
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
            gl3D.End();

            gl3D.Begin(BeginMode.Polygon);
                gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
                numNor++;
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
            gl3D.End();
        }

        gl3D.Begin(OpenGL.GL_TRIANGLES);
            gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
            numNor++;
            gl3D.Vertex(Figure[^1].section1[0], Figure[^1].section1[1], Figure[^1].section1[2]);
            gl3D.Vertex(Figure[^1].section2[0], Figure[^1].section2[1], Figure[^1].section2[2]);
            gl3D.Vertex(Figure[^1].section3[0], Figure[^1].section3[1], Figure[^1].section3[2]);
        gl3D.End();
    }

    //: Рисование фигуры в каркасном виде
    private void DrawSceleton() {

        // Установка цвета
        gl3D.Color((byte)255, (byte)255, (byte)0);

        // Первая секция 
        gl3D.Begin(OpenGL.GL_LINE_STRIP);
        for (int i = 0; i < Figure.Count; i++)
            gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
        gl3D.End();

        // Вторая секция
        gl3D.Begin(OpenGL.GL_LINE_STRIP);
        for (int i = 0; i < Figure.Count; i++)
            gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
        gl3D.End();

        // Третья секция
        gl3D.Begin(OpenGL.GL_LINE_STRIP);
        for (int i = 0; i < Figure.Count; i++)
            gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
        gl3D.End();

        // Треугольники
        for (int i = 0; i < Figure.Count; i++) {
            gl3D.Begin(OpenGL.GL_LINE_LOOP);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
            gl3D.End();
        }

    }

    //: Рисование несглаженныx нормалей
    private void DrawNotSmoothNormal() {

        // Устанавливаем цвет
        gl3D.Color((byte)153, (byte)255, (byte)153);

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Vertex(Figure[0].section1[0], Figure[0].section1[1], Figure[0].section1[2]);
            gl3D.Vertex(Figure[0].section1[0] + Normals[0][0], Figure[0].section1[1] + Normals[0][1], Figure[0].section1[2] + Normals[0][2]);
        gl3D.End();

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Vertex(Figure[0].section2[0], Figure[0].section2[1], Figure[0].section2[2]);
            gl3D.Vertex(Figure[0].section2[0] + Normals[0][0], Figure[0].section2[1] + Normals[0][1], Figure[0].section2[2] + Normals[0][2]);
        gl3D.End();

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Vertex(Figure[0].section3[0], Figure[0].section3[1], Figure[0].section3[2]);
            gl3D.Vertex(Figure[0].section3[0] + Normals[0][0], Figure[0].section3[1] + Normals[0][1], Figure[0].section3[2] + Normals[0][2]);
        gl3D.End();

        int numNor = 1;
        for (int i = 0; i < Figure.Count - 1; i++) {
            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.Vertex(Figure[i].section1[0] + Normals[numNor][0], Figure[i].section1[1] + Normals[numNor][1], Figure[i].section1[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.Vertex(Figure[i + 1].section1[0] + Normals[numNor][0], Figure[i + 1].section1[1] + Normals[numNor][1], Figure[i + 1].section1[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.Vertex(Figure[i + 1].section2[0] + Normals[numNor][0], Figure[i + 1].section2[1] + Normals[numNor][1], Figure[i + 1].section2[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.Vertex(Figure[i].section2[0] + Normals[numNor][0], Figure[i].section2[1] + Normals[numNor][1], Figure[i].section2[2] + Normals[numNor][2]);
            gl3D.End();

            numNor++;

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
                gl3D.Vertex(Figure[i].section3[0] + Normals[numNor][0], Figure[i].section3[1] + Normals[numNor][1], Figure[i].section3[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.Vertex(Figure[i + 1].section3[0] + Normals[numNor][0], Figure[i + 1].section3[1] + Normals[numNor][1], Figure[i + 1].section3[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.Vertex(Figure[i + 1].section1[0] + Normals[numNor][0], Figure[i + 1].section1[1] + Normals[numNor][1], Figure[i + 1].section1[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.Vertex(Figure[i].section1[0] + Normals[numNor][0], Figure[i].section1[1] + Normals[numNor][1], Figure[i].section1[2] + Normals[numNor][2]);
            gl3D.End();

            numNor++;

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.Vertex(Figure[i].section2[0] + Normals[numNor][0], Figure[i].section2[1] + Normals[numNor][1], Figure[i].section2[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.Vertex(Figure[i + 1].section2[0] + Normals[numNor][0], Figure[i + 1].section2[1] + Normals[numNor][1], Figure[i + 1].section2[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.Vertex(Figure[i + 1].section3[0] + Normals[numNor][0], Figure[i + 1].section3[1] + Normals[numNor][1], Figure[i + 1].section3[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
                gl3D.Vertex(Figure[i].section3[0] + Normals[numNor][0], Figure[i].section3[1] + Normals[numNor][1], Figure[i].section3[2] + Normals[numNor][2]);
            gl3D.End();

            numNor++;
        }

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Vertex(Figure[^1].section1[0], Figure[^1].section1[1], Figure[^1].section1[2]);
            gl3D.Vertex(Figure[^1].section1[0] + Normals[^1][0], Figure[^1].section1[1] + Normals[^1][1], Figure[^1].section1[2] + Normals[^1][2]);
        gl3D.End();

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Vertex(Figure[^1].section2[0], Figure[^1].section2[1], Figure[^1].section2[2]);
            gl3D.Vertex(Figure[^1].section2[0] + Normals[^1][0], Figure[^1].section2[1] + Normals[^1][1], Figure[^1].section2[2] + Normals[^1][2]);
        gl3D.End();

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Vertex(Figure[^1].section3[0], Figure[^1].section3[1], Figure[^1].section3[2]);
            gl3D.Vertex(Figure[^1].section3[0] + Normals[^1][0], Figure[^1].section3[1] + Normals[^1][1], Figure[^1].section3[2] + Normals[^1][2]);
        gl3D.End();
    }

    //: Рисование сглаженных нормалей
    private void DrawSmoothNormal() {

        // Устанавливаем цвет
        gl3D.Color((byte)153, (byte)255, (byte)153);

        for (int i = 0, numNor = 0; i < Figure.Count; i++, numNor += 3) {
            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.Vertex(Figure[i].section1[0] + SmoothNormal[numNor][0], Figure[i].section1[1] + SmoothNormal[numNor][1], Figure[i].section1[2] + SmoothNormal[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.Vertex(Figure[i].section2[0] + SmoothNormal[numNor + 1][0], Figure[i].section2[1] + SmoothNormal[numNor + 1][1], Figure[i].section2[2] + +SmoothNormal[numNor + 1][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
                gl3D.Vertex(Figure[i].section3[0] + SmoothNormal[numNor + 2][0], Figure[i].section3[1] + SmoothNormal[numNor + 2][1], Figure[i].section3[2] + SmoothNormal[numNor + 2][2]);
            gl3D.End();
        }
    }

    //: Рисование текстурированной фигуры
    private void DrawTexture(List<Vector<float>> normals) {
        // Разрешаем текстурирование, указываем текстуру
        gl3D.Enable(OpenGL.GL_TEXTURE_2D);
        texture.Create(gl3D, dictTexture[_texture]);
        texture.Bind(gl3D);

        int numNor = 0;

        gl3D.Begin(OpenGL.GL_TRIANGLES);
            gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
            numNor++;
            gl3D.Color((byte)255, (byte)255, (byte)(255));
            gl3D.TexCoord(0f, 0f);
            gl3D.Vertex(Figure[0].section1[0], Figure[0].section1[1], Figure[0].section1[2]);
            gl3D.TexCoord(0.5f, 1f);
            gl3D.Vertex(Figure[0].section2[0], Figure[0].section2[1], Figure[0].section2[2]);
            gl3D.TexCoord(1f, 0f);
            gl3D.Vertex(Figure[0].section3[0], Figure[0].section3[1], Figure[0].section3[2]);
        gl3D.End();

        for (int i = 0; i < Figure.Count - 1; i++)
        {
            gl3D.Begin(BeginMode.Polygon);
                gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
                numNor++;
                gl3D.Color((byte)255, (byte)255, (byte)(255));
                gl3D.TexCoord(0f, 1f);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.TexCoord(1f, 1f);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.TexCoord(1f, 0f);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.TexCoord(0f, 0f);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
            gl3D.End();

            gl3D.Begin(BeginMode.Polygon);
                gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
                numNor++;
                gl3D.Color((byte)255, (byte)255, (byte)(255));
                gl3D.TexCoord(0f, 1f);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
                gl3D.TexCoord(1f, 1f);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.TexCoord(1f, 0f);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.TexCoord(0f, 0f);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
            gl3D.End();

            gl3D.Begin(BeginMode.Polygon);
                gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
                numNor++;
                gl3D.Color((byte)255, (byte)255, (byte)(255));
                gl3D.TexCoord(0f, 1f);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.TexCoord(1f, 1f);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.TexCoord(1f, 0f);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.TexCoord(0f, 0f);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
            gl3D.End();
        }

        gl3D.Begin(OpenGL.GL_TRIANGLES);
            gl3D.Normal(normals[numNor][0], normals[numNor][1], normals[numNor][2]);
            numNor++;
            gl3D.Color((byte)255, (byte)255, (byte)(255));
            gl3D.TexCoord(0f, 0f);
            gl3D.Vertex(Figure[^1].section1[0], Figure[^1].section1[1], Figure[^1].section1[2]);
            gl3D.TexCoord(0.5f, 1f);
            gl3D.Vertex(Figure[^1].section2[0], Figure[^1].section2[1], Figure[^1].section2[2]);
            gl3D.TexCoord(1f, 0f);
            gl3D.Vertex(Figure[^1].section3[0], Figure[^1].section3[1], Figure[^1].section3[2]);
        gl3D.End();

        // Отключаем текстурирование
        gl3D.Disable(OpenGL.GL_TEXTURE_2D);
    }
}
