using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BombEscape
{
    public class GameController : MonoBehaviour
    {
        private const float SpawnInterval = 0.5f;
        private const float InitTimerValue = 25;
        private const float EffectDuration = 1f;
        
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private ObjectSpawner _objectSpawner;
        [SerializeField] private GameType _gameType;
        [SerializeField] private EndScreen _endScreen;
        [SerializeField] private GameObject _minus100Sprite;
        [SerializeField] private GameObject _redScreen;
        [SerializeField] private GameObject _plusOneSprite;
        [SerializeField] private Player _player;

        private int _score;
        private int _crystalsCatched;
        private int _bombsCatched;
        private float _timer;
        
        private IEnumerator _spawnCoroutine;
        private IEnumerator _timerCoroutine;
        
        private enum GameState
        {
            Starting,
            Playing,
            End,
        }
        
        private GameState _currentGameState;
        
        private void Start()
        {
            StartNewGame();
        }
        
        private void OnEnable()
        {
            _player.BombCatched += ProcessBombCatched;
            _player.CrystalCatched += ProcessCrystalCatched;

            _endScreen.PlayAgain += StartNewGame;
        }

        private void OnDisable()
        {
            _player.BombCatched -= ProcessBombCatched;
            _player.CrystalCatched -= ProcessCrystalCatched;

            _endScreen.PlayAgain -= StartNewGame;
        }
        
        public void ProcessGoToMainScene()
        {
            SceneManager.LoadScene("MainScene");
        }
        
        private void StartNewGame()
        {
            _endScreen.Disable();
            SetGameState(GameState.Playing);
        }

        private void SetGameState(GameState newState)
        {
            _currentGameState = newState;

            switch (_currentGameState)
            {
                case GameState.Starting:
                    ResetAllValues();
                    break;

                case GameState.Playing:
                    ResetAllValues();
                    _player.StartDetectingTouch();

                    if (_spawnCoroutine == null)
                    {
                        _spawnCoroutine = StartSpawning();
                        StartCoroutine(_spawnCoroutine);
                    }

                    if (_timerCoroutine == null)
                    {
                        _timerCoroutine = TimerCountdown();
                        StartCoroutine(_timerCoroutine);
                    }

                    break;

                case GameState.End:
                    ProcessGameEnd();
                    break;
            }
        }
        
        private IEnumerator StartSpawning()
        {
            WaitForSeconds interval = new WaitForSeconds(SpawnInterval);

            while (true)
            {
                _objectSpawner.Spawn();
                yield return interval;
            }
        }
        
        private IEnumerator TimerCountdown()
        {
            _timer = InitTimerValue;

            while (_timer > 0)
            {
                _timer -= Time.deltaTime;

                int minutes = Mathf.FloorToInt(_timer / 60);
                int seconds = Mathf.FloorToInt(_timer % 60);

                _timerText.text = $"{minutes:00}:{seconds:00}";

                yield return null;
            }

            _timer = 0;
            _timerText.text = "00:00";
            SetGameState(GameState.End);
        }
        
        private void ProcessCrystalCatched(ClickableObject @object)
        {
            _crystalsCatched++;
            _score++;
            StartCoroutine(ShowPlusOneEffect(@object.transform));
            _objectSpawner.ReturnToPool(@object);
            _scoreText.text = _score.ToString();
        }
        
        private IEnumerator ShowPlusOneEffect(Transform position)
        {
            _plusOneSprite.SetActive(true);
            _plusOneSprite.transform.position = position.position;

            yield return new WaitForSeconds(EffectDuration);

            _plusOneSprite.SetActive(false);
        }
        
        private void ProcessBombCatched(ClickableObject clickableObject)
        {
            _bombsCatched++;
            _score = Mathf.Clamp(_score - 100, 0, int.MaxValue);

            StartCoroutine(ShowMinus100AndRedScreenEffect(clickableObject.transform));
            _objectSpawner.ReturnToPool(clickableObject);
            _scoreText.text = _score.ToString();

            if (_timer <= 0)
            {
                SetGameState(GameState.End);
            }
        }
        
        private IEnumerator ShowMinus100AndRedScreenEffect(Transform transform)
        {
            _minus100Sprite.SetActive(true);
            _minus100Sprite.transform.position = transform.position;

            _redScreen.SetActive(true);

            yield return new WaitForSeconds(EffectDuration);

            _minus100Sprite.SetActive(false);
            _redScreen.SetActive(false);
        }

        private void ProcessGameEnd()
        {
            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);

            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);

            _player.StopDetectingTouch();
            _endScreen.Enable(_score);
            _objectSpawner.ReturnAllObjectsToPool();

            if (_score > 0)
            {
                PlayerBalanceController.IncreaseBalance(_score);
                GameProgressSaver.UpdateGameProgress(_gameType, _bombsCatched, _crystalsCatched);
                HistoryDataHolder.AddNewData(_gameType, _score);
            }
        }
        
        private void ResetAllValues()
        {
            _score = 0;
            _crystalsCatched = 0;
            _bombsCatched = 0;
            _timer = InitTimerValue;
            _minus100Sprite.SetActive(false);
            _redScreen.SetActive(false);
            _plusOneSprite.SetActive(false);

            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);

            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);

            _player.StopDetectingTouch();
            _spawnCoroutine = null;
            _timerCoroutine = null;

            _scoreText.text = _score.ToString();
            _timerText.text = Mathf.CeilToInt(_timer).ToString();
            _objectSpawner.ReturnAllObjectsToPool();
        }
    }
}
