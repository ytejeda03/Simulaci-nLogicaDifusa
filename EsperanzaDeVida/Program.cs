using System;
using System.Collections.Generic;

namespace EsperanzaDeVida
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Se correrán 3 casos de pruebas:");
            Console.WriteLine("Paciente 1: Adrián Fernández\n  Edad: 27, RCP: 33");
            var PacientRes = Simulate(27, 33);
            Console.WriteLine("Sus esperanzas de vida aplicando Mandani con Centroide es: {0}" , PacientRes.Item1);
            Console.WriteLine("Sus esperanzas de vida aplicando Larsen con Centroide es: {0}" , PacientRes.Item2);
            Console.WriteLine("Sus esperanzas de vida aplicando Mandani con Bisección es: {0}" , PacientRes.Item3);
            Console.WriteLine("Sus esperanzas de vida aplicando Larsen con Bisección es: {0}\n\n" , PacientRes.Item4);

            Console.WriteLine("Paciente 2: Mariana Lopez\n  Edad: 45, RCP: 15");
            PacientRes = Simulate(45, 15);
            Console.WriteLine("Sus esperanzas de vida aplicando Mandani con Centroide es: {0}", PacientRes.Item1);
            Console.WriteLine("Sus esperanzas de vida aplicando Larsen con Centroide es: {0}", PacientRes.Item2);
            Console.WriteLine("Sus esperanzas de vida aplicando Mandani con Bisección es: {0}", PacientRes.Item3);
            Console.WriteLine("Sus esperanzas de vida aplicando Larsen con Bisección es: {0}\n\n", PacientRes.Item4);

            Console.WriteLine("Paciente 3: Cristian Montez\n  Edad: 65, RCP: 30");
            PacientRes = Simulate(65, 30);
            Console.WriteLine("Sus esperanzas de vida aplicando Mandani con Centroide es: {0}", PacientRes.Item1);
            Console.WriteLine("Sus esperanzas de vida aplicando Larsen con Centroide es: {0}", PacientRes.Item2);
            Console.WriteLine("Sus esperanzas de vida aplicando Mandani con Bisección es: {0}", PacientRes.Item3);
            Console.WriteLine("Sus esperanzas de vida aplicando Larsen con Bisección es: {0}\n\n", PacientRes.Item4);

            while (true)
            {
                float age;
                float crp;
                Console.WriteLine("Si desea hacer una simulación introduzca la Edad del Paciente:");
                while (true)
                {
                    var ageText = Console.ReadLine();
                    if (float.TryParse(ageText, out age))
                    {
                        break;
                    }
                    Console.WriteLine("La edad debe ser un número, introduzca nuavemente:");
                }
                Console.WriteLine("Introduzca el CRP del paciente:");
                while (true)
                {
                    var crpText = Console.ReadLine();
                    if (float.TryParse(crpText, out crp))
                    {
                        break;
                    }
                    Console.WriteLine("El CRP debe ser un número, introduzca nuavemente:");
                }
                PacientRes = Simulate(age, crp);
                Console.WriteLine("Sus esperanzas de vida aplicando Mandani con Centroide es: {0}", PacientRes.Item1);
                Console.WriteLine("Sus esperanzas de vida aplicando Larsen con Centroide es: {0}", PacientRes.Item2);
                Console.WriteLine("Sus esperanzas de vida aplicando Mandani con Bisección es: {0}", PacientRes.Item3);
                Console.WriteLine("Sus esperanzas de vida aplicando Larsen con Bisección es: {0}\n\n", PacientRes.Item4);
            }
        }

        private static Tuple<float, float, float, float> Simulate(float x, float RCP)
        {
            List<LinguisticVariable> inputVars = new List<LinguisticVariable>();
            List<LinguisticVariable> outputVars = new List<LinguisticVariable>();
            List<Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>> ageConditions = new List<Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>>();
            ageConditions.Add(new Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>("Joven", delegate {
                float a = 0; float b = 30;
                if (x <= a || x > b)
                {
                    return 0;
                }
                var m = (a + b) / 2;
                if (x <= m)
                {
                    return (x - a) / (m - a);
                }
                else
                {
                    return (b - x) / (b - m);
                }
            }, null, 0, 30));
            ageConditions.Add(new Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>("Mediana Edad", delegate {
                float a = 25; float b = 55;
                if (x <= a || x > b)
                {
                    return 0;
                }
                var m = (a + b) / 2;
                if (x <= m)
                {
                    return (x - a) / (m - a);
                }
                else
                {
                    return (b - x) / (b - m);
                }
            }, null, 25, 55));
            ageConditions.Add(new Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>("Adulto Mayor", delegate {
                float a = 50; float b = 80;
                if (x <= a)
                {
                    return 0;
                }
                if (x >= b)
                {
                    return 1;
                }
                var m = (a + b) / 2;
                if (x <= m)
                {
                    return (x - a) / (m - a);
                }
                else
                {
                    return (b - x) / (b - m);
                }
            }, null, 50, 80));
            inputVars.Add(new LinguisticVariable("Edad", ageConditions, x, true));
            List<Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>> crpConditions = new List<Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>>();
            crpConditions.Add(new Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>("Bajo", delegate {
                float a = 0; float b = 20;
                if (RCP <= a || RCP > b)
                {
                    return 0;
                }
                var m = (a + b) / 2;
                if (RCP <= m)
                {
                    return (RCP - a) / (m - a);
                }
                else
                {
                    return (b - RCP) / (b - m);
                }
            }, null, 0, 20));
            crpConditions.Add(new Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>("Medio", delegate {
                float a = 15; float b = 30;
                if (RCP <= a || RCP > b)
                {
                    return 0;
                }
                var m = (a + b) / 2;
                if (RCP <= m)
                {
                    return (RCP - a) / (m - a);
                }
                else
                {
                    return (b - RCP) / (b - m);
                }
            }, null, 15, 30));
            crpConditions.Add(new Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>("Alto", delegate {
                float a = 30; float b = 50;
                if (RCP <= a)
                {
                    return 0;
                }
                if (RCP >= b)
                {
                    return 1;
                }
                var m = (a + b) / 2;
                if (RCP <= m)
                {
                    return (RCP - a) / (m - a);
                }
                else
                {
                    return (b - RCP) / (b - m);
                }
            }, null, 30, 50));
            inputVars.Add(new LinguisticVariable("CRP", crpConditions, RCP, true));
            List<Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>> lifeHopeConditions = new List<Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>>();
            lifeHopeConditions.Add(new Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>("Corta", null, delegate (float x) {
                float a = 0; float b = 20;
                if (x <= a || x > b)
                {
                    return 0;
                }
                var m = (a + b) / 2;
                if (x <= m)
                {
                    return (x - a) / (m - a);
                }
                else
                {
                    return (b - x) / (b - m);
                }
            }, 0, 20));
            lifeHopeConditions.Add(new Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>("Media", null, delegate (float x) {
                float a = 15; float b = 30;
                if (x <= a || x > b)
                {
                    return 0;
                }
                var m = (a + b) / 2;
                if (x <= m)
                {
                    return (x - a) / (m - a);
                }
                else
                {
                    return (b - x) / (b - m);
                }
            }, 15, 30));
            lifeHopeConditions.Add(new Tuple<string, LinguisticVariable.MembershipPertenence, LinguisticVariable.MembershipPertenenceOutput, float, float>("Larga", null, delegate (float x) {
                float a = 30; float b = 50;
                if (x <= a)
                {
                    return 0;
                }
                if (x >= b)
                {
                    return 1;
                }
                var m = (a + b) / 2;
                if (x <= m)
                {
                    return (x - a) / (m - a);
                }
                else
                {
                    return (b - x) / (b - m);
                }
            }, 30, 50));
            outputVars.Add(new LinguisticVariable("Esperanza de Vida", lifeHopeConditions, 0, false));
            List<FuzzyRule> fuzzyRules = new List<FuzzyRule>();
            fuzzyRules.Add(new FuzzyRule(new List<int> { 1 },
                                            new List<Tuple<string, string>> { new Tuple<string, string>("Edad", "Joven"), new Tuple<string, string>("CRP", "Bajo") },
                                            null,
                                            new List<Tuple<string, string>> { new Tuple<string, string>("Esperanza de Vida", "Corta") }));
            fuzzyRules.Add(new FuzzyRule(new List<int> { 1 },
                                            new List<Tuple<string, string>> { new Tuple<string, string>("Edad", "Adulto Mayor"), new Tuple<string, string>("CRP", "Medio") },
                                            null,
                                            new List<Tuple<string, string>> { new Tuple<string, string>("Esperanza de Vida", "Corta") }));
            fuzzyRules.Add(new FuzzyRule(new List<int> { 1 },
                                            new List<Tuple<string, string>> { new Tuple<string, string>("Edad", "Mediana Edad"), new Tuple<string, string>("CRP", "Medio") },
                                            null,
                                            new List<Tuple<string, string>> { new Tuple<string, string>("Esperanza de Vida", "Media") }));
            fuzzyRules.Add(new FuzzyRule(new List<int> { 1 },
                                            new List<Tuple<string, string>> { new Tuple<string, string>("Edad", "Joven"), new Tuple<string, string>("CRP", "Alto") },
                                            null,
                                            new List<Tuple<string, string>> { new Tuple<string, string>("Esperanza de Vida", "Larga") }));
            return FuzzyMethods.ExecuteMandaniAndLarsen(inputVars, outputVars, fuzzyRules);
        }
    }
}
