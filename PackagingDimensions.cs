using System;

public class PackagingDimensions
{
    private int _length;
    private int _width;
    private int _height;

    public int length { get { return _length; } set { _length = value; } }
    public int width { get { return _width; } set { _width = value; } }
    public int height { get { return _height; } set { _height = value; } }


    public PackagingDimensions()
    {
        _length = 0;
        _width = 0;
        _height= 0;
    }


    public PackagingDimensions(int _length, int _width, int height)
    {
        this._length = _length;
        this._width = _width;
        this._height= height;
    }


    public PackagingDimensions(PackagingDimensions other)
    {
        this._length = other._length;
        this._width = other._width;
        this._height= other.height;
    }


    public override string ToString()
    {
        return $"Упаковка: Длина = {_length}, Ширина = {_width}, Высота = {height}";
    }
}
