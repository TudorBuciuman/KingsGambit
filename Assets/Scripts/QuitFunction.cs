using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitFunction : MonoBehaviour
{
    private Image quitImage;

    [SerializeField]
    private Sprite[] sprites;

    private float frames;

    private void Awake()
    {
        quitImage = base.transform.GetChild(0).GetComponent<Image>();
    }

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (frames < 1f)
            {
                frames += Time.deltaTime;
            }
            if (frames > 1f)
            {
                frames = 1f;
            }
        }
        else if (frames > 0f)
        {
            frames -= Time.deltaTime * 2f;
            if (frames < 0f)
            {
                frames = 0f;
            }
        }
        quitImage.color = new Color(1f, 1f, 1f, frames * 2f);
        quitImage.sprite = sprites[Mathf.Clamp(Mathf.RoundToInt(frames * 7f), 0, 4)];
        if (frames == 1f)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
                Application.Quit();
            else if (SceneManager.GetActiveScene().buildIndex == 2)
                FindFirstObjectByType<GameManager>().SaveAndClose("Game UI");
            else
                SceneManager.LoadScene("Game UI");
                Time.timeScale = 1f;
            frames = 0f;
        }
    }

    public void SetSprites(Sprite[] s)
    {
        sprites = s;
    }

    public Sprite[] GetSprites()
    {
        return sprites;
    }
}
