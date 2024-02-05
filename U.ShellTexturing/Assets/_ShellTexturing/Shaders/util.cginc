// takes in uv coords and a seed value between [0,1]
// returns a random number between [0,1]
float rand(float2 uv, float seed)
{
    float value = sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453 + seed;
    return frac(value);
}

float dist(float2 p1, float2 p2)
{
    return sqrt((p2.x - p1.x) * (p2.x - p1.x) + (p2.y - p1.y) * (p2.y - p1.y));
}