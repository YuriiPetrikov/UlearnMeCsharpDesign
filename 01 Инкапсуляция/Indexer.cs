// Вставьте сюда финальное содержимое файла Indexer.cs
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
	public class Indexer
	{
		public int Length { get; }
		public double[] Range1to4;
        readonly int start;
        public Indexer(double[] range1to4, int start, int length)
		{
            if (start < 0 || length < 0 || length + start > range1to4.Length) 
                throw new ArgumentException();
			
            this.Range1to4 = range1to4;
            this.start = start;
            Length = length;
        }

        public double this[int index]
        {
            get
            {
                if (index < 0 || index > Length - 1)
                    throw new IndexOutOfRangeException();
                return Range1to4[index + start];
            }
            set 
            {
                if (index < 0 || index > Length - 1)
                    throw new IndexOutOfRangeException();
                Range1to4[index + start] = value;
            }  
        }
    }
}
