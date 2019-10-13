using UnityEngine.UI;

namespace Synapse
{
    public class ImageView
    {
        public Image GameObject { get; set; }
        public ImageModel Model { get; set; }
        public bool Ready { get; set; }

        public ImageView(Image gameObject, ImageModel model)
        {
            GameObject = gameObject;
            Model = model;
            Ready = true;
        }
    }
}