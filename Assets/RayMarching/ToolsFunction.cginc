#include "DistanceModel.cginc"

#ifndef ToolsFunction
#define ToolsFunction 
	float model(float3 p){
		float d = 100000.;
		float temp = 10000.;
		#include "SimpleModel.cginc"
		return d;
	}

	fixed3 my_Normal(float3 surface) {
		const float2 offset = float2(0.001, 0.);
		return normalize(
			float3(
				model(surface + offset.xyy),
				model(surface + offset.yxy),
				model(surface + offset.yyx)
			) - model(surface)
		);
	}

	bool SphereTrace(inout InfoTrace info,float3 campos,fixed3 raydir){
		for(info.Iter = 0.; info.Iter<64.;info.Iter++){
			info.Surface = campos+raydir*info.Distance;
			info.NearD = model(info.Surface);
			info.Distance += info.NearD;
			if(info.NearD < 0.01 || info.Distance >100.) break;
		}
		return info.Distance<100.;
	}
//------------------------------------------------
	float Diffuse(fixed3 nDir,fixed3 lDir){
		return saturate(dot(nDir,lDir));
	}

	float Frenel(fixed3 nDir,fixed3 vDir,float coeff){
		return pow(1.-dot(nDir,vDir),coeff);
	}

	float SSAO(){}

	float SH(){}
	float SSH(){}
//------------------------------------------------
float SoftShadow(float3 ro,float3 rd,float mint,float tmax )
{
  float res = 1.0;
  float t = mint;
  for ( int i=0; i<14; i++ )
  {
    float h = model( ro + rd*t ).x;
    res = min( res, 8.0*h/t );
    t += clamp( h, 0.02, 0.10 );
    if ( h<0.001 || t>tmax ) break;
  }
  return clamp( res, 0.0, 1.0 );
}
float SSAO(float3 pos,float3 nor )
{
  float occ = 0.0;
  float sca = 1.0;
  for ( int i=0; i<5; i++ )
  {
    float hr = 0.01 + 0.12*float(i) / 4.0;
    float3 aopos =  nor * hr + pos;
    float dd = model( aopos ).x;
    occ += -(dd-hr)*sca;
    sca *= 0.95;
  }
  return clamp( 1.0 - 3.0*occ, 0.0, 1.0 );
}
//--------------Material BSDF-----------------------------------

#endif
