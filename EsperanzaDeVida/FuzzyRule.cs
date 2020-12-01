using System;
using System.Collections.Generic;
using System.Text;

namespace EsperanzaDeVida
{
    public class FuzzyRule
    {
        public List<int> AntecedentsOperators { get; private set; }
        public List<int> ConsecuentsOperators { get; private set; }
        public List<Tuple<string, string>> AntecedentsMembers { get; private set; }
        public List<Tuple<string, string>> ConsecuentsMembers { get; private set; }
        public FuzzyRule(List<int> antecedentsOperators, List<Tuple<string, string>> antecedentsMembers, List<int> consecuentsOperators, List<Tuple<string, string>> consecuentsMembers)
        {
            this.AntecedentsMembers = antecedentsMembers;
            this.AntecedentsOperators = antecedentsOperators;
            this.ConsecuentsMembers = consecuentsMembers;
            this.ConsecuentsOperators = consecuentsOperators;
        }
    }
}
