using AssemblyTranslator;
using DS2DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ItemDropObj")]
    public class ItemDropObj : PhysicsObj
    {
        [Rewrite]
        public int DropType;
        [Rewrite]
        private float m_amount;
        [Rewrite]
        public float CollectionCounter { get; set; }
        [Rewrite]
        public bool IsCollectable { get; }

        [Rewrite]
        public ItemDropObj(string spriteName) : base(spriteName, null) { }

        [Rewrite]
        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) { }

        [Rewrite]
        public void ConvertDrop(int dropType, float amount) { }

        [Rewrite]
        public override void Draw(Camera2D camera) { }

        [Rewrite]
        public void GiveReward(PlayerObj player, TextManager textManager) { }
    }
}
