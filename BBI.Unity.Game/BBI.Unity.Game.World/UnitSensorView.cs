using System;
using UnityEngine;

namespace BBI.Unity.Game.World
{
	// Token: 0x0200038A RID: 906
	public class UnitSensorView
	{
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000B4F0A File Offset: 0x000B310A
		// (set) Token: 0x06001EB6 RID: 7862 RVA: 0x000B4F12 File Offset: 0x000B3112
		public float SensorRange
		{
			get
			{
				return this.mSensorRange;
			}
			set
			{
				this.mSensorRange = value;
				this.UpdateRangeState();
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000B4F21 File Offset: 0x000B3121
		// (set) Token: 0x06001EB8 RID: 7864 RVA: 0x000B4F29 File Offset: 0x000B3129
		public bool Enabled
		{
			get
			{
				return this.mEnabled;
			}
			set
			{
				this.mEnabled = value;
				this.UpdateVisibilityState();
			}
		}

		public bool IsFriendly
		{
			get;
			set;
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000B4F38 File Offset: 0x000B3138
		// (set) Token: 0x06001EBA RID: 7866 RVA: 0x000B4F40 File Offset: 0x000B3140
		public UnitSensorView.SensorMode Mode
		{
			get
			{
				return this.mMode;
			}
			set
			{
				this.mMode = value;
				this.UpdateVisibilityState();
			}
		}

		private static string makeShader(int maskMultiplier)
		{
			int num = maskMultiplier << 1;
			return string.Format("Shader \"[ Custom ]/SensorView/SensorBall\" {{\nProperties {{\n _Color (\"Sphere Color\", Color) = (0,0,0.5,1)\n _IntersectColor (\"Intersect Color\", Color) = (0,0.15,0.15,1)\n}}\nSubShader {{ \n Tags {{ \"QUEUE\"=\"Transparent\" \"FORCENOSHADOWCASTING\"=\"true\" \"IGNOREPROJECTOR\"=\"true\" }}\n Pass {{\n  Tags {{ \"QUEUE\"=\"Transparent\" \"FORCENOSHADOWCASTING\"=\"true\" \"IGNOREPROJECTOR\"=\"true\" }}\n  ZTest Greater\n  ZWrite Off\n  Cull Front\n  Stencil {{\n   Ref {0}\n   ReadMask {1}\n   WriteMask {2}\n   Comp NotEqual\n   Pass Replace\n  }}\n  Blend SrcColor One\n  GpuProgramID 57922\nProgram \"vp\" {{\nSubProgram \"d3d9 \" {{\nBind \"vertex\" Vertex\nMatrix 9 [_Object2World] 1\nMatrix 4 [glstate_matrix_modelview0] 3\nMatrix 0 [glstate_matrix_mvp]\nVector 7 [_ProjectionParams]\nVector 8 [_ScreenParams]\n\"vs_3_0\ndef c10, 0.5, -1, 1, 0\ndcl_position v0\ndcl_position o0\ndcl_texcoord o1\ndcl_texcoord1 o2.xyz\ndcl_texcoord2 o3.x\ndp4 r0.y, c1, v0\nmul r1.x, r0.y, c7.x\nmul r1.w, r1.x, c10.x\ndp4 r0.x, c0, v0\ndp4 r0.w, c3, v0\nmul r1.xz, r0.xyww, c10.x\nmad o1.xy, r1.z, c8.zwzw, r1.xwzw\ndp4 r1.x, c4, v0\ndp4 r1.y, c5, v0\ndp4 r1.z, c6, v0\nmul o2.xyz, r1, c10.yyzw\ndp3 r1.x, c9, c9\nrsq r1.x, r1.x\nrcp o3.x, r1.x\ndp4 r0.z, c2, v0\nmov o0, r0\nmov o1.zw, r0\n\n\"\n}}\nSubProgram \"d3d11 \" {{\nBind \"vertex\" Vertex\nConstBuffer \"UnityPerCamera\" 144\nVector 80 [_ProjectionParams]\nConstBuffer \"UnityPerDraw\" 352\nMatrix 0 [glstate_matrix_mvp]\nMatrix 64 [glstate_matrix_modelview0]\nMatrix 192 [_Object2World]\nBindCB  \"UnityPerCamera\" 0\nBindCB  \"UnityPerDraw\" 1\n\"vs_4_0\nroot12:aaacaaaa\neefiecedgehkoffffnnnnkhihnkacbicklhioejeabaaaaaamaadaaaaadaaaaaa\ncmaaaaaagaaaaaaaoiaaaaaaejfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa\naaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafaepfdejfeejepeoaaklklkl\nepfdeheoiaaaaaaaaeaaaaaaaiaaaaaagiaaaaaaaaaaaaaaabaaaaaaadaaaaaa\naaaaaaaaapaaaaaaheaaaaaaaaaaaaaaaaaaaaaaadaaaaaaabaaaaaaapaaaaaa\nheaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaaahaiaaaaheaaaaaaacaaaaaa\naaaaaaaaadaaaaaaacaaaaaaaiahaaaafdfgfpfagphdgjhegjgpgoaafeeffied\nepepfceeaaklklklfdeieefcnaacaaaaeaaaabaaleaaaaaafjaaaaaeegiocaaa\naaaaaaaaagaaaaaafjaaaaaeegiocaaaabaaaaaaapaaaaaafpaaaaadpcbabaaa\naaaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadpccabaaaabaaaaaa\ngfaaaaadhccabaaaacaaaaaagfaaaaadiccabaaaacaaaaaagiaaaaacacaaaaaa\ndiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaabaaaaaaabaaaaaa\ndcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaaaaaaaaaagbabaaaaaaaaaaa\negaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaabaaaaaaacaaaaaa\nkgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa\nabaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadgaaaaafpccabaaa\naaaaaaaaegaobaaaaaaaaaaadiaaaaaiccaabaaaaaaaaaaabkaabaaaaaaaaaaa\nakiacaaaaaaaaaaaafaaaaaadiaaaaakncaabaaaabaaaaaaagahbaaaaaaaaaaa\naceaaaaaaaaaaadpaaaaaaaaaaaaaadpaaaaaadpdgaaaaafmccabaaaabaaaaaa\nkgaobaaaaaaaaaaaaaaaaaahdccabaaaabaaaaaakgakbaaaabaaaaaamgaabaaa\nabaaaaaadiaaaaaihcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiccaaaabaaaaaa\nafaaaaaadcaaaaakhcaabaaaaaaaaaaaegiccaaaabaaaaaaaeaaaaaaagbabaaa\naaaaaaaaegacbaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaaegiccaaaabaaaaaa\nagaaaaaakgbkbaaaaaaaaaaaegacbaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaa\negiccaaaabaaaaaaahaaaaaapgbpbaaaaaaaaaaaegacbaaaaaaaaaaadiaaaaak\nhccabaaaacaaaaaaegacbaaaaaaaaaaaaceaaaaaaaaaialpaaaaialpaaaaiadp\naaaaaaaadgaaaaagbcaabaaaaaaaaaaaakiacaaaabaaaaaaamaaaaaadgaaaaag\nccaabaaaaaaaaaaaakiacaaaabaaaaaaanaaaaaadgaaaaagecaabaaaaaaaaaaa\nakiacaaaabaaaaaaaoaaaaaabaaaaaahbcaabaaaaaaaaaaaegacbaaaaaaaaaaa\negacbaaaaaaaaaaaelaaaaaficcabaaaacaaaaaaakaabaaaaaaaaaaadoaaaaab\n\"\n}}\n}}\nProgram \"fp\" {{\nSubProgram \"d3d9 \" {{\nMatrix 0 [_Object2World] 3\nMatrix 3 [_ViewToWorld] 3\nVector 8 [_IntersectColor]\nVector 6 [_ProjectionParams]\nVector 7 [_ZBufferParams]\nSetTexture 0 [_CameraDepthTexture] 2D 0\n\"ps_3_0\ndef c9, 1, 0, 0, 0\ndcl_texcoord v0.xyw\ndcl_texcoord1 v1.xyz\ndcl_texcoord2 v2.x\ndcl_2d s0\nrcp r0.x, v0.w\nmul r0.xy, r0.x, v0\ntexld r0, r0, s0\nmad r0.x, c7.x, r0.x, c7.y\nrcp r0.x, r0.x\nrcp r0.y, v1.z\nmul r0.y, r0.y, c6.z\nmul r0.yzw, r0.y, v1.xxyz\nmul r0.xyz, r0.x, r0.yzww\nmov r0.w, c9.x\ndp4 r1.x, c3, r0\ndp4 r1.y, c4, r0\ndp4 r1.z, c5, r0\nmov r0.x, -c0.w\nmov r0.y, -c1.w\nmov r0.z, -c2.w\nadd r0.xyz, r0, r1\ndp3 r0.x, r0, r0\nrsq r0.x, r0.x\nrcp r0.x, r0.x\nadd r0, -r0.x, v2.x\ntexkill r0\nmov oC0, c8\n\n\"\n}}\nSubProgram \"d3d11 \" {{\nSetTexture 0 [_CameraDepthTexture] 2D 0\nConstBuffer \"$Globals\" 192\nMatrix 96 [_ViewToWorld]\nVector 176 [_IntersectColor]\nConstBuffer \"UnityPerCamera\" 144\nVector 80 [_ProjectionParams]\nVector 112 [_ZBufferParams]\nConstBuffer \"UnityPerDraw\" 352\nMatrix 192 [_Object2World]\nBindCB  \"$Globals\" 0\nBindCB  \"UnityPerCamera\" 1\nBindCB  \"UnityPerDraw\" 2\n\"ps_4_0\nroot12:abadabaa\neefieceddkammfddlcdficdkldgmfciddjlialacabaaaaaalaadaaaaadaaaaaa\ncmaaaaaaleaaaaaaoiaaaaaaejfdeheoiaaaaaaaaeaaaaaaaiaaaaaagiaaaaaa\naaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaheaaaaaaaaaaaaaaaaaaaaaa\nadaaaaaaabaaaaaaapalaaaaheaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa\nahahaaaaheaaaaaaacaaaaaaaaaaaaaaadaaaaaaacaaaaaaaiaiaaaafdfgfpfa\ngphdgjhegjgpgoaafeeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaa\naiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfe\ngbhcghgfheaaklklfdeieefcmaacaaaaeaaaaaaalaaaaaaafjaaaaaeegiocaaa\naaaaaaaaamaaaaaafjaaaaaeegiocaaaabaaaaaaaiaaaaaafjaaaaaeegiocaaa\nacaaaaaabaaaaaaafkaaaaadaagabaaaaaaaaaaafibiaaaeaahabaaaaaaaaaaa\nffffaaaagcbaaaadlcbabaaaabaaaaaagcbaaaadhcbabaaaacaaaaaagcbaaaad\nicbabaaaacaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacacaaaaaaaoaaaaah\ndcaabaaaaaaaaaaaegbabaaaabaaaaaapgbpbaaaabaaaaaaefaaaaajpcaabaaa\naaaaaaaaegaabaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadcaaaaal\nbcaabaaaaaaaaaaaakiacaaaabaaaaaaahaaaaaaakaabaaaaaaaaaaabkiacaaa\nabaaaaaaahaaaaaaaoaaaaakbcaabaaaaaaaaaaaaceaaaaaaaaaiadpaaaaiadp\naaaaiadpaaaaiadpakaabaaaaaaaaaaaaoaaaaaiccaabaaaaaaaaaaackiacaaa\nabaaaaaaafaaaaaackbabaaaacaaaaaadiaaaaahocaabaaaaaaaaaaafgafbaaa\naaaaaaaaagbjbaaaacaaaaaadiaaaaahhcaabaaaaaaaaaaaagaabaaaaaaaaaaa\njgahbaaaaaaaaaaadiaaaaaihcaabaaaabaaaaaafgafbaaaaaaaaaaaegiccaaa\naaaaaaaaahaaaaaadcaaaaaklcaabaaaaaaaaaaaegiicaaaaaaaaaaaagaaaaaa\nagaabaaaaaaaaaaaegaibaaaabaaaaaadcaaaaakhcaabaaaaaaaaaaaegiccaaa\naaaaaaaaaiaaaaaakgakbaaaaaaaaaaaegadbaaaaaaaaaaaaaaaaaaihcaabaaa\naaaaaaaaegacbaaaaaaaaaaaegiccaaaaaaaaaaaajaaaaaaaaaaaaajhcaabaaa\naaaaaaaaegacbaaaaaaaaaaaegiccaiaebaaaaaaacaaaaaaapaaaaaabaaaaaah\nbcaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaaaaaaaaaelaaaaafbcaabaaa\naaaaaaaaakaabaaaaaaaaaaaaaaaaaaibcaabaaaaaaaaaaaakaabaiaebaaaaaa\naaaaaaaadkbabaaaacaaaaaadbaaaaahbcaabaaaaaaaaaaaakaabaaaaaaaaaaa\nabeaaaaaaaaaaaaaanaaaeadakaabaaaaaaaaaaadgaaaaagpccabaaaaaaaaaaa\negiocaaaaaaaaaaaalaaaaaadoaaaaab\"\n}}\n}}\n }}\n Pass {{\n  Tags {{ \"QUEUE\"=\"Transparent\" \"FORCENOSHADOWCASTING\"=\"true\" \"IGNOREPROJECTOR\"=\"true\" }}\n  ZWrite Off\n  Stencil {{\n   Ref {3}\n   ReadMask {4}\n   WriteMask {5}\n   Comp Greater\n   Pass Replace\n  }}\n  Blend SrcColor One\n  GpuProgramID 80563\nProgram \"vp\" {{\nSubProgram \"d3d9 \" {{\nBind \"vertex\" Vertex\nMatrix 0 [glstate_matrix_mvp]\n\"vs_3_0\ndcl_position v0\ndcl_position o0\ndp4 o0.x, c0, v0\ndp4 o0.y, c1, v0\ndp4 o0.z, c2, v0\ndp4 o0.w, c3, v0\n\n\"\n}}\nSubProgram \"d3d11 \" {{\nBind \"vertex\" Vertex\nConstBuffer \"UnityPerDraw\" 352\nMatrix 0 [glstate_matrix_mvp]\nBindCB  \"UnityPerDraw\" 0\n\"vs_4_0\nroot12:aaabaaaa\neefiecedefmnijgohikialhmhnlmkmdmphjbnkfmabaaaaaaheabaaaaadaaaaaa\ncmaaaaaagaaaaaaajeaaaaaaejfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa\naaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaafaepfdejfeejepeoaaklklkl\nepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaabaaaaaaadaaaaaa\naaaaaaaaapaaaaaafdfgfpfagphdgjhegjgpgoaafdeieefcniaaaaaaeaaaabaa\ndgaaaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafpaaaaadpcbabaaaaaaaaaaa\nghaaaaaepccabaaaaaaaaaaaabaaaaaagiaaaaacabaaaaaadiaaaaaipcaabaaa\naaaaaaaafgbfbaaaaaaaaaaaegiocaaaaaaaaaaaabaaaaaadcaaaaakpcaabaaa\naaaaaaaaegiocaaaaaaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa\ndcaaaaakpcaabaaaaaaaaaaaegiocaaaaaaaaaaaacaaaaaakgbkbaaaaaaaaaaa\negaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaaaaaaaaaadaaaaaa\npgbpbaaaaaaaaaaaegaobaaaaaaaaaaadoaaaaab\"\n}}\n}}\nProgram \"fp\" {{\nSubProgram \"d3d9 \" {{\nVector 0 [_Color]\n\"ps_3_0\nmov oC0, c0\n\n\"\n}}\nSubProgram \"d3d11 \" {{\nConstBuffer \"$Globals\" 192\nVector 160 [_Color]\nBindCB  \"$Globals\" 0\n\"ps_4_0\nroot12:aaabaaaa\neefiecedoaommfiklghgkpmandkhimpifkjkbfmnabaaaaaanmaaaaaaadaaaaaa\ncmaaaaaagaaaaaaajeaaaaaaejfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaa\naaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfagphdgjhegjgpgoaa\nepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaa\naaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefceaaaaaaaeaaaaaaa\nbaaaaaaafjaaaaaeegiocaaaaaaaaaaaalaaaaaagfaaaaadpccabaaaaaaaaaaa\ndgaaaaagpccabaaaaaaaaaaaegiocaaaaaaaaaaaakaaaaaadoaaaaab\"\n}}\n}}\n }}\n}}\nFallback \"Transparent/Unlit\"\n}}", new object[] { maskMultiplier, maskMultiplier, maskMultiplier, num, num, num });
		}

		static UnitSensorView()
		{
			UnitSensorView.blueMaterial = new Material(UnitSensorView.makeShader(1));
			UnitSensorView.redMaterial = new Material(UnitSensorView.makeShader(4));
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x000B4F50 File Offset: 0x000B3150
		public UnitSensorView(Transform unitXform, float range, GameObject sensorSpherePrefab, GameObject fowPrefab)
		{
			this.mTransform = unitXform;
			this.mSensorSphereObj = UnityEngine.Object.Instantiate<GameObject>(sensorSpherePrefab);
			UnitSensorView.ParentAndSetIdentity(this.mSensorSphereObj.transform, this.mTransform);
			this.mFOWRangeObj = UnityEngine.Object.Instantiate<GameObject>(fowPrefab);
			UnitSensorView.ParentAndSetIdentity(this.mFOWRangeObj.transform, this.mTransform);
			this.mEnabled = false;
			this.mMode = UnitSensorView.SensorMode.GameView;
			this.UpdateVisibilityState();
			this.mSensorRange = range;
			this.UpdateRangeState();
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x000B4FD0 File Offset: 0x000B31D0
		private void UpdateVisibilityState()
		{
			if (MapModManager.EnableEnemySensors)
			{
				MeshRenderer component = this.mSensorSphereObj.GetComponent<MeshRenderer>();
				Color color = new Color(0.0431372561f, 0.0431372561f, 0.149019614f);
				if (this.IsFriendly)
				{
					Color color1 = new Color(0f, 0.003921569f, 0.7176471f);
					Color color2 = new Color(0.211764708f, 0.372549027f, 0.733333349f);
					component.material = UnitSensorView.blueMaterial;
					component.material.SetColor("_Color", color1 - color);
					component.material.SetColor("_IntersectColor", color2 - color);
				}
				else
				{
					Color color3 = new Color(0.360784322f, 0f, 0.7176471f);
					Color color4 = new Color(0.396078438f, 0.211764708f, 0.733333349f);
					component.material = UnitSensorView.redMaterial;
					component.material.SetColor("_Color", color3 - color);
					component.material.SetColor("_IntersectColor", color4 - color);
				}
			}
			bool flag = this.mMode == UnitSensorView.SensorMode.SensorsView;
			this.mSensorSphereObj.SetActive(this.Enabled && flag);
			this.mFOWRangeObj.SetActive(this.Enabled && !flag);
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x000B5018 File Offset: 0x000B3218
		private void UpdateRangeState()
		{
			Vector3 localScale = Vector3.one * this.mSensorRange;
			this.mSensorSphereObj.transform.localScale = localScale;
			localScale.y = 1f;
			this.mFOWRangeObj.transform.localScale = localScale;
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000B5064 File Offset: 0x000B3264
		private static void ParentAndSetIdentity(Transform child, Transform parent)
		{
			child.parent = parent;
			child.localPosition = Vector3.zero;
			child.localRotation = Quaternion.identity;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000B5083 File Offset: 0x000B3283
		public bool FOWRangeObjectEquals(GameObject obj)
		{
			return this.mFOWRangeObj == obj;
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000B5091 File Offset: 0x000B3291
		public bool SensorsSphereObjectEquals(GameObject obj)
		{
			return this.mSensorSphereObj == obj;
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000B509F File Offset: 0x000B329F
		public void SetIdentityRotations()
		{
			this.mSensorSphereObj.transform.rotation = Quaternion.identity;
			this.mFOWRangeObj.transform.rotation = Quaternion.identity;
		}

		// Token: 0x04001923 RID: 6435
		private GameObject mSensorSphereObj;

		// Token: 0x04001924 RID: 6436
		private GameObject mFOWRangeObj;

		// Token: 0x04001925 RID: 6437
		private Transform mTransform;

		// Token: 0x04001926 RID: 6438
		private float mSensorRange;

		// Token: 0x04001927 RID: 6439
		private bool mEnabled;

		// Token: 0x04001928 RID: 6440
		private UnitSensorView.SensorMode mMode;

		private static Material blueMaterial;

		private static Material redMaterial;

		// Token: 0x0200038B RID: 907
		public enum SensorMode
		{
			// Token: 0x0400192A RID: 6442
			GameView,
			// Token: 0x0400192B RID: 6443
			SensorsView
		}
	}
}
