using System.Collections;
using System.Globalization;

namespace Pudelko_Lab_
{
    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerator
    {
        private readonly double[] dimensions; //Array of dimensions (in milimeters)

        #region Constructors
        public Pudelko(double? a, double? b, double? c) : this(a, b, c, UnitOfMeasure.meter) { }
        public Pudelko(double? a, double? b, UnitOfMeasure unit) : this(a, b, null, unit) { }
        public Pudelko(double? a, double? b) : this(a, b, 0.1, UnitOfMeasure.meter) { }
        public Pudelko(double? a, UnitOfMeasure unit) : this(a, null, null, unit) { }
        public Pudelko(double? a) : this(a, 0.1, 0.1, UnitOfMeasure.meter) { }
        public Pudelko() : this(0.1, 0.1, 0.1, UnitOfMeasure.meter) { }
        public Pudelko(double? a, double? b, double? c, UnitOfMeasure unit)
        {
            int factor = 1000; //Default factor (for meter)
            if (unit == UnitOfMeasure.centimeter) factor = 10;
            else if (unit == UnitOfMeasure.milimeter) factor = 1;

            double[] dimensions = new double[3];
            if (a is not null) dimensions[0] = Math.Floor((double)a * factor);
            else dimensions[0] = 100;
            if (b is not null) dimensions[1] = Math.Floor((double)b * factor);
            else dimensions[1] = 100;
            if (c is not null) dimensions[2] = Math.Floor((double)c * factor);
            else dimensions[2] = 100;

            for (int i = 0; i < dimensions.Length; i++)
                if (dimensions[i] > 10000 | dimensions[i] < 1)
                    throw new ArgumentOutOfRangeException("Wrong dimension!");

            this.dimensions = dimensions;
        } //Main constructor
        #endregion

        #region External properties
        public double A { get => Math.Round(dimensions[0] / 1000, 3); }
        public double B { get => Math.Round(dimensions[1] / 1000, 3); }
        public double C { get => Math.Round(dimensions[2] / 1000, 3); }
        public double Objetosc { get => Math.Round((dimensions[0] / 1000) * (dimensions[1] / 1000) * (dimensions[2] / 1000), 9); }
        public double Pole { get => Math.Round(2 * (dimensions[0] / 1000) * (dimensions[1] / 1000) + 2 * (dimensions[0] / 1000) * (dimensions[2] / 1000) + 2 * (dimensions[1] / 1000) * (dimensions[2] / 1000), 6); }
        #endregion

        #region Implementation of IFormattable
        public override string ToString()
        {
            return this.ToString("M", CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            if (String.IsNullOrEmpty(format)) format = "M";
            if (provider == null) provider = CultureInfo.CurrentCulture;

            int offset;
            switch (format.ToUpper())
            {
                case "MM":
                    offset = 1000;
                    return $"{(A * offset).ToString("F0", provider)} mm × {(B * offset).ToString("F0", provider)} mm × {(C * offset).ToString("F0", provider)} mm";
                case "CM":
                    offset = 100;
                    return $"{(A * offset).ToString("F1", provider)} cm × {(B * offset).ToString("F1", provider)} cm × {(C * offset).ToString("F1", provider)} cm";
                case "M":
                    return $"{A.ToString("F3", provider)} m × {B.ToString("F3", provider)} m × {C.ToString("F3", provider)} m";
                default:
                    throw new FormatException(String.Format($"The {format} format string is not supported."));
            }
        }
        #endregion

        #region Implementation of IEquitable <Pudelko>
        public bool Equals(Pudelko other)
        {
            if (other == null) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            double[] thisBox = { this.A, this.B, this.C };
            double[] otherBox = { other.A, other.B, other.C };
            Array.Sort(thisBox);
            Array.Sort(otherBox);
            return thisBox[0] == otherBox[0] & thisBox[1] == otherBox[1] & thisBox[2] == otherBox[2];
        }

        public override bool Equals(object obj)
        {
            if (obj is Pudelko) return Equals((Pudelko)obj);
            else return false;
        }

        public override int GetHashCode() => this.dimensions.GetHashCode();

        public static bool Equals(Pudelko p1, Pudelko p2)
        {
            if ((p1 is null) && (p2 is null)) return true;
            if ((p1 is null)) return false;

            return p1.Equals(p2);
        }

        public static bool operator ==(Pudelko p1, Pudelko p2) => Equals(p1, p2);
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);
        #endregion

        #region Implementation of operators
        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            double outputA, outputB, outputC;
            double[] dimensionsFirst = { p1.A, p1.B, p1.C };
            double[] dimensionsSecond = { p2.A, p2.B, p2.C };
            Array.Sort(dimensionsFirst);
            Array.Sort(dimensionsSecond);

            if (dimensionsFirst[2] > dimensionsSecond[2])
                outputA = dimensionsFirst[2];
            else
                outputA = dimensionsSecond[2];

            if (dimensionsFirst[1] > dimensionsSecond[1])
                outputB = dimensionsFirst[1];
            else
                outputB = dimensionsSecond[1];

            outputC = dimensionsFirst[0] + dimensionsSecond[0];

            return new Pudelko(outputA, outputB, outputC);
        }
        #endregion

        #region Conversions
        public static explicit operator double[](Pudelko p1)
        {
            return new[] { p1.A, p1.B, p1.C };
        }//Explicit conversion Pudelko --> double[]

        public static implicit operator Pudelko(ValueTuple<int, int, int> input)
        {
            return new Pudelko(input.Item1, input.Item2, input.Item3, UnitOfMeasure.milimeter);
        }//Implicit conversion ValueTuple --> Pudelko
        #endregion

        #region Implementation of IEnumerator

        private int position = -1;

        public bool MoveNext()
        {
            position++;
            return (position < dimensions.Length);
        }
        public void Reset()
        {
            position = -1;
        }
        public object Current
        {
            get { return Math.Round(dimensions[position] / 1000, 3); }
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)this;
        }
        #endregion

        public double this[int i] { get => Math.Round(dimensions[i] / 1000, 3); } //Indexer (read-only)

        public static Pudelko Parse(string input)
        {
            if (String.IsNullOrWhiteSpace(input)) throw new ArgumentException(input);

            string[] inputWords = input.Split(" × ");

            double[] inputDimensions = { 100, 100, 100 };

            if (inputWords.Length > 3) throw new FormatException("Format exceeded third dimension");

            for (int i = 0; i < inputWords.Length; i++)
                if (inputWords[i] is not null)
                {
                    string[] word = inputWords[i].Split(' ');

                    int factor;

                    if (word.Length == 1) factor = 1000;
                    else if (word[1].ToUpper() == "M")
                        factor = 1000;
                    else if (word[1].ToUpper() == "CM")
                        factor = 10;
                    else if (word[1].ToUpper() == "MM")
                        factor = 1;
                    else
                        throw new FormatException($"Format {word[1]} is not supported");

                    bool conversionSuccess = double.TryParse(word[0], out double parsedValue);

                    if (!conversionSuccess) throw new FormatException("Conversion failed. Input string was not correctly formated.");

                    inputDimensions[i] = parsedValue * factor;
                }

            return new Pudelko(inputDimensions[0], inputDimensions[1], inputDimensions[2], UnitOfMeasure.milimeter);
        }//Parse from string
    }
}