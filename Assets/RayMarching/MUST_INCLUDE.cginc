#define MUSTINCLUDE
#ifdef MUSTINCLUDE
	#include "UnityCG.cginc"
	float3 my_CamPos(){ return _WorldSpaceCameraPos; }
	float3x3 my_CamRot(){ return float3x3(UNITY_MATRIX_V[0].xyz,UNITY_MATRIX_V[1].xyz,-UNITY_MATRIX_V[2].xyz); }
	float my_NearZ(){ return _ProjectionParams.y; }
	float my_FarZ(){ return _ProjectionParams.z; }
	float my_Aspect(){ return _ScreenParams.x / _ScreenParams.y; }
	float my_View_Length(){ return abs(UNITY_MATRIX_P[1][1]); }
	float3 my_Dir(in float3 dir){ dir.z = my_View_Length(); return normalize( mul(dir,my_CamRot()) ); }
	float3 my_Dir(in float2 _2Dpos){ return normalize( mul(float3(_2Dpos,my_View_Length()),my_CamRot()) ); }
	float4 my_Project_Point(float3 surface){ return mul(UNITY_MATRIX_MVP,float4(surface,1.)); }
	float my_Depth(float4 project_pos){
		#if defined(SHADER_TARGET_GLSL) || defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)
			return ((project_pos.z / project_pos.w) + 1.0) * 0.5;
		#else 
			return project_pos.z / project_pos.w;
		#endif
	}
#endif
