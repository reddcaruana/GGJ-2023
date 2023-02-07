using UnityEngine;

namespace Mobile
{
    public class GrabberID : MonoBehaviour
    {
        [SerializeField] private int id = -1;
        public int ID => id;
    }
}