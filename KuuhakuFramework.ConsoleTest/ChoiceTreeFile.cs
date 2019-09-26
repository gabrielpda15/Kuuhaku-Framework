using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ConsoleTest
{
    public class ChoiceTreeFile : ChoiceTree.Branched.IO.ChoiceTreeFile
    {
        protected override char[] Magic => new char[] { 'C', 'T', 'F' };

        protected override string Version => "1.0.0";

        protected override int PhraseLength => 16;

        public ChoiceTreeFile() : base() { }
        public ChoiceTreeFile(ChoiceTree.Branched.ChoiceTree choiceTree) : base(choiceTree) { }
    }
}
