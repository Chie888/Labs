using System;

public class PackagingDetails : PackagingDimensions
{
    private int _weight;
    private string _material;

    public int weight { get { return _weight; } set { _weight = value; } }
    public string material { get { return _material; } set { _material = value; } }


    public PackagingDetails() : base()
    {
        _weight = 0;
        _material = "Не указан";
    }


    public PackagingDetails(int length, int width, int height, 
        int _weight, string _material) : base(length, width, height)
    {
        this._weight = _weight;
        this._material = _material;
    }


    public PackagingDetails(PackagingDetails other) : base(other)
    {
        this._weight = other._weight;
        this._material = other._material;
    }

   
    public void Change_material(string new_material)
    {
        _material = new_material;
    }

  
    public void Change_weight(int new_weight)
    {
        if (new_weight >= 0)
            _weight = new_weight;
        else
            Console.WriteLine("Вес не может быть отрицательным.");
    }

   
    public override string ToString()
    {
        return base.ToString() + $", Вес = {_weight}, Материал = {_material}";
    }
}
