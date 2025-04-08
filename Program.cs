using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите длины катетов треугольника:");

        double firstLeg = InputValidator.GetPositiveDouble("firstLeg = ");
        double secondLeg = InputValidator.GetPositiveDouble("secondLeg = ");

        RightTriangle triangle = new RightTriangle(firstLeg, secondLeg);

        Console.WriteLine(triangle.ToString());
        Console.WriteLine($"Площадь: {triangle.CalculateArea()}");

       
        RightTriangle newTriangle1 = ++triangle;
        Console.WriteLine($"После ++: {newTriangle1.ToString()}");

        RightTriangle newTriangle2 = --newTriangle1;
        Console.WriteLine($"После --: {newTriangle2.ToString()}");

        double area = (double)triangle;
        Console.WriteLine($"Площадь через приведение типа: {area}");

        bool isValid = (bool)triangle;
        Console.WriteLine($"Треугольник существует: {isValid}");

        RightTriangle triangle2 = new RightTriangle(3, 4);
        Console.WriteLine($"Треугольник 1 <= Треугольник 2: {triangle <= triangle2}");
        Console.WriteLine($"Треугольник 1 >= Треугольник 2: {triangle >= triangle2}");
    }
}
