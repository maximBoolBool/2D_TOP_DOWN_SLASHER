using UnityEngine;

namespace Assets.Scripts
{
    public class Gates : MonoBehaviour
    {
        [SerializeField]
        private Gates _relatedGates;

        //public void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision.gameObject.CompareTag("Player"))
        //    {
        //        collision.gameObject.transform.position = new(
        //            _relatedGates.transform.position.x,
        //            (float)(_relatedGates.transform.position.y - 1.77),
        //            _relatedGates.transform.position.z
        //        );
        //    }
        //}

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
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
