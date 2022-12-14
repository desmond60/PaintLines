namespace IntroductionGL;

//: Структура Луча
public struct Ray
{
    //: Точка луча
    public Vector<float> Position { get; set; }

    //: Направление луча
    public Vector<float> Orientation { get; set; }

    //: Уровень
    public int level { get; set; }

    //: Конструктор
    public Ray(Vector<float> _pos)
    {
        Position = (Vector<float>)_pos.Clone();
        Orientation = new Vector<float>(3);
        level = 0;
    }
}

//: Информация о соударении
public struct CollisionInfo
{
    //: Точка соударения
    public Vector<float> CollisionPoint { get; set; }

    //: Нормаль в точке соударения
    public Vector<float> NormalPoint { get; set; }

    //: Тип объекта соударения
    public int objectType { get; set; } = -1;

    //: Номер объекта соударения
    public int objectNum { get; set; } = -1;

    //: Луч входит или выходит из объекта?
    public bool IsIncluded = false;

    //: Поверхность соударения
    public int surface { get; set; } = -1;

    //: Время соударения
    public double CollisionTime { get; set; } = -1000;

    //: Конструктор
    public CollisionInfo(Vector<float> _colP, Vector<float> _nor)
    {
        CollisionPoint = (Vector<float>)_colP.Clone();
        NormalPoint = (Vector<float>)_nor.Clone();
    }
}

//: Список соударений
public struct CollisionList
{
    //: Число содуарений для положительных значений времени
    public int numCollision = 0;

    //: Максимальное число соударений
    public static int MaxNumCollision = 8;

    //: Список соударений
    public CollisionInfo[] collision;

    //: Конструктор (с параметром)
    public CollisionList(CollisionInfo[] _lCol)
    {
        collision = new CollisionInfo[_lCol.Length];
        Array.Copy(_lCol, collision, _lCol.Length);
    }

    //: Конструктор (без параметров)
    public CollisionList()
    {
        collision = new CollisionInfo[MaxNumCollision];
    }
}

// Структура треугольник (сечения)
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

// Структура точка
public struct Point { 
    
    //: Координата X
    public float X { get; set; }

    //: Координата Y
    public float Y { get; set; }

    //: Размер вершины
    public float Size { get; set; } = 10f;

    //: Цвет вершины
    public Color color { get; set; } = new Color(100, 100, 100, 255);

    //: Вес точки (для сплайна используется)
    public float w { get; set; } = 1.0f;

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

// Структура источника света
public struct Light { 
    
    //: Позиция источника света
    public Vector<float> Position { get; set; }

    //: Цвет источника света
    public ColorF Color { get; set; }

    //: Конструктор
    public Light(Vector<float> _pos) { 
        this.Position = (Vector<float>)_pos.Clone();
    }

    //: Конструктор с цветом
    public Light(Vector<float> _pos, ColorF _color) {
        this.Position = (Vector<float>)_pos.Clone();
        this.Color = _color;
    }
}

// Структура цвет для трассировки
public struct ColorF {
    
    //: Компонента R
    public float R { get; set; }

    //: Компонента G
    public float G { get; set; }

    //: Компонента B
    public float B { get; set; }

    //: Конструктор
    public ColorF(float _R, float _G, float _B) {
        this.R = _R;
        this.G = _G;
        this.B = _B;
    }

    //: Добавление цвета значениями
    public void Add(float _R, float _G, float _B) {
        this.R += _R;
        this.G += _G;
        this.B += _B;
    }

    //: Добавление цвета другим цветом
    public void Add(ColorF _color) {
        this.R += _color.R;
        this.G += _color.G;
        this.B += _color.B;
    }

    //: Добавление цвета (отраженный оттенокы)
    public void Add(ColorF _color, ColorF _colorRef) {
        this.R += _color.R * _colorRef.R;
        this.G += _color.G * _colorRef.G;
        this.B += _color.B * _colorRef.B;
    }
}

// Структура цвет
public struct Color {

    //: Компонента R
    public byte R { get; set; }

    //: Компонента G
    public byte G { get; set; }

    //: Компонента B
    public byte B { get; set; }

    //: Компонента A
    public byte A { get; set; }

    //: Конструктор (с прозрачностью)
    public Color(byte _R, byte _G, byte _B, byte _A) {
        this.R = _R;
        this.G = _G;
        this.B = _B;
        this.A = _A;
    }

    //: Конструктор (без прозрачности)
    public Color(byte _R, byte _G, byte _B) {
        this.R = _R;
        this.G = _G;
        this.B = _B;
        this.A = 255;
    }

    //: Деконструкторы
    public void Deconstruct(out byte r,
                            out byte g,
                            out byte b,
                            out byte a) {
        r = R;
        g = G;
        b = B;
        a = A;
    }

    public void Deconstruct(out byte r,
                            out byte g,
                            out byte b)
    {
        r = R;
        g = G;
        b = B;
    }

    //: Строковое представление цвета
    public override string ToString() => $"({R}, {G}, {B})";
}

// Структура примитив "Правильный пятиугольник"
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

// Структура примитив "Линия"
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

// Структура набор примитивов
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

// Структура квадрат
public struct Square
{

    //: Координата центра квадрата
    public Vector<float> Center { get; set; }

    //: Цвет квадрата
    public Color Color { get; set; } = new Color(255, 255, 255, 255);

    //: Коэффициент растяжения по оси Z
    public float ScaleZ { get; set; }

    //: Коэффициент растяжения по оси X
    public float ScaleX { get; set; }

    //: Конструктор
    public Square(Vector<float> _center, float _scaleZ, float _scaleX)
    {
        this.Center = (Vector<float>)_center.Clone();
        this.ScaleZ = _scaleZ;
        this.ScaleX = _scaleX;
    }

    //: Материалы
    public Vector<float> Ambient { get; set; } // Фоновое отражение
    public Vector<float> Diffuse { get; set; } // Рассеянное отражение
    public Vector<float> Specular { get; set; } // Зеркальное отражение
    public Vector<float> Emission { get; set; } // Собственное излучение
    public float Shine { get; set; } // Коэффициент блеска

    //: Проверка на соударение
    public bool collision(Ray ray, ref CollisionList curCol)
    {
        // Определяем матрицу трансформации
        Matrix<float> matrix = new Matrix<float>(new float[4, 4]{
            {ScaleX, 0f, 0f,     Center[0]},
            {0f,     1f, 0f,     Center[1]},
            {0f,     0f, ScaleZ, Center[2]},
            {0f,     0f, 0f,            1f}
        });

        // Приводим луч к базовому
        Ray mainRay = Helper.EditRay(ray, matrix);

        // Луч параллелен плоскости
        if (Abs(mainRay.Orientation[1]) < 0.0001) return false;

        // Время соударения
        float time = -mainRay.Position[1] / mainRay.Orientation[1];

        // Квадрат лежит позади взгляда
        if (time <= 0.0f) return false;

        float width = mainRay.Position[0] + mainRay.Orientation[0] * time;
        float height = mainRay.Position[2] + mainRay.Orientation[2] * time;
        
        if (width > 2.0f || width < -2.0f) return false;
        if (height > 2.0f || height < -2.0f) return false;

        // Формируем информацию о раннем соударении
        curCol.numCollision = 1;
        curCol.collision[0].surface = 0;
        curCol.collision[0].CollisionTime = time;
        curCol.collision[0].objectType = 2;
        curCol.collision[0].IsIncluded = true;

        // Координаты точки соударения
        Vector<float> colPoint = Vector<float>.GetRayPosition(ray, time);
        curCol.collision[0].CollisionPoint = (Vector<float>)colPoint.Clone();

        // Координаты нормали в точке соударения
        curCol.collision[0].NormalPoint = new Vector<float>(new float[] { 0f, 1f, 0f });

        return true;
    }

    //: Проверка на соударение (для тени)
    public bool collision(Ray ray)
    {
        // Определяем матрицу трансформации
        Matrix<float> matrix = new Matrix<float>(new float[4, 4]{
            {ScaleX, 0f, 0f,     Center[0]},
            {0f,     1f, 0f,     Center[1]},
            {0f,     0f, ScaleZ, Center[2]},
            {0f,     0f, 0f,            1f}
        });

        // Приводим луч к базовому
        Ray mainRay = Helper.EditRay(ray, matrix);

        // Луч параллелен плоскости
        if (Abs(mainRay.Orientation[1]) < 0.0001) return false;

        // Время соударения
        float time = -mainRay.Position[1] / mainRay.Orientation[1];

        // Квадрат лежит позади взгляда
        if (time <= 0.0f) return false;

        float width = mainRay.Position[0] + mainRay.Orientation[0] * time;
        float height = mainRay.Position[2] + mainRay.Orientation[2] * time;

        if (width > 2.0f || width < -2.0f) return false;
        if (height > 2.0f || height < -2.0f) return false;

        if (time >= 0.0f && time <= 1.0f)
            return true;

        return false;
    }
}

// Структура сфера
public struct Sphere {

    //: Координаты центра сферы
    public Vector<float> Center { get; set; }

    //: Радиус сферы
    public float R { get; set; }

    //: Цвет сферы
    public Color Color { get; set; } = new Color(0, 0, 255, 255);

    //: Отрисовать сферу на сцене?
    public bool isShow = true;

    //: Материалы
    public Vector<float> Ambient  { get; set; } // Фоновое отражение
    public Vector<float> Diffuse  { get; set; } // Рассеянное отражение
    public Vector<float> Specular { get; set; } // Зеркальное отражение
    public Vector<float> Emission { get; set; } // Собственное излучение
    public        float  Shine    { get; set; } // Коэффициент блеска

    //: Конструктор
    public Sphere(Vector<float> _center, float _R) {
        this.Center = (Vector<float>)_center.Clone();
        this.R = _R;

        Ambient = new Vector<float>(4);
        Diffuse = new Vector<float>(4);
        Specular = new Vector<float>(4);
        Emission = new Vector<float>(4);
        Shine = 0;
    }

    //: Проверка на соударение
    public bool collision(Ray ray, ref CollisionList curCol) {

        // Определяем матрицу трансформации
        Matrix<float> matrix = new Matrix<float>(new float[4, 4]{
            {R, 0f, 0f, Center[0]},
            {0f, R, 0f, Center[1]},
            {0f, 0f, R, Center[2]},
            {0f, 0f, 0f, 1f}
        });

        // Приводим луч к базовому
        Ray mainRay = Helper.EditRay(ray, matrix);

        // Формируем коэффициенты
        float a = Vector<float>.Scalar(mainRay.Orientation, mainRay.Orientation);
        float b = Vector<float>.Scalar(mainRay.Position, mainRay.Orientation);
        float c = Vector<float>.Scalar(mainRay.Position, mainRay.Position) - 1.0f;

        // Дискриминант
        float discriminant = b * b - a * c;

        if (discriminant < 0.0f) return false;

        // Число соударений
        int num_col = 0;

        // Первый корень (Если находится спереди)
        float time = (float)((-b - Sqrt(discriminant)) / a);
        if (time > 0.00001)
        {
            // Формируем информацию о раннем соударении
            curCol.collision[0].surface = 0;
            curCol.collision[0].CollisionTime = time;
            curCol.collision[0].objectType = 0;
            curCol.collision[0].IsIncluded = true;

            // Координаты точки соударения
            Vector<float> colPoint = Vector<float>.GetRayPosition(ray, time);
            curCol.collision[0].CollisionPoint = (Vector<float>)colPoint.Clone();

            // Координаты нормали в точке соударения
            Vector<float> normPoint = Vector<float>.GetRayPosition(mainRay, time);
            curCol.collision[0].NormalPoint = (Vector<float>)normPoint.Clone();

            num_col++;
        }

        // Второй корень (Если находится спереди)
        time = (float)((-b + Sqrt(discriminant)) / a);
        if (time > 0.00001)
        {
            // Формируем информацию о позднем соударении
            curCol.collision[0].surface = 0;
            curCol.collision[0].CollisionTime = time;
            curCol.collision[0].objectType = 0;
            curCol.collision[0].IsIncluded = false; 

            // Координаты точки соударения
            Vector<float> colPoint = Vector<float>.GetRayPosition(ray, time);
            curCol.collision[0].CollisionPoint = (Vector<float>)colPoint.Clone();

            // Координаты нормали в точке соударения
            Vector<float> normPoint = Vector<float>.GetRayPosition(mainRay, time);
            curCol.collision[0].NormalPoint = (Vector<float>)normPoint.Clone();

            num_col++;
        }

        if (num_col == 0) return false;

        curCol.numCollision = num_col;
        return true;
    }

    //: Проверка на соударение (для тени)
    public bool collision(Ray ray)
    {
        // Определяем матрицу трансформации
        Matrix<float> matrix = new Matrix<float>(new float[4, 4]{
            {R, 0f, 0f, Center[0]},
            {0f, R, 0f, Center[1]},
            {0f, 0f, R, Center[2]},
            {0f, 0f, 0f, 1f}
        });

        // Приводим луч к базовому
        Ray mainRay = Helper.EditRay(ray, matrix);

        // Формируем коэффициенты
        float a = Vector<float>.Scalar(mainRay.Orientation, mainRay.Orientation);
        float b = Vector<float>.Scalar(mainRay.Position, mainRay.Orientation);
        float c = Vector<float>.Scalar(mainRay.Position, mainRay.Position) - 1.0f;

        // Дискриминант
        float discriminant = b * b - a * c;

        if (discriminant < 0.0f) return false;

        // Первый корень (Если находится спереди)
        float time = (float)((-b - Sqrt(discriminant)) / a);
        if (time >= 0.00001f && time <= 1.0f)
            return true;

        // Второй корень (Если находится спереди)
        time = (float)((-b + Sqrt(discriminant)) / a);
        if (time >= 0.00001f && time <= 1.0f)
            return true;

        return false;
    }
}

// Структура тетраэдр
public struct Tetrahedron { 
    
    //: Координаты центра тетраэдра
    public Vector<float> Center { get; set; }

    //: Координаты вершин тетраэдра
    public Vector<float>[] Node { get; set; }

    //: Цвет тетраэдра
    public Color Color { get; set; } = new Color(0, 0, 255, 255);

    //: Отрисовать тетраэдр на сцене?
    public bool isShow = true;

    //: Материалы
    public Vector<float> Ambient  { get; set; } // Фоновое отражение
    public Vector<float> Diffuse  { get; set; } // Рассеянное отражение
    public Vector<float> Specular { get; set; } // Зеркальное отражение
    public Vector<float> Emission { get; set; } // Собственное излучение
    public        float  Shine    { get; set; } // Коэффициент блеска

    //: Конструктор
    public Tetrahedron(Vector<float>[] _node, Vector<float> _center)
    {
        this.Center = (Vector<float>)_center.Clone();
        this.Node = (Vector<float>[])_node.Clone();

        Ambient = new Vector<float>(4);
        Diffuse = new Vector<float>(4);
        Specular = new Vector<float>(4);
        Emission = new Vector<float>(4);
        Shine = 0;
    }

    //: Проверка на соударение
    public bool collision(Ray ray, ref CollisionList curCol)
    {
        // Определяем матрицу трансформации
        Matrix<float> matrix = new Matrix<float>(new float[4, 4]{
            {1f, 0f, 0f, Center[0]},
            {0f, 1f, 0f, Center[1]},
            {0f, 0f, 1f, Center[2]},
            {0f, 0f, 0f, 1f}
        });

        // Приводим луч к базовому
        Ray mainRay = Helper.EditRay(ray, matrix);

        // Время соударения с каждой плоскостью
        Vector<float>[] Normals = new Vector<float>[4];
        float[] time = new float[4];

        // Смотрим пересечение с плоскостью (0 1 2)
        (Normals[0], time[0]) = colSurface(mainRay, 0, 1, 2);
        if (time[0] > 0) curCol.numCollision++;

        // Смотрим пересечение с плоскостью (1 3 2)
        (Normals[1], time[1]) = colSurface(mainRay, 1, 3, 2);
        if (time[1] > 0) curCol.numCollision++;

        // Смотрим пересечение с плоскостью (3 0 2)
        (Normals[2], time[2]) = colSurface(mainRay, 3, 0, 2);
        if (time[2] > 0) curCol.numCollision++;

        // Смотрим пересечение с плоскостью (3 0 1)
        (Normals[3], time[3]) = colSurface(mainRay, 3, 0, 1);
        if (time[3] > 0) curCol.numCollision++;

        if (curCol.numCollision < 2) return false;

        // Определяем точку входа и выхода
        int entry = 0;
        for (int i = 1; i < 4; i++)
            if (time[i] > time[entry])
                entry = i;
        int exit = entry;
        for (int i = 0; i < 4; i++)
            if (time[i] < time[exit] && time[i] > 0.0f)
                exit = i;

        if (time[entry] < 0.0f) return false;

        // Формируем информацию о первом соударении
        curCol.collision[0].surface = exit;
        curCol.collision[0].CollisionTime = time[exit];
        curCol.collision[0].objectType = 1;
        curCol.collision[0].IsIncluded = true;

        // Координаты точки соударения
        Vector<float> colPoint = Vector<float>.GetRayPosition(ray, time[exit]);
        curCol.collision[0].CollisionPoint = (Vector<float>)colPoint.Clone();

        // Координаты нормали в точке соударения
        curCol.collision[0].NormalPoint = (Vector<float>)Normals[exit].Clone();

        // Формируем информацию о втором соударении
        curCol.collision[1].surface = entry;
        curCol.collision[1].CollisionTime = time[entry];
        curCol.collision[1].objectType = 1;
        curCol.collision[1].IsIncluded = false;

        // Координаты точки соударения
        colPoint = Vector<float>.GetRayPosition(ray, time[entry]);
        curCol.collision[1].CollisionPoint = (Vector<float>)colPoint.Clone();

        // Координаты нормали в точке соударения
        curCol.collision[1].NormalPoint = (Vector<float>)Normals[entry].Clone();
        
        // Возможно если это убрать будет ошибка? А так можно оставить
        // curCol.numCollision = 2;

        return true;
    }

    //: Проверка на соударение (для тени)
    public bool collision(Ray ray)
    {
        // Определяем матрицу трансформации
        Matrix<float> matrix = new Matrix<float>(new float[4, 4]{
            {1f, 0f, 0f, Center[0]},
            {0f, 1f, 0f, Center[1]},
            {0f, 0f, 1f, Center[2]},
            {0f, 0f, 0f, 1f}
        });

        // Приводим луч к базовому
        Ray mainRay = Helper.EditRay(ray, matrix);

        // Время соударения с каждой плоскостью
        Vector<float>[] Normals = new Vector<float>[4];
        float[] time = new float[4];

        // Смотрим пересечение с плоскостью (0 1 2)
        (Normals[0], time[0]) = colSurface(mainRay, 0, 1, 2);
        if (time[0] >= 0.0f && time[0] <= 1.0f) return true;

        // Смотрим пересечение с плоскостью (1 3 2)
        (Normals[1], time[1]) = colSurface(mainRay, 1, 3, 2);
        if (time[1] >= 0.0f && time[1] <= 1.0f) return true;

        // Смотрим пересечение с плоскостью (3 0 2)
        (Normals[2], time[2]) = colSurface(mainRay, 3, 0, 2);
        if (time[2] >= 0.0f && time[2] <= 1.0f) return true;

        // Смотрим пересечение с плоскостью (3 0 1)
        (Normals[3], time[3]) = colSurface(mainRay, 3, 0, 1);
        if (time[3] >= 0.0f && time[3] <= 1.0f) return true;

        return false;
    }

    //: Проверить соударение на треуглльник тетраэдра
    private (Vector<float>, float) colSurface(Ray ray, int f, int s, int t) {

        // Переменные для облегчения
        Vector<float> Pos = (Vector<float>)ray.Position.Clone();
        Vector<float> Ori = (Vector<float>)ray.Orientation.Clone();
        Vector<float> F = (Vector<float>)Node[f].Clone();
        Vector<float> S = (Vector<float>)Node[s].Clone();
        Vector<float> T = (Vector<float>)Node[t].Clone();
        Vector<float> N = Vector<float>.GetVectorPolygon(F, S, T);

        // Время и точка соударения
        float time = -Vector<float>.Scalar(Pos - F, N) / Vector<float>.Scalar(Ori, N);
        Vector<float> P = Pos + Ori * time;

        // Проекция YZ
        if (Abs(N[0]) >= Abs(N[1]) && Abs(N[0]) >= Abs(N[2]))
        {
            if ((P[2] - F[2]) * (S[1] - F[1]) - (P[1] - F[1]) * (S[2] - F[2]) < 0) return (new Vector<float>(3), -1);
            if ((P[2] - S[2]) * (T[1] - S[1]) - (P[1] - S[1]) * (T[2] - S[2]) < 0) return (new Vector<float>(3), -1);
            if ((P[2] - T[2]) * (F[1] - T[1]) - (P[1] - T[1]) * (F[2] - T[2]) < 0) return (new Vector<float>(3), -1);
        }
        // Проекция XY
        else if (Abs(N[2]) >= Abs(N[1]))
        {
            if ((P[1] - F[1]) * (S[0] - F[0]) - (P[0] - F[0]) * (S[1] - F[1]) < 0) return (new Vector<float>(3), -1);
            if ((P[1] - S[1]) * (T[0] - S[0]) - (P[0] - S[0]) * (T[1] - S[1]) < 0) return (new Vector<float>(3), -1);
            if ((P[1] - T[1]) * (F[0] - T[0]) - (P[0] - T[0]) * (F[1] - T[1]) < 0) return (new Vector<float>(3), -1);
        }
        // Проекция XZ
        else
        {
            if ((P[2] - F[2]) * (S[0] - F[0]) - (P[0] - F[0]) * (S[2] - F[2]) < 0) return (new Vector<float>(3), -1);
            if ((P[2] - S[2]) * (T[0] - S[0]) - (P[0] - S[0]) * (T[2] - S[2]) < 0) return (new Vector<float>(3), -1);
            if ((P[2] - T[2]) * (F[0] - T[0]) - (P[0] - T[0]) * (F[2] - T[2]) < 0) return (new Vector<float>(3), -1);
        }

        return ((Vector<float>)N.Clone(), time);
    }
}

// Статический класс "Помощник"
public static class Helper
{
    //: Словарь с названиями наборов примитивов
    public static Dictionary<string, int> dictCollPrim_index = new Dictionary<string, int>();

    //: Словарь с текстурами и их путями
    public static Dictionary<int, string> dictTexture = new Dictionary<int, string>() {
        { 1, "EventOpenGL3D/texture1.bmp"},
        { 2, "EventOpenGL3D/texture2.bmp"}
    };

    //: Перечисление "Тип линии"
    public enum TypeLine {
        ORDINARY,
        POINT,
        DASHED,
        DASHEDPOINT
    }

    //: Перечисление "Тип источника света"
    public enum TypeLight {
        DIRECTED,          // Направленный
        POINT,             // Точечный
        POINT_ATTENUATION, // Точечный с затуханием
        SPOT,              // Прожеткор
        SPOT_ATTENUATION   // Прожеткор с затуханием
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

    //: Метод определяет имя источника света
    public static string GetNameLight(TypeLight type, List<EventOpenGL3D.Light> list) {
        
        // Ищу все ИС с хожим типом
        List<EventOpenGL3D.Light> list_name = list.Where(n => n.Type.Equals(type)).ToList();

        if (list_name.Count == 0)
            return type switch
            {
                TypeLight.DIRECTED => "Directed_1",
                TypeLight.POINT => "Point_1",
                TypeLight.POINT_ATTENUATION => "PointAtt_1",
                TypeLight.SPOT => "Spot_1",
                TypeLight.SPOT_ATTENUATION => "SpotAtt_1"
            };
        else {
            return type switch
            {
                TypeLight.DIRECTED => $"Directed_{list_name.Count + 1}",
                TypeLight.POINT => $"Point_{list_name.Count + 1}",
                TypeLight.POINT_ATTENUATION => $"PointAtt_{list_name.Count + 1}",
                TypeLight.SPOT => $"Spot_{list_name.Count + 1}",
                TypeLight.SPOT_ATTENUATION => $"SpotAtt_{list_name.Count + 1}"
            };
        }
    }

    //: Создание точки примитиви в зависимости от размеров экрана
    public static Point Primitive_Coordinate(Point p, double width, double height) {
        float mid_x = (float)width / 2.0f;
        float mid_y = (float)height / 2.0f;
        return new Point(p.X < mid_x
                             ? -(mid_x - p.X) / mid_x
                             : (p.X - mid_x) / mid_x,
                         p.Y < mid_y
                             ? (mid_y - p.Y) / mid_y
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

    //: Приведение луча к базовому
    public static Ray EditRay(Ray ray, Matrix<float> mat) {
        
        // Определяем начало луча и направление в однородных координатах
        Vector<float> pos_f = new Vector<float>(new float[] { ray.Position[0], ray.Position[1], ray.Position[2], 1.0f });
        Vector<float> ori_f = new Vector<float>(new float[] { ray.Orientation[0], ray.Orientation[1], ray.Orientation[2], 0.0f });

        Vector<float> pos_sol = (new EventOpenGL3D_Rays.Gauss(4, 1e-15)).solve(mat, pos_f);
        Vector<float> ori_sol = (new EventOpenGL3D_Rays.Gauss(4, 1e-15)).solve(mat, ori_f);

        // Переводим вектора в обычные координаты и создаем новый луч
        Vector<float> new_pos = new Vector<float>(new float[] { pos_sol[0], pos_sol[1], pos_sol[2] });
        Vector<float> new_ori = new Vector<float>(new float[] { ori_sol[0], ori_sol[1], ori_sol[2] });

        Ray new_ray = new Ray(new_pos);
        new_ray = new_ray with { Orientation = (Vector<float>)new_ori.Clone() };
        
        return new_ray;
    }
}