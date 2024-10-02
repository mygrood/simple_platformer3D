using UnityEngine;

namespace Platformer
{
    public class PlayerInputHandler:MonoBehaviour
    {
        private PlayerMover mover;

        private void Awake()
        {
            mover = GetComponent<PlayerMover>();
        }

        void Update()
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            mover.SetInputVector(new Vector2(x, y));
        }
    }
}