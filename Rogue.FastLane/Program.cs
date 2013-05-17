using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nhonho
{
    class Program
    {
        [DllImport("msvcrt.dll", CallingConvention=CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);


        public unsafe static int UnsafeCompareTo2(byte[] self, byte[] other)
        {
            //if (self.Length < other.Length) { return -1; }

            //if (self.Length > other.Length) { return +1; }

            GCHandle selfHandle =
                GCHandle.Alloc(self, GCHandleType.Pinned);

            GCHandle otherHandle =
                GCHandle.Alloc(self, GCHandleType.Pinned);

            byte* selfPtr = (byte*)
                selfHandle.AddrOfPinnedObject().ToPointer();

            byte* otherPtr = (byte*)
                otherHandle.AddrOfPinnedObject().ToPointer();

            int length = self.Length;

            int comparison = 0;

            for (int index = 0; index < length; index++)
            {
                comparison =
                    (*selfPtr++).CompareTo((*otherPtr++));

                if (comparison != 0) { break; }
            }
            selfHandle.Free();

            return comparison;
        }

        public static int CompareTo(byte[] self, byte[] other)
        {
            //if (self.Length < other.Length) { return -1; }

            //if (self.Length > other.Length) { return +1; }

            int comparison = 0;

            for (int i = 0; i < self.Length && i < other.Length; i++)
            {
                if ((comparison = self[i].CompareTo(other[i])) != 0)
                { return comparison; }
            }
            return comparison;
        }

        public unsafe static int UnsafeCompareTo(byte[] self, byte[] other)
        {
            //if (self.Length < other.Length) { return -1; }

            //if (self.Length > other.Length) { return +1; }

            int n = self.Length;

            fixed (byte* selfPtr = self, otherPtr = other)
            {
                byte* ptr1 = selfPtr;
                byte* ptr2 = otherPtr;

                while (n-- > 0)
                {
                    int comparison;

                    if ((comparison = (*ptr1++).CompareTo(*ptr2++)) != 0)
                    {
                        return comparison;
                    }
                }   
            }
            return 0;
        }

        static void Main(string[] args)
        {
            byte[] b1 = { 12, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            byte[] b2 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 21 };
            Stopwatch watch = new Stopwatch();

            watch.Start();
            int result, power = 7;
            
            Console.WriteLine("iteractions: {0}", Math.Pow(10, power));

            for(long i = 0; i < Math.Pow(10, power); i++)
                result = CompareTo(b1, b2);
            watch.Stop();
            Console.WriteLine("safe = {0}", watch.Elapsed);
            watch.Restart();
            for (long i = 0; i < Math.Pow(10, power); i++)
                result = UnsafeCompareTo(b1, b2);
            watch.Stop();
            Console.WriteLine("unsafe1 = {0}", watch.Elapsed);
            watch.Restart();
            for (long i = 0; i < Math.Pow(10, power); i++)
                result = memcmp(b1, b2, b1.Length);
            watch.Stop();
            Console.WriteLine("memcmp = {0}", watch.Elapsed);
            //watch.Restart();
            //for (long i = 0; i < Math.Pow(10, power); i++)
            //    result = UnsafeCompareTo2(b1, b2);
            //watch.Stop();
            //Console.WriteLine("unsafe2 = {0}", watch.Elapsed);
            Console.ReadLine();
        }
    }
}
