using KuuhakuFramework.ChoiceTree.Valued.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Valued
{
    public sealed class RunningTree
    {
        public ChoiceTree ChoiceTree { get; }

        public Reaction PresentReaction { get; private set; }

        public IFact Current { get => ChoiceTree.GetFact(PresentReaction); }

        public T GetPresent<T>() where T : IFact
        {
            return (T)Current;
        }

        public void NextFact()
        {
            Past.Add(PresentReaction);
            PresentReaction = Current.Next();
        }

        public IList<Reaction> Past { get; }

        public RunningTree()
        {
            this.ChoiceTree = new ChoiceTree();
            PresentReaction = Reaction.CreateEndReaction();
            Past = new List<Reaction>();
        }

        public RunningTree(ChoiceTree story)
        {
            this.ChoiceTree = story;
            PresentReaction = ChoiceTree.EntryPoint;
            Past = new List<Reaction>();
        }

        public void Load(IList<Reaction> past, Reaction present)
        {
            foreach (var item in past)
                this.Past.Add(item);
            this.PresentReaction = present;
        }

        public bool? IsEndPoint { get => Current?.Equals(Reaction.CreateEndReaction()); }

        public void SetChoiceSelection(int index)
        {
            if (!(Current is Choice)) throw new InvalidOperationException("O objeto atual não é uma Choice");
            ((Choice)Current).Selection = index;

            var value = ((Choice)Current).Answers.ElementAt(index).Value;
            ChoiceTree.Points = ChoiceTree.PointsCalc.Invoke(ChoiceTree.Points, value);            
        }

        public void SetVarChoiceResult(string result)
        {
            if (!(Current is VarChoice)) throw new InvalidOperationException("O objeto atual não é uma VarChoice");
            ((VarChoice)Current).Result = result;
            ChoiceTree.AddCustomVar(((VarChoice)Current).VarName, result);
        }

        public string GetPresentText()
        {
            var toProcess = Current.Text;
            foreach (var key in ChoiceTree.CustomVars.Keys)
            {
                toProcess = toProcess.Replace($"%{key}%", ChoiceTree.CustomVars[key]);
            }
            return toProcess;
        }

        public Result GetResult()
        {
            return ChoiceTree.GetResult();
        }

        public FactType FactType
        {
            get
            {
                if (Current == null)
                    return FactType.End;
                else
                    return (FactType)Enum.Parse(typeof(FactType), Current.GetType().Name);
            }
        }
    }
}
