using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ChestObj")]
    public class ChestObj : PhysicsObj
    {
        [Rewrite]
        private byte m_chestType;
        [Rewrite]
        private float GoldIncreasePerLevel;// = 1.425f;
        [Rewrite]
        private Vector2 BronzeChestGoldRange;// = new Vector2(9f, 14f);
        [Rewrite]
        private Vector2 SilverChestGoldRange;// = new Vector2(20f, 28f);
        [Rewrite]
        private Vector2 GoldChestGoldRange;// = new Vector2(47f, 57f);
        [Rewrite]
        public int Level;
        [Rewrite]
        private SpriteObj m_arrowIcon;

        [Rewrite]
        public byte ChestType { get; set; }
        [Rewrite]
        public float ForcedAmount { get; set; }
        [Rewrite]
        public int ForcedItemType { get; set; }
        [Rewrite]
        public bool IsEmpty { get; set; }
        [Rewrite]
        public bool IsLocked { get; set; }
        [Rewrite]
        public bool IsOpen { get; }
        [Rewrite]
        public bool IsProcedural { get; set; }


        [Rewrite]
        public ChestObj(PhysicsManager physicsManager) : base("Chest1_Sprite", physicsManager) { }

        [Rewrite]
        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) { }

        [Rewrite]
        protected override GameObj CreateCloneInstance() { return null; }

        [Rewrite]
        public override void Dispose() { }

        [Rewrite]
        public override void Draw(Camera2D camera) { }

        [Rewrite]
        protected override void FillCloneInstance(object obj) { }

        [Rewrite]
        public virtual void ForceOpen() { }

        [Rewrite]
        public void GiveGold(ItemDropManager itemDropManager, int amount = 0) { }

        [Rewrite]
        public void GivePrint(ItemDropManager manager, PlayerObj player) { }

        [Rewrite]
        public void GiveStatDrop(ItemDropManager manager, PlayerObj player, int numDrops, int statDropType) { }

        [Rewrite]
        public virtual void OpenChest(ItemDropManager itemDropManager, PlayerObj player) { }

        [Rewrite]
        public virtual void ResetChest() { }
    }
}
