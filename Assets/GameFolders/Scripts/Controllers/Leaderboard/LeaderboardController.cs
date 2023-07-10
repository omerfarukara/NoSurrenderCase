using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameFolders.Scripts.Controllers;
using GameFolders.Scripts.Controllers.AI;
using GameFolders.Scripts.Controllers.Player;
using GameFolders.Scripts.Managers;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private GameObject element;
    [SerializeField] private Transform elementParent;
    [SerializeField] private GameObject player;
    List<GameObject> obj = new List<GameObject>();

    private void OnEnable()
    {
        if (elementParent.childCount > 0)
        {
            obj.Clear();
            for (int i = 0; i < elementParent.childCount; i++)
            {
                Destroy(elementParent.GetChild(i).gameObject);
            }
        }

        StartCoroutine(Process());
    }

    IEnumerator Process()
    {
        obj.Add(player);
        foreach (var ai in GameController.Instance._ais)
        {
            obj.Add(ai.gameObject);
        }
        
        Dictionary<GameObject,int> list = new Dictionary<GameObject,int>();
        for (var i = 0; i < obj.Count; i++)
        {
            var data = obj[i];
            list.Add(data, i == 0 ? GameManager.Instance.Score : data.GetComponent<AIController>().Score);
        }
        
        List<KeyValuePair<GameObject, int>> sortedList = list.ToList();
        sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        for (int i = 0; i < sortedList.Count; i++)
        {
            var currentElement = Instantiate(element, elementParent);
            RectTransform elementRect = currentElement.transform.GetChild(0).GetComponent<RectTransform>();
            LeaderboardElement leaderboardElement = currentElement.GetComponent<LeaderboardElement>();

            AIController aiController = sortedList[i].Key.GetComponent<AIController>();
            if (aiController == null)
            {
                PlayerController playerController = sortedList[i].Key.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    leaderboardElement.nickname.text = playerController.gameObject.name;
                    leaderboardElement.score.text = GameManager.Instance.Score.ToString();
                }
            }
            else
            {
                leaderboardElement.nickname.text = aiController.gameObject.name;
                leaderboardElement.score.text = aiController.Score.ToString();
            }

            elementRect.anchorMin = new Vector2(-0.05f, 0);
            elementRect.anchorMax = new Vector2(-0.95f, 1);
            elementRect.DOAnchorMin(new Vector2(0.05f, 0), 0.5f);
            elementRect.DOAnchorMax(new Vector2(0.95f, 1), 0.5f);
            yield return new WaitForSeconds(0.15f);
        }
    }
}