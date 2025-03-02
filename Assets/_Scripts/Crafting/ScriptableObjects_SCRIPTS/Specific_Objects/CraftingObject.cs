using System;
using UnityEngine;

namespace Fractura.CraftingSystem
{
    [CreateAssetMenu(fileName = "NewCraftingObjectData", menuName = "CraftingSystem/Crafting Object Data", order = 1)]
    public class CraftingObject : ScriptableObject
    {
        [SerializeField] private CraftingObjectType craftingObjectType;
        public CraftingObjectType CraftingObjectType => craftingObjectType;

        [SerializeField] private string objectName;
        public string ObjectName => objectName;

        [SerializeField] private Sprite sprite;
        public Sprite ObjectSprite => sprite;

        [SerializeField] private Sprite gridSprite;
        public Sprite GetSprite => gridSprite;

        [SerializeField] private CraftingObjectRecipe recipe;
        public CraftingObjectRecipe Recipe => recipe;

        [SerializeField] private float craftingTime = 0f;
        public float CraftingTime => craftingTime;


        public ICraftingOutcomeStrategy outcomeStrategy;
        public OutcomeParameters outcomeParameters;

        public Action<CraftingObject> OnObjectExecuted;
        public CraftingEffectType effectType = CraftingEffectType.None;

        public virtual void ExecuteOutcome(GameObject user)
        {
            outcomeStrategy?.ExecuteOutcome(this, user, outcomeParameters);

            OnObjectExecuted?.Invoke(this);

            if(effectType != CraftingEffectType.None)
            {
                WorldEventManager.Instance.BroadcastOutcome(this);
            }
        }
    }
}

