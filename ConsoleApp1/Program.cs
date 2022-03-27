using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        private static readonly object LOCK = new object();
        private static readonly object LOCK1 = new object();
       // static string lockstring = "Huang";
        static void Main(string[] args)
        {
            Console.WriteLine("当前主线程是： " + Thread.CurrentThread.ManagedThreadId);
            #region 1---Action异步执行
            /* Action<string> re = res =>
             {
                 Thread.Sleep(3000);
                 Console.WriteLine(res);
                 Console.WriteLine("子线程：" + Thread.CurrentThread.ManagedThreadId);
                  //Thread.Sleep(3000);
              };
             var num = 1;

             //异步回调函数，异步方法执行完成后调用。用于监控,记录日志什么的
             AsyncCallback aaa = (ire) =>
             {
                 Console.WriteLine(ire.AsyncState); //ire.AsyncState是BeginInvoke()的第三个参数
                  Console.WriteLine($"当前线程{ire.IsCompleted} : {Thread.CurrentThread.ManagedThreadId}");

             };

             var asyncresult = re.BeginInvoke("当前线程是：", aaa, null);//异步多线程
             //asyncresult.AsyncWaitHandle.WaitOne(); //信号量，等待异步执行完成（主线程阻塞）
             asyncresult.AsyncWaitHandle.WaitOne(1000); //阻塞当前线程，等待且最多等待1000毫秒-->(用来做超时控制的)  */
            #endregion
            #region 2---Func异步执行
            /*   AsyncCallback aaa = (ire) =>
               {
                   Console.WriteLine(ire.AsyncState); //ire.AsyncState是BeginInvoke()的第三个参数
                    Console.WriteLine($"当前线程{ire.IsCompleted} : {Thread.CurrentThread.ManagedThreadId}");
               };
               Func<int> fc = () =>
               {
                   int num = 0;
                   for (int i = 0; i < 1000000000; i++)
                   {
                       num++;
                   }
                   Console.WriteLine("当前子线程是：" + Thread.CurrentThread.ManagedThreadId);
                   return DateTime.Now.Day;
               };
               var asyncresult = fc.BeginInvoke(aaa, "回调函数参数"); //asyncresult 是描述异步操作的
               var asyncresult1 = fc.BeginInvoke((ire) =>
               {
                   Console.WriteLine(ire.AsyncState); //ire.AsyncState是BeginInvoke()的第三个参数
                    Console.WriteLine($"当前线程{ire.IsCompleted} : {Thread.CurrentThread.ManagedThreadId}");
                   Console.WriteLine(fc.EndInvoke(ire));
               }, "回调函数参数");                                  //asyncresult等同于asyncresult1
               Console.WriteLine("当前线程是aaa：" + Thread.CurrentThread.ManagedThreadId);
               var day = fc.EndInvoke(asyncresult); //EndInvoke是获取异步操作返回值的（但是会阻塞，直到异步操作完成）*/
            #endregion
            #region  3---Task
            /* Action ac = new Action(
                () =>
                {
                    Console.WriteLine("this is Task start: " + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(2000);
                    Console.WriteLine("this is Task end: " + Thread.CurrentThread.ManagedThreadId);
                });
            Task ta = new Task(ac);
            //ta.Start(); 
            //Task.Run(ac); 
            List<Task> tasklist = new List<Task>();
            tasklist.Add(Task.Run(() => { Console.WriteLine("一心一意:" + Thread.CurrentThread.ManagedThreadId); }));
            tasklist.Add(Task.Run(() => { Thread.Sleep(500); Console.WriteLine("二话不说:" + Thread.CurrentThread.ManagedThreadId); }));
            tasklist.Add(Task.Run(() => { Thread.Sleep(800); Console.WriteLine("三从四德:" + Thread.CurrentThread.ManagedThreadId); }));
            tasklist.Add(Task.Run(() => { Thread.Sleep(1000); Console.WriteLine("四面八方:" + Thread.CurrentThread.ManagedThreadId); }));       */

            /* //阻塞当前主线程，等待任一异步方法执行完成
            Task.WaitAny(tasklist.ToArray());
            Console.WriteLine("某个完成。。。");
            //阻塞当前主线程，等待全部异步方法执行完成
            Task.WaitAll(tasklist.ToArray());*/

            /*TaskFactory tf = new TaskFactory();
            tf.ContinueWhenAny(tasklist.ToArray(), (taq) => { Console.WriteLine("ContinueWhenAny:" + Thread.CurrentThread.ManagedThreadId); });
            tf.ContinueWhenAll(tasklist.ToArray(), (taq)=> { Console.WriteLine("ContinueWhenAll:" + Thread.CurrentThread.ManagedThreadId); }); */

            //---1---首次进来，i=0,进入异步操作，非阻塞，所以向下执行，i++,然后循环第二次进来，同样异步操作，i++,以此类推，i加到5的过程中，
            //异步操作不知道i加到多少了，所以打印都不是顺序的
            /* for (int i = 0; i < 5; i++)
             {
                 int k = i;
                 Task.Run(()=> 
                 {
                     Console.WriteLine($"this is {i},{k} start {Thread.CurrentThread.ManagedThreadId}");
                     Thread.Sleep(2000);
                     Console.WriteLine($"this is {i},{k  } end {Thread.CurrentThread.ManagedThreadId}");
                 });
                 Console.WriteLine($"此时线程是： {Thread.CurrentThread.ManagedThreadId}");
             }*/

            //---2---
            /* List<int> li = new List<int>();
             for (int i = 0; i < 1000; i++)
             {
                 //数据会丢失，不到1000个（list队列是数组结构，在内存上，数据是连续摆放的，在异步操作的时候，可能是同时写入一个下标(cpu同时处理写入这个下标)，就会在成数据丢失）
                 Task.Run(() =>
                 {
                     li.Add(i);
                 });
             }*/
            /*List<int> li = new List<int>();
            for (int i = 0; i < 1000; i++)
            {
                Task.Run(() =>
                {
                    //Monitor.Enter();
                    lock (LOCK)
                    {
                        li.Add(i);
                    }
                    //Monitor.Exit();
                });
            }
            Thread.Sleep(5000);
            Console.WriteLine(li.Count);*/

            //---3---lock(string类型)
            //锁同一个变量，不能并发，如下两个循环，一个结束，另一个才能开始（例如end1结束，start开始）
            /* for (int i = 0; i < 5; i++)
             {
                 Task.Run(() =>
                 {
                     lock (LOCK)
                     {
                         Console.WriteLine($"this is {i}  lockstring start: {Thread.CurrentThread.ManagedThreadId}");
                         Thread.Sleep(1000);
                         Console.WriteLine($"this is {i}  lockstring end:  {Thread.CurrentThread.ManagedThreadId}");
                     }
                 });
             }
             for (int i = 0; i < 5; i++)
             {
                 Task.Run(() =>
                 {
                     lock (LOCK)
                     {
                         Console.WriteLine($"this is {i}  lockstring1 start1: {Thread.CurrentThread.ManagedThreadId}");
                         Thread.Sleep(1000);
                         Console.WriteLine($"this is {i}  lockstring1 end1:  {Thread.CurrentThread.ManagedThreadId}");
                     }
                 });
             }*/

            //---4---锁不同变量，可以两个循环都同时开始
            /*   for (int i = 0; i < 5; i++)
               {
                   Task.Run(() =>
                   {
                       lock (LOCK)
                       {
                           Console.WriteLine($"this is {i}  lockstring start: {Thread.CurrentThread.ManagedThreadId}");
                           Thread.Sleep(1000);
                           Console.WriteLine($"this is {i}  lockstring end:  {Thread.CurrentThread.ManagedThreadId}");
                       }
                   });
               }
               for (int i = 0; i < 5; i++)
               {
                   Task.Run(() =>
                   {
                       lock (LOCK1)
                       {
                           Console.WriteLine($"this is {i}  lockstring1 start1: {Thread.CurrentThread.ManagedThreadId}");
                           Thread.Sleep(1000);
                           Console.WriteLine($"this is {i}  lockstring1 end1:  {Thread.CurrentThread.ManagedThreadId}");
                       }
                   });
               }*/

            #endregion
            #region 4---async/await


            #endregion

            Console.WriteLine("主动跳出：" + Thread.CurrentThread.ManagedThreadId);
            Console.ReadKey();
        }
    }
    class Student
    {
        public void iii()
        {
            
        }

        /// <summary>
        /// 1--->Name属性
        /// </summary>
        private string name;  //关联字段  ，等价于：public string name{get; set; }    ,  get , set 访问器
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        /// <summary>
        ///2---> 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        // private string[] age = new string[3] { "20", "21", "22" };
        public string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 20: return "20岁"; 
                    case 21: return "21岁"; 
                    case 22: return "22岁"; 
                    default: return "没有合适的";
                }
            }
        }
    }
}


