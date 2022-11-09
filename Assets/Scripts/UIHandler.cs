using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIHandler : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    [SerializeField] Vector2 shopUITransform;
    [SerializeField] GameObject leaderboardUI;
    [SerializeField] Vector2 leaderboardUITransform;

    // Start is called before the first frame update
    void Start()
    {
        shopUITransform = shopUI.transform.localPosition;
        if(leaderboardUI){
            leaderboardUITransform = leaderboardUI.transform.localPosition;
        }

        shopUI.transform.localPosition = new Vector2(0, -1000);
        shopUI.SetActive(true);

        leaderboardUI.transform.localPosition = new Vector2(0, -1000);
        leaderboardUI.SetActive(true);
    }

    public void ShopController()
    {
            if(leaderboardUI.transform.localPosition.y == leaderboardUITransform.y){
                leaderboardUI.transform.LeanMoveLocalY(-1000, 0.3f).setEaseInExpo();
            }

            if(shopUI.transform.localPosition.y == shopUITransform.y){
                shopUI.transform.LeanMoveLocalY(-1000, 0.3f).setEaseInExpo();
            }

            else{
                shopUI.transform.localPosition = new Vector2(0, -1000);
                shopUI.SetActive(true);
            
                shopUI.transform.LeanMoveLocalY(shopUITransform.y, 0.3f).setEaseInExpo();
            }
    }

    public void LeaderboardController()
    {
            if(shopUI.transform.localPosition.y == shopUITransform.y){
                shopUI.transform.LeanMoveLocalY(-1000, 0.3f).setEaseInExpo();
            }
            
            if(leaderboardUI.transform.localPosition.y == leaderboardUITransform.y){
                leaderboardUI.transform.LeanMoveLocalY(-1000, 0.3f).setEaseInExpo();
            }

            else{
                leaderboardUI.transform.localPosition = new Vector2(0, -1000);
                leaderboardUI.SetActive(true);
            
                leaderboardUI.transform.LeanMoveLocalY(leaderboardUITransform.y, 0.3f).setEaseInExpo();
            }

    }
}
