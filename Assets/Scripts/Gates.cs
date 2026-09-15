using Assets.Scripts.Constants;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts
{
    public class Gates : BaseInteractiveItem
    {
        [SerializeField]
        private Gates _relatedGates;

        public override void Interact()
        {
            InteractInternal(GameObject.FindGameObjectWithTag(TagConstants.PLAYER));
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag(TagConstants.PLAYER))
            {
                InteractInternal(collider.gameObject);
            }
        }

        private void InteractInternal(GameObject go)
        {
            go.transform.position = new(
                _relatedGates.transform.position.x,
                (float)(_relatedGates.transform.position.y - 1.7),
                _relatedGates.transform.position.z
            );
        }
    }
}
