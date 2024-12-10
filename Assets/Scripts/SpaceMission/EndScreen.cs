using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceMission
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rewardText;

        private ScreenVisabilityHandler _screenVisabilityHandler;

        public event Action PlayAgain;
        
        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        public void Enable(int score)
        {
            _screenVisabilityHandler.EnableScreen();
            _rewardText.text = score.ToString();
        }

        public void Disable()
        {
            _screenVisabilityHandler.DisableScreen();
        }

        public void OnPlayAgain()
        {
            PlayAgain?.Invoke();
            Disable();
        }

        public void GoToMainScene()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
