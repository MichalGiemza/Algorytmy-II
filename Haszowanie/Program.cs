using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haszowanie
{
    class Program
    {
        static void Main(string[] args)
        {
            Zapelnienie();

            Console.ReadLine();
        }
        
        static void WypiszZapelnienieTablicy(int[] tab)
        {
            foreach (var i in tab)
            {
                Console.Write("{0} ", i);
            }
            Console.WriteLine();
        }

        static void Zapelnienie()
        {
            Random r = new Random();

            {   // int
                Hash<int> c = new Hash<int>(32);
                for (int i = 0; i < 50; i++)
                {
                    c.Insert(r.Next());
                }
                Console.WriteLine("\nZapełnienie dla typu int:");
                WypiszZapelnienieTablicy(c.GetArrayFill());
            }

            {   // double
                Hash<double> c = new Hash<double>(32);
                for (int i = 0; i < 50; i++)
                {
                    c.Insert(r.NextDouble());
                }
                Console.WriteLine("\nZapełnienie dla typu double:");
                WypiszZapelnienieTablicy(c.GetArrayFill());
            }
            
            {   // string
                Hash<string> c = new Hash<string>(32);
                byte[] b = new byte[20];
                for (int i = 0; i < 50; i++)
                {
                    r.NextBytes(b);
                    c.Insert(Encoding.ASCII.GetString(b));
                }
                Console.WriteLine("\nZapełnienie dla typu string:");
                WypiszZapelnienieTablicy(c.GetArrayFill());
            }
            
            {   // double[]
                Hash<double[]> c = new Hash<double[]>(32);
                double[] b = new double[20];
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < b.Length; j++)
                        b[j] = r.NextDouble();

                    c.Insert(b);
                }
                Console.WriteLine("\nZapełnienie dla typu double[]:");
                WypiszZapelnienieTablicy(c.GetArrayFill());
            }

            {   // object
                Hash<object> c = new Hash<object>(32);
                var objA = new object[] { null, null, null };
                for (int i = 0; i < 10; i++)
                {
                    c.Insert(r.Next());
                    c.Insert(double.NaN);
                    c.Insert(new object());
                    c.Insert(objA);
                    c.Insert(null);
                }
                Console.WriteLine("\nZapełnienie dla typu object:");
                WypiszZapelnienieTablicy(c.GetArrayFill());
            }

            {   // object[]
                Hash<object[]> c = new Hash<object[]>(32);
                object[] b = new object[20];
                for (int i = 0; i < 50; i++)
                {
                    // 5 x int
                    for (int j = 0; j < 5; j++)
                        b[j] = r.Next();
                    // 5 x double[3]
                    for (int j = 5; j < 10; j++) 
                        b[j] = new double[] { r.NextDouble(), r.NextDouble(), r.NextDouble() };

                    // 5 x string (stałe)
                    b[10] = "kot";
                    b[11] = "tok";
                    b[12] = "asgasdg asg asgasgawt qw qwg qrg qwg qwrg qg zvcth weqg qg qrer cwrg qg zvcth weqg qg qrer cwrg qg zvcth weqg qg qrer cwrg qg zvcth weqg qg qrer cwrg qg zvcth weqg qg qrer cwrg qg zvcth weqg qg qrer cwrg qg zvcth weqg qg qrer cwrg qg zvcth weqg qg qrer cwrg qg zvcth weqg qg qrer c";
                    b[13] = "ąęśćź";
                    b[14] = "";

                    // 5x inne (stałe)
                    b[15] = double.NaN;
                    b[16] = int.MaxValue;
                    b[17] = new object();
                    b[18] = new object[] { null, null, null};
                    b[19] = null;
                    
                    c.Insert(b);
                }
                Console.WriteLine("\nZapełnienie dla typu object[]:");
                WypiszZapelnienieTablicy(c.GetArrayFill());
            }
        }
    }
}
