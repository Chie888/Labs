using System;

public class RightTriangle
{
    public double firstLeg { get; set; }
    public double secondLeg { get; set; }


    public RightTriangle()
    {
        firstLeg = 0;
        secondLeg = 0;
    }

  
    public RightTriangle(double first, double second)
    {
        this.firstLeg = first;
        this.secondLeg = second;
    }

   
    /// Вычисляет площадь прямоугольного треугольника по формуле 1/2 * ширина(1-й катет) * высота(2-й катет).
    public double CalculateArea()
    {
        return 0.5 * firstLeg * secondLeg;
    }

    public override string ToString()
    {
        return $"Катеты: firstLeg = {firstLeg}, secondLeg = {secondLeg}";
    }


    public static RightTriangle operator ++(RightTriangle triangle)
    {
        return new RightTriangle(triangle.firstLeg * 2, triangle.secondLeg * 2);
    }


    public static RightTriangle operator --(RightTriangle triangle)
    {
        return new RightTriangle(triangle.firstLeg / 2, triangle.secondLeg / 2);
    }


    public static explicit operator double(RightTriangle triangle)
    {
        if (triangle.firstLeg > 0 && triangle.secondLeg > 0)
            return triangle.CalculateArea();
        else
            return -1;
    }


    public static implicit operator bool(RightTriangle triangle)
    {
        return triangle.firstLeg > 0 && triangle.secondLeg > 0;
    }

  
    public static bool operator <=(RightTriangle triangle1, RightTriangle triangle2)
    {
        return triangle1.CalculateArea() <= triangle2.CalculateArea();
    }


    public static bool operator >=(RightTriangle triangle1, RightTriangle triangle2)
    {
        return triangle1.CalculateArea() >= triangle2.CalculateArea();
    }
}

