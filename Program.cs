using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите размеры упаковки:");

        int length = InputValidator.GetInt("Длина = ");
        int width = InputValidator.GetInt("Ширина = ");
        int height = InputValidator.GetInt("Высота = ");

        PackagingDimensions basePackage = new PackagingDimensions(length, width, height);

        Console.WriteLine(basePackage.ToString());


        PackagingDimensions copiedPackage = new PackagingDimensions(basePackage);
        Console.WriteLine("Скопированная упаковка: " + copiedPackage.ToString());

       
        int weight = InputValidator.GetInt("Вес = ");
        string material = InputValidator.GetString("Материал = ");
        PackagingDetails derivedPackage = new PackagingDetails(length, width, height, weight, material);
        Console.WriteLine(derivedPackage.ToString());

        string newMaterial = InputValidator.GetString("Введите новый материал = ");
        derivedPackage.ChangeMaterial(newMaterial);
        Console.WriteLine("После изменения материала: " + derivedPackage.ToString());
       
        int newWeight = InputValidator.GetInt("Введите новый вес: ");
        derivedPackage.ChangeWeight(newWeight);
        Console.WriteLine("После изменения веса: " + derivedPackage.ToString());
        
        
        PackagingDetails copiedDerivedPackage = new PackagingDetails(derivedPackage);
        Console.WriteLine("Скопированная детальная упаковка: " + copiedDerivedPackage.ToString());
    }
}
