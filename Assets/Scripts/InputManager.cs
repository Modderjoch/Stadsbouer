using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
#pragma warning disable 649

    [Header("Scripts")]
    [SerializeField] public List<Dice> dice;

    PlayerInput input;
    PlayerInput.PlayerActions player;

    private void Awake()
    {
        input = new PlayerInput();
        player = input.Player;

        player.DiceRoll.performed += _ => dice[0].Roll();
        player.DiceRoll.performed += _ => dice[1].Roll();
    }

    public void RefreshDice()
    {
        player.DiceRoll.performed += _ => dice[0].Roll();
        if (dice[1] != null) { player.DiceRoll.performed += _ => dice[1].Roll(); }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDestroy()
    {
        input.Disable();
    }
}