namespace IntroductionGL.EventOpenGL3D_Rays;

//: Класс Data под Json
public class Data
{
    // Входные данные
    public Sphere[]      Spheres      { get; set; } // Сферы
    public Tetrahedron[] Tetrahedrons { get; set; } // Тетраэдры

    // Деконструктор
    public void Deconstruct(out Sphere[]      spheres,
                            out Tetrahedron[] tetrahedrons)
    {
        spheres      = new Sphere[Spheres.Length];
        tetrahedrons = new Tetrahedron[Tetrahedrons.Length];
        Array.Copy(Spheres, spheres, Spheres.Length);
        Array.Copy(Tetrahedrons, tetrahedrons, Tetrahedrons.Length);
    }
}