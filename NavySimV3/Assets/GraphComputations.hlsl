#include "GraphCompute.compute"

void ChangeHeight_float(float3 localPos, float3 worldPos, out float3 newLocalPos)
{
    worldPos.y = 0;
    newLocalPos = localPos;
    if(entity != -1)
        newLocalPos.y = CalculatePotential(worldPos, entity);
    else
        newLocalPos = pow(newLocalPos.x, 2) + pow(newLocalPos.z, 2);

}

