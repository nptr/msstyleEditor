using libmsstyle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libmsstyleTests
{
    [TestClass]
    public class LoadSaveConsistency
    {
        // Ensure that we can load and save a visual style with all properties intact.
        [DataTestMethod]
        [DataRow(@"..\..\..\styles\w7_aero.msstyles")]
        [DataRow(@"..\..\..\styles\w81_aero.msstyles")]
        [DataRow(@"..\..\..\styles\w10_1709_aero.msstyles")]
        [DataRow(@"..\..\..\styles\w10_1809_aero.msstyles")]
        [DataRow(@"..\..\..\styles\w10_1903_aero.msstyles")]
        [DataRow(@"..\..\..\styles\w10_2004_aero\aero.msstyles")]
        [DataRow(@"..\..\..\styles\w10_20h2_aero.msstyles")]
        [DataRow(@"..\..\..\styles\w11_pre_aero.msstyles")]
        public void VerifyLoadSave(string file, bool standalone = false)
        {
            using(var original = new VisualStyle())
            using(var saved = new VisualStyle())
            {
                original.Load(file);
                original.Save("tmp.msstyles", standalone);
                
                saved.Load("tmp.msstyles");
                Assert.IsTrue(CompareStyles(original, saved));
            }
        }


        // Ensure that we can load and save a visual style multiple times. All properties and 
        // the string table (as far as available) should stay the same.
        //
        // "standalone": the MUI get removed and the string table written into the LN instead.
        // Only set to false if there is no string table to begin with.
        [DataTestMethod]
        [DataRow(@"..\..\..\styles\w10_2004_aero\aero.msstyles", true)]
        [DataRow(@"..\..\..\styles\w10_21h2_aero\aero.msstyles", true)]
        [DataRow(@"..\..\..\styles\w11_luna_mod\Luna.msstyles", true)]
        [DataRow(@"..\..\..\styles\w11_pre_aero.msstyles", false)]
        public void VerifyConsecutiveLoadSave(string file, bool standalone = true)
        {
            using (var original = new VisualStyle())
            using (var saved = new VisualStyle())
            using (var saved2 = new VisualStyle())
            {
                original.Load(file);
                original.Save("tmp.msstyles", standalone);

                // Verify first save                
                saved.Load("tmp.msstyles");
                Assert.IsTrue(CompareStyles(original, saved));
                Assert.IsTrue(CompareStringTables(original.StringTables, saved.StringTables));
                saved.Save("tmp2.msstyles", standalone);

                // Verify second save
                saved2.Load("tmp2.msstyles");
                Assert.IsTrue(CompareStyles(saved, saved2));
                Assert.IsTrue(CompareStringTables(saved.StringTables, saved2.StringTables));
            }
        }


        [TestCleanup]
        public void TestCleanup()
        {
            try
            { 
                File.Delete("tmp.msstyles");
                File.Delete("tmp2.msstyles");
            }
            catch (Exception) { }
        }

        void LogMessage(ConsoleColor c, string message, params object[] args)
        {
            var tmp = Console.ForegroundColor;
            Console.ForegroundColor = c;
            Console.Write(String.Format(message, args));
            Console.ForegroundColor = tmp;
        }

        bool CompareStyles(VisualStyle s1, VisualStyle s2)
        {
            bool result = true;

            // for all classes in the original style
            foreach (var cls in s1.Classes)
            {
                // see if there is one in the reloaded one
                StyleClass clsOther;
                if(s2.Classes.TryGetValue(cls.Key, out clsOther))
                {
                    // foreach part in the original classes
                    foreach (var part in cls.Value.Parts)
                    {
                        // see if there is an equivalent one in the reloaded classes
                        StylePart partOther;
                        if(clsOther.Parts.TryGetValue(part.Key, out partOther))
                        {
                            // foreach state in all original parts
                            foreach (var state in part.Value.States)
                            {
                                // see if it exists in the reloaded parts as well
                                StyleState stateOther;
                                if(partOther.States.TryGetValue(state.Key, out stateOther))
                                {
                                    // foreach properties in all original states
                                    foreach (var prop in state.Value.Properties)
                                    {
                                        // see if the property exists, by just comparing the header
                                        var propOther = stateOther.Properties.FindAll((p) => p.Header.Equals(prop.Header));
                                        if (propOther.Count == 0)
                                        {
                                            result = false;
                                            LogMessage(ConsoleColor.DarkRed, "Missing prop [N: {0}, T: {1}], in\r\n", prop.Header.nameID, prop.Header.typeID);
                                            LogMessage(ConsoleColor.DarkRed, "State {0}: {1}\r\n", state.Value.StateId, state.Value.StateName);
                                            LogMessage(ConsoleColor.DarkRed, "Part {0}: {1}\r\n", part.Value.PartId, part.Value.PartName);
                                            LogMessage(ConsoleColor.DarkRed, "Class {0}: {1}\r\n", cls.Value.ClassId, cls.Value.ClassName);
                                            continue;
                                        }

                                        var valueEqual = propOther.Any((p) =>
                                        {
                                            bool eq = false;
                                            if (p.GetValue() is List<Int32> li)
                                            {
                                                eq = Enumerable.SequenceEqual(li, (List<Int32>)prop.GetValue());
                                            }
                                            else if (p.GetValue() is List<Color> lc)
                                            {
                                                eq = Enumerable.SequenceEqual(lc, (List<Color>)prop.GetValue());
                                            }
                                            else
                                            {
                                                eq = p.GetValue().Equals(prop.GetValue());
                                            }
                                            return eq;
                                        });

                                        if (!valueEqual)
                                        {
                                            result = false;
                                            LogMessage(ConsoleColor.DarkRed, "Different value for Prop [N: {0}, T: {1}], in\r\n", prop.Header.nameID, prop.Header.typeID);
                                            LogMessage(ConsoleColor.DarkRed, "State {0}: {1}\r\n", state.Value.StateId, state.Value.StateName);
                                            LogMessage(ConsoleColor.DarkRed, "Part {0}: {1}\r\n", part.Value.PartId, part.Value.PartName);
                                            LogMessage(ConsoleColor.DarkRed, "Class {0}: {1}\r\n", cls.Value.ClassId, cls.Value.ClassName);
                                        }
                                    }
                                }
                                else
                                {
                                    result = false;
                                    LogMessage(ConsoleColor.DarkRed, "Missing state {0}: {1}, in\r\n", state.Value.StateId, state.Value.StateName);
                                    LogMessage(ConsoleColor.DarkRed, "Part {0}: {1}\r\n", part.Value.PartId, part.Value.PartName);
                                    LogMessage(ConsoleColor.DarkRed, "Class {0}: {1}\r\n", cls.Value.ClassId, cls.Value.ClassName);
                                }
                            }
                        }
                        else
                        {
                            result = false;
                            LogMessage(ConsoleColor.DarkRed, "Missing part {0}: {1}, in\r\n", part.Value.PartId, part.Value.PartName);
                            LogMessage(ConsoleColor.DarkRed, "Class {0}: {1}\r\n", cls.Value.ClassId, cls.Value.ClassName);
                        }
                    }
                }
                else
                {
                    result = false;
                    LogMessage(ConsoleColor.DarkRed, "Missing class {0}: {1}\r\n", cls.Value.ClassId, cls.Value.ClassName);
                }
            }

            return result;
        }


        bool CompareStringTables(Dictionary<int, Dictionary<int, string>> s1, Dictionary<int, Dictionary<int, string>> s2)
        {
            bool result = true;

            // for all languages in the original style
            foreach (var langTable in s1)
            {
                // see if there is a table in the other one
                Dictionary<int, string> s2table;
                if (s2.TryGetValue(langTable.Key, out s2table))
                {
                    // foreach entry in the languages string table
                    foreach (var entry in langTable.Value)
                    {
                        // see if the entry exists and matches in the other one
                        string s2tablevalue;
                        if (s2table.TryGetValue(entry.Key, out s2tablevalue))
                        {
                            if(!entry.Value.Equals(s2tablevalue))
                            {
                                LogMessage(ConsoleColor.DarkRed, "For language {0}, entry {1} mismatches\r\n", langTable.Key, entry.Key);
                                result = false;
                            }
                        }
                        else
                        {
                            LogMessage(ConsoleColor.DarkRed, "For language {0}, entry {1} is missing!\r\n", langTable.Key, entry.Key);
                            result = false;
                        }
                    }
                }
                else
                {
                    LogMessage(ConsoleColor.DarkRed, "Missing string table for language {0}\r\n", langTable.Key);
                    result = false;
                }
            }

            return result;
        }
    }
}
