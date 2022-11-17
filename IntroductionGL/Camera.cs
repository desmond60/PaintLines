namespace IntroductionGL;

// % ***** Class Camera ***** % //
public class Camera
{
    public Vector<float> Position;     // Положение камеры
    public Vector<float> Orientation;  // Направление камеры
    public Vector<float> Rotation;     // Поворот камеры

    //: Конструктор
    public Camera() {
        Position    = new Vector<float>(new[] { -20.0f, 4.0f, 0.0f });
        Orientation = new Vector<float>(new[] { 0.0f, 0.0f, 0.0f });
        Rotation    = new Vector<float>(new[] { 0.0f, 1.0f, 0.0f });
    }

    //: Устанавливаем позицию камеры
    public void CameraPosition(Vector<float> pos, Vector<float> ori, Vector<float> rot) {
        Position    = (Vector<float>)pos.Clone();
        Orientation = (Vector<float>)ori.Clone();
        Rotation    = (Vector<float>)rot.Clone();
    }

    //: Передвижение камеры по Осям X, Z
    public void CameraMove(float speed) {
        // Направление взгляда
        Vector<float> opinion = Orientation - Position;

        // Передвигаем камеру
        Position[0] += opinion[0] * speed;
        Position[2] += opinion[2] * speed;

        // Меняем точку ориентира
        Orientation[0] += opinion[0] * speed;
        Orientation[2] += opinion[2] * speed;
    }

    //: Передвижение камеры по Оси Y
    public void CameraUpDown(float speed) {
        // Направление вверх
        Vector<float> opinion = new Vector<float>(new[] { 0.0f, 1.0f, 0.0f });

        // Поднимаем или отпускаем камеру
        Position[1]    += opinion[1] * speed;

        // Меняем точку ориентира
        Orientation[1] += opinion[1] * speed;
    }

    //: Вращение камеры вокруг наблюдательной точки
    public void CameraRotationObserver(float angle, Vector<float> vRot) {
        // Направление взгляда 
        Vector<float> opinion = Position - Orientation;

        // Пересчитываем координаты
        Vector<float> newPosition = new Vector<float>(vRot.Length);
        newPosition[0] = (float)((Cos(angle) + (1 - Cos(angle)) * Pow(vRot[0], 2)) * opinion[0] +
                +((1 - Cos(angle)) * vRot[0] * vRot[1] - vRot[2] * Sin(angle)) * opinion[1] +
                +((1 - Cos(angle)) * vRot[0] * vRot[2] + vRot[1] * Sin(angle)) * opinion[2]);

        newPosition[1] = (float)(((1 - Cos(angle)) * vRot[0] * vRot[1] + vRot[2] * Sin(angle)) * opinion[0] +
                +(Cos(angle) + (1 - Cos(angle)) * Pow(vRot[1], 2)) * opinion[1] +
                +((1 - Cos(angle)) * vRot[1] * vRot[2] - vRot[0] * Sin(angle)) * opinion[2]);

        newPosition[2] = (float)(((1 - Cos(angle)) * vRot[0] * vRot[2] - vRot[1] * Sin(angle)) * opinion[0] +
                +((1 - Cos(angle)) * vRot[1] * vRot[2] + vRot[0] * Sin(angle)) * opinion[1] +
                +(Cos(angle) + (1 - Cos(angle)) * Pow(vRot[2], 2)) * opinion[2]);

        // Новая позиция камеры
        Position = Orientation + newPosition;
    }

}

