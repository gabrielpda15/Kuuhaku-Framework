using KuuhakuFramework.ChoiceTree.Branched.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched.IO
{
    public class ChoiceTreeDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Reaction EntryPoint { get; set; }
        public ChoiceDTO[] Choices { get; set; }
        public EventDTO[] Events { get; set; }
        public VarChoiceDTO[] VarChoices { get; set; }
        public IDictionary<string, string> CustomVars { get; set; }

        public ChoiceTreeDTO()
        {
            EntryPoint = null;
            Choices = Array.Empty<ChoiceDTO>();
            Events = Array.Empty<EventDTO>();
            VarChoices = Array.Empty<VarChoiceDTO>();
            CustomVars = new Dictionary<string, string>();
        }
    }

    public class ChoiceDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public IEnumerable<Choice.Answer> Answers { get; set; }

        public ChoiceDTO() { }
        public ChoiceDTO(int id) : this() { Id = id; }
    }

    public class EventDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Reaction Reaction { get; set; }

        public EventDTO() { }
        public EventDTO(int id) : this() { Id = id; }
    }

    public class VarChoiceDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Result { get; set; }
        public string VarName { get; set; }
        public Reaction Reaction { get; set; }

        public VarChoiceDTO() { }
        public VarChoiceDTO(int id) : this() { Id = id; }
    }
}
