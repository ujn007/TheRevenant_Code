using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimelineSkipper : MonoBehaviour
{
    [SerializeField] PlayableDirector timelineDirector;  
    [SerializeField] float skipHoldTime = 3f;

    [SerializeField] private GameObject skiperGroup;
    [SerializeField] private Image loadingImage;
    private Transform imageGroup;

    private float holdTimer = 0f;
    private bool isHolding = false;

    private bool isEndTimeLine = false;

    private void Awake()
    {
        imageGroup = loadingImage.GetComponentInParent<Transform>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !isEndTimeLine)
        {
            isHolding = true;
            holdTimer += Time.deltaTime;
            loadingImage.fillAmount = holdTimer / skipHoldTime;

            if (holdTimer >= skipHoldTime)
            {
                SkipTimeline();
                ResetHold();
            }
        }
        else if (isHolding)
        {
            ResetHold();
        }
    }

    void SkipTimeline()
    {
        if (timelineDirector != null && timelineDirector.state == PlayState.Playing)
        {
            timelineDirector.time = timelineDirector.duration; 
            timelineDirector.Evaluate(); 
            timelineDirector.Stop();
            skiperGroup.SetActive(false);
        }
    }

    void ResetHold()
    {
        holdTimer = 0f;
        loadingImage.DOFillAmount(0, 0.2f);
        isHolding = false;
    }

    public void TimeLineEnd()
    {
        isEndTimeLine = true;
        skiperGroup.SetActive(false);
    }
}
