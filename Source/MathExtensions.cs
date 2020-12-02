using System;

namespace ArtilleryStrike
{
    public class Angle
    {
        public Angle()
        {
            Radians = 0;
        }
        public Angle(double radians)
        {
           Radians = radians;
        }
        public double Radians { get; set; }
        public double Degrees => Angle.ToDegrees(Radians);

        public void Validate()
        {
            double degrees = Degrees % 360;
            Radians = ToRadians(degrees);
        }

        public static double ToRadians(double degrees) =>
            Math.PI / 180 * degrees;
        public static double ToDegrees(double radians) =>
            radians * 180 / Math.PI;

        public static Angle operator -(Angle a) =>
            new Angle(-a.Radians);
        public static Angle operator -(Angle lhs, Angle rhs) =>
            new Angle(lhs.Radians - rhs.Radians);
        public static Angle operator +(Angle lhs, Angle rhs) =>
            new Angle(lhs.Radians + rhs.Radians);
        public static bool operator <(Angle lhs, Angle rhs) =>
            lhs.Radians < rhs.Radians;
        public static bool operator >(Angle lhs, Angle rhs) =>
            lhs.Radians > rhs.Radians;
    }

    public class Side
    {
        public Side()
        {
            Length = 0;
        }
        public Side(double length)
        {
            Length = length;
        }
        public double Length { get; set; }

    }

    static class MathExtension
    {
        public static Side LawOfCosines(Side a, Side b, Angle theta)
        {
            Side c = new Side();
            double a_squared = Math.Pow(a.Length, 2);
            double b_squared = Math.Pow(b.Length, 2);
            double cosine = 2 * a.Length * b.Length * Math.Cos(theta.Radians);

            double c_squared = a_squared + b_squared - cosine;
            c.Length = Math.Sqrt(c_squared);
            return c;
        }

        public static Angle LawOfCosines(Side a, Side b, Side c)
        {
            double a_squared = Math.Pow(a.Length, 2);
            double b_squared = Math.Pow(b.Length, 2);
            double c_squared = Math.Pow(c.Length, 2);
            double divisor = 2 * a.Length * b.Length;

            double cosC = (a_squared + b_squared - c_squared) / divisor;
            return new Angle(Math.Acos(cosC));
        }
    }
}
