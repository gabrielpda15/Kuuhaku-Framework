using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched.Facts
{
    public sealed class VarChoice : IFact
    {
        public string Text { get; set; }

        public string VarName { get; set; }
        public string Result { get; set; }

        public Reaction Reaction { get; set; }

        public VarChoice(string text, string var)
        {
            this.Text = text;
            this.VarName = var;
        }

        public VarChoice ToChoice(int link)
        {
            Reaction = Reaction.CreateChoiceReaction(link);
            return this;
        }

        public VarChoice ToVarChoice(int link)
        {
            Reaction = Reaction.CreateVarChoiceReaction(link);
            return this;
        }

        public VarChoice ToEvent(int link)
        {
            Reaction = Reaction.CreateEventReaction(link);
            return this;
        }

        public VarChoice ToEnd()
        {
            Reaction = Reaction.CreateEndReaction();
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is VarChoice varChoice)
            {
                return this.Text == varChoice.Text && this.Reaction.Equals(varChoice.Reaction);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        public override string ToString()
        {
            return Text;
        }

        public Reaction Next()
        {
            return Reaction;
        }
    }
}
