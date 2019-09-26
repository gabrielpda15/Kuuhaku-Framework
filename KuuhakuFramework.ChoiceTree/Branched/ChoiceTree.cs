using KuuhakuFramework.ChoiceTree.Branched.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched
{
    public sealed class ChoiceTree
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Reaction EntryPoint { get; set; }

        private int ChoiceOffset { get; set; }
        private int EventOffset { get; set; }
        private int VarChoiceOffset { get; set; }

        public IDictionary<int, Choice> Choices { get; set; }
        public IDictionary<int, Event> Events { get; set; }
        public IDictionary<int, VarChoice> VarChoices { get; set; }
        public IDictionary<string, string> CustomVars { get; set; }

        public ChoiceTree()
        {
            Init();
            Name = string.Empty;
            Description = string.Empty;
        }

        public ChoiceTree(string name, string desc = null) : this()
        {
            Name = name;
            Description = desc ?? string.Empty;
        }

        private void Init()
        {
            Choices = new Dictionary<int, Choice>();
            Events = new Dictionary<int, Event>();
            VarChoices = new Dictionary<int, VarChoice>();
            CustomVars = new Dictionary<string, string>();

            EntryPoint = Reaction.CreateEndReaction();
            ChoiceOffset = 0;
            EventOffset = 0;
            VarChoiceOffset = 0;
        }

        public int AddChoice(Choice choice)
        {
            Choices.Add(ChoiceOffset, choice);
            return ChoiceOffset++;
        }

        public int AddEvent(Event @event)
        {
            Events.Add(EventOffset, @event);
            return EventOffset++;
        }

        public void AddCustomVar(string var, string value)
        {
            if (CustomVars.ContainsKey(var))
                CustomVars[var] = value;
            else
                CustomVars.Add(var, value);
        }

        public int AddVarChoice(VarChoice varChoice)
        {
            VarChoices.Add(VarChoiceOffset, varChoice);
            return VarChoiceOffset++;
        }

        public IFact GetFact(Reaction reaction)
        {
            switch (reaction.Type)
            {
                case FactType.Choice:
                    return Choices[reaction.Link];
                case FactType.VarChoice:
                    return VarChoices[reaction.Link];
                case FactType.Event:
                    return Events[reaction.Link];
                case FactType.End:
                    return null;
                default:
                    throw new Exception();
            }
        }
    }
}
