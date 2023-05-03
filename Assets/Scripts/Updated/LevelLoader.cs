using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        LoseScreenController.PlayerDidRestart += OnPlayerDidRestart;
        WinScreenController.PlayerDidWinRestart += OnPlayerDidRestart;
    }

    private void OnDestroy()
    {
        LoseScreenController.PlayerDidRestart -= OnPlayerDidRestart;
        WinScreenController.PlayerDidWinRestart -= OnPlayerDidRestart;
    }

    void Update()
    {
        if (currentSceneIndex == 0 && Input.anyKeyDown)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    private void OnPlayerDidRestart()
    {
        StartCoroutine(LoadLevel(currentSceneIndex));
    }
}
