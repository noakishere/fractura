namespace Fractura.CraftingSystem
{
    public interface ICraftable
    {
        public CraftingObject Craft();
        bool CheckRecipe();
    }
}
