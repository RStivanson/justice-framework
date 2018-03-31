using System;

namespace JusticeFramework.Editor.NodeEditor {
	[Serializable]
	public class NodeMenuItem : Attribute {
		public string Path { get; }
		
		public NodeMenuItem(string path) {
			Path = path;
		}
	}
}