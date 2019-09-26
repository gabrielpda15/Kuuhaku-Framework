using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched.Facts
{
    public sealed class Reaction
    {
        public int Link { get; set; }

        public FactType Type { get; set; }

        public static Reaction CreateChoiceReaction(int link)
        {
            return new Reaction()
            {
                Link = link,
                Type = FactType.Choice
            };
        }

        public static Reaction CreateVarChoiceReaction(int link)
        {
            return new Reaction()
            {
                Link = link,
                Type = FactType.VarChoice
            };
        }

        public static Reaction CreateEventReaction(int link)
        {
            return new Reaction()
            {
                Link = link,
                Type = FactType.Event
            };
        }

        public static Reaction CreateEndReaction()
        {
            return new Reaction()
            {
                Link = -1,
                Type = FactType.End
            };
        }

        public override string ToString()
        {
            if (Type != FactType.End)
                return Type.ToString() + " at " + Link.ToString();
            return "End Point";
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Reaction reaction)
            {
                return this.Type == reaction.Type && this.Link == reaction.Link;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Link.GetHashCode() + Type.GetHashCode()).GetHashCode();
        }
    }
}
