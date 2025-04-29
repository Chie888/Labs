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

   
    public RightTriangle()
    {
        _a = 0;
        _b = 0;
    }

   
    public RightTriangle(double a, double b)
    {
        _a = a;
        _b = b;
    }

    
    public double CalculateArea()
    {
        return 0.5 * _a * _b;
    }

   
    public override string ToString()
    {
        return $"Катеты: a = {_a}, b = {_b}";
    }

    
    public static RightTriangle operator ++(RightTriangle triangle)
    {
        return new RightTriangle(triangle._a * 2, triangle._b * 2);
    }

    
    public static RightTriangle operator --(RightTriangle triangle)
    {
        return new RightTriangle(triangle._a / 2, triangle._b / 2);
    }

   
    public static explicit operator double(RightTriangle triangle)
    {
        if (triangle._a > 0 && triangle._b > 0)
            return triangle.CalculateArea();

        return -1;
    }

    
    public static implicit operator bool(RightTriangle triangle)
    {
        return triangle._a > 0 && triangle._b > 0;
    }

   
    public static bool operator <=(RightTriangle left, RightTriangle right)
    {
        return left.CalculateArea() <= right.CalculateArea();
    }

   
    public static bool operator >=(RightTriangle left, RightTriangle right)
    {
        return left.CalculateArea() >= right.CalculateArea();
    }
}
