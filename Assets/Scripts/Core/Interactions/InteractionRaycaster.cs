using UnityEngine;
using NeonProtocol.Core.Input;
using NeonProtocol.Core.UI;

namespace NeonProtocol.Core.Interactions
{
    public class InteractionRaycaster : MonoBehaviour
    {
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactLayer;
        
        private Transform _cam;

        private void Awake() => _cam = Camera.main.transform;

        private void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(_cam.position, _cam.forward, out hit, interactRange, interactLayer))
            {
                if (hit.collider.TryGetComponent(out NeonInteractable interactable))
                {
                    if (UIController.Instance != null)
                        UIController.Instance.ShowPrompt(interactable.GetPrompt());

                    if (NeonInputHandler.Instance != null && NeonInputHandler.Instance.JumpInput)
                    {
                        interactable.Interact();
                    }
                }
            }
            else
            {
                if (UIController.Instance != null)
                    UIController.Instance.HidePrompt();
            }
        }
    }
}