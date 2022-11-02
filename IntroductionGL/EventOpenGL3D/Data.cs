using System.Diagnostics;

namespace IntroductionGL.EventOpenGL3D;

//: Класс Data под Json
public class Data {
    
    // Входные данные
    public float[][] Section                  { get; set; }   // Координаты 2D сечения
    public float[][] Trajectory               { get; set; }   // Координаты траектории тиражирования
    public float[][] ChangeParam              { get; set; }   // Параметры изменения сечения
    public float[]   Angles                   { get; set; }   // Углы поворота треугольников
    public float[]   PercentForChangeParam    { get; set; }   // Проценты изменения сечения

    // Деконструктор
    public void Deconstruct(out Vector<float>[] section,
                            out Vector<float>[] trajectory,
                            out Vector<float>[] changeparam,
                            out Vector<float>   percent,
                            out Vector<float>   angles) {
        section     = new Vector<float>[Section.Length];
        trajectory  = new Vector<float>[Trajectory.Length];
        changeparam = new Vector<float>[ChangeParam.Length];

        angles      = new Vector<float>(Angles);
        percent     = new Vector<float>(PercentForChangeParam);

        for (int i = 0; i < Section.Length; i++)
            section[i] = new Vector<float>(new[] { Section[i][0], Section[i][1], Section[i][2] });

        for (int i = 0; i < Trajectory.Length; i++)
            trajectory[i] = new Vector<float>(new[] { Trajectory[i][0], Trajectory[i][1], Trajectory[i][2] });

        for (int i = 0; i < ChangeParam.Length; i++)
            changeparam[i] = new Vector<float>(new[] { ChangeParam[i][0], ChangeParam[i][1], ChangeParam[i][2] });
    }
}

