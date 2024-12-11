using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceJump
{
    public class GameController : MonoBehaviour
    {
        private const float SpawnInterval = 0.5f;
        private const float InitTimerValue = 10;
        private const float EffectDuration = 1f;

        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Player _player;
        [SerializeField] private PlatformSpawner _platformSpawner;
        [SerializeField] private GameType _gameType;
        [SerializeField] private EndScreen _endScreen;
        [SerializeField] private GameObject _fireSprite;
        [SerializeField] private GameObject _redScreen;
        [SerializeField] private GameObject _plusOneSprite;
        [SerializeField] private ObjectReturner _objectReturner;
        [SerializeField] private PlatformPositionProvider _platformPositionProvider;
        
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
        
        private void OnEnable()
        {
            _platformSpawner.BombCatched += ProcessBombCatched;
            _platformSpawner.CrystalCatched += ProcessCrystalCatched;

            _endScreen.PlayAgain += StartNewGame;
            _objectReturner.PlayerCatched += ProcessGameEnd;
        }

        private void OnDisable()
        {
            _platformSpawner.BombCatched -= ProcessBombCatched;
            _platformSpawner.CrystalCatched -= ProcessCrystalCatched;

            _endScreen.PlayAgain -= StartNewGame;
            _objectReturner.PlayerCatched -= ProcessGameEnd;
        }
        
        private void Start()
        {
            StartNewGame();
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
                    _player.StartFollowingTouch();

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
                _platformSpawner.Spawn();
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
        
        private void ProcessCrystalCatched()
        {
            _crystalsCatched++;
            _score++;
            _timer += 2;
            _scoreText.text = _score.ToString();
            StartCoroutine(ShowPlusOneEffect());
        }
        
        private IEnumerator ShowPlusOneEffect()
        {
            _plusOneSprite.SetActive(true);
            _plusOneSprite.transform.position = new Vector2(_player.transform.position.x, _player.transform.position.y + 0.6f);

            yield return new WaitForSeconds(EffectDuration);

            _plusOneSprite.SetActive(false);
        }
        
        private void ProcessBombCatched()
        {
            _bombsCatched++;
            _scoreText.text = _score.ToString();
            StartCoroutine(ShowFireAndRedScreenEffect());
        }
        
        private IEnumerator ShowFireAndRedScreenEffect()
        {
            _fireSprite.SetActive(true);
            _fireSprite.transform.position = _player.transform.position;
            _redScreen.SetActive(true);
            _player.StopFollowingTouch();

            yield return new WaitForSeconds(EffectDuration);

            _fireSprite.SetActive(false);
            _redScreen.SetActive(false);
            SetGameState(GameState.End);
        }
        
        private void ProcessGameEnd()
        {
            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);

            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);

            _endScreen.Enable(_score);
            _platformSpawner.ReturnAllObjectsToPool();

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
            _fireSprite.SetActive(false);
            _redScreen.SetActive(false);
            _plusOneSprite.SetActive(false);

            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);

            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);

            _spawnCoroutine = null;
            _timerCoroutine = null;

            _player.ResetPosition();
            _platformPositionProvider.ResetProvider(_player.transform.position.y);
            
            _scoreText.text = _score.ToString();
            _timerText.text = Mathf.CeilToInt(_timer).ToString();
            _platformSpawner.ReturnAllObjectsToPool();
        }
    }
}
