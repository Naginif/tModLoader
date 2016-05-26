﻿using System.Collections.Generic;

namespace Terraria.ModLoader
{
	public static class RecipeHooks
	{
		internal static readonly IList<GlobalRecipe> globalRecipes = new List<GlobalRecipe>();

		internal static void Add(GlobalRecipe globalRecipe)
		{
			globalRecipes.Add(globalRecipe);
		}

		internal static void Unload()
		{
			globalRecipes.Clear();
		}

		internal static void AddRecipes()
		{
			foreach (Mod mod in ModLoader.mods.Values)
			{
				try
				{
					mod.AddRecipes();
					foreach (ModItem item in mod.items.Values)
					{
						item.AddRecipes();
					}
				}
				catch
				{
					ModLoader.DisableMod(mod.File);
					throw;
				}
			}
		}

		public static bool RecipeAvailable(Recipe recipe)
		{
			ModRecipe modRecipe = recipe as ModRecipe;
			if (modRecipe != null && !modRecipe.RecipeAvailable())
			{
				return false;
			}
			foreach (GlobalRecipe globalRecipe in globalRecipes)
			{
				if (!globalRecipe.RecipeAvailable(recipe))
				{
					return false;
				}
			}
			return true;
		}

		public static void OnCraft(Item item, Recipe recipe)
		{
			ModRecipe modRecipe = recipe as ModRecipe;
			if (modRecipe != null)
			{
				modRecipe.OnCraft(item);
			}
			foreach (GlobalRecipe globalRecipe in globalRecipes)
			{
				globalRecipe.OnCraft(item, recipe);
			}
		}
	}
}