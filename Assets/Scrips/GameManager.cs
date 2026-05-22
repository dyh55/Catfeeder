using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuPanel; // 在 Inspector 中关联“暂停菜单”面板

    private bool isPaused = false;     // 记录当前游戏是否处于暂停状态

    void Start()
    {
        // 开始游戏时，确保时间流逝，并隐藏暂停菜单
        Time.timeScale = 1f;
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        // 监听 Escape 键来呼出或关闭暂停菜单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // 方法：开始游戏（通常用于主菜单的"开始"按钮）
    public void StartGame()
    {
        // 这里填你的游戏主场景名字，例如 "GameScene"
        SceneManager.LoadScene("GameScene");
    }

    // 方法：暂停游戏
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // 将游戏时间流速设为 0，实现全局暂停
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true); // 显示暂停菜单面板
        // 可选: 解锁并显示鼠标光标
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 方法：继续游戏
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 恢复正常游戏速度
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false); // 隐藏暂停菜单面板
        // 可选: 重新锁定并隐藏鼠标光标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 方法：退出游戏
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // 编辑模式下停止运行
        #else
            Application.Quit(); // 打包后关闭应用
        #endif
    }

    // 方法：重新开始当前关卡
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}