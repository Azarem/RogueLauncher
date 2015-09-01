using AssemblyTranslator;
using AssemblyTranslator.Graphs;
using AssemblyTranslator.IL;
using DS2DEngine;
using RogueAPI.Spells;
using System;
using System.Linq;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    internal static class SpellSystem
    {
        public static void Process(GraphManager graph)
        {
            var module = graph.Graph.Modules.First();

            var spellTypeGraph = module.TypeGraphs.First(x => x.Name == "SpellType");
            PullSpellInformation(spellTypeGraph);

            var spellEvGraph = module.TypeGraphs.First(x => x.Name == "SpellEV");
            PullSpellEv(spellEvGraph);

            //var lineageObj = module.TypeGraphs.First(x => x.Name == "LineageObj");
        }

        public static SpellDefinition GetSpell(byte id)
        {
            if (id == 0)
                return SpellDefinition.None;

            var def = SpellDefinition.GetById(id);

            if (def == SpellDefinition.None)
                def = SpellDefinition.Register(id);

            return def;
        }

        [Obfuscation(Exclude = true)]
        private static RogueAPI.Projectiles.ProjectileInstance NewGetProjData(byte id, GameObj source)
        {
            return SpellDefinition.GetById(id).GetProjectileInstance(source);
        }

        private static void PullSpellInformation(TypeGraph spellType)
        {
            //Should we even bother with the field values?
            //It is assumed that all switches are offset by -1

            foreach (var field in spellType.Fields.Where(x => x.ConstantValue is byte && !x.Name.StartsWith("Total")))
                GetSpell((byte)field.ConstantValue).Name = field.Name;

            foreach (var method in spellType.Methods)
            {
                if (method.Name == "ToString")
                {
                    Util.ReadSwitches<string>(method.InstructionList, (x, y, z) => GetSpell(x).DisplayName = y, (x, y, z) => { foreach (var c in SpellDefinition.All.Where(b => b.DisplayName == null)) c.DisplayName = y; });
                    method.InstructionList = new InstructionList(new Func<byte, string>((x) => SpellDefinition.GetById(x).DisplayName));
                }
                else if (method.Name == "Description")
                {
                    Util.ReadSwitches<string>(method.InstructionList, (x, y, z) => GetSpell(x).Description = y, (x, y, z) => { foreach (var c in SpellDefinition.All.Where(b => b.Description == null)) c.Description = y; });
                    method.InstructionList = new InstructionList(new Func<byte, string>((x) => SpellDefinition.GetById(x).Description));
                }
                else if (method.Name == "Icon")
                {
                    Util.ReadSwitches<string>(method.InstructionList, (x, y, z) => GetSpell(x).Icon = y, (x, y, z) => { foreach (var c in SpellDefinition.All.Where(b => b.Icon == null)) c.Icon = y; });
                    method.InstructionList = new InstructionList(new Func<byte, string>((x) => SpellDefinition.GetById(x).Icon));
                }
            }

        }

        private static void PullSpellEv(TypeGraph spellEv)
        {
            MethodGraph projMethod = null;

            foreach (var m in spellEv.Methods)
            {
                if (m.Name == "GetDamageMultiplier")
                {
                    Util.ReadSwitches<float>(m.InstructionList, (x, y, z) => GetSpell(x).DamageMultiplier = y, (x, y, z) => { foreach (var c in SpellDefinition.All.Where(b => b.DamageMultiplier == 0)) c.DamageMultiplier = y; });
                    m.InstructionList = new InstructionList(new Func<byte, float>((x) => SpellDefinition.GetById(x).DamageMultiplier));
                }
                else if (m.Name == "GetRarity")
                {
                    Util.ReadSwitches<int>(m.InstructionList, (x, y, z) => GetSpell(x).Rarity = y, (x, y, z) => { foreach (var c in SpellDefinition.All.Where(b => b.Rarity == 0)) c.Rarity = y; });
                    m.InstructionList = new InstructionList(new Func<byte, int>((x) => SpellDefinition.GetById(x).Rarity));
                }
                else if (m.Name == "GetManaCost")
                {
                    Util.ReadSwitches<int>(m.InstructionList, (x, y, z) => GetSpell(x).ManaCost = y, (x, y, z) => { foreach (var c in SpellDefinition.All.Where(b => b.ManaCost == 0)) c.ManaCost = y; });
                    m.InstructionList = new InstructionList(new Func<byte, int>((x) => SpellDefinition.GetById(x).ManaCost));
                }
                else if (m.Name == "GetXValue")
                {
                    Util.ReadSwitches<float>(m.InstructionList, (x, y, z) => GetSpell(x).MiscValue1 = y, (x, y, z) => { foreach (var c in SpellDefinition.All.Where(b => b.MiscValue1 == 0)) c.MiscValue1 = y; });
                    m.InstructionList = new InstructionList(new Func<byte, float>((x) => SpellDefinition.GetById(x).MiscValue1));
                }
                else if (m.Name == "GetYValue")
                {
                    Util.ReadSwitches<float>(m.InstructionList, (x, y, z) => GetSpell(x).MiscValue2 = y, (x, y, z) => { foreach (var c in SpellDefinition.All.Where(b => b.MiscValue2 == 0)) c.MiscValue2 = y; });
                    m.InstructionList = new InstructionList(new Func<byte, float>((x) => SpellDefinition.GetById(x).MiscValue2));
                }
                else if (m.Name == "GetProjData")
                {
                    projMethod = m;
                }
            }

            //Rebuild GetProjData to return a ProjectileDefinition, for use during initialization
            //It is less work to use the existing method instead of scraping fields from the IL

            var newMethod = new MethodGraph(Util.GetMethodInfo(() => NewGetProjData(0, null)), spellEv);
            newMethod.Source = projMethod.Source;
            newMethod.Attributes &= ~MethodAttributes.Private;
            newMethod.Attributes |= MethodAttributes.Public;
            newMethod.Name = "GetProjData";

            var newCon = Util.GetMethodInfo(() => new RogueAPI.Projectiles.ProjectileDefinition());

            //Replace the old constructor and remove usage of the Target field. The Type replacer will handle the rest.
            var instr = projMethod.InstructionList;
            var ix = 0;
            var count = instr.Count;
            while (ix < count)
            {
                var i = instr[ix];
                if (i is MethodInstruction)
                {
                    var mi = i as MethodInstruction;
                    if (mi.Operand.IsConstructor && mi.Operand.DeclaringType.Name == "ProjectileData")
                    {
                        mi.Operand = newCon;
                        instr.RemoveAt(ix - 1);
                        count--;
                        continue;
                    }
                }
                else if (i is FieldInstruction)
                {
                    var fi = i as FieldInstruction;
                    if (fi.ILCode == ILCode.Stfld && fi.Operand.Name == "Target")
                    {
                        instr.RemoveAt(ix--);
                        instr.RemoveAt(ix--);
                        instr.RemoveAt(ix);
                        count -= 3;
                        continue;
                    }
                }
                ix++;
            }

            //Change return type
            projMethod.Parameters[0].ParameterType = typeof(RogueAPI.Projectiles.ProjectileDefinition);
            //Remove GameObj parameter
            projMethod.Parameters.RemoveAt(2);

            //Change local types
            foreach (var l in instr.Locals)
                if (l.Type.Name == "ProjectileData")
                    l.Type = typeof(RogueAPI.Projectiles.ProjectileDefinition);

            //Change name and remove source from graph
            projMethod.Name = "_GetProjData";
            projMethod.Source = null;
        }

    }
}
