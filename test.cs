using JusticeFramework.Components;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using UnityEngine;

public class test : MonoBehaviour {

    public Actor actor;

    public string testEquip = "TestSword";

    private void Start() {
        Actor.Equip(actor, GameManager.Spawn(testEquip) as IEquippable);
    }

    private void Update() {
        actor.BeginAttack();
        actor.EndAttack();
    }
}
