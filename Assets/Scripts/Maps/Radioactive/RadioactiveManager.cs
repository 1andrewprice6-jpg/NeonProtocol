using UnityEngine;
using System.Collections.Generic;

namespace NeonProtocol.Maps.Radioactive
{
    [System.Serializable]
    public struct ChemicalRecipe
    {
        public string ingredientA;
        public string ingredientB;
        public GameObject resultPrefab;
    }

    public class RadioactiveManager : MonoBehaviour
    {
        [Header("Crafting Station")]
        [SerializeField] private List<ChemicalRecipe> recipes;
        
        private string _slotA;
        private string _slotB;

        public void AddIngredient(string ingredientName)
        {
            if (string.IsNullOrEmpty(_slotA))
                _slotA = ingredientName;
            else if (string.IsNullOrEmpty(_slotB))
            {
                _slotB = ingredientName;
                Mix chemicals();
            }
        }

        private void Mix chemicals()
        {
            foreach (var recipe in recipes)
            {
                if ((recipe.ingredientA == _slotA && recipe.ingredientB == _slotB) ||
                    (recipe.ingredientA == _slotB && recipe.ingredientB == _slotA))
                {
                    // Success
                    Instantiate(recipe.resultPrefab, transform.position, Quaternion.identity);
                    ClearSlots();
                    return;
                }
            }
            
            // Failure - Explosion?
            Debug.Log("Volatile Mixture! BOOM!");
            ClearSlots();
        }

        private void ClearSlots()
        {
            _slotA = "";
            _slotB = "";
        }
    }
}