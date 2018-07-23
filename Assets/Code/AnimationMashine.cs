using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct _Animation {
    public PlayerState State;
    public float CyklusPerBeat;
    public Sprite[] Sprites;
}

public class AnimationMashine : MonoBehaviour {

    [SerializeField] SpriteRenderer Renderer;
    [SerializeField] _Animation[] Animations = null;

    PlayerState State = PlayerState.Idle;

    Dictionary<PlayerState, int> Assign;
    int AnimationFrame = 0;

    private void Start() {
        Assign.Clear();
        for(int i = 0; i < Animations.Length; i++) {
            Assign.Add(Animations[i].State, i);
        }
    }
    private void Update() {
        if (GameManager.GetTimeSinceLastBeat() >= GameManager.GetBeatSeconds() / (Animations[Assign[State]].CyklusPerBeat * Animations[Assign[State]].Sprites.Length) * AnimationFrame) {
            Render();
        }
    }

    public void ChangeState(PlayerState newState) {
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
