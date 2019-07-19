using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkkaSample.Api.Deps
{
    public class CustomMouse : ICustomMouse
    {
        public void Move()
        {
            Console.WriteLine("The mouse was moved");
        }
    }
}
