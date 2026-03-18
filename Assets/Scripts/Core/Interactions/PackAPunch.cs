using UnityEngine;
using NeonProtocol.Core.Combat;

namespace NeonProtocol.Core.Interactions
{
    public class PackAPunch : NeonInteractable
    {
        [Header("Upgrade Settings")]
        [SerializeField] private float damageMultiplier = 2.0f;
        [SerializeField] private int ammoBoost = 2;
        [SerializeField] private Material upgradedMaterial;

        protected override void OnPurchaseSuccess()
        {
            if (PlayerCombat.Instance != null)
            {
                PlayerCombat.Instance.damageMultiplier *= damageMultiplier;
                PlayerCombat.Instance.BoostAmmo(ammoBoost);
                if (upgradedMaterial != null)
                {
                    var weaponModel = PlayerCombat.Instance.GetCurrentWeapon()?.weaponModel;
                    if (weaponModel != null && weaponModel.TryGetComponent(out Renderer rend))
                        rend.material = upgradedMaterial;
                }
            }
            Debug.Log("Weapon Upgraded! Neon Infused!");
        }

    }
}