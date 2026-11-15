using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanel : MonoBehaviour
{
    public Button[] optionButtons;     // los 3 botones (arrastra las 3 cartas)
    public TMP_Text[] optionTexts;     // un texto por carta, en el mismo orden
    public UpgradeManager upgradeManager;

    private Action<int> onChosen;

    private void Awake()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // copia local, necesaria para el lambda
            optionButtons[i].onClick.AddListener(() => Choose(index));
        }
    }

    public void Show(Action<int> callback)
    {
        onChosen = callback;

        // Sortea una mejora por carta y escribe su texto
        if (upgradeManager != null)
        {
            string[] texts = upgradeManager.RollOptions();
            for (int i = 0; i < optionTexts.Length && i < texts.Length; i++)
            {
                if (optionTexts[i] != null) optionTexts[i].text = texts[i];
            }
        }

        gameObject.SetActive(true);
    }

    private void Choose(int index)
    {
        if (upgradeManager != null) upgradeManager.Apply(index); // aplica la mejora
        gameObject.SetActive(false);
        onChosen?.Invoke(index);                                 // sigue la siguiente ronda, como antes
    }
}