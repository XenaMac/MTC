using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metra.Module.ServiceAlerts.Models
{
    class Program
    {
        static void Main(string[] args)
        {
            TemplateTests tt = new TemplateTests();
            Console.WriteLine("<<<<<<<<<<<<<<<<<<<<<<< Start Tests >>>>>>>>>>>>>>>>>>>>>");
            tt.TestAllTemplates();
            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>> End Tests <<<<<<<<<<<<<<<<<<<<<<<");

        }
    }
}
