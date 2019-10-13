using UnityEngine;
using UnityEngine.UI;

namespace Synapse
{
    public class GridLayoutScaler : MonoBehaviour
    {
        private void Start()
        {
            var gridLayout = GetComponent<GridLayoutGroup>();
            if (gridLayout != null)
            {
                gridLayout.cellSize = new Vector2(Screen.width, Screen.width);
            }
        }
    }
}