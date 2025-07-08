using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using HarmonyLib;
using System.Reflection;
namespace SmartOdyssey
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.SmartOdyssey");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
