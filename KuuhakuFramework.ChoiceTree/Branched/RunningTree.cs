using KuuhakuFramework.ChoiceTree.Branched.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched
{
    public sealed class RunningTree
    {
        public ChoiceTree ChoiceTree { get; }

        public Reaction PresentReaction { get; private set; }

        public IFact Present { get => ChoiceTree.GetFact(PresentReaction); }

        public T GetPresent<T>() where T : IFact
        {
            return (T)Present;
        }

        public void NextFact()
        {
            Past.Add(PresentReaction);
            PresentReaction = Present.Next();
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

        public bool? IsEndPoint { get => Present?.Equals(Reaction.CreateEndReaction()); }

        public void SetChoiceSelection(int index)
        {
            if (!(Present is Choice)) throw new InvalidOperationException("O objeto atual não é uma Choice");
            ((Choice)Present).Selection = index;
        }

        public void SetVarChoiceResult(string result)
        {
            if (!(Present is VarChoice)) throw new InvalidOperationException("O objeto atual não é uma VarChoice");
            ((VarChoice)Present).Result = result;
            ChoiceTree.AddCustomVar(((VarChoice)Present).VarName, result);
        }

        public string GetPresentText()
        {
            var toProcess = Present.Text;
            foreach (var key in ChoiceTree.CustomVars.Keys)
            {
                toProcess = toProcess.Replace($"%{key}%", ChoiceTree.CustomVars[key]);
            }
            return toProcess;
        }

        public FactType FactType
        {
            get
            {
                if (Present == null)
                    return FactType.End;
                else
                    return (FactType)Enum.Parse(typeof(FactType), Present.GetType().Name);
            }
        }
    }
}
