using JusticeFramework.Components;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using UnityEngine;

public class test : MonoBehaviour {

    public Actor actor;

    private void Start() {
        actor.Equip(GameManager.Spawn("TestSword") as IEquippable);
    }

    private void Update() {
        actor.BeginAttack();
        actor.EndAttack();
    }
}
