using JusticeFramework.AI.BehaviourTree.Nodes.Actions;
using JusticeFramework.AI.BehaviourTree.Nodes.Conditions;
using JusticeFramework.Components;
using JusticeFramework.Core.AI;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Builder;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;
using JusticeFramework.Core.AI.BehaviourTree.Nodes.Composites;
using JusticeFramework.Core.AI.BehaviourTree.Nodes.Decorators;
using JusticeFramework.Core.AI.BehaviourTree.Nodes.Leafs;
using JusticeFramework.Core.Controllers;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Models.Settings;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace JusticeFramework.AI {
    using BehaveTree = Core.AI.BehaviourTree.BehaviourTree;

    [Serializable]
    [DisallowMultipleComponent()]
    public class AiController : Controller {
		public const int HEROIC_ATTACK_BUFFER = 5;

		[SerializeField]
		private Actor self;
		
		[SerializeField]
		private Animator animator;
		
		[SerializeField]
		private NavMeshAgent agent;
		
		[SerializeField]
		private AiVision vision;
		
		public Animator Animator {
			get { return animator; }
		}
		
		public Actor Actor {
			get { return self; }
		}
		
		public AiVision Vision {
			get { return vision; }
		}
		
		public NavMeshAgent Agent {
			get { return agent; }
		}

		public bool active;
		private BehaveTree tree;
		//private List<BehaviourSet> setList;
		private TickState tickState;
		
		private void Awake() {
			InitalizeBehaviour();
		}

		private IEnumerator Start() {
			active = false;
			
			Actor?.SetRagdollActive(false);
			agent.stoppingDistance = 2.0f;
			
			yield return new WaitForSeconds(2);
			active = true;
		}
		
		private void Update() {
			if (!active) {
				return;
			}

			if (!GameManager.IsPaused) {
				tree?.Tick(tickState);
				//tickState.debug = true;
				//setList[setList.Count - 1]?.Tick(tickState);
			}

			if (Input.GetKeyDown(KeyCode.G)) {
				tickState.debug = true;
			}

            if (animator != null) {
                animator?.SetBool(SystemConstants.AnimatorIsWalkingParam, agent.hasPath);
            }
		}

		public void InitalizeBehaviour() {
			tree = CreateBehaviourTree();
			
			tickState = new TickState() {
				blackboard = new Blackboard(),
			};
			
			tickState.blackboard.Set("controller", this);
		}

		private BehaveTree CreateBehaviourTree() {
			return new BehaviourTreeBuilder()
					.Composite<Selector>()
						.Include(BuildCombatSubTree())
						.Include(BuildWanderSubTree())
					.End()
				.Build();
		}

		private static Node BuildWanderSubTree() {
			return new BehaviourTreeBuilder()
					.Composite<MemSequence>()
						.Leaf<SetLocationRandom>().End()
						.Leaf<MoveToDestination>().End()
						.Leaf<Wait>(5).End()
					.End()
				.Root();
		}
		
		private static Node BuildCombatSubTree() {
			return new BehaviourTreeBuilder()
				.Composite<Selector>()
					.Composite<Sequence>()
						.Leaf<HasTarget>().End()
						.Composite<Selector>()
							.Leaf<TargetWithinInterestDistance>().End()
							.Decorator<AlwaysFail>()
								.Leaf<RemoveTarget>().End()
							.End()
						.End()
						.Leaf<SetLocationToTarget>().End()
						.Composite<Selector>()
							.Leaf<IsInCombat>().End()
							.Leaf<EnterCombat>().End()
						.End()
						.Composite<MemSelector>()
							.Composite<MemSequence>()
								.Leaf<IsScaredOfTarget>().End()
								.Leaf<SetLocationRandom>().End()
								.Leaf<MoveToDestination>().End()
							.End()
							.Composite<MemSequence>() // Melee
								.Leaf<TargetInRange>(5).End()
								.Leaf<ClearDestination>().End()
								.Leaf<LookAtTarget>().End()
								.Decorator<DelayBetween>(0.25f)
									.Leaf<Attack>().End()
								.End()
							.End()
							.Leaf<MoveToDestination>().End()
						.End()
					.End()
					.Composite<Sequence>()
						.Leaf<UpdateTarget>().End()
						.Composite<Selector>()
							.Leaf<IsInCombat>().End()
							.Leaf<EnterCombat>().End()
						.End()
					.End()
				.End()
			.Root();
		}
	}
}