﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EzySlice {

	/**
	 * Quick Internal structure which checks where the point lays on the
	 * Plane. UP = Upwards from the Normal, DOWN = Downwards from the Normal
	 * ON = Point lays straight on the plane
	 */
	public enum SideOfPlane {
		UP,
		DOWN,
		ON
	}

	/**
	 * Represents a simple 3D Plane structure with a position
	 * and direction which extends infinitely in its axis. This provides
	 * an optimal structure for collision tests for the slicing framework.
	 */
	public struct Plane {
		private Vector3 m_normal;
		private float m_dist;

		// this is for editor debugging only!
		#if UNITY_EDITOR
		private Transform trans_ref;
		#endif

		public Plane(Vector3 pos, Vector3 norm) {
			this.m_normal = norm;
			this.m_dist = Vector3.Dot(norm, pos);

			#if UNITY_EDITOR
			trans_ref = null;
			#endif
		}

		public Plane(Vector3 norm, float dot) {
			this.m_normal = norm;
			this.m_dist = dot;

			#if UNITY_EDITOR
			trans_ref = null;
			#endif
		}

		public void Compute(Vector3 pos, Vector3 norm) {
			this.m_normal = norm;
			this.m_dist = Vector3.Dot(norm, pos);
		}

		public void Compute(Transform trans) {
			Compute(trans.position, trans.up);

			// this is for editor debugging only!
			#if UNITY_EDITOR
			trans_ref = trans;
			#endif
		}

		public void Compute(GameObject obj) {
			Compute(obj.transform);
		}

		public Vector3 normal {
			get { return this.m_normal; }
		}

		public float dist {
			get { return this.m_dist; }
		}

		/**
		 * Checks which side of the plane the point lays on.
		 */
		public SideOfPlane SideOf(Vector3 pt) {
			float result = Vector3.Dot(m_normal, pt) - m_dist;

			if (result > 0.001f) {
				return SideOfPlane.UP;
			}

			if (result < 0.001f) {
				return SideOfPlane.DOWN;
			}

			return SideOfPlane.ON;
		}

		#if UNITY_EDITOR

		/**
		 * Editor only DEBUG functionality. This should not be compiled in the final
		 * Version.
		 */
		public void OnDebugDraw() {
			OnDebugDraw(Color.white);
		}

		public void OnDebugDraw(Color drawColor) {
			if (trans_ref == null) {
				return;
			}

			Color prevColor = Gizmos.color;
			Matrix4x4 prevMatrix = Gizmos.matrix;

			// TO-DO
			Gizmos.matrix = Matrix4x4.TRS(trans_ref.position, trans_ref.rotation, trans_ref.localScale);
			Gizmos.color = drawColor;

			Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.0f, 0.0f, 1.0f));

			Gizmos.color = prevColor;
			Gizmos.matrix = prevMatrix;
		}

		#endif
	}
}