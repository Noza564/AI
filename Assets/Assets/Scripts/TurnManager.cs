using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Character[] characters;
    [SerializeField] private float nextTurnDelay = 1.0f;

    private int curCharacterIndex = -1;
    public Character CurrentCharacter;

    public event UnityAction<Character> OnBeginTurn;
    public event UnityAction<Character> OnEndTurn;

    // Singleton
    public static TurnManager Instance;

    void Awake ()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void OnEnable ()
    {
        Character.OnDie += OnCharacterDie;
    }

    void OnDisable ()
    {
        Character.OnDie -= OnCharacterDie;
    }

    void Start ()
    {
        BeginNextTurn();
    }

    // Called when a new player is ready for their turn.
    public void BeginNextTurn ()
    {
        curCharacterIndex++;

        if(curCharacterIndex == characters.Length)
            curCharacterIndex = 0;

        CurrentCharacter = characters[curCharacterIndex];
        OnBeginTurn?.Invoke(CurrentCharacter);
    }

    // Called after the current character has casted their combat action.
    public void EndTurn ()
    {
        OnEndTurn?.Invoke(CurrentCharacter);
        Invoke(nameof(BeginNextTurn), nextTurnDelay);
    }

    // Called when a character dies.
    void OnCharacterDie (Character character)
    {
        if(character.IsPlayer)
            Debug.Log("You lost!");
        else
            Debug.Log("You win!");
    }
}