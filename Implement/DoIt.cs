using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Declare;

namespace Implement
{
    public class DoIt : IDoIt
    {
        public void Say()
        {
            Console.WriteLine("do it!");
        }
    }
}
