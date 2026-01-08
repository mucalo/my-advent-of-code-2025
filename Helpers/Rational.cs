namespace AdventOfCode2025.Helpers
{
    using System;
    using System.Numerics;

    public readonly struct Rational : IEquatable<Rational>
    {
        public BigInteger Num { get; }   // numerator
        public BigInteger Den { get; }   // denominator (always > 0)

        public Rational(BigInteger num, BigInteger den)
        {
            if (den == 0) throw new DivideByZeroException();

            // keep denominator positive
            if (den < 0) { num = -num; den = -den; }

            var g = BigInteger.GreatestCommonDivisor(BigInteger.Abs(num), den);
            Num = num / g;
            Den = den / g;
        }

        public static Rational FromInt(int x) => new Rational(x, 1);

        public static Rational operator +(Rational a, Rational b) =>
            new Rational(a.Num * b.Den + b.Num * a.Den, a.Den * b.Den);

        public static Rational operator -(Rational a, Rational b) =>
            new Rational(a.Num * b.Den - b.Num * a.Den, a.Den * b.Den);

        public static Rational operator *(Rational a, Rational b) =>
            new Rational(a.Num * b.Num, a.Den * b.Den);

        public static Rational operator /(Rational a, Rational b) =>
            new Rational(a.Num * b.Den, a.Den * b.Num);

        public decimal ToDecimal(int digits = 28)
        {
            // decimal has ~28-29 significant digits
            decimal n = (decimal)Num;
            decimal d = (decimal)Den;
            return Math.Round(n / d, digits);
        }

        public override string ToString()
        {
            if (this.Den == 1) return this.Num.ToString();
            else return $"{Num}/{Den}";
        }
        public bool Equals(Rational other) => Num == other.Num && Den == other.Den;
    }

}
