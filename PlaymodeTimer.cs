using UnityEngine;

public class PlaymodeTimer : MonoBehaviour
{

    void Update()
    {
        UIButtons.Instance.TimerText(Time.deltaTime);
    }
}
