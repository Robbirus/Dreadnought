using UnityEditor;

[CustomEditor(typeof(Interactable), true)]
public class NewMonoBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        if (target.GetType() == typeof(EventOnlyInteractable)) 
        { 
            interactable.promptMessage = EditorGUILayout.TextField("Prompt Message", interactable.promptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract can ONLY use UnityEvents.", MessageType.Info);
            if(interactable.GetComponent<InteractionEvent>() == null)
            {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<InteractionEvent>();

            }
        } 
        else
        {
            base.OnInspectorGUI();
            if (interactable.useEvents)
            {
                // We are using events, add component
                if (interactable.GetComponent<InteractionEvent>() == null)
                {
                    interactable.gameObject.AddComponent<InteractionEvent>();
                }
            }
            else
            {
                // we are not using events, remove the component
                if (interactable.GetComponent<InteractionEvent>())
                {
                    DestroyImmediate(interactable.GetComponent<InteractionEvent>());

                }

            }
        }
    }
}
