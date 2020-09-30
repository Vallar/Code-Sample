using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Soda;
using UnityEngine;
using UnityEngine.UI;

public class BuildBase : MonoBehaviour
{
    [SerializeField] private GameEvent switchNoneState;
    [SerializeField] private GlobalVector2 basePosition;
    [SerializeField] private Transform basePrefab;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Transform playerBase = Instantiate(basePrefab, basePosition.value, Quaternion.identity);
        
        PlayerEntitiesList.entities.Add(playerBase);

        switchNoneState.Raise();
     
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }
}
