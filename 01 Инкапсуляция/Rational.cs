using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public int Numerator { get; }
        public int Denominator { get; }

        public bool IsNan
        {
            get
            {
                return Denominator == 0;
            }
        }

        static int SearchNOD(int a, int b)
        {
            while (a != b)
            {
                if (a > b) { a -= b; }
                else       { b -= a; }
            }
            return b;
        }

        public static Rational operator + (Rational op1, Rational op2)
        {
            if (op1.Denominator == 0 || op2.Denominator == 0)
                return new Rational(0, 0);
            
            int denominator = op1.Denominator * op2.Denominator;
            int numerator = denominator / op1.Denominator * op1.Numerator 
                          + denominator / op2.Denominator * op2.Numerator;
            int nod = SearchNOD(numerator, denominator);

            Rational rational = new Rational(numerator / nod, denominator / nod);

            return rational;
        }

        public static Rational operator - (Rational op1, Rational op2)
        {
            if (op1.Denominator == 0 || op2.Denominator == 0)
                return new Rational(0, 0);

            int denominator = op1.Denominator * op2.Denominator;
            int numerator = denominator / op1.Denominator * op1.Numerator 
                          - denominator / op2.Denominator * op2.Numerator;
            int nod = SearchNOD(numerator, denominator);

            Rational rational = new Rational(numerator / nod, denominator / nod);
            return rational;
        }

        public static Rational operator * (Rational op1, Rational op2)
        {
            Rational rational = new Rational(op1.Numerator * op2.Numerator, op1.Denominator * op2.Denominator);
            return rational;
        }

        public static Rational operator / (Rational op1, Rational op2)
        {
            Rational rational;
            if (op2.Denominator == 0 )
                rational = new Rational(op1.Numerator / op2.Numerator, 0);
            else if (op2.Numerator == 0)
                rational = new Rational(0, 0);
            else
                rational = new Rational(op1.Numerator / op2.Numerator, op1.Denominator / op2.Denominator);

            return rational;
        }

        public static implicit operator double(Rational rational)
        {
            if (rational.Denominator == 0)
                return double.NaN;
            else
                return (double)rational.Numerator / rational.Denominator;
        }
        
        public static implicit operator int(Rational rational)
        {
            if (rational.Numerator % rational.Denominator != 0)
                throw new Exception();
            return rational.Numerator / rational.Denominator;
        }

        public static implicit operator Rational(int n)
        {
            return new Rational(n);
        }

        public Rational(int numerator, int denominator = 1)
        {
            int nod = 1;
            if(numerator != 0 && denominator != 0)
                nod = SearchNOD(Math.Abs(numerator), Math.Abs(denominator));
           
            if (denominator < 0)
            {
                denominator *= (-1);
                numerator *= (-1);
            }

            Numerator = numerator / nod;
            if (numerator == 0 && denominator != 0)
                Denominator = 1;
            else
                Denominator = denominator / nod;
        }
    }
}
