using System;

[Flags]
public enum SystemType
{
    None = 0,
    Logic = 1 << 0,
    Animation = 1 << 1,
    Command = 1 << 2,

    //AllSmoothedRopes = PathSmoother | ExtrudedRope | LineRope | MeshRope,
    //AllRopes = PathSmoother | ExtrudedRope | ChainRope | LineRope | MeshRope | Particles | InstancedParticles,
    //AllClothes = Cloth | SkinnedCloth | TearableCloth | Particles | InstancedParticles,
    //AllParticles = Fluid | Particles | InstancedParticles | FoamParticles
};
