namespace IntroductionGL.EventOpenGL3D;

//: % ***** Light class ***** % ://
public class Light {

    //: Поля и свойства
    public TypeLight Type { get; set; } // Тип источника света
    public string Name { get; set; }    // Название ИС
    
    public Vector<float> Position  { get; set; } // Позиция источника света
    public Vector<float> Direction { get; set; } // Направление
    public Vector<float> Ambient   { get; set; } // Фоновая составляющая 
    public Vector<float> Diffuse   { get; set; } // Диффузная составляющая
    public Vector<float> Specular  { get; set; } // Зеркальная составляющая

    //: Поля для прожектора
    public float Exponent { get; set; } // Функция распределения
    public float Cutoff   { get; set; } // Угол распространения

    //: Поля для затухания
    public bool IsAttenuation { get; set; } = false; // Есть ли затухание?
    public float Constant     { get; set; } // Константное затухание
    public float Linear       { get; set; } // Линейное затухание
    public float Quadratic    { get; set; } // Квадратичное затухание

    //: Конструктор
    public Light() {
        Position  = new Vector<float>(new[] { 0f, 0f, 0f, 1f });
        Direction = new Vector<float>(new[] { 0f, 0f, 0f, 1f });
        Ambient   = new Vector<float>(new[] { 0f, 0f, 0f, 1f });
        Diffuse   = new Vector<float>(new[] { 0f, 0f, 0f, 1f });
        Specular  = new Vector<float>(new[] { 0f, 0f, 0f, 1f });
    }
}

