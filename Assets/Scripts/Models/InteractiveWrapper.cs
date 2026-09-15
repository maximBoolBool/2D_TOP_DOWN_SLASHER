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
        private GameObject _buttonLable;

        public void Interact()
        {
            if (isPlayerInRange)
            {
                _interactiveItem.Interact();
                animator.SetTrigger(ButtonAnimatorConstants.Pressed);
            }
        }

        private void Awake()
        {
            _interactiveItem = GetComponentInChildren<BaseInteractiveItem>();
            animator = GetComponent<Animator>();
            _buttonLable = transform.Find("ButtonSprite").gameObject;
            _buttonLable.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(TagConstants.PLAYER))
            {
                return;
            }

            isPlayerInRange = true;
            _buttonLable.SetActive(true);
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
            _buttonLable.SetActive(false);
            collision.GetComponent<PlayerInteractController>()?.SetInteractiveWrapper(null);
        }
    }

    public static class ButtonAnimatorConstants
    {
        public const string Pressed = "Pressed";
    }
}
