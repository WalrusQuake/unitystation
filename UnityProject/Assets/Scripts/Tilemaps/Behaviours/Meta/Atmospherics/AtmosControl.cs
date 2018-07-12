﻿using System.Collections.Generic;
using System.Threading;
using Tilemaps.Behaviours.Layers;
using UnityEngine;
using UnityEngine.Networking;

namespace Tilemaps.Behaviours.Meta
{
	public class AtmosControl : SystemBehaviour
	{
		private AtmosThread thread;
		
		public override void Initialize()
		{
			thread = new AtmosThread(metaDataLayer);
			new Thread(thread.Run).Start();
		}
		
		public override void UpdateAt(Vector3Int position)
		{
			thread?.Enqueue(position);
		}

		private void OnDestroy()
		{
			thread?.Stop();
		}
	}
}