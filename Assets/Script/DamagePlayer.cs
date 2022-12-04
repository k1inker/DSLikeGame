using UnityEngine;
namespace DS
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25;
        private void OnTriggerEnter(Collider other)
        {
            PlayerStats pl = other.GetComponent<PlayerStats>();

            if(pl != null)
            {
                pl.TakeDamage(damage);
            }
        }
    }
}
