using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using RogueAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace RogueLauncher.Rewrite
{
    internal static class SpellSystem
    {
        public static void Rewrite(GraphManager graph)
        {
            var module = graph.Graph.Modules.First();

            var spellTypeGraph = module.TypeGraphs.First(x => x.Name == "SpellType");
            PullSpellInformation(spellTypeGraph);

            var spellEvGraph = module.TypeGraphs.First(x => x.Name == "SpellEV");
            PullSpellEv(spellEvGraph);
        }

        private static SpellDefinition GetSpell(byte id)
        {
            if (id == 0)
                return SpellDefinition.None;

            var def = SpellDefinition.GetById(id);

            if (def == SpellDefinition.None)
                def = SpellDefinition.Register(id);

            return def;
        }

        private static string NewIcon(byte id)
        {
            return SpellDefinition.GetById(id).Icon;
        }
        private static string NewToString(byte id)
        {
            return SpellDefinition.GetById(id).Name;
        }
        private static string NewDescription(byte id)
        {
            return SpellDefinition.GetById(id).Description;
        }
        private static float NewGetDamageMultiplier(byte id)
        {
            return SpellDefinition.GetById(id).DamageMultiplier;
        }
        private static int NewGetRarity(byte id)
        {
            return SpellDefinition.GetById(id).Rarity;
        }
        private static int NewGetManaCost(byte id)
        {
            return SpellDefinition.GetById(id).ManaCost;
        }
        private static float NewGetXValue(byte id)
        {
            return SpellDefinition.GetById(id).MiscValue1;
        }
        private static float NewGetYValue(byte id)
        {
            return SpellDefinition.GetById(id).MiscValue2;
        }

        private static void PullSpellInformation(TypeGraph spellType)
        {
            //Should we even bother with the field values?
            //It is assumed that all switches are offset by -1

            foreach (var method in spellType.Methods)
            {
                if (method.Name == "ToString")
                {
                    ReadSwitches(method.InstructionList, 0);
                    method.InstructionList = new InstructionList(Util.GetMethodInfo(() => NewToString(0)));
                }
                else if (method.Name == "Description")
                {
                    ReadSwitches(method.InstructionList, 1);
                    method.InstructionList = new InstructionList(Util.GetMethodInfo(() => NewDescription(0)));
                }
                else if (method.Name == "Icon")
                {
                    ReadSwitches(method.InstructionList, 2);
                    method.InstructionList = new InstructionList(Util.GetMethodInfo(() => NewIcon(0)));
                }
            }

        }

        private static void PullSpellEv(TypeGraph spellEv)
        {
            foreach (var m in spellEv.Methods)
            {
                if (m.Name == "GetDamageMultiplier")
                {
                    ReadSwitches(m.InstructionList, 3);
                    m.InstructionList = new InstructionList(Util.GetMethodInfo(() => NewGetDamageMultiplier(0)));
                }
                else if (m.Name == "GetRarity")
                {
                    ReadSwitches(m.InstructionList, 4);
                    m.InstructionList = new InstructionList(Util.GetMethodInfo(() => NewGetRarity(0)));
                }
                else if (m.Name == "GetManaCost")
                {
                    ReadSwitches(m.InstructionList, 5);
                    m.InstructionList = new InstructionList(Util.GetMethodInfo(() => NewGetManaCost(0)));
                }
                else if (m.Name == "GetXValue")
                {
                    ReadSwitches(m.InstructionList, 6);
                    m.InstructionList = new InstructionList(Util.GetMethodInfo(() => NewGetXValue(0)));
                }
                else if (m.Name == "GetYValue")
                {
                    ReadSwitches(m.InstructionList, 7);
                    m.InstructionList = new InstructionList(Util.GetMethodInfo(() => NewGetYValue(0)));
                }
            }
        }

        private static void SetField(byte id, int type, object value)
        {
            var def = GetSpell(id);
            switch (type)
            {
                case 0: def.Name = (string)value; break;
                case 1: def.Description = (string)value; break;
                case 2: def.Icon = (string)value; break;
                case 3: def.DamageMultiplier = (float)value; break;
                case 4: def.Rarity = Convert.ToInt32(value); break;
                case 5: def.ManaCost = Convert.ToInt32(value); break;
                case 6: def.MiscValue1 = (float)value; break;
                case 7: def.MiscValue2 = (float)value; break;
            }
        }


        private static object ParseOperand(InstructionList instr, InstructionBase i, int type)
        {
            object value = i.RawOperand;

            if (type == 1)
            {
                string accum = (string)value;
                var ix3 = instr.IndexOf(i) + 1;
                while (true)
                {
                    var i2 = instr[ix3++];
                    if (i2 is Int8Instruction || i2 is StringInstruction)
                        accum += i2.RawOperand;
                    else if (i2.ILCode == ILCode.Ret)
                        break;
                }
                value = accum;
            }

            return value;
        }

        private static void ReadSwitches(InstructionList instr, int type)
        {
            bool first = true;
            int ix = 0, count = instr.Count, ix2;

            while (ix < count)
            {
                var i1 = instr[ix++];
                if (i1 is SwitchInstruction)
                {
                    //Read string values from switch statement
                    var switchJumps = ((SwitchInstruction)i1).JumpToInstructions;
                    var jCount = switchJumps.Count;
                    ix2 = 0;
                    while (ix2 < jCount)
                        SetField((byte)(ix2 + 1), type, ParseOperand(instr, switchJumps[ix2++], type));
                }
                else if (i1 is LabelInstruction)
                {
                    if (first)
                    {
                        ix2 = ((Int8Instruction)instr[ix - 2]).Operand;
                        first = false;
                    }
                    else
                        ix2 = 0;

                    SetField((byte)ix2, type, ParseOperand(instr, ((LabelInstruction)i1).JumpToInstruction, type));

                    if (ix2 == 0)
                        break;
                }
            }
        }
    }
}
