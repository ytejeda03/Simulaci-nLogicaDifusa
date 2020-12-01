using System;
using System.Collections.Generic;
using System.Text;

namespace EsperanzaDeVida
{
    public static class FuzzyMethods
    {
        public static Tuple<float, float, float, float> ExecuteMandaniAndLarsen(List<LinguisticVariable> inputVars, List<LinguisticVariable> outputVars, List<FuzzyRule> rules)
        {
            var fuzzyOutPut = GetFuzzyDomine(inputVars, outputVars, rules);
            var result = ExecuteCentroidMandaniAndLarsen(fuzzyOutPut);
            return result;
        }

        private static Tuple<float, float, float, float> ExecuteCentroidMandaniAndLarsen(List<Tuple<LinguisticVariable, LinguisticVariable.MembershipPertenenceOutput, string, float, float, float>> fuzzyOutPut)
        {
            Dictionary<float, float> fuzzyIntervalMandani = new Dictionary<float, float>();
            Dictionary<float, float> fuzzyIntervalLarsen = new Dictionary<float, float>();
            float minA = float.MaxValue;
            float maxA = float.MinValue;
            foreach (var item in fuzzyOutPut)
            {
                if (item.Item6 == 0)
                {
                    continue;
                }
                var a = item.Item4;
                if (minA > a)
                {
                    minA = a;
                }
                if (maxA < item.Item5)
                {
                    maxA = item.Item5;
                }
                while (a <= item.Item5)
                {
                    if (!fuzzyIntervalMandani.ContainsKey(a))
                    {
                        fuzzyIntervalMandani.Add(a, Math.Min(item.Item2.Invoke(a), item.Item6));
                        fuzzyIntervalLarsen.Add(a, Math.Min(item.Item2.Invoke(a) * item.Item6, item.Item6));
                    }
                    else
                    {
                        if (Math.Min(item.Item2.Invoke(a), item.Item6) > fuzzyIntervalMandani[a])
                        {
                            fuzzyIntervalMandani[a] = Math.Min(item.Item2.Invoke(a), item.Item6);
                        }
                        if (Math.Min(item.Item2.Invoke(a) * item.Item6, item.Item2.Invoke(a)) > fuzzyIntervalLarsen[a])
                        {
                            fuzzyIntervalLarsen[a] = Math.Min(item.Item2.Invoke(a) * item.Item6, item.Item6);
                        }
                    }
                    a += 1;
                }
            }
            float MandaniSumFxTimesX = 0;
            float LarsenSumFxTimesX = 0;
            float MandaniSumFx = 0;
            float LarsenSumFx = 0;
            float intervalA = minA;
            while(intervalA <= maxA)
            {
                if (fuzzyIntervalMandani.ContainsKey(intervalA))
                {
                    MandaniSumFxTimesX += (intervalA * fuzzyIntervalMandani[intervalA]);
                    MandaniSumFx += fuzzyIntervalMandani[intervalA];
                }
                if (fuzzyIntervalLarsen.ContainsKey(intervalA))
                {
                    LarsenSumFxTimesX += (intervalA * fuzzyIntervalLarsen[intervalA]);
                    LarsenSumFx += fuzzyIntervalLarsen[intervalA];
                }
                intervalA += 1;
            }
            intervalA = minA + (float)1;
            if (!fuzzyIntervalMandani.ContainsKey(minA))
            {
                fuzzyIntervalMandani.Add(minA, 0);
            }
            if (!fuzzyIntervalLarsen.ContainsKey(minA))
            {
                fuzzyIntervalLarsen.Add(minA, 0);
            }
            while (intervalA <= maxA)
            {
                if (!fuzzyIntervalMandani.ContainsKey(intervalA))
                {
                    fuzzyIntervalMandani.Add(intervalA, fuzzyIntervalMandani[intervalA-(float)1]);
                }
                else
                {
                    fuzzyIntervalMandani[intervalA] += fuzzyIntervalMandani[intervalA - (float)1];
                }
                if (!fuzzyIntervalLarsen.ContainsKey(intervalA))
                {
                    fuzzyIntervalLarsen.Add(intervalA, fuzzyIntervalLarsen[intervalA - (float)1]);
                }
                else
                {
                    fuzzyIntervalLarsen[intervalA] += fuzzyIntervalLarsen[intervalA - (float)1];
                }
                intervalA += (float)1;
            }
            intervalA = minA;
            float currentMandani = 0;
            float currentMandaniPos = minA;
            bool mandaniFounded = false;
            float currentLarsen = 0;
            float currentLarsenPos = minA;
            bool larsenFounded = false;
            while (intervalA <= maxA)
            {
                if (!mandaniFounded)
                {
                    if (fuzzyIntervalMandani[intervalA] < (fuzzyIntervalMandani[maxA]/2))
                    {
                        currentMandani = fuzzyIntervalMandani[intervalA];
                        currentMandaniPos = intervalA;
                    }
                    else
                    {
                        if (!((fuzzyIntervalMandani[maxA]/2 - currentMandani < fuzzyIntervalMandani[maxA] / 2 - fuzzyIntervalMandani[intervalA])))
                        {
                            currentMandani = fuzzyIntervalMandani[intervalA];
                            currentMandaniPos = intervalA;
                        }
                        mandaniFounded = true;
                    }
                }
                if (!larsenFounded)
                {
                    if (fuzzyIntervalLarsen[intervalA] < (fuzzyIntervalLarsen[maxA]/2))
                    {
                        currentLarsen = fuzzyIntervalLarsen[intervalA];
                        currentLarsenPos = intervalA;
                    }
                    else
                    {
                        if (!((fuzzyIntervalLarsen[maxA]/2 - currentLarsen < fuzzyIntervalLarsen[maxA] / 2 - fuzzyIntervalLarsen[intervalA])))
                        {
                            currentLarsen = fuzzyIntervalLarsen[intervalA];
                            currentLarsenPos = intervalA;
                        }
                        larsenFounded = true;
                    }
                }
                intervalA += (float)1;
            }
            return new Tuple<float, float, float, float>(MandaniSumFxTimesX/MandaniSumFx, LarsenSumFxTimesX/LarsenSumFx, currentMandaniPos, currentLarsenPos);
        }

        private static List<Tuple<LinguisticVariable, LinguisticVariable.MembershipPertenenceOutput, string, float, float, float>> GetFuzzyDomine(List<LinguisticVariable> inputVars, List<LinguisticVariable> outputVars, List<FuzzyRule> rules)
        {
            var fuzzyOutPut = new List<Tuple<LinguisticVariable, LinguisticVariable.MembershipPertenenceOutput, string, float, float, float>>();
            foreach (var rule in rules)
            {
                for (int i = 0; i < rule.AntecedentsOperators.Count; i++)
                {
                    float current = 0;
                    if (rule.AntecedentsOperators.Count > 0)
                    {
                        if (rule.AntecedentsOperators[i] == 1)
                        {
                            foreach (var item in inputVars)
                            {
                                if (item.Name == rule.AntecedentsMembers[i].Item1)
                                {
                                    foreach (var condition in item.Clasifications)
                                    {
                                        if (condition.Item1 == rule.AntecedentsMembers[i].Item2)
                                        {
                                            current = condition.Item2.Invoke();
                                            break;
                                        }
                                    }
                                }
                            }
                            foreach (var item in inputVars)
                            {
                                if (item.Name == rule.AntecedentsMembers[i + 1].Item1)
                                {
                                    foreach (var condition in item.Clasifications)
                                    {
                                        if (condition.Item1 == rule.AntecedentsMembers[i + 1].Item2)
                                        {
                                            if (current < condition.Item2.Invoke())
                                            {
                                                current = condition.Item2.Invoke();
                                            }
                                            foreach (var outPutMember in rule.ConsecuentsMembers)
                                            {
                                                foreach (var outPutVar in outputVars)
                                                {
                                                    if (outPutMember.Item1 == outPutVar.Name)
                                                    {
                                                        foreach (var clasification in outPutVar.Clasifications)
                                                        {
                                                            if (clasification.Item1 == outPutMember.Item2)
                                                            {
                                                                fuzzyOutPut.Add(new Tuple<LinguisticVariable, LinguisticVariable.MembershipPertenenceOutput, string, float, float, float>(outPutVar, clasification.Item3, clasification.Item1, clasification.Item4, clasification.Item5, current));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in inputVars)
                            {
                                if (item.Name == rule.AntecedentsMembers[i].Item1)
                                {
                                    foreach (var condition in item.Clasifications)
                                    {
                                        if (condition.Item1 == rule.AntecedentsMembers[i].Item2)
                                        {
                                            current = condition.Item2.Invoke();
                                            break;
                                        }
                                    }
                                }
                            }
                            foreach (var item in inputVars)
                            {
                                if (item.Name == rule.AntecedentsMembers[i + 1].Item1)
                                {
                                    foreach (var condition in item.Clasifications)
                                    {
                                        if (condition.Item1 == rule.AntecedentsMembers[i + 1].Item2)
                                        {
                                            if (current > condition.Item2.Invoke())
                                            {
                                                current = condition.Item2.Invoke();
                                            }
                                            foreach (var outPutMember in rule.ConsecuentsMembers)
                                            {
                                                foreach (var outPutVar in outputVars)
                                                {
                                                    if (outPutMember.Item1 == outPutVar.Name)
                                                    {
                                                        foreach (var clasification in outPutVar.Clasifications)
                                                        {
                                                            if (clasification.Item1 == outPutMember.Item2)
                                                            {
                                                                fuzzyOutPut.Add(new Tuple<LinguisticVariable, LinguisticVariable.MembershipPertenenceOutput, string, float, float, float>(outPutVar, clasification.Item3, clasification.Item1, clasification.Item4, clasification.Item5, current));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in inputVars)
                        {
                            if (item.Name == rule.AntecedentsMembers[0].Item1)
                            {
                                foreach (var condition in item.Clasifications)
                                {
                                    if (condition.Item1 == rule.AntecedentsMembers[0].Item2)
                                    {
                                        current = condition.Item2.Invoke();

                                        foreach (var outPutMember in rule.ConsecuentsMembers)
                                        {
                                            foreach (var outPutVar in outputVars)
                                            {
                                                if (outPutMember.Item1 == outPutVar.Name)
                                                {
                                                    foreach (var clasification in outPutVar.Clasifications)
                                                    {
                                                        if (clasification.Item1 == outPutMember.Item2)
                                                        {
                                                            fuzzyOutPut.Add(new Tuple<LinguisticVariable, LinguisticVariable.MembershipPertenenceOutput, string, float, float, float>(outPutVar, clasification.Item3, clasification.Item1, clasification.Item4, clasification.Item5, current));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return fuzzyOutPut;
        }
    }
}
