using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;

public class PersistoMatic : Singleton<PersistoMatic>
{
    public string ObjectAnchorStoreName;

    WorldAnchorStore anchorStore;

    // Use this for initialization
    void Start()
    {
        WorldAnchorStore.GetAsync(AnchorStoreReady);

        //GestureManager.Instance.event_ManipulationStart.AddListener(Placing);
        //GestureManager.Instance.event_ManipulationStop.AddListener(StopPlacing);
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;

        Debug.Log("looking for " + ObjectAnchorStoreName);
        string[] ids = anchorStore.GetAllIds();
        for (int index = 0; index < ids.Length; index++)
        {
            Debug.Log(ids[index]);
            if (ids[index] == ObjectAnchorStoreName)
            {
                WorldAnchor wa = anchorStore.Load(ids[index], gameObject);
                break;
            }
        }
    }

    public void StopPlacing()
    {
        if (anchorStore == null)
        {
            return;
        }

        WorldAnchor attachingAnchor = gameObject.AddComponent<WorldAnchor>();
        if (attachingAnchor.isLocated)
        {
            bool saved = anchorStore.Save(ObjectAnchorStoreName, attachingAnchor);
        }
        else
        {
            attachingAnchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
        }
    }

    public void Placing()
    {
        if (anchorStore == null)
        {
            return;
        }

        WorldAnchor anchor = gameObject.GetComponent<WorldAnchor>();
        if (anchor != null)
        {
            DestroyImmediate(anchor);
        }

        string[] ids = anchorStore.GetAllIds();
        for (int index = 0; index < ids.Length; index++)
        {
            Debug.Log(ids[index]);
            if (ids[index] == ObjectAnchorStoreName)
            {
                bool deleted = anchorStore.Delete(ids[index]);
                break;
            }
        }
    }

    private void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (located)
        {
            bool saved = anchorStore.Save(ObjectAnchorStoreName, self);
            self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
        }
    }
}
