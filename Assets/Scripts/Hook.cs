using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    private GameObject grabbedObj;
    private Swingable swingable;
    [SerializeField] private Harpoon harpoon;

    private int hookIndex;
    public AudioClip scrapClip;
    public AudioClip thrownClip;
    public AudioClip hookHitClip;
    public AudioClip shieldClip;

    private void Start()
    {
        swingable = GetComponent<Swingable>();
        LayerMask.NameToLayer("Obstacles");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.childCount == 0 && harpoon.hState != Harpoon.HookState.Retracting)
        {
            if (collision.transform.gameObject.layer == 6)
                HookObj(collision.transform.gameObject);
        }
    }

    private void HookObj(GameObject selObj)
    {
        //Instantiate(hookHit, transform.position, Quaternion.identity);
        Debug.Log($"Hooked: {selObj.name}");

        grabbedObj = Instantiate(selObj,
        transform.position, selObj.transform.rotation,
        gameObject.transform).gameObject;

        Rigidbody2D grabbedRigidbody = grabbedObj.GetComponent<Rigidbody2D>();
        grabbedRigidbody.isKinematic = true;

        Whippable whip = grabbedObj.AddComponent<Whippable>();
        whip.p = harpoon.player.GetComponent<Player>();
        whip.playerRb = harpoon.player.GetComponent<Rigidbody2D>();

        Destroy(selObj);
    }
    public void UnhookObj()
    {
        if (grabbedObj == null)
        {
            harpoon.hState = Harpoon.HookState.Retracting;
            return;
        }

        grabbedObj.transform.SetParent(null);
        grabbedObj = null;

        harpoon.hState = Harpoon.HookState.Retracting;
    }

    public void LaunchObj()
    {
        if (grabbedObj == null)
        {
            harpoon.hState = Harpoon.HookState.Retracting;
            return;
        }
        grabbedObj.transform.SetParent(null);
        Rigidbody2D grabbedRigidbody = grabbedObj.GetComponent<Rigidbody2D>();
        grabbedRigidbody.isKinematic = false;
        grabbedRigidbody.AddForce(harpoon.transform.right * 700);
        grabbedObj.gameObject.name += "(Launched)";
        //AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, thrownClip);

        harpoon.hState = Harpoon.HookState.Retracting;
    }

    public void UnhookFromFracture()
    {
        grabbedObj.transform.SetParent(null);

        harpoon.hState = Harpoon.HookState.Retracting;
    }
}
