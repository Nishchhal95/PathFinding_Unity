using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PathFindingManager pathFindingManager;

    [Header("Main Options")]
    [SerializeField] TMP_Dropdown pathFindingAlgorithmDropdown;
    [SerializeField] Toggle debugToggle;
    [SerializeField] Button findPathButton;

    [Header("Debug Options")]
    [SerializeField] GameObject debugButtonsContainer;
    [SerializeField] Button prevButton;
    [SerializeField] Button pauseButton;
    [SerializeField] Button nextButton;

    [Header("Grid Options")]
    [SerializeField] Button clearPathButton;
    [SerializeField] Button clearLevelButton;

    private void Start()
    {
        SetupDropdown();
    }

    private void OnEnable()
    {
        pathFindingAlgorithmDropdown.onValueChanged.AddListener(PathFindingAlgorithmChanged);
        debugToggle.onValueChanged.AddListener(DebugValueChanged);
        findPathButton.onClick.AddListener(FindPathClicked);

        prevButton.onClick.AddListener(PrevButtonClicked);
        pauseButton.onClick.AddListener(PauseButtonClicked);
        nextButton.onClick.AddListener(NextButtonClicked);

        clearPathButton.onClick.AddListener(ClearPathButtonClicked);
        clearLevelButton.onClick.AddListener(ClearLevelButtonClicked);
    }

    private void OnDisable()
    {
        pathFindingAlgorithmDropdown.onValueChanged.RemoveListener(PathFindingAlgorithmChanged);
        debugToggle.onValueChanged.RemoveListener(DebugValueChanged);
        findPathButton.onClick.RemoveListener(FindPathClicked);

        prevButton.onClick.RemoveListener(PrevButtonClicked);
        pauseButton.onClick.RemoveListener(PauseButtonClicked);
        nextButton.onClick.RemoveListener(NextButtonClicked);

        clearPathButton.onClick.RemoveListener(ClearPathButtonClicked);
        clearLevelButton.onClick.RemoveListener(ClearLevelButtonClicked);
    }

    private void SetupDropdown()
    {
        List<string> pathFindingAlgorithmNames = new List<string>();
        foreach (int i in Enum.GetValues(typeof(PathFindingAlgorithm)))
        {
            pathFindingAlgorithmNames.Add(((PathFindingAlgorithm)i).ToString());
        }
        pathFindingAlgorithmDropdown.AddOptions(pathFindingAlgorithmNames);
    }

    private void PathFindingAlgorithmChanged(int index)
    {

        pathFindingManager.PathFindingAlgorithm = (PathFindingAlgorithm)index;
    }

    private void DebugValueChanged(bool debugValue)
    {
        pathFindingManager.IsDebug = !pathFindingManager.IsDebug;
        debugButtonsContainer.SetActive(pathFindingManager.IsDebug);
    }

    private void FindPathClicked()
    {
        pathFindingManager.StartPathFinding();
    }

    private void PrevButtonClicked()
    {
        //TODO: Implement Me
    }

    private void PauseButtonClicked()
    {
        //TODO: Implement Me
    }

    private void NextButtonClicked()
    {
        //TODO: Implement Me
    }

    private void ClearPathButtonClicked()
    {
        gridManager.ClearPath();
        pathFindingManager.StartTile = null;
        pathFindingManager.EndTile = null;
    }

    private void ClearLevelButtonClicked()
    {
        gridManager.ClearLevel();
    }
}

public enum PathFindingAlgorithm
{
    BFS,
    DFS,
    GreedySearch,
    Dijskstra,
    AStar
}
