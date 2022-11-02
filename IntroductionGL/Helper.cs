namespace IntroductionGL;

// Структура треугольника (сечения)
public struct Triangle {

    //: Первая секция
    public Vector<float> section1 { get; set; }
    //: Вторая секция
    public Vector<float> section2 { get; set; }
    //: Третья секция
    public Vector<float> section3 { get; set; }

    //: Конструктор
    public Triangle(Vector<float> sec1, Vector<float> sec2, Vector<float> sec3) {
        this.section1 = (Vector<float>)sec1.Clone();
        this.section2 = (Vector<float>)sec2.Clone();
        this.section3 = (Vector<float>)sec3.Clone();
    }
}

// Структура точки
public struct Point { 
    
    //: Координата X
    public float X { get; set; }

    //: Координата Y
    public float Y { get; set; }

    //: Размер вершины
    public float Size { get; set; } = 10f;

    //: Цвет вершины
    public Color color { get; set; } = new Color(100, 100, 100, 255);

    //: Конструктор
    public Point(float x, float y) { 
        this.X = x;
        this.Y = y;
    }

    //: Строковое представление точки
    public override string ToString() => $"({X}, {Y})";

    //: Деконструктор
    public void Deconstruct(out float x,
                            out float y)
    {
        x = X;
        y = Y;
    }
}

// Структура цвета
public struct Color {

    //: Компонента R
    public byte R { get; set; }

    //: Компонента G
    public byte G { get; set; }

    //: Компонента B
    public byte B { get; set; }

    //: Компонента A
    public byte A { get; set; }

    //: Конструктор
    public Color(byte R, byte G, byte B, byte A) {
        this.R = R;
        this.G = G;
        this.B = B;
        this.A = A;
    }

    //: Деконструктор
    public void Deconstruct(out byte r,
                            out byte g,
                            out byte b,
                            out byte a) {
        r = R;
        g = G;
        b = B;
        a = A;
    }

    //: Строковое представление цвета
    public override string ToString() => $"({R}, {G}, {B})";
}

// Структура примитива правильный пятиугольник
public struct PrimitiveFiveRect { 
    
    //: Вершины примитива
    public Point[] points { get; set; }

    //: Название примитива
    public string Name { get; set; }

    //: Конструктор
    public PrimitiveFiveRect(Point[] points, string name) {
        this.points = points;
        this.Name   = name;
    }

    //: Строковое представление примитива
    public override string ToString() {
        StringBuilder str = new StringBuilder($"{Name}: [");
        foreach (var item in points)
            str.Append($"{item.ToString}\n");
        str.Append($"]");
        return str.ToString();
    }
}

// Структура примитива Линии
public struct Primitive {

    //: Вершины примитива
    public Point fPoint { get; set; }
    public Point sPoint { get; set; }

    //: Название примитива
    public string Name { get; set; }

    //: Ширина линии
    public float LineWidth { get; set; } = 1.0f;

    //: Тип линии
    public TypeLine type { get; set; } = TypeLine.ORDINARY;

    //: Конструктор 
    public Primitive(Point fpoint, Point spoint, string name) {
        this.fPoint = fpoint;
        this.sPoint = spoint;
        this.Name = name;
    }

    //: Строковое представление примитива
    public override string ToString() => $"({fPoint.X},{fPoint.Y}) -> ({sPoint.X},{sPoint.Y})";

    //: Деконструктор
    public void Deconstruct(out Point Fpoint,
                            out Point Spoint)
    {
        Fpoint = fPoint;
        Spoint = sPoint;
    }
}

// Структура набора примитивов
public struct CollectionPrimitives : IEnumerable {

    //: Лист с примитивами
    public List<Primitive> Primitives;
    
    //: Метод перечисления коллекций
    public IEnumerator GetEnumerator() => Primitives.GetEnumerator();

    //: Имя набора примитивов
    public string Name { get; set; }

    //: Конструктор
    public CollectionPrimitives(List<Primitive> primitives) {
        Primitives = new List<Primitive>(primitives);
        Name       = "Default";
    }

    //: Строковое представление набора примитивов
    public override string ToString() => $"CollPrimitives_{Primitives.Count}";
}

// Статический класс "Помощник"
public static class Helper
{
    //: Словарь с названиями наборов примитивов
    public static Dictionary<string, int> dictCollPrim_index = new Dictionary<string, int>();

    //: Перечисление "Тип линии"
    public enum TypeLine { 
        ORDINARY,
        POINT,
        DASHED,
        DASHEDPOINT
    }

    //: Getter for enum TypeLine
    public static string GetTypeLine(TypeLine type) {
        return type switch
        {
            TypeLine.ORDINARY => "Обычный",
            TypeLine.POINT => "Точечный",
            TypeLine.DASHED => "Штриховой",
            TypeLine.DASHEDPOINT => "Штрихпунктирный",
            _ => "Обычный"
        };
    }

    //: Создание точки примитиви в зависимости от размеров экрана
    public static Point Primitive_Coordinate(Point p, double width, double height) {
        float mid_x = (float)width  / 2.0f;
        float mid_y = (float)height / 2.0f;
        return new Point(p.X < mid_x
                             ? -(mid_x - p.X) / mid_x
                             :  (p.X - mid_x) / mid_x,
                         p.Y < mid_y
                             ?  (mid_y - p.Y) / mid_y
                             : -(p.Y - mid_y) / mid_y);
    }

    //: Дать название набору примитивов
    public static string NameCollPrim(List<CollectionPrimitives> lColPrim, ref CollectionPrimitives add_item) {

        if (dictCollPrim_index.TryGetValue(add_item.ToString(), out int index)) {
            add_item.Name = add_item.ToString() + $"_{++dictCollPrim_index[add_item.ToString()]}";
            return add_item.Name;
        }
               
        dictCollPrim_index.Add(add_item.ToString(), 0);
        add_item.Name = add_item.ToString();
        return add_item.Name;
    }
}

