using UnityEngine;
using NeonProtocol.Core.Player;
using NeonProtocol.Core.Movement;
using NeonProtocol.Core.Combat;

namespace NeonProtocol.Core.Interactions
{
    public enum PerkType { TuffNuff, Quickies, BangBangs, RacingStripes, BlueBolts, UpNAtoms, TrailBlazers }

    public class NeonPerkMachine : NeonInteractable
    {
        [Header("Perk Settings")]
        [SerializeField] private PerkType perkType;

        protected override void OnPurchaseSuccess()
        {
            Debug.Log($"Drank {perkType}!");
            // Apply perk modifier to player
            ApplyPerkEffect();
        }

        private void ApplyPerkEffect()
        {
            switch (perkType)
            {
                case PerkType.TuffNuff: // Juggernog equivalent — increases max health to 250
                    if (PlayerHealth.Instance != null)
                        PlayerHealth.Instance.ApplyTuffNuff();
                    break;
                case PerkType.RacingStripes: // Stamin-Up — boosts sprint speed by 40 %
                    if (NeonMovement.Instance != null)
                        NeonMovement.Instance.ApplySprintBoost(1.4f);
                    break;
                case PerkType.Quickies: // Speed Cola — halves reload time
                    if (PlayerCombat.Instance != null)
                        PlayerCombat.Instance.reloadTimeMultiplier = 0.5f;
                    break;
                case PerkType.BangBangs: // Double Tap — doubles damage and increases fire rate
                    if (PlayerCombat.Instance != null)
                    {
                        PlayerCombat.Instance.damageMultiplier *= 2.0f;
                        PlayerCombat.Instance.fireRateMultiplier *= 0.67f;
                    }
                    break;
                case PerkType.UpNAtoms: // Up N Atoms — enables self-revive
                    if (PlayerHealth.Instance != null)
                        PlayerHealth.Instance.hasUpNAtoms = true;
                    break;
            }
        }
    }
}