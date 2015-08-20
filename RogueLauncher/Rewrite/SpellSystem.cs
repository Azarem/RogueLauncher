using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using RogueAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    internal class SpellSystem
    {
        public static void Rewrite(GraphManager graph)
        {
            var module = graph.Graph.Modules.First();

            var spellTypeGraph = module.TypeGraphs.First(x => x.Name == "SpellType");

            PullSpellInformation(spellTypeGraph);
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

        private static void PullSpellInformation(TypeGraph spellType)
        {
            //Should we even bother with the field values?
            //It is assumed that all switches are offset by -1
            foreach (var method in spellType.Methods)
            {
                if (method.Name == "ToString")
                {
                    ReadSwitches(method.InstructionList, 0);
                    method.InstructionList = new InstructionList(Util.GetMethodInfo<SpellSystem>(x => NewToString(0)));
                }
                else if (method.Name == "Description")
                {
                    ReadSwitches(method.InstructionList, 1);
                    method.InstructionList = new InstructionList(Util.GetMethodInfo<SpellSystem>(x => NewDescription(0)));
                }
                else if (method.Name == "Icon")
                {
                    ReadSwitches(method.InstructionList, 2);
                    method.InstructionList = new InstructionList(Util.GetMethodInfo<SpellSystem>(x => NewIcon(0)));
                }
            }

        }

        private static void SetField(byte id, int type, string value)
        {
            var def = GetSpell(id);
            switch (type)
            {
                case 0: def.Name = value; break;
                case 1: def.Description = value; break;
                case 2: def.Icon = value; break;
            }
        }


        private static void ReadSwitches(InstructionList instr, int type)
        {
            StringInstruction stIn;
            List<InstructionBase> switchJumps;
            bool first = true, falseFirst;
            int ix = 0, count = instr.Count, ix2, jCount;
            string accum;

            while (ix < count)
            {
                var i1 = instr[ix++];
                if (i1 is SwitchInstruction)
                {
                    //Read string values from switch statement
                    switchJumps = ((SwitchInstruction)i1).JumpToInstructions;
                    jCount = switchJumps.Count;
                    ix2 = 0;
                    while (ix2 < jCount)
                    {
                        stIn = switchJumps[ix2++] as StringInstruction;
                        accum = stIn.Operand;
                        if (type == 1)
                        {
                            var ix3 = instr.IndexOf(stIn) + 1;
                            while (true)
                            {
                                var i2 = instr[ix3++];
                                if (i2 is Int8Instruction)
                                    accum += ((Int8Instruction)i2).Operand;
                                else if (i2 is StringInstruction)
                                    accum += ((StringInstruction)i2).Operand;
                                else if (i2 is MethodInstruction)
                                    break;
                            }
                        }
                        SetField((byte)ix2, type, accum);
                    }
                }
                else if (i1 is LabelInstruction)
                {
                    stIn = ((LabelInstruction)i1).JumpToInstruction as StringInstruction;
                    accum = stIn.Operand;
                    if (first)
                    {
                        ix2 = ((Int8Instruction)instr[ix - 2]).Operand;
                        if (type == 1)
                        {
                            var ix3 = instr.IndexOf(stIn) + 1;
                            while (true)
                            {
                                var i2 = instr[ix3++];
                                if (i2 is Int8Instruction)
                                    accum += ((Int8Instruction)i2).Operand;
                                else if (i2 is StringInstruction)
                                    accum += ((StringInstruction)i2).Operand;
                                else if (i2 is MethodInstruction)
                                    break;
                            }
                        }
                        SetField((byte)ix2, type, accum);
                        first = false;
                    }
                    else
                    {
                        SetField(0, type, accum);
                        break;
                    }
                }
            }
        }
    }
}
