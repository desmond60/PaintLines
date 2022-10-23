namespace IntroductionGL.EventOpenGL3D;

//: Класс нормали
public class Vector : ICloneable
{
    //: Координаты нормали
    public float x, y, z;

    //: Конструктор с параметрами
    public Vector(float x, float y, float z) { 
        this.x = x;
        this.y = y;
        this.z = z;
    }

    //: Конструктор без параметров
    public Vector() { }

    //: Перегрузка операторов
    public static Vector operator +(Vector vec1, Vector vec2) {
        return new Vector(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
    }

    public static Vector operator -(Vector vec1, Vector vec2) {
        return new Vector(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
    }

    public static Vector operator *(Vector vec1, float val) {
        return new Vector(vec1.x * val, vec1.y * val, vec1.z * val);
    }

    public static Vector operator /(Vector vec1, float val) {
        return new Vector(vec1.x / val, vec1.y / val, vec1.z / val);
    }

    //: Вычисление нормали двух векторов
    public static Vector GetNormLine(Vector vec1, Vector vec2) {
        float x = vec1.y * vec2.z - vec1.z * vec2.y;
        float y = vec1.z * vec2.x - vec1.x * vec2.z;
        float z = vec1.x * vec2.y - vec1.y * vec2.x;

        return new Vector(x, y, z);
    }

    //: Нормализация 
    public static Vector Normalize(Vector vec) {
        return vec / Vector.Norm(vec);
    }

    //: Вычисление нормы вектора
    public static float Norm(Vector vec) {
        return (float)Sqrt(Pow(vec.x, 2) + Pow(vec.y, 2) + Pow(vec.z, 2));
    }

    //: Вычисление скалярного произведения
    public static float Scalar(Vector vec1, Vector vec2) {
        return vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z;
    }

    //: Вектор между двумя точками
    public static Vector GetVector(Vector vec1, Vector vec2) {
        float x = vec1.x - vec2.x;
        float y = vec1.y - vec2.y;
        float z = vec1.z - vec2.z;

        return new Vector(x, y, z);
    }

    //: Вычисление нормали полигона
    public static Vector GetVectorPolygon(Vector vec1, Vector vec2, Vector vec3) {
        Vector getvec1 = Vector.GetVector(vec3, vec2);
        Vector getvec2 = Vector.GetVector(vec2, vec1);
        Vector normal  = Vector.GetNormLine(getvec1, getvec2);
        return Vector.Normalize(normal);
    }

    //: Копирование объектов Vector
    public object Clone() {
        return MemberwiseClone();
    }
}

