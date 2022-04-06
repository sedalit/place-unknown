using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Play,
    Pause
}
public class GameStateController : MonoBehaviour
{
    public static UnityAction<GameState> GameStateChanged;

    private GameState currentState;

    private void Start()
    {
        currentState = GameState.Play;
        GameStateChanged += OnGameStateChanged;
    }

    private void OnDisable() => GameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState state) => currentState = state;
}
