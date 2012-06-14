using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteroidsClone
{
    public class Projection
    {
        public float Min { get; private set; }
        public float Max { get; private set; }

        public Projection(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public bool Overlaps(Projection p)
        {
            if (p.Min < this.Min)
            {
                if (p.Max >= this.Min)
                {
                    return true;
                }

                else return false;
            }

            else
            {
                if (this.Max >= p.Min)
                {
                    return true;
                }

                else return false;
            }
        }
    }
}
