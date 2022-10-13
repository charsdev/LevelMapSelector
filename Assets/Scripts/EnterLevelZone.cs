using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLevelZone : MonoBehaviour
{
    public string LevelName;
    public bool OnZone;
    private Walker _walker;

    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && OnZone)
        {
            SceneManager.LoadScene(LevelName);
            PlayerPrefs.SetString("LastWaypoint", gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Walker"))
        {
            OnZone = true;
            _walker = collision.GetComponent<Walker>();
            LevelNameUI.OnChangeLevelName.Invoke(LevelName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Walker"))
        {
            OnZone = false;
            LevelNameUI.OnChangeLevelName.Invoke("...");
            _walker = null;
        }
    }
}
