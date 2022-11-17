namespace IntroductionGL.numerics;

// % ***** Class Matrix ***** % //
public class Matrix<T> : ICloneable
                where T : System.Numerics.INumber<T>
{
    //: Поля и свойства
    private T[,] matrix;           // Матрица (квадратная)
    public int Dim { get; init; }  // Размерность матрицы

    //: Конструктор (с размерностью)
    public Matrix(int _dim)
    {
        matrix = new T[_dim, _dim];
        this.Dim = _dim;
    }

    //: Конструктор (с двумерным массивом)
    public Matrix(T[,] _mat)
    {
        Dim = _mat.GetUpperBound(0) + 1;
        matrix = new T[Dim, Dim];
        for (int i = 0; i < Dim; i++)
            for (int j = 0; j < Dim; j++)
                matrix[i, j] = _mat[i, j];
    }

    //: Деконструктор
    public void Deconstruct(out T[,] mat)
    {
        mat = this.matrix;
    }

    //: Индексаторы
    public T this[int i, int j]
    {
        get { return matrix[i, j]; }
        set { matrix[i, j] = value; }
    }

    //: Перегрузка умножение числа на матрицу
    public static Matrix<T> operator *(T Const, Matrix<T> mat)
    {
        var result = new Matrix<T>(mat.Dim);
        for (int i = 0; i < result.Dim; i++)
            for (int j = 0; j < result.Dim; j++)
                result[i, j] = Const * mat[i, j];
        return result;
    }

    //: Перегрузка сложение матриц 
    public static Matrix<T> operator +(Matrix<T> mat1, Matrix<T> mat2)
    {
        var result = new Matrix<T>(mat1.Dim);
        for (int i = 0; i < result.Dim; i++)
            for (int j = 0; j < result.Dim; j++)
                result[i, j] = mat1[i, j] + mat2[i, j];
        return result;
    }

    //: Копирование матриц
    public static void Copy(Matrix<T> source, Matrix<T> dest)
    {
        for (int i = 0; i < source.Dim; i++)
            for (int j = 0; j < source.Dim; j++)
                dest[i, j] = source[i, j];
    }

    //: Очистка матрицы
    public static void Clear(Matrix<T> Matrix)
    {
        for (int i = 0; i < Matrix.Dim; i++)
            for (int j = 0; j < Matrix.Dim; j++)
                Matrix[i, j] = T.Zero;
    }

    //: Строковое представление матрицы
    public override string ToString()
    {
        StringBuilder mat = new StringBuilder();
        if (matrix == null) return mat.ToString();

        for (int i = 0; i < Dim; i++)
        {
            for (int j = 0; j < Dim; j++)
                mat.Append(matrix[i, j] + "\t");
            mat.Append("\n");
        }
        return mat.ToString();
    }

    //: Копирование объектов Matrix
    public object Clone()
    {
        return MemberwiseClone();
    }

}