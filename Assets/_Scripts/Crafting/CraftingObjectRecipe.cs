using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fractura.CraftingSystem
{
    [Serializable]
    public class CraftingObjectRecipe
    {
        [SerializeField] private List<RecipeIngredient> ingredients;
        public List<RecipeIngredient> Ingredients => ingredients;
    }
}
