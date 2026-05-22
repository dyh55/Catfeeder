using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject newGameBtn;
    private GameObject continueBtn;
    private GameObject exitBtn;
    private GameObject title;

    private enum GameState { Menu, Playing, Paused }
    private GameState currentState = GameState.Menu;

    void Start()
    {
        // 自动查找按钮（名称必须完全一致）
        newGameBtn = GameObject.Find("NewGameButton");
        continueBtn = GameObject.Find("ContinueButton");
        exitBtn = GameObject.Find("ExitButton");
        title = GameObject.Find("Title");

        // 动态绑定并清除旧监听
        BindButton(newGameBtn, () => { Debug.Log(">>> NewGameButton 被点击 <<<"); OnNewGame(); });
        BindButton(continueBtn, () => { Debug.Log(">>> ContinueButton 被点击 <<<"); OnContinue(); });
        BindButton(exitBtn, () => { Debug.Log(">>> ExitButton 被点击 <<<"); OnExit(); });

        SetMenuVisible(true);
        EnableCats(false);
        Time.timeScale = 1f;
        currentState = GameState.Menu;
    }

    void Update()
    {
        // 额外输出鼠标点击状态（调试用）
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("鼠标左键被按下，当前状态：" + currentState);
        }

        if (currentState == GameState.Playing && Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
        else if (currentState == GameState.Paused && Input.GetKeyDown(KeyCode.Escape))
            ResumeGame();
    }

    public void OnNewGame()
    {
        Debug.Log("【UIManager】OnNewGame 执行");
        ResetGameWorld();
        StartGame();
    }

    public void OnContinue()
    {
        Debug.Log("【UIManager】OnContinue 执行");
        if (currentState == GameState.Paused)
            ResumeGame();
        else if (currentState == GameState.Menu)
            StartGame();
    }

    public void OnExit()
    {
        Debug.Log("Exit Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void StartGame()
    {
        SetMenuVisible(false);
        EnableCats(true);
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        Debug.Log("游戏开始 (Playing)");
    }

    private void PauseGame()
    {
        if (currentState != GameState.Playing) return;
        SetMenuVisible(true);
        Time.timeScale = 0f;
        currentState = GameState.Paused;
        Debug.Log("游戏暂停 (Paused) - Time.timeScale = 0");
    }

    private void ResumeGame()
    {
        if (currentState != GameState.Paused) return;
        SetMenuVisible(false);
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        Debug.Log("游戏恢复 (Playing) - Time.timeScale = 1");
    }

    private void SetMenuVisible(bool visible)
    {
        if (newGameBtn) newGameBtn.SetActive(visible);
        if (continueBtn) continueBtn.SetActive(visible);
        if (exitBtn) exitBtn.SetActive(visible);
        if (title) title.SetActive(visible);
    }

    private void EnableCats(bool enable)
    {
        CatController[] cats = FindObjectsOfType<CatController>();
        foreach (var cat in cats)
            cat.enabled = enable;
    }

    private void ResetGameWorld()
    {
        Debug.Log("ResetGameWorld 开始");
        if (currentState == GameState.Paused)
            Time.timeScale = 1f;

        CatController[] cats = FindObjectsOfType<CatController>();
        foreach (var cat in cats)
            cat.ResetState();

        LitterTrayController[] trays = FindObjectsOfType<LitterTrayController>();
        foreach (var tray in trays)
            tray.ResetState();

        currentState = GameState.Menu;
        SetMenuVisible(true);
        EnableCats(false);
        Time.timeScale = 1f;
        Debug.Log("游戏世界已重置，回到主菜单");
    }

    private void BindButton(GameObject btnObj, UnityEngine.Events.UnityAction action)
    {
        if (btnObj == null) return;
        Button btn = btnObj.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
        }
    }
}