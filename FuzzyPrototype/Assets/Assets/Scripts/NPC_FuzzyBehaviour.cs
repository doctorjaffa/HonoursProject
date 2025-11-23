using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Profiling;

public class NPC_FuzzyBehaviour : MonoBehaviour
{

    // Curves to weigh partial truths
    [SerializeField] private AnimationCurve low;
    [SerializeField] private AnimationCurve mid;
    [SerializeField] private AnimationCurve high;

    // Arrogance value set in editor (1-10) 
    [SerializeField] private float arrogance;

    // The level of partial truths evaluated against the graphs
    [SerializeField] private float low_value = 0f;
    [SerializeField] private float mid_value = 0f;
    [SerializeField] private float high_value = 0f;

    private NPC_BinaryBehaviour npc_behaviour;

    // When the crime committed signal is broadcasted, evaluate which reaction the NPC should have
    private void OnEnable() => EventManager.OnCommitCrime += EvaluateStatements;

    private void OnDisable() => EventManager.OnCommitCrime -= EvaluateStatements;

    void Start()
    {
        npc_behaviour = GetComponent<NPC_BinaryBehaviour>();
    }

    // Evaluate the different graphs and determine which reaction state to set corresponding to which is most truthful
    public async void EvaluateStatements()
    {
        Profiler.BeginSample("FuzzyBehaviour");
        // 
        low_value = low.Evaluate(arrogance);
        mid_value = mid.Evaluate(arrogance);
        high_value = high.Evaluate(arrogance);

        // If high arrogance is the most truthful (greater than) then the NPC will fight the criminal
        if (high_value > mid_value)
        {

            // Set reaction state to FIGHT and call respond to signal (causes state change)
            npc_behaviour.SetReactionState(NPC_BinaryBehaviour.ReactionState.FIGHT);

        }
        // If the mid value is greater, npc will flee (confident enough to flee)
        else if (mid_value >= low_value && mid_value >= high_value) 
        {
            npc_behaviour.SetReactionState(NPC_BinaryBehaviour.ReactionState.FLEE);

        }
        // Otherwise, NPC is too scared and will cower
        else
        {
            npc_behaviour.SetReactionState(NPC_BinaryBehaviour.ReactionState.COWER);
        }

        // Wait a random amount of brief time before responding to signal
        await Task.Delay(Random.Range(200, 800));

        npc_behaviour.RespondToSignal();
    }
}
