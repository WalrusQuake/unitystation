﻿using UnityEngine;
using UnityEngine.Tilemaps;


	[ExecuteInEditMode]
	public class Layer : MonoBehaviour
	{
		private SubsystemManager subsystemManager;

		public LayerType LayerType;
		protected Tilemap tilemap;

		public BoundsInt Bounds => tilemap.cellBounds;

		public Vector3Int WorldToCell(Vector3 pos) => tilemap.WorldToCell(pos);

		public void Awake()
		{
			tilemap = GetComponent<Tilemap>();
			subsystemManager = GetComponentInParent<SubsystemManager>();
		}

		public void Start()
		{
			if (!Application.isPlaying)
			{
				return;
			}

			if (MatrixManager.Instance == null)
			{
				Logger.LogError("Matrix Manager is missing from the scene", Category.Matrix);
			}
			else
			{
				// TODO Clean up

				if (LayerType == LayerType.Walls)
				{
					MatrixManager.Instance.wallsTileMaps.Add(GetComponent<TilemapCollider2D>(), tilemap);
				}

			}
		}

		public virtual bool IsPassableAt(Vector3Int from, Vector3Int to)
		{
			if (from == to)
			{
				return true;
			}

			BasicTile tileTo = tilemap.GetTile<BasicTile>(to);

			return TileUtils.IsPassable(tileTo);
		}

		public virtual bool IsPassableAt(Vector3Int position)
		{
			BasicTile tile = tilemap.GetTile<BasicTile>(position);
			return TileUtils.IsPassable(tile);
		}

		public virtual bool IsAtmosPassableAt(Vector3Int position)
		{
			BasicTile tile = tilemap.GetTile<BasicTile>(position);
			return TileUtils.IsAtmosPassable(tile);
		}

		public virtual bool IsSpaceAt(Vector3Int position)
		{
			BasicTile tile = tilemap.GetTile<BasicTile>(position);
			return TileUtils.IsSpace(tile);
		}

		public virtual void SetTile(Vector3Int position, GenericTile tile, Matrix4x4 transformMatrix)
		{
			tilemap.SetTile(position, tile);
			tilemap.SetTransformMatrix(position, transformMatrix);
			subsystemManager.UpdateAt(position);
		}

		public virtual LayerTile GetTile(Vector3Int position)
		{
			return tilemap.GetTile<LayerTile>(position);
		}

		public virtual bool HasTile(Vector3Int position)
		{
			return GetTile(position);
		}

		public virtual void RemoveTile(Vector3Int position, bool removeAll=false)
		{
			if (removeAll)
			{
				position.z = 0;

				while (tilemap.HasTile(position))
				{
					tilemap.SetTile(position, null);

					position.z--;
				}
			}
			else
			{
				tilemap.SetTile(position, null);
			}

			position.z = 0;
			subsystemManager.UpdateAt(position);
		}

		public virtual void ClearAllTiles()
		{
			tilemap.ClearAllTiles();
		}

#if UNITY_EDITOR
		public void SetPreviewTile(Vector3Int position, LayerTile tile, Matrix4x4 transformMatrix)
		{
			tilemap.SetEditorPreviewTile(position, tile);
			tilemap.SetEditorPreviewTransformMatrix(position, transformMatrix);
		}

		public void ClearPreview()
		{
			tilemap.ClearAllEditorPreviewTiles();
		}
#endif
	}
