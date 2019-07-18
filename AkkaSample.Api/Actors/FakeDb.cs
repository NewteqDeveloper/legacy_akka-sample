using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaSample.Api.Actors
{
    public static class FakeDb
    {
        public static List<string> Rows { get; set; }

        static FakeDb()
        {
            Rows = new List<string>();
        }
    }
}
