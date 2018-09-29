﻿using UnityEngine;


public abstract class SubsystemBehaviour : MonoBehaviour
	{
		protected MetaDataLayer metaDataLayer;
		protected MetaTileMap metaTileMap;

		public virtual int Priority => 0;

		public void Awake()
		{
			metaDataLayer = GetComponentInChildren<MetaDataLayer>();
			metaTileMap = GetComponentInChildren<MetaTileMap>();

			GetComponent<SubsystemManager>().Register(this);
		}

		public abstract void Initialize();

		public abstract void UpdateAt(Vector3Int position);
	}
