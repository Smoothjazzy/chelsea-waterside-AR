using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

public enum FountainType { WATER, POPCORN, BUBBLES, SPRINKLES, CONFETTI }

public class ManagerScript : MonoBehaviour {

    public static FountainType CurrentFountain;

    public GameObject PopcornButton;
    public GameObject BubbleButton;
    public GameObject SprinklesButton;
    public GameObject ConfettiButton;
    public Button ClearButton;
    [SerializeField] private ARSessionController ARSessionController;

    private GameObject[] Buttons;
    private Button popcornButtonButton;
    private Button bubbleButtonButton;
    private Button sprinklesButtonButton;
    private Button confettiButtonButton;

    [Range(0,30)]
    [SerializeField] private int buttonTimer;
    [Range(0, 60)]
    public int selectionDuration;

    public static ManagerScript instance;

    public UnityAction<FountainType> FountainSelected;

    public event Action OnClear;

    private bool fountainVisible = false;

    private void Awake()
    {
        instance = this;

        popcornButtonButton = PopcornButton.GetComponent<Button>();
        bubbleButtonButton = BubbleButton.GetComponent<Button>();
        sprinklesButtonButton = SprinklesButton.GetComponent<Button>();
        confettiButtonButton = ConfettiButton.GetComponent<Button>();
        Buttons = new GameObject[]{ PopcornButton, BubbleButton, ConfettiButton, SprinklesButton  };
    }
        
    void Start () {
        popcornButtonButton.onClick.AddListener(() => { FountainSelected?.Invoke(FountainType.POPCORN); PopcornButton.SetActive(false); });
        bubbleButtonButton.onClick.AddListener(() => { FountainSelected?.Invoke(FountainType.BUBBLES); BubbleButton.SetActive(false); });
        sprinklesButtonButton.onClick.AddListener(() => { FountainSelected?.Invoke(FountainType.SPRINKLES); SprinklesButton.SetActive(false); });
        confettiButtonButton.onClick.AddListener(() => { FountainSelected?.Invoke(FountainType.CONFETTI); ConfettiButton.SetActive(false); });
        ClearButton.onClick.AddListener(ClearClicked);

        FountainSelected?.Invoke(FountainType.WATER);
        CurrentFountain = FountainType.WATER;

        foreach (GameObject button in Buttons) button.SetActive(false);
        ClearButton.gameObject.SetActive(false);

        ARSessionController.SessionStarted += InitiateButtons;
	}

    private void OnDestroy()
    {
        popcornButtonButton.onClick.RemoveAllListeners();
        bubbleButtonButton.onClick.RemoveAllListeners();
        sprinklesButtonButton.onClick.RemoveAllListeners();
        confettiButtonButton.onClick.RemoveAllListeners();
        ClearButton.onClick.RemoveListener(ClearClicked);
    }

    void Update () {
		
	}
    
    private async void InitiateButtons()
    {
        ClearButton.gameObject.SetActive(true);
        fountainVisible = true;

        while (fountainVisible) 
        {
            await Task.Delay((Random.Range(buttonTimer/2, buttonTimer) * 1000));
            ShowButton(Random.Range(0, Buttons.Length));
        }
            
    }

    void ShowButton(int index)
    {
        for (int i = 0; i < Buttons.Length; i++ )
        {
            if (i == index)
            {
                Buttons[i].SetActive(true);
            }
        }
    }

    void ClearClicked()
    {
        ClearButton.gameObject.SetActive(false);
        fountainVisible = false;
        OnClear?.Invoke();
    }
    
}

