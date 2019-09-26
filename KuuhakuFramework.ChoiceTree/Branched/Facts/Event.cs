using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched.Facts
{
    public sealed class Event : IFact
    {
        public string Text { get; set; }

        public Reaction Reaction { get; set; }

        public Event(string text)
        {
            this.Text = text;
        }

        public Event ToChoice(int link)
        {
            Reaction = Reaction.CreateChoiceReaction(link);
            return this;
        }

        public Event ToEvent(int link)
        {
            Reaction = Reaction.CreateEventReaction(link);
            return this;
        }

        public Event ToVarChoice(int link)
        {
            Reaction = Reaction.CreateVarChoiceReaction(link);
            return this;
        }

        public Event ToEnd()
        {
            Reaction = Reaction.CreateEndReaction();
            return this;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Event @event)
            {
                return this.Text == @event.Text && this.Reaction.Equals(@event.Reaction);
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
