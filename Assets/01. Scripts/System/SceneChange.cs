using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료 요청됨");

#if UNITY_EDITOR
        // ▶ 에디터에서 테스트 중일 때
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ▶ 실제 빌드된 게임에서
        Application.Quit();
#endif
    }
}
