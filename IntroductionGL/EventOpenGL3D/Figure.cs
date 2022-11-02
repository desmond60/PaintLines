﻿namespace IntroductionGL;

//: Рисование фигуры в разных режимах
public partial class OpenGL3D : Window {

    //: Главная функция рисование фигуры
    private void DrawFigure() {

        if (!isSceleton)
        {
            DrawNotSceleton();
        }
        else {
            DrawSceleton();
        }

        if (isDrawNormal)
        {
            if (!isSmoothNormal)
                DrawNotSmoothNormal();
            else
                DrawSmoothNormal();
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
    private void DrawNotSceleton() {
        int numNor = 0;

        gl3D.Begin(OpenGL.GL_TRIANGLES);
            gl3D.Normal(Normals[numNor][0], Normals[numNor][1], Normals[numNor][2]);
            numNor++;
            gl3D.Color((byte)0, (byte)255, (byte)(255));
            gl3D.Vertex(Figure[0].section1[0], Figure[0].section1[1], Figure[0].section1[2]);
            gl3D.Vertex(Figure[0].section2[0], Figure[0].section2[1], Figure[0].section2[2]);
            gl3D.Vertex(Figure[0].section3[0], Figure[0].section3[1], Figure[0].section3[2]);
        gl3D.End();

        for (int i = 0; i < Figure.Count - 1; i++) {
            gl3D.Begin(BeginMode.Polygon);
                gl3D.Normal(Normals[numNor][0], Normals[numNor][1], Normals[numNor][2]);
                numNor++;
                gl3D.Color((byte)0, (byte)255, (byte)(255));
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
            gl3D.End();

            gl3D.Begin(BeginMode.Polygon);
                gl3D.Normal(Normals[numNor][0], Normals[numNor][1], Normals[numNor][2]);
                numNor++;
                gl3D.Color((byte)0, (byte)255, (byte)(255));
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
            gl3D.End();

            gl3D.Begin(BeginMode.Polygon);
                gl3D.Normal(Normals[numNor][0], Normals[numNor][1], Normals[numNor][2]);
                numNor++;
                gl3D.Color((byte)0, (byte)255, (byte)(255));
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
            gl3D.End();
        }

        gl3D.Begin(OpenGL.GL_TRIANGLES);
            gl3D.Normal(Normals[numNor][0], Normals[numNor][1], Normals[numNor][2]);
            numNor++;
            gl3D.Color((byte)0, (byte)255, (byte)(255));
            gl3D.Vertex(Figure[^1].section1[0], Figure[^1].section1[1], Figure[^1].section1[2]);
            gl3D.Vertex(Figure[^1].section2[0], Figure[^1].section2[1], Figure[^1].section2[2]);
            gl3D.Vertex(Figure[^1].section3[0], Figure[^1].section3[1], Figure[^1].section3[2]);
        gl3D.End();
    }

    //: Рисование фигуры в каркасном виде
    private void DrawSceleton() {

        // Первая секция 
        gl3D.Begin(OpenGL.GL_LINE_STRIP);
        gl3D.Color((byte)255, (byte)255, (byte)0);
        for (int i = 0; i < Figure.Count; i++)
            gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
        gl3D.End();

        // Вторая секция
        gl3D.Begin(OpenGL.GL_LINE_STRIP);
        gl3D.Color((byte)255, (byte)255, (byte)0);
        for (int i = 0; i < Figure.Count; i++)
            gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
        gl3D.End();

        // Третья секция
        gl3D.Begin(OpenGL.GL_LINE_STRIP);
        gl3D.Color((byte)255, (byte)255, (byte)0);
        for (int i = 0; i < Figure.Count; i++)
            gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
        gl3D.End();

        // Треугольники
        for (int i = 0; i < Figure.Count; i++) {
            gl3D.Begin(OpenGL.GL_LINE_LOOP);
                gl3D.Color((byte)255, (byte)255, (byte)0);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
            gl3D.End();
        }

    }

    //: Рисование несглаженныx нормалей
    private void DrawNotSmoothNormal() {
        
        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Color((byte)153, (byte)255, (byte)153);
            gl3D.Vertex(Figure[0].section1[0], Figure[0].section1[1], Figure[0].section1[2]);
            gl3D.Vertex(Figure[0].section1[0] + Normals[0][0], Figure[0].section1[1] + Normals[0][1], Figure[0].section1[2] + Normals[0][2]);
        gl3D.End();

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Color((byte)153, (byte)255, (byte)153);
            gl3D.Vertex(Figure[0].section2[0], Figure[0].section2[1], Figure[0].section2[2]);
            gl3D.Vertex(Figure[0].section2[0] + Normals[0][0], Figure[0].section2[1] + Normals[0][1], Figure[0].section2[2] + Normals[0][2]);
        gl3D.End();

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Color((byte)153, (byte)255, (byte)153);
            gl3D.Vertex(Figure[0].section3[0], Figure[0].section3[1], Figure[0].section3[2]);
            gl3D.Vertex(Figure[0].section3[0] + Normals[0][0], Figure[0].section3[1] + Normals[0][1], Figure[0].section3[2] + Normals[0][2]);
        gl3D.End();

        int numNor = 1;
        for (int i = 0; i < Figure.Count - 1; i++) {
            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.Vertex(Figure[i].section1[0] + Normals[numNor][0], Figure[i].section1[1] + Normals[numNor][1], Figure[i].section1[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.Vertex(Figure[i + 1].section1[0] + Normals[numNor][0], Figure[i + 1].section1[1] + Normals[numNor][1], Figure[i + 1].section1[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.Vertex(Figure[i + 1].section2[0] + Normals[numNor][0], Figure[i + 1].section2[1] + Normals[numNor][1], Figure[i + 1].section2[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.Vertex(Figure[i].section2[0] + Normals[numNor][0], Figure[i].section2[1] + Normals[numNor][1], Figure[i].section2[2] + Normals[numNor][2]);
            gl3D.End();

            numNor++;

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
                gl3D.Vertex(Figure[i].section3[0] + Normals[numNor][0], Figure[i].section3[1] + Normals[numNor][1], Figure[i].section3[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.Vertex(Figure[i + 1].section3[0] + Normals[numNor][0], Figure[i + 1].section3[1] + Normals[numNor][1], Figure[i + 1].section3[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i + 1].section1[0], Figure[i + 1].section1[1], Figure[i + 1].section1[2]);
                gl3D.Vertex(Figure[i + 1].section1[0] + Normals[numNor][0], Figure[i + 1].section1[1] + Normals[numNor][1], Figure[i + 1].section1[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i].section1[0], Figure[i].section1[1], Figure[i].section1[2]);
                gl3D.Vertex(Figure[i].section1[0] + Normals[numNor][0], Figure[i].section1[1] + Normals[numNor][1], Figure[i].section1[2] + Normals[numNor][2]);
            gl3D.End();

            numNor++;

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i].section2[0], Figure[i].section2[1], Figure[i].section2[2]);
                gl3D.Vertex(Figure[i].section2[0] + Normals[numNor][0], Figure[i].section2[1] + Normals[numNor][1], Figure[i].section2[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i + 1].section2[0], Figure[i + 1].section2[1], Figure[i + 1].section2[2]);
                gl3D.Vertex(Figure[i + 1].section2[0] + Normals[numNor][0], Figure[i + 1].section2[1] + Normals[numNor][1], Figure[i + 1].section2[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i + 1].section3[0], Figure[i + 1].section3[1], Figure[i + 1].section3[2]);
                gl3D.Vertex(Figure[i + 1].section3[0] + Normals[numNor][0], Figure[i + 1].section3[1] + Normals[numNor][1], Figure[i + 1].section3[2] + Normals[numNor][2]);
            gl3D.End();

            gl3D.Begin(OpenGL.GL_LINE_STRIP);
                gl3D.Color((byte)153, (byte)255, (byte)153);
                gl3D.Vertex(Figure[i].section3[0], Figure[i].section3[1], Figure[i].section3[2]);
                gl3D.Vertex(Figure[i].section3[0] + Normals[numNor][0], Figure[i].section3[1] + Normals[numNor][1], Figure[i].section3[2] + Normals[numNor][2]);
            gl3D.End();

            numNor++;
        }

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Color((byte)153, (byte)255, (byte)153);
            gl3D.Vertex(Figure[^1].section1[0], Figure[^1].section1[1], Figure[^1].section1[2]);
            gl3D.Vertex(Figure[^1].section1[0] + Normals[^1][0], Figure[^1].section1[1] + Normals[^1][1], Figure[^1].section1[2] + Normals[^1][2]);
        gl3D.End();

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Color((byte)153, (byte)255, (byte)153);
            gl3D.Vertex(Figure[^1].section2[0], Figure[^1].section2[1], Figure[^1].section2[2]);
            gl3D.Vertex(Figure[^1].section2[0] + Normals[^1][0], Figure[^1].section2[1] + Normals[^1][1], Figure[^1].section2[2] + Normals[^1][2]);
        gl3D.End();

        gl3D.Begin(OpenGL.GL_LINE_STRIP);
            gl3D.Color((byte)153, (byte)255, (byte)153);
            gl3D.Vertex(Figure[^1].section3[0], Figure[^1].section3[1], Figure[^1].section3[2]);
            gl3D.Vertex(Figure[^1].section3[0] + Normals[^1][0], Figure[^1].section3[1] + Normals[^1][1], Figure[^1].section3[2] + Normals[^1][2]);
        gl3D.End();
    }

    //: Рисование сглаженных нормалей
    private void DrawSmoothNormal() { 
        
    }
}