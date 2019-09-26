using KuuhakuFramework.ChoiceTree.Branched.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched.IO
{
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
