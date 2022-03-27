using ConsoleApp2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Class1
    {
        static void abc02(string[] args) 
        {
            string [] name = new string[3];
            name[0] = "123";
            name[1] = "456";
            var t = string.Format("nihao1,{0},{1}", name[0], name[1]);
            string i = $"nihao,{name[0]}"; 
            Console.WriteLine(i);
            Console.ReadKey();

        }
    }
}
