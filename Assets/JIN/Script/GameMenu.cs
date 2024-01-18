using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform startPoint;
    [SerializeField] UIContral UIContral;

    [SerializeField] float teleportCoolTime = 1200;
    float count;

    private void Start()
    {
        startPoint.position = new Vector3(20, 0, 10);
        count = teleportCoolTime;
    }

    private void Update()
    {
        if(count <= teleportCoolTime)
            count += Time.deltaTime;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void EscapeMap()
    {
        if (count >= teleportCoolTime)
        {
            player.position = startPoint.position;
            //player.rotation = startPoint.rotation;
            count = 0;
            UIContral.MenuWindowContral();
        }
    }
}
