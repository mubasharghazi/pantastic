using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Buttons ko control karne ke liye zaroori

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainMenuPanel;      // Wo panel jisme Play/Levels/Exit buttons hain
    public GameObject LevelSelectPanel;   // Wo panel jisme Level 1, 2, 3... buttons hain

    [Header("Level Buttons")]
    // Yahan hum apne 4 buttons drag karenge (Btn_Lvl1, Btn_Lvl2, etc.)
    public Button[] LevelButtons;

    void Start()
    {
        // Shuru mein Level Panel band aur Main Menu on
        if (LevelSelectPanel != null) LevelSelectPanel.SetActive(false);
        if (MainMenuPanel != null) MainMenuPanel.SetActive(true);

        CheckLevels(); // Game start hote hi check karo kaunse levels khule hain
    }

    void CheckLevels()
    {
        // Unity se poocho ke kitne level khule hain (Default = 1)
        int unlockedLevel = PlayerPrefs.GetInt("LevelsUnlocked", 1);

        for (int i = 0; i < LevelButtons.Length; i++)
        {
            // Logic: Agar Button ka number unlocked level se chota ya barabar hai
            // i=0 means Level 1. So 0+1 = 1.
            if (i + 1 <= unlockedLevel)
            {
                LevelButtons[i].interactable = true; // Button ON (Clickable)
            }
            else
            {
                LevelButtons[i].interactable = false; // Button OFF (Dim/Locked)
            }
        }
    }

    // --- NAVIGATION FUNCTIONS (Buttons se connect karne ke liye) ---

    public void PlayGame()
    {
        // Play button dabane par hamesha Level 1 chalate hain
        SceneManager.LoadScene("game");
    }

    public void OpenLevelSelect()
    {
        if (MainMenuPanel != null) MainMenuPanel.SetActive(false);
        if (LevelSelectPanel != null) LevelSelectPanel.SetActive(true);
        CheckLevels(); // Jab bhi panel kholo, fresh check karo
    }

    public void BackToMenu()
    {
        if (LevelSelectPanel != null) LevelSelectPanel.SetActive(false);
        if (MainMenuPanel != null) MainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // --- LEVEL LOAD FUNCTION ---
    // Har button ke liye alag function banane ki bajaye ek hi function use karenge
    // Button ke OnClick mein value (1, 2, 3, 4) pass karna mat bhoolna
    public void LoadLevelByIndex(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}