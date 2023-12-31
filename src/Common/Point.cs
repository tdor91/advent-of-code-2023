﻿namespace Common;

public readonly record struct Point(int X, int Y)
{
    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);

    public Point Invert() => new(X * -1, Y * -1);

    public (long X, long Y) DistanceTo(Point a) => (Math.Abs(X - a.X), Math.Abs(Y - a.Y));
}
