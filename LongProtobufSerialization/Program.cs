using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ProtoBuf;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace LongProtobufSerialization
{
    internal class Program
    {
        private static void Main()
        {
            using (var inputStream = new FileStream("data.bin", FileMode.Open, FileAccess.Read))
            {
                var root = Serializer.Deserialize<T1>(inputStream);
                if (root == null)
                {
                    throw new Exception("Failed to deserialize");
                }
                var subdata = root.P6?.Data?.P1 ?? new T8[0];
                if (subdata[0]?.P1 == null || subdata.All(x => x.P2 == null))
                {
                    throw new Exception("Failed to deserialize");
                }

                using (var outputStream = new FileStream($"{Guid.NewGuid():N}.bin", FileMode.CreateNew, FileAccess.Write))
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    Serializer.Serialize(outputStream, root);
                    sw.Stop();

                    Console.WriteLine($"Serialized in {sw.Elapsed}");
                }
            }
        }
    }

    public class T1
    {
        public T1(
            ulong p1,
            T2 p2,
            T3 p3,
            DataOrError<T7> p4,
            T3 p5,
            DataOrError<T6> p6,
            T4 p7,
            long p8)

        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
            P5 = p5;
            P6 = p6;
            P7 = p7;
            P8 = p8;
        }

        [ProtoMember(1)]
        public ulong P1 { get; }

        [ProtoMember(2)]
        public T2 P2 { get; }

        [ProtoMember(3)]
        public T3 P3 { get; }

        [ProtoMember(4)]
        public DataOrError<T7> P4 { get; }
        [ProtoMember(5)]
        public T3 P5 { get; }

        [ProtoMember(8)]
        public DataOrError<T6> P6 { get; }
        [ProtoMember(6)]
        public T4 P7 { get; }
        [ProtoMember(7)]
        public long P8 { get; }
    }

    public class T6
    {
        public T6(T8[] p1, T9? p2)
        {
            P1 = p1;
            P2 = p2;
        }

        [ProtoMember(1)]
        public T8[] P1 { get; }
        [ProtoMember(2)]
        public T9? P2 { get; }

        public enum T9
        {
            E1 = 0,
            E2 = 1
        }
    }

    public sealed class T8
    {
        public string P1 { get; }
        public T10[] P2 { get; }
        public T11[] P3 { get; }

        public bool P4 { get; }

        public T8(string p1, T10[] p2, T11[] p3, bool p4)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
        }
    }

    public class T11
    {
        [ProtoMember(1)]
        public string P1 { get; }
        [ProtoMember(2)]
        public T12[] P2 { get; }

        public T11(string p1, T12[] p2)
        {
            P1 = p1;
            P2 = p2;
        }

        [ProtoContract]
        public struct T12
        {
            [ProtoMember(1)]
            public ulong P3 { get; set; }
            [ProtoMember(2)]
            public string P4 { get; set; }
        }
    }

    public class T10
    {
        [ProtoMember(1)]
        public string P1 { get; }
        [ProtoMember(2)]
        public string[] P2 { get; }

        public T10(string p1, string[] p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }

    public class T7
    {
        [ProtoMember(1)]
        public bool P1 { get; }
        [ProtoMember(2)]
        public T13[] P2 { get; }
        [ProtoMember(3)]
        public ulong[] P3 { get; }
        [ProtoMember(4)]
        public ulong[] P4 { get; }
        [ProtoMember(5)]
        public ulong[] P5 { get; }
        [ProtoMember(6)]
        public ulong[] P6 { get; }
        [ProtoMember(7)]
        public ulong[] P7 { get; }
        [ProtoMember(8)]
        public ulong[] P8 { get; }

        public T7(
            bool p1,
            T13[] p2,
            ulong[] p3,
            ulong[] p4,
            ulong[] p5,
            ulong[] p6,
            ulong[] p7,
            ulong[] p8)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
            P5 = p5;
            P6 = p6;
            P7 = p7;
            P8 = p8;
        }
    }

    public class T13
    {
        public string P1 { get; private set; }
        public ulong P2 { get; private set; }

        public T13(string p1, ulong p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }

    public class T2
    {
        public T2(string p1, bool? p2)
        {
            P1 = p1;
            P2 = p2;
        }
        [ProtoMember(1)]
        public string P1 { get; }
        [ProtoMember(2)]
        public bool? P2 { get; }
    }

    public class T3
    {
        [ProtoMember(1)]
        public string P1 { get; private set; }
        [ProtoMember(2)]
        public T4 P2 { get; private set; }

        public T3(string p1, T4 p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }

    public class T4
    {
        [ProtoMember(1)]
        public T5 P1 { get; private set; }
        [ProtoMember(2)]
        public string P2 { get; private set; }

        public T4(T5 p1, string p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }

    public enum T5
    {
        E1 = 0,
        E2 = 1,
        E3 = 2,
        E4 = 3
    }

    public class DataOrError<T> where T : class
    {
        [ProtoMember(1)]
        public T Data { get; private set; }
        [ProtoMember(2)]
        public T4 Error { get; private set; }

        public DataOrError(T data, T4 error)
        {
            Data = data;
            Error = error;
        }
    }
}
