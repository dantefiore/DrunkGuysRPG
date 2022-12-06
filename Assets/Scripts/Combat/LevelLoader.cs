using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//LATER WE MAY NEED TO ADD A LOADING SCREEN TO MAKE IT LOOK BETTER
public class LevelLoader : MonoBehaviour
{
    // MAKES SURE THAT THIS IS THE ONLY INSTANCE OF THIS SCRIPT
    #region Singleton
    public static LevelLoader instance;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public Animator transition; // the transition animation
    public float transitionTime = 1f;   // how long it waits before it plays again

    // CALLS THE COROUTINE LOADNAMESLEVEL
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadNamedLevel(levelName));
    }

    /* PLAYS THE STARTS THE ANIMATION, WAITS A SECOND, THEN PLAYS THE END ANIMATION */
    IEnumerator LoadNamedLevel(string levelName)
    {
        // Start transition animation
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);

        // End transition animation
        transition.SetTrigger("End");
    }
}
