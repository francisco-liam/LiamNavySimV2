struct ShipData
{
    float mass;
    float length;
    float repulsiveCoefficient;
    float reuplsiveExpoenent;
    float targetAngleCoefficient;
    float targetAngleExponent;
    float relativeBearingCoefficient;
    float relativeBearingExponent;
    int numFields;
    
    float3 position;
    
};

StructuredBuffer<ShipData> ships;
int numShips;

void ChangeHeight_float(float3 pos, out float3 newPos)
{
    float repulsivePotential;
    newPos = pos;
    
    for (int i = 0; i < numShips; i++)
    {
        
        float3 diff = ships[i].position - pos;
        float dist = length(diff);
        ships[i].mass;
        
        if(dist < 1000)
            newPos.y += 10;
    }
}