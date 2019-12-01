using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoFSM : MonoBehaviour
{
    protected IState CurState;

    private float timer;

    private Dictionary<Type, IState> states;

    private Coroutine changeStateRoutine;

    protected void Initialize<T>(Dictionary<Type, IState> newStates) where T : IState
    {
        states = newStates;

        CurState = states[typeof(T)];
        CurState.Enter();
    }

    protected void Initialize<T>(IState[] newStates) where T : IState
    {
        states = new Dictionary<Type, IState>(newStates.Length);

        foreach (var state in newStates)
            states.Add(state.GetType(), state);

        CurState = states[typeof(T)];
        CurState.Enter();
    }

    public void ChangeState<T>() where T : IState
    {
        ChangeState(typeof(T));
    }

    public void ChangeState(Type state)
    {
        var newState = states[state];

        // Do not change if the states are the same
        if (newState == CurState)
            return;

        AssertStateNotNull(CurState);
        CurState.Exit();
        CurState.TimeElapsed = 0.0f;

        timer = 0;
        CurState = states[state];
        CurState.Enter();
    }

    public void ChangeState(Type state, float time)
    {
        if (changeStateRoutine != null)
            StopCoroutine(changeStateRoutine);

        changeStateRoutine = StartCoroutine(ChangeStateRoutine(state, time));
    }

    private IEnumerator ChangeStateRoutine(Type state, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(state);
    }

    public void Pause(float seconds)
    {
        timer = seconds;
    }

    protected virtual void Tick() { }
    protected virtual void FixedTick() { }

    protected void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f)
            return;

        Tick();

        AssertStateNotNull(CurState);
        CurState.Update();
        CurState.TimeElapsed += Time.deltaTime;
    }

    protected void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer > 0f)
            return;

        FixedTick();

        AssertStateNotNull(CurState);
        CurState.FixedUpdate();
    }

    [System.Diagnostics.Conditional("DEBUG")]
    private void AssertStateNotNull(IState state)
    {
        UnityEngine.Assertions.Assert.IsNotNull(state, $"state should not be null! for {this}");
    }
}

public interface IState
{
    void FixedUpdate();
    void Update();
    void Enter();
    void Exit();

    float TimeElapsed { get; set; }
}

public abstract class State<TAgent> : IState where TAgent : MonoFSM
{
    public virtual void FixedUpdate() { }
    public virtual void Update() { }
    public virtual void Enter() { }
    public virtual void Exit() { }

    public float TimeElapsed { get; set; }
    protected TAgent Agent;
    protected Transform T;

    protected State(TAgent agent)
    {
        Agent = agent;
        T = agent.transform;
    }
}
