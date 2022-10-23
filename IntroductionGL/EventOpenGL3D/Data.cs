using System.Diagnostics;

namespace IntroductionGL.EventOpenGL3D;

//: Класс Data под Json
public class Data {
    
    // Входные данные
    public float[][] Section     { get; set; }   // Координаты 2D сечения
    public float[][] Trajectory  { get; set; }   // Координаты траектории тиражирования
    public float[][] ChangeParam { get; set; }   // Параметры изменения сечения

    // Деконструктор
    public void Deconstruct(out Vector[] section,
                            out Vector[] trajectory,
                            out Vector[] changeparam) {
        section = new Vector[Section.Length];
        trajectory = new Vector[Trajectory.Length];
        changeparam = new Vector[ChangeParam.Length];

        for (int i = 0; i < Section.Length; i++)
            section[i] = new Vector(Section[i][0], Section[i][1], Section[i][2]);

        for (int i = 0; i < Trajectory.Length; i++)
            trajectory[i] = new Vector(Trajectory[i][0], Trajectory[i][1], Trajectory[i][2]);

        for (int i = 0; i < ChangeParam.Length; i++)
            changeparam[i] = new Vector(ChangeParam[i][0], ChangeParam[i][1], ChangeParam[i][2]);
    }
}

