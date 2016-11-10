#define G_BUFFER
#ifdef G_BUFFER
	struct vOUT{
		float4 vertex : SV_POSITION;
		float2 pos_uv : TEXCOORD0;
	};
	struct fOUT{
		half4 Diffuse_Occlusion   : SV_Target0;// RT0: Diffuse color (rgb), occlusion (a)
		half4 Specular_Smoothness : SV_Target1;// RT1: spec color (rgb), smoothness (a)
		half4 Normal 			  : SV_Target2;// RT2: normal (rgb), --unused, very low precision-- (a)
		half4 Emission			  : SV_Target3;// RT3: Emission (rgb), --unused-- (a)
		float Depth 			  : SV_Depth;  // depth 
	};
	#include "UnityCG.cginc"
	vOUT vert(float4 vertex : POSITION){
		vOUT o;
			o.vertex = vertex;
			o.pos_uv = vertex.xy;
		return o;
	}
#endif