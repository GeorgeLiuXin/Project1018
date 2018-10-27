using UnityEngine;

namespace XWorld.Test
{
    public class DependenciesTest : MonoBehaviour
    {
        public GameObject go;

        private void Start()
        {
            Instantiate(go);
        }
    }
}
