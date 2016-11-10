Shader "My/SphereTracing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	CGINCLUDE
		struct InfoTrace{
			fixed3 nDir;
			fixed3 Col;
			fixed3 Spe;
			fixed  Smo;
			fixed Occ;
			fixed Emi;
			float ID;
			//-----------------
			half Iter;
			half NearD;
			float Distance;
			float3 Surface;
			float4 Project_Pos;
			float Depth;
		};
		InfoTrace F;
	ENDCG
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		//LOD 100
		Cull Off
		Pass //0
		{
			Name "RayMarching_g_buffer"
			Tags { "LightMode" = "Deferred" }
			Stencil 
			{
				Comp Always
				Pass Replace
				Ref 128 //[_StencilNonBackground]
			}
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert 
			#pragma fragment frag 
			#include "G_Buffer.cginc" 
			#include "MUST_INCLUDE.cginc"
			#include "ToolsFunction.cginc"
			fOUT frag(vOUT ou){
				fOUT G_Buffer;
					#if UNITY_UV_STARTS_AT_TOP
						ou.pos_uv.y *= -1.0;
					#endif
					ou.pos_uv.x *= my_Aspect();
					float3 campos = my_CamPos();
					fixed3 rayDir = my_Dir(ou.pos_uv);
					
					F.nDir = fixed3(0.,0.,0.);					
					F.Col = fixed3(0.5,0.5,0.5);
					F.Spe = fixed3(1.,1.,1.);
					F.Occ = 0.8;
					F.Smo = 1.;
					F.Emi = 0.;
					F.ID = 0.;

					F.NearD = 0.;
					F.Distance = my_NearZ();
					F.Surface = float3(0.,0.,0.);
					F.Depth = my_FarZ();
					F.Project_Pos = float4(0.,0.,0.,1.);
					//--------------------------------------
					if (SphereTrace(F,campos,rayDir))
					{
						F.nDir = my_Normal(F.Surface);
						//--------------------
						if(F.ID == 1.){
							F.Col = fixed3(0.5,0.5,0.5);
							F.Spe = fixed3(0.8,0.2,0.);
							F.Occ = 0.7;
							F.Smo = 0.9;
							F.Emi = 0.;
						}
						#include "SimpleRender.cginc"
						//--------------------
						F.Project_Pos = my_Project_Point(F.Surface);
						F.Depth = my_Depth(F.Project_Pos); 
					}
					//--------------------------------------
					G_Buffer.Diffuse_Occlusion = half4(F.Col,F.Occ);
					G_Buffer.Specular_Smoothness = half4(F.Spe,F.Smo);
					G_Buffer.Normal = float4(F.nDir*0.5+0.5,0.);
					G_Buffer.Emission = 1.-F.Emi;//- half4(my_Col/2.,0);
					G_Buffer.Depth = F.Depth;
				return G_Buffer;
			}
			ENDCG
		}
	}
	Fallback Off
}
