using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts
{
    public class Gates : MonoBehaviour
    {
        [SerializeField]
        private Gates _relatedGates;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag(TagConstants.PLAYER))
            {
                collider.gameObject.transform.position = new(
                    _relatedGates.transform.position.x,
                    (float)(_relatedGates.transform.position.y - 1.7),
                    _relatedGates.transform.position.z
                );
            }
        }
    }
}
