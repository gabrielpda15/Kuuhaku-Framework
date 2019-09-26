using KuuhakuFramework.ChoiceTree.Branched.Facts;
using KuuhakuFramework.Extensions;
using KuuhakuFramework.Security.Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuuhakuFramework.ChoiceTree.Branched.IO
{
    public abstract class ChoiceTreeFile
    {
        protected abstract char[] Magic { get; }
        protected abstract string Version { get; }
        protected abstract int PhraseLength { get; }
        protected int HeaderSize => Magic.Length + Version.Length + 1 + 8 + 1;

        public ChoiceTree ChoiceTree { get; protected set; }

        protected ChoiceTreeFile()
        {
            ChoiceTree = new ChoiceTree();
        }

        protected ChoiceTreeFile(ChoiceTree choiceTree)
        {
            ChoiceTree = choiceTree;
        }

        public void WriteBytes(string file)
        {
            if (File.Exists(file)) File.Delete(file);

            using (var buffer = File.Create(file))
            {
                using (var bw = new BinaryWriter(buffer, Encoding.UTF8, false))
                {
                    bw.Write(Magic);
                    bw.Write(Version);
                    var sizeOffset = buffer.Length;
                    bw.Write((long)0);
                    var phrase = Phraser.GenerateBytePhrase(length: PhraseLength);
                    bw.Write(phrase);

                    var choiceTree = ChoiceTree.Map(x => new
                    {
                        x.Name, x.Description, x.EntryPoint,
                        Choices = x.Choices.ForEach(y => y.Value.Map(new ChoiceDTO(y.Key)), new List<ChoiceDTO>()),
                        Events = x.Events.ForEach(y => y.Value.Map(new EventDTO(y.Key)), new List<EventDTO>()),
                        VarChoices = x.VarChoices.ForEach(y => y.Value.Map(new VarChoiceDTO(y.Key)), new List<VarChoiceDTO>()),
                        x.CustomVars
                    });

                    var text = JsonConvert.SerializeObject(choiceTree, Formatting.None);

                    bw.Write(Cryptor.EncryptBytes(text.FromUTF8(), phrase));

                    bw.Seek((int)sizeOffset, SeekOrigin.Begin);
                    bw.Write(buffer.Length);

                    bw.Flush();
                }
            }
        }

        public void ReadBytes(string file)
        {
            if (!File.Exists(file)) throw new FileNotFoundException("Não foi possivel localizar o arquivo designado", file);

            using (var buffer = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (var br = new BinaryReader(buffer, Encoding.UTF8))
                {
                    var magic = br.ReadChars(Magic.Length);
                    var version = br.ReadString();
                    var size = br.ReadInt64();

                    if (!magic.Compare(this.Magic) || version != this.Version) throw new NotSupportedException("Esse arquivo não é um arquivo valido!");
                    if (buffer.Length != size) throw new NotSupportedException("Arquivo corrompido!");

                    var phrase = br.ReadBytes(PhraseLength);

                    var data = br.ReadBytes((int)size - HeaderSize);
                    var json = Cryptor.DecryptBytes(data, phrase).ToUTF8();
                    var result = JsonConvert.DeserializeObject(json);
                }
            }
        }
    }
}
