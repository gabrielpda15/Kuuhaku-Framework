using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Valued.Facts
{
    public interface IFact
    {
        string Text { get; set; }
        Reaction Next();
    }
}
