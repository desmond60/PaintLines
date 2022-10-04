using System.Drawing;

namespace IntroductionGL;

// Структура точки
public struct Point { 
    
    //: Координата X
    public float X { get; set; }

    //: Координата Y
    public float Y { get; set; }

    //: Размер вершины
    public float Size { get; set; } = 10f;

    //: Цвет вершины
    public Color color { get; set; } = new Color(100, 100, 100);

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

    //: Конструктор
    public Color(byte R, byte G, byte B) {
        this.R = R;
        this.G = G;
        this.B = B;
    }

    //: Деконструктор
    public void Deconstruct(out float r,
                            out float g,
                            out float b) {
        r = R;
        g = G;
        b = B;
    }

    //: Строковое представление цвета
    public override string ToString() => $"({R}, {G}, {B})";
}

// Структура примитива
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
    public static Dictionary<string, uint> dictCollPrim_index = new Dictionary<string, uint>();

    //: Перечисление "Тип линии"
    public enum TypeLine { 
        ORDINARY,
        POINT,
        DASHED,
        DASHEDPOINT
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

        if (dictCollPrim_index.TryGetValue(add_item.ToString(), out uint index)) {
            add_item.Name = add_item.ToString() + $"_{++dictCollPrim_index[add_item.ToString()]}";
            return add_item.Name;
        }
               
        dictCollPrim_index.Add(add_item.ToString(), 0);
        add_item.Name = add_item.ToString();
        return add_item.Name;
    }
}

