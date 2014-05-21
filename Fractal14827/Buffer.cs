using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fractal14827
{
    class Buffer
    {
        
            private readonly int ancho;
            private readonly int altura;
            public int offset;
            public int[,] cache;
            
            public Buffer(int ancho, int altura)
            {
                cache = new int[ancho, altura];
                this.ancho = ancho;
                this.altura = altura;
            }

            public int this[int x, int y]
            {
                get { return cache[x, y]; }
                set { cache[x, y] = value; }
            }
        
    }
}
