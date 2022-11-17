namespace IntroductionGL.EventOpenGL3D_Rays;

// % ***** Class Gauss ***** % //
public class Gauss
{
    //: Поля и свойства
    private Matrix<float> mat;    // Матрица
    private Vector<float> vec;    // Правая часть
    private int N { get; }        // Размерность СЛАУ
    private double EPS { get; }   // Точность

    //: Конструктор
    public Gauss(int N, double eps)
    {
        this.N = N;
        this.mat = new Matrix<float>(N);
        this.vec = new Vector<float>(N);
        this.EPS = eps;
    }

    //: Основной метод решения СЛАУ
    public Vector<float> solve(Matrix<float> matrix, Vector<float> vector)
    {
        Matrix<float>.Copy(matrix, mat);
        Vector<float>.Copy(vector, vec);

        for (int i = 0; i < N; i++)
        {

            // Ищем максимальный элемент
            double MAX = Abs(mat[i, i]);
            int ind = i;
            for (int s = i + 1; s < N; s++)
                if (Abs(mat[s, i]) > MAX)
                    (MAX, ind) = (Abs(mat[s, i]), s);

            // Меняем строки СЛАУ (т.е. вместе с правой частью)
            for (int k = 0; k < N; k++)
                (mat[i, k], mat[ind, k]) = (mat[ind, k], mat[i, k]);
            (vec[i], vec[ind]) = (vec[ind], vec[i]);

            // Нормируем уравнения
            for (int j = i; j < N; j++)
            {
                double temp = mat[j, i];
                // Для нуля if пропускается
                if (Abs(temp) > EPS)
                {
                    for (int p = 0; p < N; p++)
                        mat[j, p] /= (float)temp;
                    vec[j] /= (float)temp;

                    // if на то чтобы не вычитать строку саму из себя
                    if (j != i)
                    {
                        for (int p = 0; p < N; p++)
                            mat[j, p] -= mat[i, p];
                        vec[j] -= vec[i];
                    }
                }
            }
        }

        // Решаем обратным ходом
        Vector<float> res = new Vector<float>(N);
        for (int i = N - 1; i >= 0; i--)
        {
            res[i] = vec[i];
            for (int j = 0; j < i; j++)
                vec[j] -= mat[j, i] * res[i];
        }

        return res;
    }

}

