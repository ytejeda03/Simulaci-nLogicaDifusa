using System;
using System.Collections.Generic;
using System.Text;

namespace EsperanzaDeVida
{
    public class LinguisticVariable
    {
        public string Name { get; private set; }
        private float val;
        private bool valPassed;
        public delegate float MembershipPertenence();
        public delegate float MembershipPertenenceOutput(float x);
        public List<Tuple<string, MembershipPertenence,MembershipPertenenceOutput, float, float>> Clasifications { get; private set; }
        public float Val { get => val; }
        public LinguisticVariable(string name, List<Tuple<string, MembershipPertenence, MembershipPertenenceOutput, float, float>> clasifications, float val, bool valPassed)
        {
            this.Name = name;
            this.Clasifications = clasifications;
            this.val = val;
            this.valPassed = valPassed;
        }
    }
}
