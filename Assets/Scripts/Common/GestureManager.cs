using System;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Events;

/// <summary>
/// Get the gestures of the user
/// </summary>
public class GestureManager : Singleton<GestureManager>
{
    public GestureRecognizer ActionRecognizer { get; private set; }

    GestureRecognizer ManipulationRecognizer;
    GestureRecognizer ActiveRecognizer;

    public bool IsNavigating { get; private set; }

    public Vector3 NavigationPosition { get; private set; }

    public bool IsManipulating { get; private set; }

    public Vector3 ManipulationPosition { get; private set; }
    public Vector3 ManipulationDeltaPosition { get; private set; }

    public UnityEvent event_AirTap = new UnityEvent();
    public UnityEvent event_ManipulationStart = new UnityEvent();
    public UnityEvent event_ManipulationUpdate = new UnityEvent();
    //public UnityEvent event_ManipulationStop = new UnityEvent();

    [SerializeField]
    LayerMask airTapLayer;

    /// <summary>
    /// Initialization of recognizers
    /// </summary>
    void Awake()
    {
        //___________Action________________
        ActionRecognizer = new GestureRecognizer();
        ActionRecognizer.SetRecognizableGestures(
            GestureSettings.Tap | GestureSettings.NavigationX);
        ActionRecognizer.TappedEvent += ActionRecognizer_TappedEvent;

        ActionRecognizer.NavigationStartedEvent += ActionRecognizer_NavigationStartedEvent;
        ActionRecognizer.NavigationUpdatedEvent += ActionRecognizer_NavigationUpdatedEvent; 
        ActionRecognizer.NavigationCompletedEvent += ActionRecognizer_NavigationCompletedEvent;
        ActionRecognizer.NavigationCanceledEvent += ActionRecognizer_NavigationCanceledEvent;

        //__________Manipulation______________________

        ManipulationRecognizer = new GestureRecognizer();
        ManipulationRecognizer.SetRecognizableGestures(
            GestureSettings.Tap | GestureSettings.ManipulationTranslate);
        ManipulationRecognizer.TappedEvent += ManipulationRecognizer_TappedEvent;

        ManipulationRecognizer.ManipulationStartedEvent += ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent += ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent += ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent += ManipulationRecognizer_ManipulationCanceledEvent;

        //____________Active Recogniser____________
        ActiveRecognizer = ActionRecognizer;
        ActiveRecognizer.StartCapturingGestures();
    }

    void OnDestroy()
    {
        ActionRecognizer.TappedEvent -= ActionRecognizer_TappedEvent;
        ActionRecognizer.NavigationStartedEvent -= ActionRecognizer_NavigationStartedEvent;
        ActionRecognizer.NavigationUpdatedEvent -= ActionRecognizer_NavigationUpdatedEvent;
        ActionRecognizer.NavigationCompletedEvent-= ActionRecognizer_NavigationCompletedEvent;
        ActionRecognizer.NavigationCanceledEvent -= ActionRecognizer_NavigationCanceledEvent;

        ManipulationRecognizer.TappedEvent -= ManipulationRecognizer_TappedEvent;
        ManipulationRecognizer.ManipulationStartedEvent -= ManipulationRecognizer_ManipulationStartedEvent;
        ManipulationRecognizer.ManipulationUpdatedEvent -= ManipulationRecognizer_ManipulationUpdatedEvent;
        ManipulationRecognizer.ManipulationCompletedEvent -= ManipulationRecognizer_ManipulationCompletedEvent;
        ManipulationRecognizer.ManipulationCanceledEvent -= ManipulationRecognizer_ManipulationCanceledEvent;
    }

    public void StartManipulation()
    {
        Transition(ManipulationRecognizer);
    }

    public void StartNavigation()
    {
        Transition(ActionRecognizer);
    }

    /// <summary>
    /// Transition to the new recognizer
    /// </summary>
    /// <param name="newRecognizer"></param>
    void Transition(GestureRecognizer newRecognizer)
    {
        if (newRecognizer == null)
        {
            return;
        }

        if (ActiveRecognizer != null)
        {
            if (ActiveRecognizer == newRecognizer)
            {
                return;
            }

            ActiveRecognizer.CancelGestures();
            ActiveRecognizer.StopCapturingGestures();
        }

        newRecognizer.StartCapturingGestures();
        ActiveRecognizer = newRecognizer;
    }

    /// <summary>
    /// Air tap callback
    /// </summary>
    /// <param name="source"></param>
    /// <param name="tapCount"></param>
    /// <param name="ray"></param>
    private void ActionRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,20, airTapLayer))
        {
            //Tells the hit object that it has been targeted by an air tap
            hit.collider.SendMessage("OnAirTap");
        }
        else
        {
            //Throw the air tap event
            event_AirTap.Invoke();
        }
    }

    private void ManipulationRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, airTapLayer))
        {
            //Tells the hit object that it has been targeted by an air tap
            hit.collider.SendMessage("OnAirTap");
        }
        else
        {
            //Throw the air tap event
            event_AirTap.Invoke();
        }
    }

    private void ActionRecognizer_NavigationStartedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        IsNavigating = true;

        NavigationPosition = relativePosition;
    }

    private void ActionRecognizer_NavigationUpdatedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        IsNavigating = true;

        NavigationPosition = relativePosition;
    }

    private void ActionRecognizer_NavigationCompletedEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        IsNavigating = false;
    }

    private void ActionRecognizer_NavigationCanceledEvent(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        IsNavigating = false;
    }

    private void ManipulationRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = true;

        ManipulationDeltaPosition = position - ManipulationPosition;
        ManipulationPosition = position;

        event_ManipulationStart.Invoke();
    }

    private void ManipulationRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = true;

        ManipulationDeltaPosition = position - ManipulationPosition;
        ManipulationPosition = position;

        event_ManipulationUpdate.Invoke();
    }

    private void ManipulationRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = false;

        //event_ManipulationStop.Invoke();
    }

    private void ManipulationRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 position, Ray ray)
    {
        IsManipulating = false;

        //event_ManipulationStop.Invoke();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out hit, 20, airTapLayer))
            {
                hit.collider.SendMessage("OnAirTap");
            }
            else
            {
                //Throw the air tap event
                event_AirTap.Invoke();
            }

        }
    }
}