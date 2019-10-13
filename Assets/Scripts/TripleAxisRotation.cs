using UnityEngine;

namespace Synapse
{
    public class TripleAxisRotation : MonoBehaviour
    {
        [Range(0, 360)]
        [SerializeField] private float rotationX = 0;
        [Range(0, 360)]
        [SerializeField] private float rotationY = 0;
        [Range(0, 360)]
        [SerializeField] private float rotationZ = 0;

        private void Update()
        {
            var rotation = new Vector3(rotationX, rotationY, rotationZ) * Time.deltaTime;
            transform.Rotate(rotation);
        }
    }
}