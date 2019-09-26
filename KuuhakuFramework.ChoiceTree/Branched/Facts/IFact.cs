using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched.Facts
{
    public interface IFact
    {
        string Text { get; set; }
        Reaction Next();
    }
}
