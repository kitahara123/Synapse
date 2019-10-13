using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Synapse
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button achievement;
        [SerializeField] private string achievementLabel;
        [SerializeField] private Button settings;
        [SerializeField] private string settingsLabel;
        [SerializeField] private Button about;
        [SerializeField] private string aboutLabel;

        private void Start()
        {
            achievement.onClick.AddListener(ShowAchievements);
            settings.onClick.AddListener(ShowSettings);
            about.onClick.AddListener(ShowAbout);

            var achiviementTM = achievement.GetComponentInChildren<TextMeshProUGUI>();
            if (achiviementTM != null) achiviementTM.text = achievementLabel;
            
            var settingsTM = settings.GetComponentInChildren<TextMeshProUGUI>();
            if (settingsTM != null) settingsTM.text = settingsLabel;
            
            var aboutTM = about.GetComponentInChildren<TextMeshProUGUI>();
            if (aboutTM != null) aboutTM.text = aboutLabel;
        }

        private void OnMouseDown()
        {
            SceneManager.LoadScene(1);
        }

        private void ShowAchievements()
        {
            print("ShowAchievements");
        }
        private void ShowSettings()
        {
            print("ShowSettings");
        }
        private void ShowAbout()
        {
            print("ShowAbout");
        }
    }
}