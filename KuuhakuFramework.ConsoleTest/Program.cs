using KuuhakuFramework.ChoiceTree.Branched.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var choiceTree = new ChoiceTree.Branched.ChoiceTree("Teste", "Um testiculo muito fofo");

            var trab = choiceTree.AddEvent(new Event("Burguesia, Burguesia, Burguesiaaaaaaaaa").ToEnd());
            var vagaba = choiceTree.AddEvent(new Event("Bora joga lol então!").ToEnd());

            var choice1 = choiceTree.AddChoice(new Choice("Você é o que?")
                            .AddAnswer("Estudante").ToEnd()
                            .AddAnswer("Trabalhador Brasileiro").ToEvent(trab)
                            .AddAnswer("Vagabundo").ToEvent(vagaba));
                
            choiceTree.EntryPoint = Reaction.CreateVarChoiceReaction(choiceTree.AddVarChoice(new VarChoice("Qual o seu nome?", "name").ToChoice(choice1)));

            var file = new ChoiceTreeFile(choiceTree);

            file.WriteBytes("temp.dat");*/

            var file = new ChoiceTreeFile();
            file.ReadBytes("temp.dat");
        }
    }
}
