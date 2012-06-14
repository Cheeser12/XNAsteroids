using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteroidsClone
{
    public class DifficultyRules
    {
        public int MaxSmallAsteroids { get; private set; }
        public int MaxNormalAsteroids { get; private set; }
        public int MaxLargeAsteroids { get; private set; }
        public int MaxHugeAsteroids { get; private set; }

        public float MinSpawnTime { get; private set; }
        public float MaxSpawnTime { get; private set; }

        public DifficultyRules(int maxSmall, int maxNormal, int maxLarge, int maxHuge,
            float minSpawn, float MaxSpawn)
        {
            MaxSmallAsteroids = maxSmall;
            MaxNormalAsteroids = maxNormal;
            MaxLargeAsteroids = maxLarge;
            MaxHugeAsteroids = maxHuge;
            MinSpawnTime = minSpawn;
            MaxSpawnTime = MaxSpawn;
        }
    }
}
