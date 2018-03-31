using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Data.Dialogue {
	[Serializable]
	public class Topic {
		[SerializeField]
		private readonly int id;
		
		[SerializeField]
		private bool isDefault;
		
		[SerializeField]
		private string text;
		
		[SerializeField]
		private List<Response> responses;

		public int Id {
			get { return id; }
		}

		public bool IsDefault {
			get { return isDefault; }
			set { isDefault = value; }
		}

		public string TopicText {
			get { return text; }
			set { text = value; }
		}

		public List<Response> Responses {
			get { return responses; }
			set { responses = value; }
		}
		
		public Topic(int id) {
			this.id = id;
			
			isDefault = false;
			text = string.Empty;
			responses = new List<Response>();
		}
	}
}