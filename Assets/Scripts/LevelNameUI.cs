using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LevelNameUI : MonoBehaviour
{
    public TextMeshProUGUI CurrentLevelName;
    public static UnityEvent<string> OnChangeLevelName = new UnityEvent<string>();

    private void OnEnable() => OnChangeLevelName.AddListener(SetText);

    private void OnDisable() => OnChangeLevelName.RemoveListener(SetText);

    private void SetText(string levelName) => CurrentLevelName.text = levelName;
}

  
