using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Incomplete code to an arrow who follows an objective
/// </summary>
[Obsolete]
public class NextZone : MonoBehaviour
{
    //[SerializeField] Vector3 target;
    [SerializeField] SpriteRenderer icon;
    [SerializeField] Collider2D transitionCollider;
    //[SerializeField] GameObject player;
    //[SerializeField] RectTransform pointerTransform;

    private void Start()
    {
        icon = GetComponent<SpriteRenderer>();
        transitionCollider = GetComponentInParent<Collider2D>();
        //player = GameObject.FindGameObjectWithTag("Player");
        //pointerTransform = GameObject.Find("Pointer").GetComponent<RectTransform>();
    }

    private void Update()
    {
        icon.enabled = transitionCollider.isTrigger;
        /*print(!MainQuest.mainQuest.shohtzsfenCompleted);
        print(MainQuest.mainQuest.eilighvindCompleted);
        print(!MainQuest.mainQuest.currentlyInBattle);
        print(SceneManager.GetActiveScene().name != "Eilighvind");

        if (!MainQuest.mainQuest.shohtzsfenCompleted || MainQuest.mainQuest.eilighvindCompleted)
           if (!MainQuest.mainQuest.currentlyInBattle && SceneManager.GetActiveScene().name != "Eilighvind")
           {
                pointerTransform.gameObject.SetActive(false);
           }
        else if (MainQuest.mainQuest.shohtzsfenCompleted && !MainQuest.mainQuest.ballaghanCompleted)
        {
            pointerTransform.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().name == "Shohtzsfen")
                target = new(-72, 35.5f);
        }
        else if (MainQuest.mainQuest.ballaghanCompleted && !MainQuest.mainQuest.ayotzorCompleted)
        {
            pointerTransform.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().name == "Ballaghan")
                target = new(28, -5);
            if (SceneManager.GetActiveScene().name == "Shohtzsfen")
                target = new(57, -1);
        }
        else if (MainQuest.mainQuest.ayotzorCompleted && !MainQuest.mainQuest.eilighvindCompleted)
        {
            pointerTransform.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().name == "Ayotzor")
                target = new(5, 23);
        }

        Vector3 toPos = target;
        Vector3 fromPos = Camera.main.transform.position;
        fromPos.z = 0;
        Vector3 dir = (toPos - fromPos).normalized;
        float angle = UtilsClass.GetAngleFromVectorFloat(dir);
        pointerTransform.localEulerAngles = new(0, 0, angle);*/
    }
}
