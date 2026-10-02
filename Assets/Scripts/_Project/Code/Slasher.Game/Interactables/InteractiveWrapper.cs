using UnityEngine;
using Assets.Scripts._Project.Code.Slasher.Core.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.PlayerControllers;
using Zenject;

namespace Assets.Scripts._Project.Code.Slasher.Game.Interactables
{
    public class InteractiveWrapper : MonoBehaviour
    {
        private bool isPlayerInRange = false;
        private BaseInteractiveItem _interactiveItem;
        private Animator animator;
        private GameObject _buttonLable;

        [Inject] private PlayerInteractController _playerInteractController;

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
            _buttonLable = transform.Find("ButtonSprite").gameObject;
            animator = _buttonLable.GetComponent<Animator>();
            _buttonLable.SetActive(false);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(TagConstants.PLAYER))
            {
                return;
            }

            isPlayerInRange = true;
            _buttonLable.SetActive(true);
            // сообщаем игроку, что теперь именно этот объект — текущий интерактивный
            _playerInteractController.SetInteractiveWrapper(this);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag(TagConstants.PLAYER))
            { 
                return;
            }

            isPlayerInRange = false;
            _buttonLable.SetActive(false);
            _playerInteractController.SetInteractiveWrapper(null);
        }
    }

    public static class ButtonAnimatorConstants
    {
        public const string Pressed = "Pressed";
    }
}
