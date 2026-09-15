using Assets.Scripts.Constants;
using Assets.Scripts.PlayerControllers;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class InteractiveWrapper : MonoBehaviour
    {
        private bool isPlayerInRange = false;
        private BaseInteractiveItem _interactiveItem;
        private Animator animator;

        public void Interact()
        {
            if (isPlayerInRange)
            {
                _interactiveItem.Interact();
            }
        }

        private void Awake()
        {
            _interactiveItem = GetComponentInChildren<BaseInteractiveItem>();
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(TagConstants.PLAYER))
            {
                return;
            }

            isPlayerInRange = true;
            // сообщаем игроку, что теперь именно этот объект — текущий интерактивный
            collision.GetComponent<PlayerInteractController>()?.SetInteractiveWrapper(this);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(TagConstants.PLAYER))
            { 
                return;
            }

            isPlayerInRange = false;
            collision.GetComponent<PlayerInteractController>()?.SetInteractiveWrapper(null);
        }
    }
}
