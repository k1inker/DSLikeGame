using UnityEngine;

namespace DS
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockingCollider;
        private void Awake()
        {
            blockingCollider = GetComponent<BoxCollider>();
        }
        public void EnableBlockingCollider()
        {
            blockingCollider.enabled = true;
        }
        public void DisableBlockingCollider()
        {
            blockingCollider.enabled = false;
        }

    }
}