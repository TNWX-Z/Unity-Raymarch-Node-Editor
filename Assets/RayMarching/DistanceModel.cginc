#ifndef DistanceModel
#define DistanceModel
//---------------------------------------------------
	float Subtract(float a,float b){
		return max(a,-b);
	}

	float SmoothMin(float a,float b){
    	float k = 22.0;
		return -log(exp(-k*a)+exp(-k*b))/k;
	}

	float Min(float a,float b,float _id){
		if(a<b)return a;else{F.ID=_id;return b;}
	}
//---------------------------------------------------
	float Sphere(float3 p,float r){
		return length(p)-r;
	}

	float Cube(float3 p,float r){
		return length(max(abs(p)-r,0.))-0.01;
	}

	float Cubuid(){
		return 0.;
	}

	float Cylinder(){
		return 0.;
	}

	float Capsule(){
		return 0.;
	}

	float Cone(float3 p,float r){
		return 0.;
	}

	float Turus(float3 p,float r){
		return length(p.zx)-r;;
	}

	float Line3D(){
		return 0.;
	}

	float Plane(){
		return 0.;
	}

	float Bezier3D(float3 A,float3 B,float3 C){
		return 0.;
	}
	
#endif