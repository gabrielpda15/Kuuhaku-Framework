﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched.Facts
{
    public sealed class Choice : IFact
    {
        public string Text { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public int Selection { get; set; }

        public Choice(string text)
        {
            Text = text;
            Answers = new List<Answer>();
            Selection = -1;
        }

        public AnswerAddEvent AddAnswer(string text)
        {
            return new AnswerAddEvent(this, text);
        }

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Choice choice)
            {
                if (this.Text == choice.Text && this.Selection == choice.Selection)
                {
                    if (this.Answers.Count() == choice.Answers.Count())
                    {
                        for (int i = 0; i < this.Answers.Count(); i++)
                        {
                            if (!this.Answers.ElementAt(i).Equals(choice.Answers.ElementAt(i)))
                                return false;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }

        public Reaction Next()
        {
            return Answers.ElementAt(Selection).Reaction;
        }

        public class AnswerAddEvent
        {
            private readonly Choice choice;
            private readonly string text;

            public AnswerAddEvent(Choice choice, string text)
            {
                this.choice = choice;
                this.text = text;
            }

            public Choice ToChoice(int link)
            {
                ((List<Answer>)choice.Answers).Add(new Answer()
                {
                    Text = text,
                    Reaction = Reaction.CreateChoiceReaction(link)
                });
                return choice;
            }

            public Choice ToVarChoice(int link)
            {
                ((List<Answer>)choice.Answers).Add(new Answer()
                {
                    Text = text,
                    Reaction = Reaction.CreateVarChoiceReaction(link)
                });
                return choice;
            }

            public Choice ToEvent(int link)
            {
                ((List<Answer>)choice.Answers).Add(new Answer()
                {
                    Text = text,
                    Reaction = Reaction.CreateEventReaction(link)
                });
                return choice;
            }

            public Choice ToEnd()
            {
                ((List<Answer>)choice.Answers).Add(new Answer()
                {
                    Text = text,
                    Reaction = Reaction.CreateEndReaction()
                });
                return choice;
            }
        }

        public class Answer
        {
            public string Text { get; set; }

            public Reaction Reaction { get; set; }

            public override string ToString()
            {
                return Text;
            }

            public override bool Equals(object obj)
            {
                if (obj != null && obj is Answer answer)
                {
                    return this.Text == answer.Text &&
                           this.Reaction.Equals(answer.Reaction);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Text.GetHashCode();
            }
        }
    }
}
