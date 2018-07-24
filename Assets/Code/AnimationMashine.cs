using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct _Animation {
    public PlayerState State;
    public float CyklusPerBeat; //TODO: make it work with non multibles of BeatSeconds
    public Sprite[] Sprites;
}

public class AnimationMashine : MonoBehaviour {

    [SerializeField] SpriteRenderer Renderer;
    [SerializeField] _Animation[] Animations = null;

    public PlayerState State = PlayerState.Idle;

    Dictionary<PlayerState, int> Assign;
    int AnimationFrame = 0;
    float ReferencBeat;

    private void Start() {
        Assign = new Dictionary<PlayerState, int>();
        for(int i = 0; i < Animations.Length; i++) {
            Assign.Add(Animations[i].State, i);
            if (Animations[i].CyklusPerBeat == 0)
                Animations[i].CyklusPerBeat = 1;
        }
    }
    private void Update() {
        if (ReferencBeat == 0)
            ReferencBeat = GameManager.GetTimeOfLastBeat();

        if (GameManager.GetTime() >= ReferencBeat + AnimationFrame * (GameManager.GetBeatSeconds()/ (Animations[Assign[State]].CyklusPerBeat * Animations[Assign[State]].Sprites.Length))) {
            Render();
        }
    }

    public void ChangeState(PlayerState newState) {
        State = newState;
        ReferencBeat = GameManager.GetTimeOfLastBeat();
        AnimationFrame = (int)(((GameManager.GetTime()-ReferencBeat)/GameManager.GetBeatSeconds())
                            *(Animations[Assign[State]].CyklusPerBeat * Animations[Assign[State]].Sprites.Length));

        Render();
    }

    void Render() {
        if (!Assign.ContainsKey(State))
            return;

        Renderer.sprite = Animations[Assign[State]].Sprites[AnimationFrame % Animations[Assign[State]].Sprites.Length];//sucht die richtige animation im Animations Array raus und hohlt sich dann den richtigen sprite in dessen Sprites Array
        if (AnimationFrame % Animations[Assign[State]].Sprites.Length == 0)
            print(GameManager.GetTime() - GameManager.GetTimeOfLastBeat());
        AnimationFrame++;
    }

}
