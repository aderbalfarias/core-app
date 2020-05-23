using System;

namespace CoreApp.Domain.CSharp8
{
    public struct ReadonlyMembers
    {
        public double X { get; set; }
        public double Y { get; set; }
        //public double Distance => Math.Sqrt(X * X + Y * Y);
        public readonly double Distance => Math.Sqrt(X * X + Y * Y);

        //public override string ToString() =>
        //    $"({X}, {Y}) is {Distance} from the origin";

        public readonly override string ToString() =>
            $"({X}, {Y}) is {Distance} from the origin";
    }
}
