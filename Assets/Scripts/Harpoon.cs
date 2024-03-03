using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public enum HookState
    {
        Unlaunched,
        Launching,
        Retracting,
        Launched
    }

    public GameObject player;
    public PlayerInput playerInput;
    [SerializeField] private GameObject hookObj;
    private Transform hookT;
    public Hook hook;
    private Swingable hookSwing;
    [SerializeField] private float rotation;

    public bool onCooldown;

    private RopeSegment harpoonRope;
    private List<GameObject> ropeRungs = new List<GameObject>();
    [SerializeField] private GameObject rungPrefab;

    public HookState hState = HookState.Unlaunched;

    public AnimationCurve analogIntensityCurve;
    public float currentLerpRate = 0;

    [Header("Audioclips")]
    public AudioClip launchClip;

    const float HookLaunchSpeed = 22.37f;
    const int HarpoonLength = 6;
    const float HarpoonCooldown = 1f;
    const float HarpoonSwingMax = 10f;

    

    private void Start()
    {
        hookT = hookObj.GetComponent<Transform>();
        hook = hookObj.GetComponent<Hook>();
        hookSwing = hookObj.GetComponent<Swingable>();

        harpoonRope = GetComponent<RopeSegment>();
    }

    void OnEnable()
    {
        onCooldown = false;
        playerInput.OnFireStarted += OnLaunchHook;
        playerInput.OnFireStarted += OnLaunchHookedObj;
    }

    private void Update()
    {
        if (player == null)
            return;
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
        transform.Rotate(0, 0, rotation);
    }

    private void FixedUpdate()
    {
        if (hState == HookState.Launched
            || hState == HookState.Retracting)
            UpdateRopeStretch();
        HandleState();
    }

    private void UpdateRopeStretch()
    {
        float distance = Vector3.Distance(hookT.position, transform.position);
        float newIncrement = distance / (float)Mathf.Max(1, ropeRungs.Count);

        for (int i = 0; i < ropeRungs.Count; i++)
        {
            Swingable rung = ropeRungs[i].GetComponent<Swingable>();
            rung.radius = newIncrement * (i + 1);
        }
    }

    private void HandleState()
    {
        if (hState == HookState.Unlaunched)
        {
            currentLerpRate = 0;
            return;
        }
          

        float distance = Vector3.Distance(hookT.position, transform.position);

        if (hState == HookState.Launched
            || hState == HookState.Retracting)
        {
            if (distance < 1.75f)
            {
                DisableRope();
                hState = HookState.Unlaunched;
                return;
            }
        }
        if (hState == HookState.Launched)
        {
            // Retracting at half speed
            hookSwing.radius -= HookLaunchSpeed * Time.fixedDeltaTime * 0.05f;

            if (distance > HarpoonLength)
                hookSwing.radius = HarpoonLength;

            return;
        }
        if (hState == HookState.Launching)
        {
            if (distance > HarpoonLength)
                hState = HookState.Retracting;
            else
                Extend();
        }
        else if (hState == HookState.Retracting)
        {
            currentLerpRate += Time.deltaTime;
            Retract();
        }

        if (hook.transform.childCount != 0)
        {
            ConvertToSwingable();
            hState = HookState.Launched;
            return;
        }
    }

    public void Retract()
    {
        hookSwing.radius -= (analogIntensityCurve.Evaluate(currentLerpRate));
    }

    public void Extend()
    {
        hookSwing.radius += HookLaunchSpeed * Time.fixedDeltaTime;
    }

    public void Detach()
    {
        hook.UnhookObj();
    }


    /// <summary>
    /// Launches the hook when the ability is available.
    /// </summary>
    /// <returns>If the harpoon successfuly launched.</returns>
    public void OnLaunchHook()
    {
        if (onCooldown) return;
        if (hState != HookState.Unlaunched) return;

        StartCoroutine(StartHookCooldown());

        hState = HookState.Launching;
        hookObj.SetActive(true);
        hookSwing.radius = 0.1f;
        harpoonRope.target = hookObj;
    }

    public void OnRelease()
    {
        if (hook.transform.childCount != 0)
        {
            hook.UnhookObj();

            hState = HookState.Retracting;
        }
    }

    public void OnLaunchHookedObj()
    {
        if (hState != HookState.Launched) return;

        hook.LaunchObj();
        //AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, launchClip);
        hState = HookState.Retracting;

        return;
    }

    private IEnumerator StartHookCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(HarpoonCooldown);
        onCooldown = false;
    }

    /// <summary>
    /// Activates when the rope has caught a Grabbable.
    /// Converts simple rope into a set of Swingable rungs.
    /// </summary>
    private void ConvertToSwingable()
    {
        foreach (GameObject rope in ropeRungs)
            Destroy(rope);

        ropeRungs.Clear();

        int numRungs = Mathf.RoundToInt(HarpoonLength * 2);
        float angleIncrement = hookSwing.angleLimit / (float)Mathf.Max(1, numRungs);
        for (int i = 0; i < numRungs; i++)
        {
            GameObject rung = Instantiate(rungPrefab);
            rung.SetActive(false);
            Swingable rungSwing = rung.GetComponent<Swingable>();

            rungSwing.radius = i * 0.5f;
            rungSwing.angleLimit = angleIncrement * (i + 1);
            rungSwing.origin = player;

            rung.transform.SetParent(transform);

            ropeRungs.Add(rung);
        }

        harpoonRope.target = gameObject;
        foreach (GameObject r in ropeRungs)
        {
            RopeSegment rSegment = r.GetComponent<RopeSegment>();
            rSegment.target = r;
            r.SetActive(true);
        }

        hookObj.SetActive(true);

        StartCoroutine(VisualizeRope());
    }

    // Necessary to let rope rungs move into place before being rendered.
    private IEnumerator VisualizeRope()
    {
        yield return new WaitForSeconds(0.02f);

        harpoonRope.target = ropeRungs[0];
        for (int i = 0; i < ropeRungs.Count - 1; i++)
        {
            RopeSegment rSegment = ropeRungs[i].GetComponent<RopeSegment>();
            rSegment.target = ropeRungs[i + 1];
        }
        ropeRungs[ropeRungs.Count - 1].GetComponent<RopeSegment>().target = hookObj;
    }

    private void DisableRope()
    {
        foreach (GameObject rope in ropeRungs)
            Destroy(rope);

        ropeRungs.Clear();

        if (hook.transform.childCount != 0)
            Destroy(hook.transform.GetChild(0).gameObject);

        harpoonRope.target = hookObj;
        hookSwing.radius = 0.1f;
        hookObj.SetActive(false);

        harpoonRope.target = gameObject;
    }
}
