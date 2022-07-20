using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class AbilityDisplay : MonoBehaviour
{

    public GameObject abilityDisplayPrefab;

    PollingStation station;


    List<Image> foundImageIcons, foundImageSelectionIndicators;
    List<TMPro.TextMeshProUGUI> foundLabels;
    TMPro.TextMeshProUGUI abilityUseCount;
    int abilityCount;

    private void Awake()
    {
        if (!PollingStation.TryRegisterStationToGameObject(ref station, gameObject.name))
        {
            return;
        }

        station.abilityDisplay = this;

        abilityUseCount = GameObject.FindGameObjectWithTag("Ability Count").GetComponent<TMPro.TextMeshProUGUI>();
        foundImageSelectionIndicators = new List<Image>();
        foundImageIcons = new List<Image>();
        foundLabels = new List<TMPro.TextMeshProUGUI>();

        abilityCount = (station.abilityController ?? FindObjectOfType<AbilityController>()).abilities.Count;

        for (int i = 0; i < abilityCount; i++)
        {
            GameObject obj = Instantiate(abilityDisplayPrefab, transform);
            foundImageIcons.Add(obj.GetComponentsInChildren<Image>()[0]);
            foundImageSelectionIndicators.Add(obj.GetComponentsInChildren<Image>()[1]);
            foundLabels.Add(obj.GetComponentInChildren<TMPro.TextMeshProUGUI>());
        }
    }

    public void SetHotbarActive(bool aValue, float stateChangeDuration, bool affectEverything = false)
    {
        for (int i = 0; i < abilityCount; i++)
        {
            foundImageIcons[i].DOFade(aValue ? 1 : 0, stateChangeDuration);
            foundLabels[i].DOFade(aValue ? 1 : 0, stateChangeDuration);
            if (affectEverything)
                foundImageSelectionIndicators[i].DOFade(aValue ? 1 : 0, stateChangeDuration);
        }
    }

    public void UpdateHotbarSelectionIndicator(int anIndex, float stateChangeDuration)
    {
        for (int i = 0; i < abilityCount; i++)
        {
            foundImageSelectionIndicators[i].DOFade(0, stateChangeDuration);
        }


        foundImageSelectionIndicators[anIndex].DOFade(1, stateChangeDuration);
    }
}
