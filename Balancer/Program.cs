using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Balancer
{
    class Program
    {
        static int[] couners = new int[100];
        static void MultiThreadTest(Balancer<int> balancer, int iterations)
        {
            if (iterations == 0)
            {
                while (true)
                {
                    //var it = balancer.AcquireNext();
                    var it = balancer.Min().Acquire();
                    Interlocked.Increment(ref couners[it.Get()]);
                    it.UpdateStatistic(it.Get()); //balancer item performance base on balancer index
                    //it.UpdateStatistic(10);   //all balancer items have same performance
                    Thread.Sleep(0);
                }
            }
            else
            {
                for (int i = 0; i < iterations; i++)
                {
                    var it = balancer.AcquireNext();
                    Interlocked.Increment(ref couners[it.Get()]);
                    it.UpdateStatistic(it.Get()); //balancer item performance base on balancer index
                    //it.UpdateStatistic(10);   //all balancer items have same performance
                    Thread.Sleep(0);
                }
            }
        }


        static void Main(string[] args)
        {

            var bl = new Balancer<string>( new List<string> { "server1", "server2" } );

            var id1 = bl.AcquireNext();
            var id2 = bl.AcquireNext();
            if (id1 == id2)
            {
                throw new Exception("Object not balancing. Cycle doesn't work");
            }

            id1.UpdateStatistic(4);
            id2.UpdateStatistic(10);
            string strFast = id1.Get();
            string strSlow = id2.Get();
            for (int i = 1; i < 3; i++ )
            {
                id1 = bl.AcquireNext();
                if (id1.Get() != strFast)
                {
                    //2 times returns id1. 2 times * 4 weight = 8 total < 10 weight for id2
                    throw new Exception("Object not balancing. Not received 2 times fastest one");
                }
            }

            id2 = bl.AcquireNext();
            if (id2.Get() != strSlow)
            {
                //3 times * 4 weight = 12 total > 10 weight for id 1, so id 1 should be returned
                throw new Exception("Object not balancing. Not back to balance");
            }

            //Linq version
            bl = new Balancer<string>(new List<string> { "server1", "server2" });
            id1 = bl.Min().Acquire();
            id2 = bl.Min().Acquire();
            if (id1 == id2)
            {
                throw new Exception("Object not balancing. Cycle doesn't work for linq");
            }
            bl.ToList().ForEach(it => Console.WriteLine(it.Get()));
            bl.Where(it=>it.Get() == "server1").ToList().ForEach(it => Console.WriteLine(it.Get()));

            var cbl = bl.Where(it => it.Get() == "server3").Min();


            int ctasks = 10;
            for (int tn = 0; tn < 10; tn++)
            {
                Task[] tasks = new Task[ctasks];
                var balans = new Balancer<int>(new List<int> { 1, 5, 10, 15, 20, 50 });
                int iter = 10000;
                for (int i = 0; i < ctasks; i++)
                {
                    tasks[i] = Task.Run(() => MultiThreadTest(balans, iter));
                }
                try
                {
                    Task.WaitAll(tasks);
                }
                catch (AggregateException ae)
                {
                    Console.WriteLine("One or more exceptions occurred: ");
                    foreach (var ex in ae.Flatten().InnerExceptions)
                        Console.WriteLine("   {0}", ex.Message);
                }

                Console.WriteLine("Test #{0}", tn);
                for (int i = 0; i < couners.Length; i++)
                {
                    if (couners[i] != 0)
                    {
                        Console.WriteLine("{0}:{1}", i, couners[i]);
                    }
                    couners[i] = 0;
                }
                Console.WriteLine();
            }

            bool loop = true;
            if (loop)
            {
                Task[] tasks = new Task[ctasks];
                var balans = new Balancer<int>(new List<int> {0, 1, 10, 20, 30, 40, 50, 60 });

                for (int i = 0; i < ctasks; i++)
                {
                    tasks[i] = Task.Run(() => MultiThreadTest(balans, 0));
                }
                while (!Console.KeyAvailable)
                {
                    Console.Clear();
                    for (int i = 0; i < couners.Length; i++)
                    {
                        if (couners[i] != 0)
                        {
                            var item = balans.Where( it => it.Get() == i ).First();
                            Console.WriteLine("{0}:{1}\tW:{2}\tT:{3}", i, couners[i], item.Weight, item.Total);
                        }
                    }
                    Console.WriteLine("Press any key to stop test");
                    Thread.Sleep(1000);
                }
            }

            Console.WriteLine("All tests passed");
        }
    }
}
