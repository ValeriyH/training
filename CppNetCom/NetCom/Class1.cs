using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace NetCom
{
    //https://www.codeproject.com/Articles/612604/Best-Practice-in-Writing-a-COM-Visible-Assembly-Cs


    [DataContract]
    [ComVisible(true)]
    public struct TICKSTRUCT
    {
        public TICKSTRUCT(double date, double price, long volume)
        {
            Date = date;
            Price = price;
            Volume = volume;
        }

        [DataMember]
        double Date;
        [DataMember]
        double Price;
        [DataMember]
        long Volume;
    }

    [ComVisible(true)]
    [Guid("67F6AA4C-A9A5-4682-98F9-15BDF2246A74")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMyClass
    {
        int GetData();
        int[] GetArray();
        TICKSTRUCT[] GetStructs();
    }

    [ComVisible(true)]
    [Guid("7884998B-0504-4CBE-9FF9-6BBAA9776188")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("MyNamespace.MyClass")]
    public class Class1 : IMyClass
    {
        public class COMException : Exception
        {
            public COMException(string messsage, int error)
                :base (messsage)
            {
                HResult = error;
            }
        }

        public int[] GetArray()
        {
            return new int[] { 1, 2, 3 };
        }

        public int GetData()
        {
            //throw new IndexOutOfRangeException("Out of range");
            //throw new ObjectNotFoundException("ObjectNotFoundException");
            //throw new COMException("COM Exception", 5);
            //throw new InvalidOperationException("Item not found");

            return 17;
        }

        public TICKSTRUCT[] GetStructs()
        {
            TICKSTRUCT[] result = new TICKSTRUCT[] { new TICKSTRUCT(10, 10, 10), new TICKSTRUCT(20, 20, 20) };
            return result;
        }
    }
}
