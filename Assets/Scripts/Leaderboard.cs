 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    int leaderboardID = 8413;
    public TMP_Text playerNames;
    public TMP_Text playerScores;
    public Leaderboard leaderboardPlayerPrefab;
    public GameObject leaderboardUI;
    public Transform leaderboardPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) => 
        {
            if(response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighScoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreListMain(leaderboardID, 10, 0, (response) =>
        {
            if(response.success)
            {
                clearLeaderboardPanel();
                string tempPlayerNames = "";
                //string tempPlayerScores = "PPS\n";

                LootLockerLeaderboardMember[] members = response.items;

                for(int i = 0; i < members.Length; i++)
                {
                    if(members[i].player.name != "")
                    {
                        tempPlayerNames = members[i].rank + ". " + members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames = members[i].player.id.ToString();
                    }
                    Leaderboard player = Instantiate(leaderboardPlayerPrefab, leaderboardPanel);
                    player.playerNames.text = tempPlayerNames;
                    player.playerScores.text = members[i].score.ToString() + " PPS";

                    // tempPlayerScores += members[i].score + "\n";
                    // tempPlayerNames += "\n";
                }
                done = true;
                
                // playerNames.text = tempPlayerNames;
                // playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Failed: " + response.Error);
                done = true;
                
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public void LeaderboardController()
    {
        if(leaderboardUI.activeSelf)
        {
            leaderboardUI.SetActive(false);
        } 

        else
        {
            leaderboardUI.SetActive(true);
            
        }
    }

    public void clearLeaderboardPanel()
    {
        int children = leaderboardPanel.childCount;
        //print(children);
        for(int i = 1; i < children; i++)
        {
            // print(i);
            // print(leaderboardPanel.GetChild(i));
            Destroy(leaderboardPanel.GetChild(i).gameObject);
        }
    }
}
