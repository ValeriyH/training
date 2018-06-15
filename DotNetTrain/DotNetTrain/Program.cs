using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Threading;
using LibraryTest.Logging;

namespace DotNetTrain
{
    //Can't be accessed outside, but visible by ilDasm
    internal class Util
    {
        int Get()
        {
            return 11;
        }
    }

    public class RandomCollection
    {
        [Obsolete("RandomEnumerator class implements IEnumerator interface, but not declare it")]
        public class RandomEnumerator
        {
            private Random rnd;
            private int position = 0;

            public RandomEnumerator()
            {
                rnd = new Random(DateTime.Now.Millisecond);
                Current = rnd.Next();
                Size = rnd.Next(0, Program.MAX_COLLECTION_SIZE);
            }

            public int Current { get; set; }
            public int Size { get; set; }

            public bool MoveNext()
            {
                Current = rnd.Next();
                return position++ < Size;
            }

            public void Reset()
            {
                position = 0;
            }
        }

        //GetEnumerator is defined in IEnumerable interface 
        public RandomEnumerator GetEnumerator()
        {
            return new RandomEnumerator();
        }
    }

    public class YieldCollection
    {
        public IEnumerator<int> GetEnumerator()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            int count = rnd.Next(1, 30);
            for (int i = 0; i < count; i++)
            {
                //Implement somekind of lambda function. for each next element the 'yield return' resumens and takes next element.
                yield return rnd.Next();
            }
        }
    }

    public class DeserializationTest
    {
        #region JSON input string
        static private string JSON_INPUT = @"
            {   ""kind"": ""plus#person"",
                ""etag"": ""\""MoxPKeu0NQD8g5Gtts3ebh50504/MiwH8xwMZWt1V_0zqJ8KOTZML4M\"""",
                ""gender"": ""male"",
                ""objectType"": ""person"",
                ""id"": ""100462969152161038349"",
                ""displayName"": ""Valeriy H"",
                ""name"": {
                ""familyName"": ""H"",
                ""givenName"": ""Valeriy""
                },
                  ""emails"": [
                    {
                      ""value"": ""firsOne"",
                      ""primary"": true
                    },
                    {
                      ""value"": ""secondOne"",
                      ""primary"": false
                    }
                  ],
                ""url"": ""https://plus.google.com/100462969152161038349"",
                ""image"": {
                ""url"": ""https://lh4.googleusercontent.com/-dny9qN0ykfw/AAAAAAAAAAI/AAAAAAAAABA/1xIn9pkDgyQ/photo.jpg?sz=50"",
                ""isDefault"": false
                },
                ""isPlusUser"": true,
                ""circledByCount"": 0,
                ""verified"": false
            }";
        #endregion

        class UserData
        {
            public string id = null;
            public string displayName = null;

            public class Name
            {
                public string familyName = null;
                public string givenName = null;
            };
            public Name name = null;
        }

        public static void Execute()
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                UserData data = serializer.Deserialize<UserData>(JSON_INPUT);
                string name = data.name.familyName;
                Console.WriteLine("Name: {0}", name);

                Type email_type = data.name.GetType();
                Console.WriteLine(email_type.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Dictionary<string, object> dict = serializer.DeserializeObject(JSON_INPUT) as Dictionary<string, object>;
            Console.WriteLine("verified: {0}", dict["verified"]);

            var emails = dict["emails"] as Array;
            if (emails != null)
            {
                //var emails = new List<Dictionary<string, string>> (emails_array);
                //Console.WriteLine(emails[0].First());
                //Console.WriteLine(emails[0]["value"]);
                //Console.WriteLine(emails[0]["primary"]);
            }
        }
    }

    public class DisposableTest: IDisposable
    {
        public void Execute()
        {
            Console.WriteLine("Executing");

        }

        public void Dispose()
        {
            Console.WriteLine("Disposing");
        }
    }

    public class VirtualTest
    {
        public abstract class A1
        {
            protected int data;
            protected A1(int data)
            {
                this.data = data;
            }
            abstract public int Get();
        }

        public class B1 : A1
        {
            public B1():base(10){}
            protected B1(int data):base(data){}
            override public int Get() { Console.WriteLine("Inside B1");  return data; }
        }

        public class C1 : B1
        {
            public C1():base(20){}
            override public int Get() { Console.WriteLine("Inside C1"); return data; }
        }


        public class A
        {
            public virtual int Get() {return 10;}
        }

        public class B : A
        {
            public override int Get() { return 20; }
        }

        public class C : A
        {
            public int Get() { return 20; }
        }

        public class D : A
        {
            new public int Get() { return 20; }
        }

        public class E: B
        {
            public override int Get() { return 30; }
        }

        public class F : D
        {
            public int Get() { return 40; }
        }

        sealed public class G : E
        {
            new public int Get() { return 50; }
        }

        //Following code will generated error. sealed - do not allow to inherent from class G
        //public class M : G
        //{
        //    public int Get() { return 40; }
        //}


        static public void Execute()
        {
            A a;

            a = new A();
            Console.WriteLine("A::Get callled {0}", a.Get());

            a = new B();
            Console.WriteLine("B::Get called {0}", a.Get());
            Type t = a.GetType();
            Console.WriteLine("(B) A.GetType() {0} == B = {1}", t, typeof(B) == t);

            a = new C();
            Console.WriteLine("A::Get callled {0}", a.Get());

            a = new D();
            Console.WriteLine("A::Get callled {0}", a.Get());

            a = new E();
            Console.WriteLine("E::Get callled {0}", a.Get());

            a = new F();
            Console.WriteLine("A::Get callled {0}", a.Get());

            a = new G();
            Console.WriteLine("E::Get callled {0}", a.Get());


            A1 a1;

            a1 = new B1();
            Console.WriteLine("B1::Get callled {0}", a1.Get());

            a1 = new C1();
            Console.WriteLine("C1::Get callled {0}", a1.Get());
        }
    }

    public class Indexsator
    {
        public string this[string str1, string str2]
        {
            get
            {
                return str1 + " " + str2;
            }
            set
            {
                //nothing todo here
            }
        }

        public int this[int i1, int i2]
        {
            get
            {
                return i1 + i2;
            }
        }
    }

    public class Generics
    {
        class  GBase
        {
            public static void SetData<T>(object data)
            {
                Console.WriteLine(data.GetType());
                Console.WriteLine("== B = {0}", typeof(T) == typeof(B));
                //data.val = 10;
            }
        }

        class G1C<T> : GBase where T : class
        {
            public void ShowMe()
            {
                Console.WriteLine("Class");
            }
        }

        class GA : G1C<A>
        {
             
        }

        //We can't have same name for class and struct generics
        //class G1<T> where T : struct
        //class G1<T> where T : class
        //will not compile
        class G1S<T> where T : struct
        {
            T value;
            public void ShowMe()
            {
                Console.WriteLine("Struct: {0}", value.GetType());
            }
        }

        class A
        {
            private A()
            { }

            public A Create()
            {
                return new A();
            }
        }

        class  B : GBase
        {
            public int val { get; set; }
        }

        static public void ExecuteTest()
        {
            G1S<int> g1i = new G1S<int>();
            g1i.ShowMe();
            //This one will not compile
            //G1C<int> g1c = new G1C<int>();

            //G1C<A> ga = new GA();
            G1C<A> ga = new G1C<A>();
            ga.ShowMe();

            GBase gb = ga;
            var type = gb.GetType();    // type == G1C<A>
            var t2 = type.GetGenericArguments(); //t2[0] == A
            var t3 = type.GetGenericTypeDefinition(); //t3 == G1C<T>

            Console.WriteLine("{0}", t2[0] == typeof(A));


            object b = new B();
            GBase.SetData<object>(b);

        }
    }

    public class StreamTest
    {

        static public void Excecute()
        {
            using (FileStream stream = File.Open("D:\\test.txt", FileMode.OpenOrCreate))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    string text = "Hello World!!";

                    //for (int i = 0; i < 255; i++)
                    //{
                    //    text += "Hello World!!";
                    //}

                    writer.Write(text);
                    writer.Close();
                }
                stream.Close();
            }
        }
    }

    //static class StringExtension
    //{
    //    static public string MyMethod(this string s)
    //    {
    //        return String.Format("My Method is added to String :) {0}", s);
    //    }

    //    static public string operator +(this string c1, string c2)
    //    {
    //        return "";
    //    }
    //}

    static public class ExtensionTest
    {
        static public void ExecuteTest()
        {
            string test = "Test";
            //Console.WriteLine(test.MyMethod());

            //var p; - Error. var should be always initiated
            var anonimus = new { Name = "Vasya", Age = 30 };
            //Note if we change 'var' to 'object' it will not work
            Console.WriteLine("{0} is {1} years old", anonimus.Name, anonimus.Age);
            Console.WriteLine(anonimus.GetType());
        }
    }

    static class LinqExtension
    {
        class Person
        {
            public string Name {get; set;}
            public int Age {get; set;}
            public string Street {get; set;}
        }

        static public void ExecuteTest()
        {
            var persons = new List<Person>();
            persons.Add(new Person(){Name = "Vasil", Age = 12, Street="Street 1"});
            persons.Add(new Person() { Name = "Koly", Age = 22, Street = "Street 2" });
            persons.Add(new Person() { Name = "Vania", Age = 21, Street = "Street 1" });

            var result1 = from person in persons where person.Age > 20 select String.Format("{0} is {1}. He lives at {2}", person.Name, person.Age, person.Street);
            foreach(string s in result1)
            {
                Console.WriteLine(s);
            }

            //it is the same Linq with modified results
            var result2 = persons.Where(it => it.Age > 20).Select(it => new {FullName = String.Format("{0} from {1}", it.Name, it.Street) });
            foreach (var it in result2)
            {
                Console.WriteLine("{0}", it.FullName);
            }

            var result3 = persons.Where(it => it.Age > 20);
            foreach (var it in result3)
            {
                Console.WriteLine("{0}", it.Name);
            }

        }
    }

    static class AsyncTest
    {
        static async Task<int> Wait()
        {
			//This function starts in MAIN thread
            Console.WriteLine("Waiting thread {0}. Start", Thread.CurrentThread.ManagedThreadId);
            //HttpClient web = new HttpClient();
            //string s = await web.GetStringAsync("http://microsoft.com");
            await Task.Run(() =>
                {
					//Creating working thread
                    Console.WriteLine("Task.Run thread {0}", Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(100);
	                
                });
			//But finished in WORKING thread
            Console.WriteLine("Waiting thread {0}. End", Thread.CurrentThread.ManagedThreadId);
            return 0;
        }

        static async void StartAsync()
        {
			//This function starts in MAIN thread
            Console.WriteLine("StartAsync thread {0}. Start", Thread.CurrentThread.ManagedThreadId);
            var task = Wait();
			//Do some stuff till task result is not requred
	        int res = await task; //await is entry point for working thread. Here the Main thread exit from this function.
			//But finished in WORKING thread
            Console.WriteLine("StartAsync thread {0}. End", Thread.CurrentThread.ManagedThreadId);
        }

        public static void ExecuteTest()
        {
			////////////////////////////////////////////////////////////////////////////////////////////
			// Taks workflow
			////////////////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("ExecuteTest thread {0}. Starting", Thread.CurrentThread.ManagedThreadId);
            StartAsync();
            Console.WriteLine("ExecuteTest thread {0}. Slepping", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
            Console.WriteLine("ExecuteTest thread {0}. Ending", Thread.CurrentThread.ManagedThreadId);


			////////////////////////////////////////////////////////////////////////////////////////////
			// Exception handling in task
			////////////////////////////////////////////////////////////////////////////////////////////

			Task task = Task.Factory.StartNew(() => { Thread.Sleep(1000); throw new InvalidDataException("This is test exception"); });
			//if exception caused during task.Wait, this exception is throwed to current(caller) thread.
			//If exception caused in task, but Wait (await) funcaion is not called, the task.Exception is set and task.IsFaulted is set

			task.Wait(300); //No exception, because it is not caused during waiting time
			Thread.Sleep(1000);

	        if (task.IsFaulted)
	        {
				//Exception caused in task. Task fails, but no exception is thrown to the caller thread
		        try
		        {
			        Console.WriteLine("Failed task with exception {0}", task.Exception.Message);
			        throw task.Exception.GetBaseException();
		        }
		        catch (InvalidDataException ex)
		        {
			        Console.WriteLine("Invalid exception. {0}", ex.Message);
		        }
		        catch (Exception ex)
		        {
					Console.WriteLine("General exception. {0}", ex.Message);
		        }
	        }

	        try
	        {
				//Fault task will generate exception
				task.Wait();
	        }
			catch (InvalidDataException ex)
			{
				Console.WriteLine("Invalid exception. {0}", ex.Message);
			}
	        catch (Exception ex)
	        {
				Console.WriteLine("task.Wait() generate exception: '{0}'. Base exception: '{1}'", ex.Message, ex.GetBaseException().Message);
	        }

        }
    }

    static class LoggingTest
    {
        public static void ExecuteTest()
        {
            var fileLogger = LogManager.GetLogger("TestLogger");
            fileLogger.Info("This logs appropriately.");

            var bogusLogger = LogManager.GetLogger("Bogus");
            bogusLogger.Info("This should go to a general log. Bogus is not a recognized appender");
            
            LogManager.Default.Info("Default logger");
        }
    }

    class Program
    {
        public const int MAX_COLLECTION_SIZE = 30;

        static void EnumerationTest()
        {
            var random = new RandomCollection();

            Console.WriteLine("Iterate for 1st time");
            int position = 0;
            foreach (int item in random)
            {
                Console.WriteLine("{0} {1}", position++, item);
            }
            Console.WriteLine();

            Console.WriteLine("Iterate for 2nd time");
            position = 0;
            foreach (int item in random)
            {
                Console.WriteLine("{0} {1}", position++, item);
            }
            Console.WriteLine();

            var it = random.GetEnumerator();
            Console.WriteLine(it.Current);
            it.Current = 10;
            Console.WriteLine(it.Current);

            Console.WriteLine("\nIterate yield");
            position = 0;
            var yieldCollection = new YieldCollection();
            foreach (int item in yieldCollection)
            {
                Console.WriteLine("{0} {1}", position++, item);
            }
        }


        static void ReflectionTest(string sTypeName)
        {
            try
            {
                Type type = Type.GetType(sTypeName);

                Console.WriteLine("Type name: {0}", type.FullName);
                Console.WriteLine("\tHasElementType = {0}", type.HasElementType);
                Console.WriteLine("\tIsAbstract = {0}", type.IsAbstract);
                Console.WriteLine("\tIsAnsiClass = {0}", type.IsAnsiClass);
                Console.WriteLine("\tIsArray = {0}", type.IsArray);
                Console.WriteLine("\tFields:");
                foreach (var field in type.GetFields())
                {
                    Console.WriteLine("\t\t{0}", field);
                }
                Console.WriteLine("\tMethods:");
                foreach (var method in type.GetMethods())
                {
                    Console.WriteLine("\t\t{0}", method);
                }
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("{0} is not valid type", sTypeName);
            }
        }

        private static void WebTest()
        {
            //Uri uriAddress = new Uri("http://localhost:63997/Account/ExternalLoginGoogleCallback?__provider__=google#1");
            Uri uriAddress = new Uri("http://localhost:63997/Account/ExternalLoginGoogleCallback");
            Console.WriteLine(uriAddress.Fragment);
            Console.WriteLine("Uri {0} the default port ", uriAddress.IsDefaultPort ? "uses" : "does not use");

            Console.WriteLine("The path of this Uri is {0}", uriAddress.GetLeftPart(UriPartial.Path));
            Console.WriteLine("The query with path of this Uri is {0}", uriAddress.GetLeftPart(UriPartial.Query));
            string quert = uriAddress.Query;
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(quert);
            Console.WriteLine("The query of this Uri is {0}", uriAddress.Query);
            Console.WriteLine("Hash code {0}", uriAddress.GetHashCode());

            System.Collections.Specialized.NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString.Add("field1", "value1");
            outgoingQueryString.Add("field2", "value2");
            string postdata = outgoingQueryString.ToString();
            Console.WriteLine("Posting {0}", postdata);
        }

        static void Main(string[] args)
        {
            List<int> tl = new List<int>();
            tl.Add(10);
            tl.Add(20);
            tl.Add(30);
            tl.Add(40);

            var en1 = tl.GetEnumerator();
            var en2 = en1;
            en1.MoveNext();
            Console.WriteLine(en1.Current);
            Console.WriteLine(en2.Current);

            bool b;

            en1 = tl.GetEnumerator();
            b = en1 == en2;

            b = en2.MoveNext();
            b = en2.MoveNext();
            b = en2.MoveNext();
            b = en2.MoveNext();
            b = en2.MoveNext();
            b = en2.MoveNext();
            b = en2.MoveNext();


            //EnumerationTest();

            //----------------

            //ReflectionTest("System.Int32");
            //ReflectionTest("System.Collections.ArrayList");
            //ReflectionTest("DotNetTrain.RandomCollection");
            //ReflectionTest("DotNetTrain.Program");
            //ReflectionTest("DotNetTrain.Program.ReflectionTest");

            //Console.WriteLine("\nAssembly Types:");
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //Type[] assemblyTypes = assembly.GetTypes();
            //foreach (Type t in assemblyTypes)
            //    Console.WriteLine("\t {0}", t.Name);

            //----------------

            //WebTest();
            //DeserializationTest.Execute();
            //using (DisposableTest test = new DisposableTest())
            //{
            //    test.Execute();
            //}
            //VirtualTest.Execute();
            //Console.WriteLine((new Indexsator())["hello", "world"]);
            //Console.WriteLine((new Indexsator())[1, 2]);

            //Generics.ExecuteTest();
            //StreamTest.Excecute();

            //Console.WriteLine(5.ToString());

            //ExtensionTest.ExecuteTest();
            //LinqExtension.ExecuteTest();
            //AsyncTest.ExecuteTest();

            //Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            //Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name);

            //AssemblyTest.ShowAssembly();
            //AssemblyTest.Test();


            LoggingTest.ExecuteTest();
        }
    }
}


