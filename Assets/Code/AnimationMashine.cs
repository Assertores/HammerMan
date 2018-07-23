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

    private void Start() {
        Assign = new Dictionary<PlayerState, int>();
        for(int i = 0; i < Animations.Length; i++) {
            Assign.Add(Animations[i].State, i);
            if (Animations[i].CyklusPerBeat == 0)
                Animations[i].CyklusPerBeat = 1;
        }
    }
    private void Update() {
        float time = GameManager.GetTimeSinceLastBeat();
        //print(!(AnimationFrame == 0 && time > GameManager.GetBeatSeconds()/2) + " because TimeSinceLastBeat is: " + time + " and next frame is " + AnimationFrame);
        //print((time >= (GameManager.GetBeatSeconds() / (Animations[Assign[State]].CyklusPerBeat * Animations[Assign[State]].Sprites.Length)) * AnimationFrame) + " its breater than " + ((GameManager.GetBeatSeconds() / (Animations[Assign[State]].CyklusPerBeat * Animations[Assign[State]].Sprites.Length)) * AnimationFrame));
        if (!(AnimationFrame == 0 && time > GameManager.GetBeatSeconds()/2) && time >= (GameManager.GetBeatSeconds() / (Animations[Assign[State]].CyklusPerBeat * Animations[Assign[State]].Sprites.Length)) * AnimationFrame) {
            //print(time + " is greater than " + (GameManager.GetBeatSeconds() / (Animations[Assign[State]].CyklusPerBeat * Animations[Assign[State]].Sprites.Length)) * AnimationFrame);
            Render();
        }
    }

    public void ChangeState(PlayerState newState) {
        State = newState;
        AnimationFrame = (int)((GameManager.GetTimeSinceLastBeat()/GameManager.GetBeatSeconds())
                            *(Animations[Assign[State]].CyklusPerBeat * Animations[Assign[State]].Sprites.Length));

        AnimationFrame %= Animations[Assign[State]].Sprites.Length;
        Render();
    }

    void Render() {
        if (!Assign.ContainsKey(State))
            return;

        Renderer.sprite = Animations[Assign[State]].Sprites[AnimationFrame];
        AnimationFrame++;
        AnimationFrame %= Animations[Assign[State]].Sprites.Length;
    }

}
