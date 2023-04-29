namespace Pudelko_Lab_
{
    public static class ExtensionMethod
    {
        public static Pudelko Kompresuj(this Pudelko input)
        {
            if (input.A == input.B & input.A == input.C) 
                return new Pudelko(input.A, input.B, input.C);

            double edge = Math.Round(Math.Pow(input.Objetosc, 1D / 3), 3);

            return new Pudelko(edge, edge, edge, UnitOfMeasure.meter);
        }
    }
}
