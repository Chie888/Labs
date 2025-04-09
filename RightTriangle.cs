using System;

public class RightTriangle
{
    private double _firstLeg;
    private double _secondLeg;

    public double firstLeg { get { return _firstLeg; } set { _firstLeg = value; } }
    public double secondLeg { get { return _secondLeg; } set { _secondLeg = value; }

    
    public RightTriangle()
    {
        _firstLeg = 0;
        _secondLeg = 0;
    }

  
    public RightTriangle(double first, double second)
    {
        this._firstLeg = first;
        this._secondLeg = second;
    }

   
    /// Вычисляет площадь прямоугольного треугольника по формуле 1/2 * ширина(1-й катет) * высота(2-й катет).
    public double CalculateArea()
    {
        return 0.5 * _firstLeg * _secondLeg;
    }

    public override string ToString()
    {
        return $"Катеты: _firstLeg = {_firstLeg}, _secondLeg = {_secondLeg}";
    }


    public static RightTriangle operator ++(RightTriangle triangle)
    {
        return new RightTriangle(triangle._firstLeg * 2, triangle._secondLeg * 2);
    }


    public static RightTriangle operator --(RightTriangle triangle)
    {
        return new RightTriangle(triangle._firstLeg / 2, triangle._secondLeg / 2);
    }


    public static explicit operator double(RightTriangle triangle)
    {
        if (triangle._firstLeg > 0 && triangle._secondLeg > 0)
            return triangle.CalculateArea();
        else
            return -1;
    }


    public static implicit operator bool(RightTriangle triangle)
    {
        return triangle._firstLeg > 0 && triangle._secondLeg > 0;
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

