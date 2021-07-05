using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingTable : MonoBehaviour
{
    // Init
    public GameObject player;
    public GameObject snakes;

    public Player P;
    public List<BasicEnemyScript> BEs;

    public List<string> orderUsername = new List<string>();
    public List<int> orderScores = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        P = player.transform.GetChild(0).GetComponent<Player>();

        for (int i=0; i<snakes.transform.childCount; ++i)
        {
            BEs.Add(snakes.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<BasicEnemyScript>());
        }
    }

    void FixedUpdate()
    {
        // Add to ranking
        orderUsername.Add(P._username);
        orderScores.Add(P._score);

        for (int i = 0; i < BEs.Count; ++i)
        {
            orderUsername.Add(BEs[i]._username);
            orderScores.Add(BEs[i]._score);
        }

        for (int i=0; i<orderScores.Count-1; ++i)
        {
            for (int j=0; j<orderScores.Count-1; ++j)
            {
                if (orderScores[j] < orderScores[j+1])
                {
                    int tempScore = orderScores[j];
                    string tempUsername = orderUsername[j];

                    orderScores[j] = orderScores[j + 1];
                    orderScores[j + 1] = tempScore;

                    orderUsername[j] = orderUsername[j + 1];
                    orderUsername[j + 1] = tempUsername;
                }
            }
        }

        // Show
        string rankingText = "";
        for (int i=0; i<orderScores.Count; ++i)
        {
            rankingText += orderUsername[i] + " " + orderScores[i].ToString() + "\n";
        }

        transform.GetComponent<Text>().text = rankingText;

        // Init
        orderUsername.Clear();
        orderScores.Clear();
    }
}
