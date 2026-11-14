using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public Button[] optionButtons;   // los 3 botones (arrastra las 3 cartas)

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
        gameObject.SetActive(true);
    }

    private void Choose(int index)
    {
        gameObject.SetActive(false);
        onChosen?.Invoke(index);
    }
}