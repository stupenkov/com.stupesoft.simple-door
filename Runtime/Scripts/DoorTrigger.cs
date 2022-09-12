using UnityEngine;

namespace Stupesoft.Doors
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private SimpleDoor _simpleDoor;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                _simpleDoor.Open();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                _simpleDoor.Close();
            }
        }
    }
}