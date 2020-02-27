using System;
using System.Collections.Generic;
using System.Text.Json;

namespace main
{
    class Program
    {
        class OtherTestClass
        {
            int[] array1 = new int[] { 1, 3, 5, 7, 9 };

            List<string> list1 = new List<string>()
            {
                "carrot",
                "fox",
                "explorer"
            };
        }

        class TestClass
        {
            public bool field = true;

            private OtherTestClass f = new OtherTestClass();

            int intprop { get; set; } = 10;
        }

        static void Main(string[] args)
        {
            TestClass a = new TestClass();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            Console.WriteLine(JsonSerializer.Serialize(json.MyJSON.Serialize(a), options));
        }
    }
}
