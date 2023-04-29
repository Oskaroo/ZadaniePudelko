using System.Net.Http.Headers;

namespace Pudelko_Lab_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Exceptions - wrong box dimensions
            try
            {
                var exception1 = new Pudelko(-1.0, 0.8, 0.8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd {ex.Message} - ujemna wartość boku");
            }
            try
            {
                var exception2 = new Pudelko(0.0, 0.8, 0.8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd {ex.Message} - zerowa wartość boku");
            }

            //Comparing of boxes
            var compareA = new Pudelko(2.5, 9.321, 0.1);
            var compareB = new Pudelko(2500, 9321, UnitOfMeasure.milimeter);
            Console.WriteLine($"\nPudełko A: {compareA.ToString("mm")}");
            Console.WriteLine($"Pudełko B: {compareA.ToString("cm")}");
            var compareC = compareA + compareB;
            Console.WriteLine($"\nPudełko C: A + B = {compareC.ToString("m")}");
            Console.WriteLine($"\nA == B {compareA == compareB}");
            Console.WriteLine($"C != B {compareC != compareB}");
            Console.WriteLine($"C == A {compareC == compareB}");

            //Indexer
            Console.WriteLine("\nBoki pudełka C wypisane przez Indexer: ");
            Console.WriteLine($"Bok A: {compareC[0]}");
            Console.WriteLine($"Bok B: {compareC[1]}");
            Console.WriteLine($"Bok C: {compareC[2]}");

            //Create list of Pudelko using different contructors, parsing and conversion
            Console.WriteLine("\nNiesortowana lista (Obiekty zostały stworzone przy użycu wielu konstruktorów i konwersji niejawnej):");
            var (a, b, c) = (7967, 9879, 5686);
            List<Pudelko> boxList = new()
            {
                Pudelko.Parse("2 × 9,321 × 0,100"),
                Pudelko.Parse("200 cm"),
                Pudelko.Parse("2000 mm × 9321 mm × 100 cm"),
                Pudelko.Parse("200 cm × 932,1 cm"),
                Pudelko.Parse("7"),
                (a,b,c),
                new Pudelko(2222,2222,2313, UnitOfMeasure.milimeter),
                new Pudelko(123,434,334, UnitOfMeasure.centimeter),
                new Pudelko(2,4,8, UnitOfMeasure.meter),
                new Pudelko(124,235,UnitOfMeasure.milimeter),
                new Pudelko(456,346, UnitOfMeasure.centimeter),
                new Pudelko(3,7, UnitOfMeasure.meter),
                new Pudelko(2332, UnitOfMeasure.milimeter),
                new Pudelko(867, UnitOfMeasure.centimeter),
                new Pudelko(6, UnitOfMeasure.meter),
                new Pudelko(0.1, 0.1, 0.1, UnitOfMeasure.meter),
                new Pudelko(10, 10, 10, UnitOfMeasure.centimeter),
                new Pudelko(100, 100, 100, UnitOfMeasure.milimeter),
                new Pudelko(0.1, 0.1, 0.1),
                new Pudelko(0.1, 0.1),
                new Pudelko(0.1)
            };

            //Print the list using foreach loop            
            foreach (Pudelko p in boxList)
                Console.WriteLine(p.ToString());

            //Sort the list using delegate "Comparison"
            boxList.Sort(Comparison);

            //Print again sorted list
            Console.WriteLine("\nPosortowana lista: \n");
            foreach (Pudelko p in boxList)
                Console.WriteLine(p.ToString());

            //Quantity, compresing, conversion from Pudelko to double [] and using foreach to get dimensions
            var box = new Pudelko(400,200,800, UnitOfMeasure.centimeter);
            var boxCompressed = box.Kompresuj();
            Console.WriteLine($"\nObjętość pudełka źródłowego: {box.Objetosc},  boki (wypisane pętlą foreach: ");
            foreach (double d in box)
                Console.WriteLine(d);

            Console.WriteLine($"Objętość pudełka skompresowanego: {boxCompressed.Objetosc}, boki (konwertowane do tablicy double):");
            double[] doublesFromBoxCompressed = (double[])boxCompressed;
            foreach (double d  in doublesFromBoxCompressed)
                Console.WriteLine(d);
        }

        private static int Comparison(Pudelko p1, Pudelko p2)
        {
            if (p2 is null) //Null is assumed to be smallest
                return 1;
            else if (p1.Objetosc.CompareTo(p2.Objetosc) != 0)
                return p1.Objetosc.CompareTo(p2.Objetosc);
            else if (p1.Pole.CompareTo(p2.Pole) != 0)
                return p1.Pole.CompareTo(p2.Pole);
            else
                return (p1.A + p1.B + p1.C).CompareTo(p2.A + p2.B + p2.C);
        }
    }
}