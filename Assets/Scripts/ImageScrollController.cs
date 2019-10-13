using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Synapse
{
    public class ImageScrollController : MonoBehaviour
    {
        [SerializeField] private ImageModel[] images;
        [SerializeField] private Image viewPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private TextMeshProUGUI imageDescription;
        [SerializeField] private HorizontalScrollSnap scrollSnap;
        [SerializeField] private TextMeshProUGUI leftCounterLabel;
        [SerializeField] private TextMeshProUGUI rightCounterLabel;
        [SerializeField] private bool clearPrefs;
        private ImageView lastView;
        private ImageView currentView;
        private ImageView nextView;
        private List<ImageModel> randomOrderedImages;
        private List<ImageView> imageViews = new List<ImageView>();
        private int currentScreen = 0;
        private int rightCounter = 0;
        private int leftCounter = 0;

        private int RightCounter
        {
            get { return rightCounter; }
            set
            {
                rightCounter = value;
                rightCounterLabel.text = $"{rightCounter}";
            }
        }

        private int LeftCounter
        {
            get { return leftCounter; }
            set
            {
                leftCounter = value;
                leftCounterLabel.text = $"{leftCounter}";
            }
        }

        private void Awake()
        {
            if (images.Length == 0) return;

            var rnd = new System.Random();
            randomOrderedImages = images.OrderBy(x => rnd.Next()).ToList();

            imageDescription.text = randomOrderedImages[0].Description;
            foreach (var image in randomOrderedImages)
            {
                var go = Instantiate(viewPrefab, container);
                go.sprite = image.Sprite;
                var view = new ImageView(go, image);
                imageViews.Add(view);
            }

            scrollSnap.OnSelectionPageChangedEvent.AddListener(OnScreenChanged);
            scrollSnap.OnSelectionChangeEndEvent.AddListener(OnLerpEnded);
            
            if (clearPrefs)
            {
                PlayerPrefs.DeleteAll();
            }

            LoadData();
            OnScreenChanged(0);
        }

        private void Start()
        {
            if (images.Length == 1)
            {
                scrollSnap._scroll_rect.horizontal = false;
            }
        }

        private bool toggle = false;

        public void OnLerpEnded(int screenIndex)
        {
            if (lastView == null || toggle) return;

            var randomModel = GetFirstNotShownModel();
            if (randomModel == null)
            {
                ShuffleImages();
                return;
            }

            lastView.GameObject.sprite = randomModel.Sprite;
            lastView.Model = randomModel;
            lastView.Ready = true;

            toggle = true;
        }

        public void OnScreenChanged(int screenIndex)
        {
            toggle = false;
            
            if (lastView != null) UpdateCounters(screenIndex);
            lastView = currentView;

            currentView = GetViewByScreenIndex(screenIndex);
            nextView = GetNextView(screenIndex);

            currentScreen = screenIndex;

            imageDescription.text = currentView.Model.Description;
            currentView.Model.Shown = true;
            currentView.Model.LastScreenIndex = currentScreen;

            foreach (var view in imageViews)
            {
                if (view.Model.Shown && Math.Abs(view.Model.LastScreenIndex - currentScreen) > 1 || !view.Ready)
                {
                    var randomModel = GetFirstNotShownModel();
                    if (randomModel == null)
                    {
                        ShuffleImages();
                        break;
                    }

                    view.GameObject.sprite = randomModel.Sprite;
                    view.Model = randomModel;
                    view.Ready = true;
                }
            }
        }

        private void ShuffleImages()
        {
            if (images.Length == 0) return;
            toggle = true;
            var rnd = new System.Random();
            randomOrderedImages = images.OrderBy(x => rnd.Next()).ToList();

            randomOrderedImages.Remove(currentView.Model);
            var viewIndex = imageViews.IndexOf(currentView);
            imageViews.Remove(currentView);

            for (var i = 0; i < imageViews.Count; i++)
            {
                var view = imageViews[i];
                var model = randomOrderedImages[i];

                view.Model = model;
                view.GameObject.sprite = model.Sprite;
                model.Shown = false;
                view.Ready = true;
            }

            imageViews.Insert(viewIndex, currentView);
        }

        private ImageView GetNextView(int screenIndex)
        {
            if (currentScreen > screenIndex)
            {
                return GetViewByScreenIndex(screenIndex - 1);
            }

            return GetViewByScreenIndex(screenIndex + 1);
        }

        private void UpdateCounters(int screenIndex)
        {
            if (screenIndex > currentScreen)
                //Swipe to right
                ++RightCounter;
            else
                //Swipe to left
                ++LeftCounter;
        }

        private ImageModel GetFirstNotShownModel()
        {
            var tmp = new List<ImageView>();
            foreach (var view in imageViews)
            {
                if (!view.Ready) continue;
                if (view.Model.Shown) continue;
                if (view == nextView) continue;
                if (view == lastView) continue;
                if (view.Model == lastView.Model) continue;
                if (view.Model == nextView.Model) continue;

                tmp.Add(view);
            }

            // shuffle images if < 2 not shown images left, because we have left and right way to swipe 
            if (tmp.Count > 1)
            {
                tmp[0].Ready = false;
                return tmp[0].Model;
            }

            return null;
        }

        private ImageView GetViewByScreenIndex(int screenIndex)
        {
            if (screenIndex > 0)
                return imageViews[screenIndex % imageViews.Count];

            var coef = screenIndex % imageViews.Count;
            var indx = coef == 0 ? coef : Math.Abs(imageViews.Count + coef);
            return imageViews[indx];
        }

        private void LoadData()
        {
            LeftCounter = PlayerPrefs.GetInt("leftCounter", leftCounter);
            RightCounter = PlayerPrefs.GetInt("rightCounter", rightCounter);

            var currentModelIndex = PlayerPrefs.GetInt("currentModelIndex", 0);
            var currentModel = images[currentModelIndex];
            currentView = GetViewByScreenIndex(0);
            
            currentView.Model = currentModel;
            currentView.GameObject.sprite = currentModel.Sprite;
            ShuffleImages();
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt("leftCounter", LeftCounter);
            PlayerPrefs.SetInt("rightCounter", RightCounter);

            var currentModelIndex = Array.IndexOf(images, currentView.Model);
            PlayerPrefs.SetInt("currentModelIndex", currentModelIndex);
            PlayerPrefs.Save();
        }

        private void OnDestroy()
        {
            SaveData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }
    }
}