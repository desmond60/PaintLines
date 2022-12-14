namespace IntroductionGL.EventOpenGL3D_Rays;

// % ***** Class Stage ***** % //
public class Stage
{
    //: Поля и свойства
    public Square        Square       { get; set; } // Плоскость
    public Sphere[]      Spheres      { get; set; } // Набор сфер
    public Tetrahedron[] Tetrahedrons { get; set; } // Набор тетраэдров
    public Light[]       Lights       { get; set; } // Набор источников света

    //: Конструктор
    public Stage(Square _square, Sphere[] _spheres, Tetrahedron[] _tetrahedrons, Light[] _light) { 
        this.Square = _square;
        Spheres = new Sphere[_spheres.Length];
        Tetrahedrons = new Tetrahedron[_tetrahedrons.Length];
        Lights = new Light[_light.Length];
        Array.Copy(_spheres, Spheres, _spheres.Length);
        Array.Copy(_tetrahedrons, Tetrahedrons, _tetrahedrons.Length);
        Array.Copy(_light, Lights, _light.Length);
    }

    //: Метод обработки тени
    public bool IsShade(Ray ray) {

        // Проходимся по сферам
        for (int i = 0; i < Spheres.Length; i++)
            if (Spheres[i].isShow)
                if (Spheres[i].collision(ray))
                    return true;

        // Проходимся по тетраэдрам
        for (int i = 0; i < Tetrahedrons.Length; i++)
            if (Tetrahedrons[i].isShow)
                if (Tetrahedrons[i].collision(ray))
                    return true;
        
        // Проверим плоскость
        if (Square.collision(ray))
            return true;

        return false;
    }
}

