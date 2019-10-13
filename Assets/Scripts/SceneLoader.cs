using UnityEngine;
using UnityEngine.SceneManagement;

namespace Synapse
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}