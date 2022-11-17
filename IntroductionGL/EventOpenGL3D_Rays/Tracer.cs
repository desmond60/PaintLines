namespace IntroductionGL;

//: Логика взаимодействия с окном OpenGL3D_Rays (трассировка)
public partial class OpenGL3D_Rays : Window {

    //: Основной метод трассировки
    private void RayTracing() {

        float N = 0.1f;                                                                     // Расстояние от точки взгляда, до ближайшей плоскости
        float tetha = (float)(60 * (PI / 180.0f));                                          // Угол обхвата
        float aspect = (float)(openGLControl3D.ActualWidth / openGLControl3D.ActualHeight); // Вычисляем соотношение между шириной и высотой
        
        // Переменные для вычисления координат (r,c) пикселя и инициалзиция PixelSize
        float H = (float)(N * Tan(tetha / 2.0));
        float W = H * aspect;
        int pixelsize = 1;

        // Вектора 
        Vector<float> w = Vector<float>.Normalize(camera.Position - camera.Orientation);
        Vector<float> u = Vector<float>.Normalize(Vector<float>.GetNormLine(camera.Rotation, w));
        Vector<float> v = Vector<float>.Normalize(Vector<float>.GetNormLine(w, u));

        // Луч
        Ray ray = new Ray(camera.Position);

        // OpenGL для 2D рисования
        //gl3D.MatrixMode(OpenGL.GL_MODELVIEW);
        //gl3D.LoadIdentity();
        //gl3D.MatrixMode(OpenGL.GL_PROJECTION);
        //gl3D.LoadIdentity();
        //gl3D.Ortho2D(0, openGLControl3D.ActualWidth, 0, openGLControl3D.ActualHeight);
        //gl3D.Disable(OpenGL.GL_LIGHTING);

        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap((int)openGLControl3D.ActualWidth, (int)openGLControl3D.ActualHeight);

        //(int)openGLControl3D.ActualHeight
        //(int)openGLControl3D.ActualWidth
        for (int r = 0; r < (int)openGLControl3D.ActualHeight; r += pixelsize)
        {
            for (int c = 0; c < (int)openGLControl3D.ActualWidth; c += pixelsize)
            {
                // Координаты
                float u_c = -W + (2 * c * W) / (float)openGLControl3D.ActualWidth;
                float v_c = -H + (2 * r * H) / (float)openGLControl3D.ActualHeight;

                // Коэффициент отклонения луча
                float coeff = 0.0001f;

                // Получаем нужный цвет
                // Для сглаживание изображения посылаем 4 луча, с небольшим отклонением
                ColorF color = new ColorF(0f, 0f, 0f);

                for (int i = 0; i < 2; i++) {
                    Vector<float> ori = new Vector<float>(new float[] {
                        (float)(-N * w[0] + u_c * u[0] + v_c * v[0] + coeff),
                        (float)(-N * w[1] + u_c * u[1] + v_c * v[1] - coeff),
                        (float)(-N * w[2] + u_c * u[2] + v_c * v[2] - coeff)
                    });
                    ray = ray with { Orientation = (Vector<float>)Vector<float>.Normalize(ori).Clone() };
                    color.Add(Tint(ray));
                        
                    ori = new Vector<float>(new float[] {
                        (float)(-N * w[0] + u_c * u[0] + v_c * v[0] - coeff),
                        (float)(-N * w[1] + u_c * u[1] + v_c * v[1] + coeff),
                        (float)(-N * w[2] + u_c * u[2] + v_c * v[2] + coeff)
                    });
                    ray = ray with { Orientation = (Vector<float>)Vector<float>.Normalize(ori).Clone() };
                    color.Add(Tint(ray));

                    coeff = -coeff;
                }

                int R = (int)(color.R / 4.0f * 255);
                int G = (int)(color.G / 4.0f * 255);
                int B = (int)(color.B / 4.0f * 255);

                R = R > 255 ? 255 : R;
                G = G > 255 ? 255 : G;
                B = B > 255 ? 255 : B;

                // Закрашывание блока пикселей цветом, который получился
                System.Drawing.Color colorBMP = System.Drawing.Color.FromArgb(R, G, B);
                bmp.SetPixel(c, r, colorBMP);

                //gl3D.Color((float)(color.R / 4.0f), (float)(color.G / 4.0f), (float)(color.B / 4.0f));
                //gl3D.Rect(c, r, c + pixelsize, r + pixelsize);
            }
        }
        IsRaysTracer = false;
        bmp.Save(@"D:\img.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
    }

    private ColorF Tint(Ray ray)
    {
        int MaxLevel = 4; // Максимальный уровень
        CollisionList best = GetFirstCollision(ray);

        // Вернем фоновый цвет, если луч прошел мимо
        if (best.numCollision == 0) return new ColorF(0f, 0f, 0f);

        // Копия данных о первом соударении
        CollisionInfo col = best.collision[0] with { };
        int nObj = col.objectNum;

        // Смотрим с чем было соударение и создаем цвет
        ColorF color = new ColorF(0f , 0f, 0f);
        switch (col.objectType)
        {
            #region Sphere
            // Сфера
            case 0:

                // Устанавливаем эмиссионный цвет объекта
                color = new ColorF(
                    stage.Spheres[nObj].Emission[0],
                    stage.Spheres[nObj].Emission[1],
                    stage.Spheres[nObj].Emission[2]
                );

                // Для тени
                Vector<float> normal = Vector<float>.Normalize(col.NormalPoint);
                Ray probe = new Ray(col.CollisionPoint - 0.0001f * ray.Orientation);
                probe = probe with { level = 1 };

                // Проходимся по всем заданным источникам света
                for (int i = 0; i < stage.Lights.Length; i++)
                {
                    // Добавим в цвет фоновую компоненту
                    ColorF colorAmbient = new ColorF(
                        stage.Spheres[nObj].Ambient[0] * stage.Lights[i].Color.R,
                        stage.Spheres[nObj].Ambient[1] * stage.Lights[i].Color.G,
                        stage.Spheres[nObj].Ambient[2] * stage.Lights[i].Color.B
                    );
                    color.Add(colorAmbient);

                    // Обрабатываем тень (если точка в тени, то диффузная и зеркальная компонента не учитывается)
                    probe = probe with { Orientation = stage.Lights[i].Position - col.CollisionPoint };
                    if (stage.IsShade(probe)) continue;

                    // Подсчет вектора от точки соударения до источника света
                    Vector<float> v = Vector<float>.Normalize(stage.Lights[i].Position - col.CollisionPoint);

                    // Считаем член Ламберта
                    float Lambert = Vector<float>.Scalar(v, normal);
                    if (Lambert > 0.0f)
                    {
                        // Добавляем диффузную компоненту
                        ColorF colorDiffuse = new ColorF(
                            Lambert * stage.Spheres[nObj].Diffuse[0] * stage.Lights[i].Color.R,
                            Lambert * stage.Spheres[nObj].Diffuse[1] * stage.Lights[i].Color.G,
                            Lambert * stage.Spheres[nObj].Diffuse[2] * stage.Lights[i].Color.B
                        );
                        color.Add(colorDiffuse);
                    }

                    // Направление на наблюдателя
                    Vector<float> o = Vector<float>.Normalize(new Vector<float>(new float[] {
                        -ray.Orientation[0],
                        -ray.Orientation[1],
                        -ray.Orientation[2]
                    }));
                    Vector<float> h = Vector<float>.Normalize(o + v);

                    // Считаем член Фонга
                    float Phong = Vector<float>.Scalar(h, normal);
                    if (Phong > 0.0f)
                    {
                        // Добавляем зеркальную компоненту
                        float coeffPhong = (float)Pow(Phong, stage.Spheres[nObj].Shine);
                        ColorF colorSpecular = new ColorF(
                            coeffPhong * stage.Spheres[nObj].Specular[0] * stage.Lights[i].Color.R,
                            coeffPhong * stage.Spheres[nObj].Specular[1] * stage.Lights[i].Color.G,
                            coeffPhong * stage.Spheres[nObj].Specular[2] * stage.Lights[i].Color.B
                        );
                        color.Add(colorSpecular);
                    }

                    // Если достигут максимальный уровень рекурсии
                    if (ray.level == MaxLevel) return color;

                    // Если объект достаточно блестящий
                    if (stage.Spheres[nObj].Shine > 0.5)
                    {
                        // Направление отраженного луча
                        Vector<float> reflectedOri = ray.Orientation - 2 * normal * Vector<float>.Scalar(ray.Orientation, normal);

                        // Отраженный луч
                        Ray reflected = new Ray(col.CollisionPoint - 0.0001f * ray.Orientation);
                        reflected = reflected with
                        {
                            level = ray.level + 1,
                            Orientation = Vector<float>.Normalize(reflectedOri)
                        };

                        // Добавляем отраженный свет
                        ColorF ColorReflected = new ColorF(
                            stage.Spheres[nObj].Specular[0],
                            stage.Spheres[nObj].Specular[1],
                            stage.Spheres[nObj].Specular[2]
                        );
                        color.Add(Tint(reflected), ColorReflected);
                    }
                }

                break;
            #endregion

            #region Tetrahedron
            // Тетраэдр
            case 1:

                // Устанавливаем эмиссионный цвет объекта
                color = new ColorF(
                    stage.Tetrahedrons[nObj].Emission[0],
                    stage.Tetrahedrons[nObj].Emission[1],
                    stage.Tetrahedrons[nObj].Emission[2]
                );

                // Для тени
                normal = Vector<float>.Normalize(col.NormalPoint);
                probe = new Ray(col.CollisionPoint - 0.0001f * ray.Orientation);
                probe = probe with { level = 1 };

                // Проходимся по всем заданным источникам света
                for (int i = 0; i < stage.Lights.Length; i++)
                {
                    // Добавим в цвет фоновую компоненту
                    ColorF colorAmbient = new ColorF(
                        stage.Tetrahedrons[nObj].Ambient[0] * stage.Lights[i].Color.R,
                        stage.Tetrahedrons[nObj].Ambient[1] * stage.Lights[i].Color.G,
                        stage.Tetrahedrons[nObj].Ambient[2] * stage.Lights[i].Color.B
                    );
                    color.Add(colorAmbient);

                    // Обрабатываем тень (если точка в тени, то диффузная и зеркальная компонента не учитывается)
                    probe = probe with { Orientation = stage.Lights[i].Position - col.CollisionPoint };
                    if (stage.IsShade(probe)) continue;

                    // Подсчет вектора от точки соударения до источника света
                    Vector<float> v = Vector<float>.Normalize(stage.Lights[i].Position - col.CollisionPoint);

                    // Считаем член Ламберта
                    float Lambert = Vector<float>.Scalar(v, normal);
                    if (Lambert > 0.0f)
                    {
                        // Добавляем диффузную компоненту
                        ColorF colorDiffuse = new ColorF(
                            Lambert * stage.Tetrahedrons[nObj].Diffuse[0] * stage.Lights[i].Color.R,
                            Lambert * stage.Tetrahedrons[nObj].Diffuse[1] * stage.Lights[i].Color.G,
                            Lambert * stage.Tetrahedrons[nObj].Diffuse[2] * stage.Lights[i].Color.B
                        );
                        color.Add(colorDiffuse);
                    }

                    // Направление на наблюдателя
                    Vector<float> o = Vector<float>.Normalize(new Vector<float>(new float[] {
                        -ray.Orientation[0],
                        -ray.Orientation[1],
                        -ray.Orientation[2]
                    }));
                    Vector<float> h = Vector<float>.Normalize(o + v);

                    // Считаем член Фонга
                    float Phong = Vector<float>.Scalar(h, normal);
                    if (Phong > 0.0f)
                    {
                        // Добавляем зеркальную компоненту
                        float coeffPhong = (float)Pow(Phong, stage.Tetrahedrons[nObj].Shine);
                        ColorF colorSpecular = new ColorF(
                            coeffPhong * stage.Tetrahedrons[nObj].Specular[0] * stage.Lights[i].Color.R,
                            coeffPhong * stage.Tetrahedrons[nObj].Specular[1] * stage.Lights[i].Color.G,
                            coeffPhong * stage.Tetrahedrons[nObj].Specular[2] * stage.Lights[i].Color.B
                        );
                        color.Add(colorSpecular);
                    }

                    // Если достигут максимальный уровень рекурсии
                    if (ray.level == MaxLevel) return color;

                    // Если объект достаточно блестящий
                    if (stage.Tetrahedrons[nObj].Shine > 0.5)
                    {
                        // Направление отраженного луча
                        Vector<float> reflectedOri = ray.Orientation - 2 * normal * Vector<float>.Scalar(ray.Orientation, normal);

                        // Отраженный луч
                        Ray reflected = new Ray(col.CollisionPoint - 0.0001f * ray.Orientation);
                        reflected = reflected with
                        {
                            level = ray.level + 1,
                            Orientation = Vector<float>.Normalize(reflectedOri)
                        };

                        // Добавляем отраженный свет
                        ColorF ColorReflected = new ColorF(
                            stage.Tetrahedrons[nObj].Specular[0],
                            stage.Tetrahedrons[nObj].Specular[1],
                            stage.Tetrahedrons[nObj].Specular[2]
                        );
                        color.Add(Tint(reflected), ColorReflected);
                    }
                }
                break;
            #endregion

            #region Square
            // Плоскость
            case 2:

                // Устанавливаем эмиссионный цвет объекта
                color = new ColorF(
                    stage.Square.Emission[0],
                    stage.Square.Emission[1],
                    stage.Square.Emission[2]
                );

                // Для тени
                normal = Vector<float>.Normalize(col.NormalPoint);
                probe = new Ray(col.CollisionPoint - 0.0001f * ray.Orientation);
                probe = probe with { level = 1 };

                // Проходимся по всем заданным источникам света
                for (int i = 0; i < stage.Lights.Length; i++)
                {
                    // Добавим в цвет фоновую компоненту
                    ColorF colorAmbient = new ColorF(
                        stage.Square.Ambient[0] * stage.Lights[i].Color.R,
                        stage.Square.Ambient[1] * stage.Lights[i].Color.G,
                        stage.Square.Ambient[2] * stage.Lights[i].Color.B
                    );
                    color.Add(colorAmbient);

                    // Обрабатываем тень (если точка в тени, то диффузная и зеркальная компонента не учитывается)
                    probe = probe with { Orientation = stage.Lights[i].Position - col.CollisionPoint };
                    if (stage.IsShade(probe)) continue;

                    // Подсчет вектора от точки соударения до источника света
                    Vector<float> v = Vector<float>.Normalize(stage.Lights[i].Position - col.CollisionPoint);

                    // Считаем член Ламберта
                    float Lambert = Vector<float>.Scalar(v, normal);
                    if (Lambert > 0.0f)
                    {
                        // Добавляем диффузную компоненту
                        ColorF colorDiffuse = new ColorF(
                            Lambert * stage.Square.Diffuse[0] * stage.Lights[i].Color.R,
                            Lambert * stage.Square.Diffuse[1] * stage.Lights[i].Color.G,
                            Lambert * stage.Square.Diffuse[2] * stage.Lights[i].Color.B
                        );
                        color.Add(colorDiffuse);
                    }

                    // Направление на наблюдателя
                    Vector<float> o = Vector<float>.Normalize(new Vector<float>(new float[] {
                        -ray.Orientation[0],
                        -ray.Orientation[1],
                        -ray.Orientation[2]
                    }));
                    Vector<float> h = Vector<float>.Normalize(o + v);

                    // Считаем член Фонга
                    float Phong = Vector<float>.Scalar(h, normal);
                    if (Phong > 0.0f)
                    {
                        // Добавляем зеркальную компоненту
                        float coeffPhong = (float)Pow(Phong, stage.Square.Shine);
                        ColorF colorSpecular = new ColorF(
                            coeffPhong * stage.Square.Specular[0] * stage.Lights[i].Color.R,
                            coeffPhong * stage.Square.Specular[1] * stage.Lights[i].Color.G,
                            coeffPhong * stage.Square.Specular[2] * stage.Lights[i].Color.B
                        );
                        color.Add(colorSpecular);
                    }

                    // Если достигут максимальный уровень рекурсии
                    if (ray.level == MaxLevel) return color;

                    // Если объект достаточно блестящий
                    if (stage.Square.Shine > 0.5)
                    {
                        // Направление отраженного луча
                        Vector<float> reflectedOri = ray.Orientation - 2 * normal * Vector<float>.Scalar(ray.Orientation, normal);

                        // Отраженный луч
                        Ray reflected = new Ray(col.CollisionPoint - 0.0001f * ray.Orientation);
                        reflected = reflected with
                        {
                            level = ray.level + 1,
                            Orientation = Vector<float>.Normalize(reflectedOri)
                        };

                        // Добавляем отраженный свет
                        ColorF ColorReflected = new ColorF(
                            stage.Square.Specular[0],
                            stage.Square.Specular[1],
                            stage.Square.Specular[2]
                        );
                        color.Add(Tint(reflected), ColorReflected);
                    }
                }
                break;
            #endregion
        }

        return color;
    }

    private CollisionList GetFirstCollision(Ray ray)
    {
        CollisionList curCol; // Текущая запись пересечений
        CollisionList best = new CollisionList();   // Лучшая запись пересечений

        // Проходимся по всем сферам
        for (int i = 0; i < stage.Spheres.Length; i++)
        {
            if (!stage.Spheres[i].isShow) continue;

            curCol = new CollisionList();
            if (!stage.Spheres[i].collision(ray, ref curCol)) continue;

            // Соударение есть, запомним номер объекта
            for (int j = 0; j < curCol.numCollision; j++)
                curCol.collision[j].objectNum = i;

            // Переписываем если это первое соударение или если оно лучше
            if (best.numCollision == 0 || curCol.collision[0].CollisionTime < best.collision[0].CollisionTime)
                best = curCol with { };
        }

        // Проходимся по всем тетраэдрам
        for (int i = 0; i < stage.Tetrahedrons.Length; i++)
        {
            if (!stage.Tetrahedrons[i].isShow) continue;

            curCol = new CollisionList();
            if (!stage.Tetrahedrons[i].collision(ray, ref curCol)) continue;

            // Соударение есть, запомним номер объекта
            for (int j = 0; j < curCol.numCollision; j++)
                curCol.collision[j].objectNum = i;

            // Переписываем если это первое соударение или если оно лучше
            if (best.numCollision == 0 || curCol.collision[0].CollisionTime < best.collision[0].CollisionTime)
                best = curCol with { };
        }

        // Проходимся по плоскости
        curCol = new CollisionList();
        if (stage.Square.collision(ray, ref curCol))
            if (best.numCollision == 0 || curCol.collision[0].CollisionTime < best.collision[0].CollisionTime)
                best = curCol with { };

        return best;
    }
}