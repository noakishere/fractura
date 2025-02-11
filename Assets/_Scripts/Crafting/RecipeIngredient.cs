using Fractura.CraftingSystem;
using System;
using UnityEngine;


namespace Fractura.CraftingSystem
{

    [Serializable]
    public class RecipeIngredient
    {
        [SerializeField] private CraftingObject ingredient;
        public CraftingObject Ingredient => ingredient;

        [SerializeField] private int quantity = 1;
        public int Quantity => quantity;
    }
}
