float d1=min(Sphere(p-float3(1,1,1),1),Sphere(p-float3(0,0.968,0.383),1));
float d3=max(Sphere(p-float3(3.99,1.55,0),1),Sphere(p-float3(4.99,1.55,0),1));
float d4=Subtract(Cube(p-float3(2.72,0.84,-1.45),1),Sphere(p-float3(2.72,1.84,-1.45),1));
d=Min(d,d1,0);
d=Min(d,d3,0);
d=Min(d,d4,0);
