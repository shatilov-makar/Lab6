using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Program
    {
        const double h = 0.2;
        static void Main(string[] args)
        {
            var matA = new double[,]
            {
               {0.4, 0.2, 0, 0 },
               {0.2, 0.4, 0.2, 0 },
               {0, 0.2, 0.4, 0.2 },
               {0, 0, 0.2, 0.4 }
            };
            var ys = new double[] { 1.2, 2.2, 3.0, 4.0, 4.8, 5.8 };//ChooseParameters();
            var matB = GetYs(ys);
            var f = SolveEquations(matA, matB);
            var cs = new double[matB.Length + 1];
            for (var i = 1; i < cs.Length; i++)
                cs[i] = f[i - 1];
            cs[0] = 0;
            var ds = GetDs(cs);
            var bs = GetBs(cs, ys);
            var a_s = GetAs(ys);
            Console.WriteLine("Введите значение х,  1 < x < 2");
            var x = double.Parse(Console.ReadLine());
            var t = ChooseInerval(x);
            if (t == null)
            {
                Console.WriteLine("Введен недопустимый интервал");
                Console.ReadKey();
                return;
            }
            var j = (int)t[0];
            var l = (x - t[1]);
            var result = a_s[j] + bs[j] * l + cs[j] * l * l + ds[j] * l * l * l;
            Console.WriteLine($"x = {x}, y = {result}");
            Console.ReadKey();
        }


        static double[] ChooseInerval(double input)
        {
            double[] interval;
            if (input >= 2 || input <= 1.0)
                return null;
            if (input < 1.6)
            {
                if (input < 1.2)
                    interval = new double[] { 0, 1.0 };
                else
                {
                    if (input < 1.4)
                        interval = new double[] { 1, 1.2 };
                    else interval = new double[] { 2, 1.4 };
                }
            }
            else
            {
                if (input < 1.8)
                    interval = new double[] { 3, 1.6 };
                else interval = new double[] { 4, 1.8 }; ;
            }
            return interval;
        }

        static double[] GetYs(double[]ys)
        {
            var res = new double[ys.Length-2];
            for (var i = 2; i < ys.Length; i++)
                res[i-2] = 3 * (((ys[i] - ys[i - 1]) / h) - ((ys[i - 1] - ys[i - 2]) / h));
            return res;
        }

        static double[] GetAs(double[] ys)
        {
            var a_s = new double[ys.Length - 1];
            for (var i = 0; i < a_s.Length; i++)
                a_s[i] = ys[i];
            return a_s;
        }

        static double[] GetBs (double[]cs, double[]ys)
        {
            var bs = new double[cs.Length];
            for (var i = 0; i < cs.Length - 1; i++)
                bs[i] = ((ys[i+1] - ys[i]) / h) - h / 3 * (cs[i + 1] + 2 * cs[i]);
            var n = cs.Length - 1;
            bs[n] = ((ys[n] - ys[n - 1]) / h) - 2 / 3 * h * cs[n];
            return bs;

        }

        static double[] GetDs(double[]cs)
        {
            var ds = new double[cs.Length];
            for (var i = 0; i < cs.Length - 1; i++)
                ds[i] = (cs[i + 1] - cs[i]) / (3 * h);
            ds[cs.Length - 1] = -cs[cs.Length - 1] / (3 * h);
            return ds;
        }


        private static double[] ChooseParameters()
        {
            var parameters = new double[6];
            Console.WriteLine("Введите по порядку коэффициенты y");
            while (true)
            {
                var m = GetUserInput();
                if (m.Length == 6)
                {
                    for (var j = 0; j < 6; j++)
                        parameters[j] = m[j];
                    break;
                }
                ShowError();
            }
            return parameters;
        }

        static double[] GetUserInput()
        {
            return Console.ReadLine().Split(' ')
                           .Where(x => !string.IsNullOrWhiteSpace(x) && double.TryParse(x, out var d))
                           .Select(x => double.Parse(x))
                           .ToArray();
        }
        static void ShowError()
        {
            Console.WriteLine("Ошибка! Следуйте командам");
            Console.WriteLine();
        }

        static double[] SolveEquations(double[,] matA, double[] matB)
        {
            var N = matB.Length;
            var N1 = matB.Length - 1;
            var y = matA[0, 0];
            var a = new double[N];
            var B = new double[N];
            var matRes = new double[N];
            a[0] = -matA[0, 1] / y;
            B[0] = matB[0] / y;
            for (int i = 1; i < N1; i++)
            {
                y = matA[i, i] + matA[i, i - 1] * a[i - 1];
                a[i] = -matA[i, i + 1] / y;
                B[i] = (matB[i] - matA[i, i - 1] * B[i - 1]) / y;
            }
            matRes[N1] = (matB[N1] - matA[N1, N1 - 1] * B[N1 - 1]) / (matA[N1, N1] + matA[N1, N1 - 1] * a[N1 - 1]);
            for (int i = N1 - 1; i >= 0; i--)
                matRes[i] = a[i] * matRes[i + 1] + B[i];
            return matRes;
        }
    }
}
