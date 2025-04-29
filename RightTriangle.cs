using System;

/// <summary>
/// Класс, представляющий прямоугольный треугольник с двумя катетами.
/// </summary>
public class RightTriangle
{
    private double _a;
    private double _b;

    /// <summary>
    /// Длина первого катета.
    /// </summary>
    public double A
    {
        get { return _a; }
        set { _a = value; }
    }

    /// <summary>
    /// Длина второго катета.
    /// </summary>
    public double B
    {
        get { return _b; }
        set { _b = value; }
    }

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public RightTriangle()
    {
        _a = 0;
        _b = 0;
    }

    /// <summary>
    /// Конструктор с параметрами.
    /// </summary>
    /// <param name="a">Длина первого катета.</param>
    /// <param name="b">Длина второго катета.</param>
    public RightTriangle(double a, double b)
    {
        _a = a;
        _b = b;
    }

    /// <summary>
    /// Вычисляет площадь прямоугольного треугольника.
    /// </summary>
    /// <returns>Площадь треугольника.</returns>
    public double CalculateArea()
    {
        return 0.5 * _a * _b;
    }

    /// <summary>
    /// Возвращает строковое представление треугольника.
    /// </summary>
    /// <returns>Строка с длинами катетов.</returns>
    public override string ToString()
    {
        return $"Катеты: a = {_a}, b = {_b}";
    }

    /// <summary>
    /// Увеличивает длины катетов в 2 раза.
    /// </summary>
    /// <param name="triangle">Исходный треугольник.</param>
    /// <returns>Новый треугольник с увеличенными катетами.</returns>
    public static RightTriangle operator ++(RightTriangle triangle)
    {
        return new RightTriangle(triangle._a * 2, triangle._b * 2);
    }

    /// <summary>
    /// Уменьшает длины катетов в 2 раза.
    /// </summary>
    /// <param name="triangle">Исходный треугольник.</param>
    /// <returns>Новый треугольник с уменьшенными катетами.</returns>
    public static RightTriangle operator --(RightTriangle triangle)
    {
        return new RightTriangle(triangle._a / 2, triangle._b / 2);
    }

    /// <summary>
    /// Явное приведение к double - площадь треугольника, если он существует, иначе отрицательное число.
    /// </summary>
    /// <param name="triangle">Треугольник.</param>
    public static explicit operator double(RightTriangle triangle)
    {
        if (triangle._a > 0 && triangle._b > 0)
            return triangle.CalculateArea();

        return -1;
    }

    /// <summary>
    /// Неявное приведение к bool - true, если треугольник существует, иначе false.
    /// </summary>
    /// <param name="triangle">Треугольник.</param>
    public static implicit operator bool(RightTriangle triangle)
    {
        return triangle._a > 0 && triangle._b > 0;
    }

    /// <summary>
    /// Сравнивает площади треугольников (меньше или равно).
    /// </summary>
    public static bool operator <=(RightTriangle left, RightTriangle right)
    {
        return left.CalculateArea() <= right.CalculateArea();
    }

    /// <summary>
    /// Сравнивает площади треугольников (больше или равно).
    /// </summary>
    public static bool operator >=(RightTriangle left, RightTriangle right)
    {
        return left.CalculateArea() >= right.CalculateArea();
    }
}
