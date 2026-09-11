using UnityEngine;

namespace Assets.Scripts.GameStates
{
    public class GlobalGameState : MonoBehaviour
    {
        public GlobalGameState Instance { get; private set; }

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }       
    }
}
