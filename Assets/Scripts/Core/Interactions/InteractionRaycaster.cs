using UnityEngine;
using NeonProtocol.Core.Input;
using NeonProtocol.Core.UI;

namespace NeonProtocol.Core.Interactions
{
    public class InteractionRaycaster : MonoBehaviour
    {
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactLayer;
        [SerializeField] private float checkInterval = 0.1f; // Raycast 10x/sec instead of every frame
        
        private Transform _cam;
        private float _nextCheckTime;

        private void Awake() => _cam = Camera.main.transform;

        private void Update()
        {
            if (Time.time < _nextCheckTime) return;
            _nextCheckTime = Time.time + checkInterval;

            RaycastHit hit;
            if (Physics.Raycast(_cam.position, _cam.forward, out hit, interactRange, interactLayer))
            {
                if (hit.collider.TryGetComponent(out NeonInteractable interactable))
                {
                    // Update UI Prompt (e.g., "Press X to Buy Door [$1000]")
                    // UIController.Instance.ShowPrompt(interactable.GetPrompt());

                    if (NeonInputHandler.Instance.JumpInput) // Using Jump as temp Interact button
                    {
                        interactable.Interact();
                    }
                }
            }
            else
            {
                // UIController.Instance.HidePrompt();
            }
        }
    }
}