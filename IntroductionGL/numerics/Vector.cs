namespace IntroductionGL.numerics;

// % ****** Class Vector ***** % //
public class Vector<T> : ICloneable
                where T : System.Numerics.INumber<T>
{
    public T[] vector;                    // Вектор
    public int Length => vector.Length;   // Размерность вектора

    //: Перегрузка неявного преобразования
    public static explicit operator T[](Vector<T> vec) {
        return vec.vector;
    }

    //: Деконструктор
    public void Deconstruct(out T[] vec)
    {
        vec = this.vector;
    }

    [JsonConstructor]
    public Vector() { }

    //: Конструктор (с размерностью)
    public Vector(int lenght)
    {
        vector = new T[lenght];
    }

    //: Конструктор (с массивом)
    public Vector(T[] array)
    {
        vector = new T[array.Length];
        Array.Copy(array, vector, array.Length);
    }

    //: Индексатор
    public T this[int index]
    {
        get => vector[index];
        set => vector[index] = value;
    }

    //: Перегрузка умножения двух векторов
    public static T operator *(Vector<T> vec1, Vector<T> vec2)
    {
        T result = T.Zero;
        for (int i = 0; i < vec1.Length; i++)
            result += vec1[i] * vec2[i];
        return result;
    }

    //: Перегрузка умножения на констунту (double)
    public static Vector<T> operator *(T Const, Vector<T> vector)
    {
        var result = new Vector<T>(vector.Length);
        for (int i = 0; i < vector.Length; i++)
            result[i] = Const * vector[i];
        return result;
    }
    public static Vector<T> operator *(Vector<T> vector, T Const) => Const * vector;

    //: Перегрузка умножения (на числовой вектор)
    public static Vector<T> operator *(Matrix<T> mat, Vector<T> vec)
    {
        var result = new Vector<T>(vec.Length);
        for (int i = 0; i < vec.Length; i++)
            for (int j = 0; j < vec.Length; j++)
                result[i] += T.CreateTruncating(mat[i, j]) * vec[j];
        return result;
    }

    //: Перегрузка деления на константу (double)
    public static Vector<T> operator /(Vector<T> vector, T Const)
    {
        var result = new Vector<T>(vector.Length);
        for (int i = 0; i < vector.Length; i++)
            result[i] = vector[i] / Const;
        return result;
    }

    //: Перегрузка сложения двух векторов
    public static Vector<T> operator +(Vector<T> vec1, Vector<T> vec2)
    {
        var result = new Vector<T>(vec1.Length);
        for (int i = 0; i < vec1.Length; i++)
            result[i] = vec1[i] + vec2[i];
        return result;
    }

    //: Перегрузка вычитания двух векторов
    public static Vector<T> operator -(Vector<T> vec1, Vector<T> vec2)
    {
        var result = new Vector<T>(vec1.Length);
        for (int i = 0; i < vec1.Length; i++)
            result[i] = vec1[i] - vec2[i];
        return result;
    }

    //: Перегрузка тернарного минуса
    public static Vector<T> operator -(Vector<T> vector)
    {
        var result = new Vector<T>(vector.Length);
        for (int i = 0; i < vector.Length; i++)
            result[i] = -vector[i];
        return result;
    }

    //: Заполнение вектора числом (double)
    public void Fill(T val)
    {
        for (int i = 0; i < Length; i++)
            vector[i] = val;
    }

    //: Копирование вектора
    public static void Copy(Vector<T> source, Vector<T> dest)
    {
        for (int i = 0; i < source.Length; i++)
            dest[i] = source[i];
    }

    //: Очистка вектора
    public static void Clear(Vector<T> vector)
    {
        for (int i = 0; i < vector.Length; i++)
            vector[i] = T.Zero;
    }

    //: Выделение памяти под вектор
    public static void Resize(ref Vector<T> vector, int lenght)
    {
        vector = new(lenght);
    }

    //: Строковое представление вектора
    public override string ToString()
    {
        StringBuilder vec = new StringBuilder();
        if (vector == null) return vec.ToString();

        for (int i = 0; i < Length; i++)
            vec.Append(vector[i].ToString() + "\n");

        return vec.ToString();
    }

    //: Копирование объектов Vector
    public object Clone() { return MemberwiseClone(); }


    // % ***** Функция только для Vector(x,y,z) ***** % //
    //: Вычисление нормали двух векторов
    public static Vector<float> GetNormLine(Vector<float> vec1, Vector<float> vec2) {
        float x = vec1[1] * vec2[2] - vec1[2] * vec2[1];
        float y = vec1[2] * vec2[0] - vec1[0] * vec2[2];
        float z = vec1[0] * vec2[1] - vec1[1] * vec2[0];
        return new Vector<float>(new[] { x, y, z });
    }

    //: Нормализация 
    public static Vector<float> Normalize(Vector<float> vec) {
        return vec / Norm(vec);
    }

    //: Вычисление нормы вектора
    public static float Norm(Vector<float> vec) {
        return (float)Sqrt(Pow(vec[0], 2) + Pow(vec[1], 2) + Pow(vec[2], 2));
    }

    //: Вычисление скалярного произведения
    public static float Scalar(Vector<float> vec1, Vector<float> vec2) {
        return vec1[0] * vec2[0] + vec1[1] * vec2[1] + vec1[2] * vec2[2];
    }

    //: Вектор между двумя точками
    public static Vector<float> GetVector(Vector<float> vec1, Vector<float> vec2) {
        float x = vec1[0] - vec2[0];
        float y = vec1[1] - vec2[1];
        float z = vec1[2] - vec2[2];
        return new Vector<float>(new[] { x, y, z });
    }

    //: Вычисление нормали полигона
    public static Vector<float> GetVectorPolygon(Vector<float> vec1, Vector<float> vec2, Vector<float> vec3) {
        Vector<float> getvec1 = GetVector(vec3, vec2);
        Vector<float> getvec2 = GetVector(vec2, vec1);
        Vector<float> normal = GetNormLine(getvec1, getvec2);
        return Normalize(normal);
    }

    //: Вычисление позиции луча
    public static Vector<float> GetRayPosition(Ray ray, float ch) {
        return new Vector<float>(new float[] {
            (float)(ray.Position[0] + ray.Orientation[0] * ch),
            (float)(ray.Position[1] + ray.Orientation[1] * ch),
            (float)(ray.Position[2] + ray.Orientation[2] * ch)
        });
    }
}
