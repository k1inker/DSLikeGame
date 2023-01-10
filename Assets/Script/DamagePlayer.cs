using UnityEngine;
namespace DS
{
    public class DamagePlayer : MonoBehaviour
    {
        public int damage = 25;
        private void OnTriggerEnter(Collider other)
        {
            PlayerStatsManager pl = other.GetComponent<PlayerStatsManager>();

            if(pl != null)
            {
                pl.TakeDamage(damage);
            }
        }
    }
}
