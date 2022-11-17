namespace IntroductionGL.EventOpenGL3D_Rays;

// % ***** Class Data for Json ***** % //
public class Data {

    //: Входные данные
    public Sphere[]      Spheres      { get; set; } // Сферы
    public Tetrahedron[] Tetrahedrons { get; set; } // Тетраэдры
    public Square        Square       { get; set; } // Плоскость

    //: Деконструктор
    public void Deconstruct(out Sphere[]      spheres,
                            out Tetrahedron[] tetrahedrons,
                            out Square        square)
    {
        spheres      = new Sphere[Spheres.Length];
        tetrahedrons = new Tetrahedron[Tetrahedrons.Length];
        square       = Square with { };
        Array.Copy(Spheres, spheres, Spheres.Length);
        Array.Copy(Tetrahedrons, tetrahedrons, Tetrahedrons.Length);
    }
}