using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
namespace SmartOdyssey
{
	[DefOf]
	public static class InternalDefOf
	{
		static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}

        [MayRequire("m00nl1ght.GeologicalLandforms")]
        public static TileMutatorDef GL_RiverTerrain;

    }
}
