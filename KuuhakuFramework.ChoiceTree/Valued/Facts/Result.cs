using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Valued.Facts
{
    public sealed class Result
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Func<double, bool> Range { get; set; }

        public Result()
        {
            Name = string.Empty;
            Description = string.Empty;
            Range = (x) => false;
        }

        public Result(string name, string desc = null) : this()
        {
            Name = name;
            Description = desc ?? string.Empty;
        }

        public Result SetName(string value)
        {
            Name = value;
            return this;
        }

        public Result SetDescription(string value)
        {
            Description = value;
            return this;
        }


        /// <summary>
        /// Range irá receber valor menor igual à <paramref name="max"/> e maior igual a <paramref name="min"/>
        /// </summary>
        /// <param name="min">Menor valor da comparação inclusivamente</param>
        /// <param name="max">Maior valor da comparação inclusivamente</param>
        public Result SetRangeBetween(double min, double max)
        {
            Range = (x) => x >= min && x <= max;
            return this;
        }

        /// <summary>
        /// Range irá receber valor maior igual a <paramref name="min"/>
        /// </summary>
        /// <param name="min">Menor valor da comparação inclusivamente</param>
        public Result SetRangeGreater(double min)
        {
            Range = (x) => x >= min;
            return this;
        }

        /// <summary>
        /// Range irá receber valor menor igual a <paramref name="max"/>
        /// </summary>
        /// <param name="min">Menor valor da comparação inclusivamente</param>
        public Result SetRangeLess(double max)
        {
            Range = (x) => x <= max;
            return this;
        }
    }
}
