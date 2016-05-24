using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Picky.Ioc;
using System.Diagnostics;
using Declare;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Factory.Register<IHello>(typeof(Hello));
            Factory.Register<ISub>(typeof(Sub));
            Factory.Register<ISub>(typeof(Sub2));
            Factory.Register<IDoIt>(AppDomain.CurrentDomain.BaseDirectory + "Implement.dll");
            var f = new Foo();
            var mapping = new Dictionary<string, string>();
            mapping["Test.IHello"] = "Test.Hello";
            mapping["Test.ISub"] = "Test.Sub2";
           
            for (var i = 0; i < 10; i++)
            {
                var sw = new Stopwatch();
                sw.Start();
                f.Build();
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
                f.Exc();
            }
            Console.ReadKey();
        }
    }

    public class Foo
    {
        public IHello hello;

        public IDoIt doIt;

        public void Exc()
        {
            hello.Say();
            doIt.Say();
        }
    }

    public interface IHello
    {
        void Say();
    }

    public class Hello : IHello
    {
        public ISub sub;
        public void Say()
        {
            Console.WriteLine("hello!");
            sub.SubSay();
        }
    }

    public interface ISub
    {
        void SubSay();
    }

    public class Sub : ISub
    {
        public void SubSay()
        {
            Console.WriteLine("你好 from sub!");
        }
    }

    public class Sub2:ISub
    {
        public void SubSay()
        {
            Console.WriteLine("你好 from sub2!");
        }
    }
}
