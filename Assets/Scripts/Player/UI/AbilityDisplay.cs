using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AbilityDisplay : MonoBehaviour
{

    public GameObject abilityDisplayPrefab;

    AbilityController abilityController;


    List<Image> foundImages;
    List<TMPro.TextMeshProUGUI> foundLabels;
    TMPro.TextMeshProUGUI abilityUseCount;

    private void Awake()
    {
        abilityController = GameObject.FindObjectOfType<AbilityController>();

        abilityUseCount = GameObject.FindGameObjectWithTag("Ability Count").GetComponent<TMPro.TextMeshProUGUI>();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
