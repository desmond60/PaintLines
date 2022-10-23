namespace IntroductionGL.EventOpenGL3D;

//: Класс с положением и направлением камеры
public class Camera
{
    public Vector Position;     // Положение точки наблюдателя
    public Vector Orientation;  // Направление наблюдателя
    public Vector Rotation;     // Поворот сцены

    //: Конструктор
    public Camera() {
        Position    = new Vector();
        Orientation = new Vector();
        Rotation    = new Vector();
    }

    //: Устанавливаем позицию камеры
    public void CameraPosition(Vector pos, Vector ori, Vector rot) {
        Position    = (Vector)pos.Clone();
        Orientation = (Vector)ori.Clone();
        Rotation    = (Vector)rot.Clone();
    }

    //: Передвижение камеры
    public void CameraMove(float speed) {
        // Направление взгляда
        Vector opinion = Orientation - Position;

        // Передвигаем камеру
        Position.x += opinion.x * speed;
        Position.z += opinion.z * speed;

        Orientation.x += opinion.x * speed;
        Orientation.z += opinion.z * speed;
    }

    //: Вращение камеры вокруг задней оси
    public void CameraRotation(float angle, Vector vRot) {
        // Направление взгляда 
        Vector opinion = Orientation - Position;

        // Пересчитываем координаты
        Vector newOrientation = new Vector();
        newOrientation.x = (float)((Cos(angle) + (1 - Cos(angle)) * Pow(vRot.x, 2))  * opinion.x +
                +((1 - Cos(angle)) * vRot.x * vRot.y - vRot.z * Sin(angle)) * opinion.y +
                +((1 - Cos(angle)) * vRot.x * vRot.z + vRot.y * Sin(angle)) * opinion.z);

        newOrientation.y = (float)(((1 - Cos(angle)) * vRot.x * vRot.y + vRot.z * Sin(angle)) * opinion.x +
                +(Cos(angle) + (1 - Cos(angle)) * vRot.y * vRot.y)                   * opinion.y +
                +((1 - Cos(angle)) * vRot.y * vRot.z - vRot.x * Sin(angle))          * opinion.z);

        newOrientation.z = (float)(((1 - Cos(angle)) * vRot.x * vRot.z - vRot.y * Sin(angle)) * opinion.x +
                +((1 - Cos(angle)) * vRot.y * vRot.z + vRot.x * Sin(angle)) * opinion.y +
                +(Cos(angle) + (1 - Cos(angle)) * vRot.z * vRot.z) * opinion.z);

        // Новое направление наблюдателя
        Orientation = Position + newOrientation;
    }

    //: Вращение вокруг наблюдателя
    public void CameraRotationObserver(float angle, Vector vCenter, Vector vRot) {
        // Направление взгляда 
        Vector opinion = Position - vCenter;

        // Пересчитываем координаты
        Vector newPosition = new Vector();
        newPosition.x = (float)((Cos(angle) + (1 - Cos(angle)) * Pow(vRot.x, 2)) * opinion.x +
                +((1 - Cos(angle)) * vRot.x * vRot.y - vRot.z * Sin(angle)) * opinion.y +
                +((1 - Cos(angle)) * vRot.x * vRot.z + vRot.y * Sin(angle)) * opinion.z);

        newPosition.y = (float)(((1 - Cos(angle)) * vRot.x * vRot.y + vRot.z * Sin(angle)) * opinion.x +
                +(Cos(angle) + (1 - Cos(angle)) * vRot.y * vRot.y) * opinion.y +
                +((1 - Cos(angle)) * vRot.y * vRot.z - vRot.x * Sin(angle)) * opinion.z);

        newPosition.z = (float)(((1 - Cos(angle)) * vRot.x * vRot.z - vRot.y * Sin(angle)) * opinion.x +
                +((1 - Cos(angle)) * vRot.y * vRot.z + vRot.x * Sin(angle)) * opinion.y +
                +(Cos(angle) + (1 - Cos(angle)) * vRot.z * vRot.z) * opinion.z);

        // Новая позиция камеры
        Position = vCenter + newPosition;
    }

    //: Установка вида с помощью мыши
    public void SetOrientation() { 
    
    }


}

