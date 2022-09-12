using UnityEngine;
using Random = UnityEngine.Random;

namespace Stupesoft.Doors
{
    public class ToggleDoor : MonoBehaviour
    {
        [SerializeField] private SimpleDoor _simpleDoor;
        private Material _material;

        private void Awake()
        {
            _material = GetComponentInChildren<MeshRenderer>().material;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            _material.color = Random.ColorHSV();
            _simpleDoor.Toggle();
        }
    }
}