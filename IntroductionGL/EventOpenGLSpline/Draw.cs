namespace IntroductionGL;

//: Рисование сетки, осей и сплайна
public partial class OpenGLSpline : Window {

    //: Рисование сетки
    private void DrawGrid() {

        // Запоминаем матрицу
        gl3D.PushMatrix();

        // Перемещаем сетку
        gl3D.Translate(Position.X, Position.Y, 0);

        // Рисуем сетку
        gl3D.LineWidth(1);
        gl3D.Color((byte)203, (byte)200, (byte)197);
        for (int i = 0; i < 500; i++) {
            gl3D.Begin(BeginMode.Lines);
                gl3D.Vertex(i*90, 8000f);
                gl3D.Vertex(i*90, -8000f);
            gl3D.End();
            gl3D.Begin(BeginMode.Lines);
                gl3D.Vertex(-i * 90, 8000f);
                gl3D.Vertex(-i * 90, -8000f);
            gl3D.End();
            gl3D.Begin(BeginMode.Lines);
                gl3D.Vertex(8000f, i * 90);
                gl3D.Vertex(-8000f, i * 90);
            gl3D.End();
            gl3D.Begin(BeginMode.Lines);
                gl3D.Vertex(8000f, -i * 90);
                gl3D.Vertex(-8000f, -i * 90);
            gl3D.End();
        }

        // Возвращаем матрицу
        gl3D.PopMatrix();
    }

    //: Рисование осей
    private void DrawAxes() {

        float centerX   = (float)(openGLControl3D.ActualWidth / 2f) + 26;   // Центр Ox
        float centerY   = (float)(openGLControl3D.ActualHeight / 2f) + 10;  // Центр Oy
        float lenStreak = 8f;                                               // Длина штриха
        float shiftLineX      = 20;   // Сдвиг подписи c Ox
        float shiftLineY      = 15;   // Сдвиг подписи c Oy
        float shiftSeparatorX = -16;  // Сдвиг подписи от линии разделения c Ox
        float shiftSeparatorY = -4;   // Сдвиг подписи от линии разделения c Oy
        float distance = 90;          // Расстояние между подписями

        
        // Запоминаем матрицу
        gl3D.PushMatrix();

        // Перемещаемся 
        gl3D.Translate(Position.X, Position.Y, 0);

        // Подпись в начале координат
        gl3D.DrawText((int)(centerX + 2 + Position.X), (int)(centerY - 14 + Position.Y), 0f, 0f, 0f, "Courier New Bold", 12, "0,00");

        // Рисуем координатные подписи
        for (int i = 1; i < 500; i++) {
            gl3D.LineWidth(2f);
            gl3D.Color(1f, 0f, 1f);

            // По Оx
            gl3D.Begin(BeginMode.Lines);
                gl3D.Vertex(centerX + i * distance, centerY - lenStreak);
                gl3D.Vertex(centerX + i * distance, centerY + lenStreak);
            gl3D.End();
            gl3D.Begin(BeginMode.Lines);
                gl3D.Vertex(centerX - i * distance, centerY - lenStreak);
                gl3D.Vertex(centerX - i * distance, centerY + lenStreak);
            gl3D.End();

            // По Oy
            gl3D.Begin(BeginMode.Lines);
                gl3D.Vertex(centerX - lenStreak, centerY + i * distance);
                gl3D.Vertex(centerX + lenStreak, centerY + i * distance);
            gl3D.End();
            gl3D.Begin(BeginMode.Lines);
                gl3D.Vertex(centerX - lenStreak, centerY - i * distance);
                gl3D.Vertex(centerX + lenStreak, centerY - i * distance);
            gl3D.End();
        
            // Подписываем циферки на Осях
            string sign_pos = (i * Scale).ToString("F2");        // Положительная
            string sign_neg = ((-1) * i * Scale).ToString("F2"); // Отрицательная

            // По Ox
            gl3D.DrawText((int)(centerX + i * distance + shiftSeparatorX + Position.X), (int)(centerY - shiftLineX + Position.Y), 0f, 0f, 0f, "Courier New Bold", 12, sign_pos);
            gl3D.DrawText((int)(centerX - i * distance + shiftSeparatorX + Position.X), (int)(centerY - shiftLineX + Position.Y), 0f, 0f, 0f, "Courier New Bold", 12, sign_neg);

            // По Oy
            gl3D.DrawText((int)(centerX + shiftLineY + Position.X), (int)(centerY + i * distance + shiftSeparatorY + Position.Y), 0f, 0f, 0f, "Courier New Bold", 12, sign_pos);
            gl3D.DrawText((int)(centerX + shiftLineY + Position.X), (int)(centerY - i * distance + shiftSeparatorY + Position.Y), 0f, 0f, 0f, "Courier New Bold", 12, sign_neg);
        }


        // Рисование координатных осей
        gl3D.LineWidth(4f);
        gl3D.Color(0f, 0f, 0f, 1f);
        gl3D.Begin(BeginMode.Lines);
            gl3D.Vertex(-8000f, centerY);
            gl3D.Vertex(8000f, centerY);
        gl3D.End();
        gl3D.Begin(BeginMode.Lines);
            gl3D.Vertex(centerX, -8000f);
            gl3D.Vertex(centerX, 8000f);
        gl3D.End();

        // Возвращаем матрицу
        gl3D.PopMatrix();
    }

    //: Рисование ломанной
    private void DrawLineStrip() {

        // Запоминаем матрицу
        gl3D.PushMatrix();

        // Перемещаемся 
        gl3D.Translate(Position.X, Position.Y, 0);

        // Для сглаживания
        gl3D.Enable(OpenGL.GL_BLEND);
        gl3D.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

        // Рисование ломанной линии
        gl3D.Enable(OpenGL.GL_LINE_SMOOTH);
        gl3D.Color(0f, 1f, 1f, 0.5f);
        gl3D.Begin(BeginMode.LineStrip);
        for (int i = 0; i < ScreenPoint.Count; i++)
            gl3D.Vertex(ScreenPoint[i].X, ScreenPoint[i].Y);
        gl3D.End();

        // Рисование точек
        gl3D.Enable(OpenGL.GL_POINT_SMOOTH);
        gl3D.PointSize(9f);
        gl3D.Begin(BeginMode.Points);
        for (int i = 0; i < ScreenPoint.Count; i++) {
            (var R, var G, var B) = ScreenPoint[i].color;
            gl3D.Color(R, G, B);
            gl3D.Vertex(ScreenPoint[i].X, ScreenPoint[i].Y);
        }
        gl3D.End();

        // Возвращаем матрицу
        gl3D.PopMatrix();
    }

    //: Рисование сплайна NURBS
    private void DrawSpline() {
        
        // Запоминаем матрицу
        gl3D.PushMatrix();

        // Перемещаем сетку
        gl3D.Translate(Position.X, Position.Y, 0);
        
        // Рисование сплайна
        gl3D.Color(0.8f, 0.3333f, 0f);
        gl3D.Begin(BeginMode.LineStrip);
        for (int i = 0; i < Spline.Count; i++)
            gl3D.Vertex(Spline[i].X, Spline[i].Y);
        gl3D.End();

        // Возвращаем матрицу
        gl3D.PopMatrix();
    }

    //: Рисование текста позиции мыши
    private void DrawMouseCoordinate(Point pos) {

        // Высчитываем OpenGL позицию мыши;
        Point newpos = ConvertCoordinateToReal(pos);

        // Подписываем циферки на Осях
        string sCoordinate = $"({newpos.X.ToString("F2")}, {newpos.Y.ToString("F2")})";

        // Положение на экране OpenGL
        var location = new Point(
            (float)(pos.X + 50),
            (float)(pos.Y + 40)
        );

        // Показ текста возле курсора
        gl3D.DrawText((int)(location.X), (int)(location.Y), 0f, 0f, 0f, "Courier New Bold", 12, sCoordinate);
    }
    
    //: Рисование текста весов точек
    private void DrawWeights() {

        // Если не показывать веса
        if (!IsViewWeights) return;

        // Запоминаем матрицу
        gl3D.PushMatrix();

        // Перемещаем сетку
        gl3D.Translate(Position.X, Position.Y, 0);

        // Печатаем вес точки
        for (int i = 0; i < ScreenPoint.Count; i++) {
            
            // Строка с текстом
            string strWeight = "w = " + ScreenPoint[i].w.ToString("F2");

            // Позиции текста
            var pos = new Point(
                ScreenPoint[i].X - 10 + Position.X,
                ScreenPoint[i].Y + 10 + Position.Y
            );

            // Показ строки
            gl3D.DrawText((int)(pos.X), (int)(pos.Y), 0f, 0f, 0f, "Courier New Bold", 12, strWeight);
        }

        // Возвращаем матрицу
        gl3D.PopMatrix();
    }

    //: Преобразование экранных координат к реальным
    private Point ConvertCoordinateToReal(Point point) {

        // Создаем новые координаты с учетом сдвига
        float x = point.X - Position.X;
        float y = point.Y - Position.Y;

        // Учитываем положение центра координат
        x = (float)(x - openGLControl3D.ActualWidth / 2f);
        y = (float)(y - openGLControl3D.ActualHeight / 2f);

        // Учет пикселей на одно деление
        x /= 90f;
        y /= 90f;

        // Учет масштабирования
        x *= Scale;
        y *= Scale;

        return new Point(x, y);
    }
    
    //: Преобразование реальных координат к экранным
    private Point ConvertCoordinateToScreen(Point point) {

        // Учет масштабирования
        float x = point.X / Scale;
        float y = point.Y / Scale;

        // Учет пикселей на одно деление
        x *= 90f;
        y *= 90f;

        // Учитываем положение центра координат
        x = (float)(x + openGLControl3D.ActualWidth / 2f);
        y = (float)(y + openGLControl3D.ActualHeight / 2f);

        // Учитываем сдвиг
        x = x + 26;
        y = y + 10;

        return new Point(x, y);
    }

    //: Расчет координат сплайна
    private void CalculationSpline() {

        // Количество требуемых точек для сплайна
        int countNeedPoint = OrderBasicFunction;

        // Количество заданных контрольных точек
        int countPoint = ControlPoint.Count;

        // Если не хватает, сплайн не рисуем
        if (ControlPoint.Count <= countNeedPoint) return; // Не хватает точек

        // Если точек хватает, заполняем парметры
        List<int> param = new List<int>();
        for (int i = 0; i < countNeedPoint + 1; i++)
            param.Add(0);

        for (int i = 0; i < countPoint - (countNeedPoint + 1); i++)
            param.Add(i + 1);

        for (int i = countPoint - countNeedPoint; i < countPoint + 1; i++)
            param.Add(countPoint - countNeedPoint);

        // Очищаем сплайн
        Spline.Clear();

        // Число точек для построения сплайна
        int countSplinePoint = countPoint * 50;

        // Вычисляем шаг по вектору параметров
        float h = (param[^1] - param[0]) / (float)countSplinePoint;

        // Вычисляем координаты сплайна
        for (int i = 0; i < countSplinePoint + 1; i++) {

            float u = i == countSplinePoint ? param[^1] : param[0] + i * h;
            float x = 0, y = 0, w = 0;

            for (int k = 0; k < countPoint; k++) {

                // Вычисление базовой функии
                float coeff = BasicFunc(u, k, param);
                w += ControlPoint[k].w * coeff;
                x += ControlPoint[k].w * ControlPoint[k].X * coeff;
                y += ControlPoint[k].w * ControlPoint[k].Y * coeff;
            }
            Spline.Add(ConvertCoordinateToScreen(new Point(x / w, y / w)));
        }

    }

    //: Расчет базовой функии
    private float BasicFunc(float U, int I, List<int> param) {

        // Количество требуемых точек для сплайна
        int countNeedPoint = OrderBasicFunction;
        int countPointInd = param.Count - 1;

        if ((I == 0 && U == param[0]) ||
            (I == countPointInd - countNeedPoint - 1 && U == param[countPointInd])) return 1;

        if (U < param[I] || U >= param[I + countNeedPoint + 1])
            return 0;

        float[] B = new float[30];
        for (int p = 0; p <= countNeedPoint; p++)
            if (U >= param[I + p] && U < param[I + p + 1])
                B[p] = 1;
            else B[p] = 0;

        for (int s = 1; s <= countNeedPoint; s++) {

            float coeff;
            if (B[0] == 0)
                coeff = 0;
            else
                coeff = ((U - param[I]) * B[0]) / (param[I + s] - param[I]);

            for (int j = 0; j < countNeedPoint - s + 1; j++) {

                if (B[j + 1] == 0) {
                    B[j] = coeff;
                    coeff = 0;
                }
                else {
                    float Ul = param[I + j + 1];
                    float UR = param[I + j + s + 1];
                    float sub = B[j + 1] / (UR - Ul);
                    B[j] = coeff + (UR - U) * sub;
                    coeff = (U - Ul) * sub;
                }
            }
        }
        return B[0];
    }
}