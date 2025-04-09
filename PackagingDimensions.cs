using System;

public class PackagingDimensions
{
    private int length { get; set; }
    private int width { get; set; }
    private int height { get; set; }


    public PackagingDimensions()
    {
        length = 0;
        width = 0;
        height = 0;
    }


    public PackagingDimensions(int length, int width, int height)
    {
        this.length = length;
        this.width = width;
        this.height = height;
    }


    public PackagingDimensions(PackagingDimensions other)
    {
        this.length = other.length;
        this.width = other.width;
        this.height = other.height;
    }


    public override string ToString()
    {
        return $"Упаковка: Длина = {length}, Ширина = {width}, Высота = {height}";
    }
}
