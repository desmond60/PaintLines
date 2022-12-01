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

        // Компоненты глобального освещения
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_TWO_SIDE, OpenGL.GL_TRUE);
        var coeff_amb = Background.Text == String.Empty ? 0 : Single.Parse(Background.Text);
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, new[] { coeff_amb, coeff_amb, coeff_amb, 1f });
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_LOCAL_VIEWER, OpenGL.GL_FALSE);
        gl3D.LightModel(OpenGL.GL_LIGHT_MODEL_COLOR_CONTROL_EXT, OpenGL.GL_SINGLE_COLOR_EXT);

        // Задание материала
        gl3D.Enable(OpenGL.GL_COLOR_MATERIAL);

        gl3D.Enable(OpenGL.GL_NORMALIZE);

        // Если туман включен
        if (isFog) {
            gl3D.ClearColor(colorFog.R / 255f, colorFog.G / 255f, colorFog.B / 255f, colorFog.A / 255f);
            gl3D.Enable(OpenGL.GL_FOG);
            gl3D.Fog(OpenGL.GL_FOG_MODE, OpenGL.GL_EXP2);
            gl3D.Fog(OpenGL.GL_FOG_COLOR, new float[] { colorFog.R / 255f, colorFog.G / 255f, colorFog.B / 255f, colorFog.A / 255f });
            gl3D.Fog(OpenGL.GL_FOG_DENSITY, 0.1f);
        }
        else {
            gl3D.ClearColor(0f, 0f, 0f, 1f);
            gl3D.Disable(OpenGL.GL_FOG);
        }


        uint light_num = 16384;
        for (int i = 0, id = 0; i < lights.Count; i++) {

            // Показывать источник свет?
            if (lights[i].IsShow)
            {
                uint cur_light = (uint)(light_num + id++); // Оперделяем номер источника света

                // Максимум в OpenGL 8 исчтоников света
                if (id == 8)
                    return;

                // Инициализируем 3 главные компоненты
                gl3D.Light(cur_light, OpenGL.GL_AMBIENT, (float[])lights[i].Ambient);
                gl3D.Light(cur_light, OpenGL.GL_DIFFUSE, (float[])lights[i].Diffuse);
                gl3D.Light(cur_light, OpenGL.GL_SPECULAR, (float[])lights[i].Specular);

                // Если исчтоник с затуханием
                if (lights[i].IsAttenuation) {
                    gl3D.Light(cur_light, OpenGL.GL_CONSTANT_ATTENUATION, lights[i].Constant);
                    gl3D.Light(cur_light, OpenGL.GL_LINEAR_ATTENUATION, lights[i].Linear);
                    gl3D.Light(cur_light, OpenGL.GL_QUADRATIC_ATTENUATION, lights[i].Quadratic);
                }
                else {
                    gl3D.Light(cur_light, OpenGL.GL_CONSTANT_ATTENUATION, 1f);
                    gl3D.Light(cur_light, OpenGL.GL_LINEAR_ATTENUATION, 0f);
                    gl3D.Light(cur_light, OpenGL.GL_QUADRATIC_ATTENUATION, 0f);
                }

                switch (lights[i].Type)
                {
                    case TypeLight.DIRECTED:
                        var directed = (float[])(-lights[i].Direction);
                        directed[3] = 0f; // Делаем омегу равную нулю, чтобы свет был направленный, а иначе будет точечный
                        gl3D.Light(cur_light, OpenGL.GL_POSITION, directed);
                        break;

                    case TypeLight.POINT or TypeLight.POINT_ATTENUATION:
                        gl3D.Light(cur_light, OpenGL.GL_POSITION, (float[])lights[i].Position);
                        break;

                    case TypeLight.SPOT or TypeLight.SPOT_ATTENUATION:
                        gl3D.Light(cur_light, OpenGL.GL_POSITION, (float[])lights[i].Position);
                        gl3D.Light(cur_light, OpenGL.GL_SPOT_EXPONENT, lights[i].Exponent);
                        gl3D.Light(cur_light, OpenGL.GL_SPOT_CUTOFF, lights[i].Cutoff);
                        gl3D.Light(cur_light, OpenGL.GL_SPOT_DIRECTION, (float[])lights[i].Direction);

                        break;
                }

                // Включаем источник
                gl3D.Enable(cur_light);
            }
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

    //: Рисование кубика под ИС
    private void DrawCube(Vector<float> center, Color color) {

        // Сохраняем текущую матрицу
        gl3D.PushMatrix();

        // Перемещяем плоскость в координаты центра
        gl3D.Translate(center[0], center[1], center[2]);

        // Рисуем куб
        gl3D.Color(color.R / 255f, color.G / 255f, color.B / 255f);

        // FRONT
        gl3D.Begin(BeginMode.Polygon);
        gl3D.Vertex(0.5, -0.5, -0.5);
        gl3D.Vertex(0.5, 0.5, -0.5);
        gl3D.Vertex(-0.5, 0.5, -0.5);
        gl3D.Vertex(-0.5, -0.5, -0.5);
        gl3D.End();

        // BACK
        gl3D.Begin(BeginMode.Polygon);
        gl3D.Vertex(0.5, -0.5, 0.5);
        gl3D.Vertex(0.5, 0.5, 0.5);
        gl3D.Vertex(-0.5, 0.5, 0.5);
        gl3D.Vertex(-0.5, -0.5, 0.5);
        gl3D.End();

        // RIGHT
        gl3D.Begin(BeginMode.Polygon);
        gl3D.Vertex(0.5, -0.5, -0.5);
        gl3D.Vertex(0.5, 0.5, -0.5);
        gl3D.Vertex(0.5, 0.5, 0.5);
        gl3D.Vertex(0.5, -0.5, 0.5);
        gl3D.End();

        // LEFT
        gl3D.Begin(BeginMode.Polygon);
        gl3D.Vertex(-0.5, -0.5, 0.5);
        gl3D.Vertex(-0.5, 0.5, 0.5);
        gl3D.Vertex(-0.5, 0.5, -0.5);
        gl3D.Vertex(-0.5, -0.5, -0.5);
        gl3D.End();

        // TOP
        gl3D.Begin(BeginMode.Polygon);
        gl3D.Vertex(0.5, 0.5, 0.5);
        gl3D.Vertex(0.5, 0.5, -0.5);
        gl3D.Vertex(-0.5, 0.5, -0.5);
        gl3D.Vertex(-0.5, 0.5, 0.5);
        gl3D.End();

        // BOTTOM
        gl3D.Begin(BeginMode.Polygon);
        gl3D.Vertex(0.5, -0.5, -0.5);
        gl3D.Vertex(0.5, -0.5, 0.5);
        gl3D.Vertex(-0.5, -0.5, 0.5);
        gl3D.Vertex(-0.5, -0.5, -0.5);
        gl3D.End();

        // Восстанавливаем матрицу
        gl3D.PopMatrix();
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
