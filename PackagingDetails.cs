using System;

public class PackagingDetails : PackagingDimensions
{
    public int weight { get; set; }
    public string material { get; set; }


    public PackagingDetails() : base()
    {
        weight = 0;
        material = "Не указан";
    }


    public PackagingDetails(int length, int width, int height, 
        int weight, string material) : base(length, width, height)
    {
        this.weight = weight;
        this.material = material;
    }


    public PackagingDetails(PackagingDetails other) : base(other)
    {
        this.weight = other.weight;
        this.material = other.material;
    }

   
    public void ChangeMaterial(string newMaterial)
    {
        material = newMaterial;
    }

  
    public void ChangeWeight(int newWeight)
    {
        if (newWeight >= 0)
            weight = newWeight;
        else
            Console.WriteLine("Вес не может быть отрицательным.");
    }

   
    public override string ToString()
    {
        return base.ToString() + $", Вес = {weight}, Материал = {material}";
    }
}
