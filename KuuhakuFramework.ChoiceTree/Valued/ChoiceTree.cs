using KuuhakuFramework.ChoiceTree.Valued.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Valued
{
    public sealed class ChoiceTree
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Reaction EntryPoint { get; set; }

        public double Points { get; set; }

        public delegate double PointsCalcHandler(double current, double value);
        public PointsCalcHandler PointsCalc { get; set; }

        private int ChoiceOffset { get; set; }
        private int VarChoiceOffset { get; set; }

        public IDictionary<int, Choice> Choices { get; set; }
        public IList<Result> Results { get; set; }
        public IDictionary<int, VarChoice> VarChoices { get; set; }
        public IDictionary<string, string> CustomVars { get; set; }

        public ChoiceTree()
        {
            Init();
            Name = string.Empty;
            Description = string.Empty;
            Points = 0;
            PointsCalc = (value, @new) => value + @new;
        }

        public ChoiceTree(string name, string desc = null) : base()
        {
            Name = name;
            Description = desc == null ? string.Empty : desc;
        }

        public void SetPointsCalc(PointsCalcHandler func, double initialValue = 0)
        {
            Points = initialValue;
            PointsCalc = func;
        }

        private void Init()
        {
            Choices = new Dictionary<int, Choice>();
            Results = new List<Result>();
            VarChoices = new Dictionary<int, VarChoice>();
            CustomVars = new Dictionary<string, string>();

            EntryPoint = Reaction.CreateEndReaction();
            ChoiceOffset = 0;
            VarChoiceOffset = 0;
        }

        public int AddChoice(Choice choice)
        {
            Choices.Add(ChoiceOffset, choice);
            return ChoiceOffset++;
        }

        public void AddResult(Result result)
        {
            Results.Add(result);
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
                case FactType.End:
                    return null;
                default:
                    throw new Exception("Invalid fact type!");
            }
        }

        public Result GetResult()
        {
            var results = Results.Where(x => x.Range(Points));

            switch (results.Count())
            {
                case 0:
                    throw new ResultNotFoundException("Não existe um resultado para tal pontuação");
                case 1:
                    return results.Single();
                default:
                    throw new MultipleResultException("Existe muitos resultados para tal pontuação");
            }
        }

        public class ResultNotFoundException : KeyNotFoundException
        {
            public ResultNotFoundException(string message) : base(message) { }
        }

        public class MultipleResultException : Exception
        {
            public MultipleResultException(string message) : base(message) { }
        }
    }
}
