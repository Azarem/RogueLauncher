using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ItemDropManager")]
    public class ItemDropManager
    {
        [Rewrite]
        private DS2DPool<ItemDropObj> m_itemDropPool;
        [Rewrite]
        private List<ItemDropObj> m_itemDropsToRemoveList;
        [Rewrite]
        private int m_poolSize;
        [Rewrite]
        private PhysicsManager m_physicsManager;
        [Rewrite]
        private bool m_isDisposed;


        [Rewrite]
        public int AvailableItems { get; }
        [Rewrite]
        public bool IsDisposed { get; }


        [Rewrite]
        public ItemDropManager(int poolSize, PhysicsManager physicsManager) { }

        [Rewrite]
        public void DestroyAllItemDrops() { }

        [Rewrite]
        public void DestroyItemDrop(ItemDropObj itemDrop) { }

        [Rewrite]
        public void Dispose() { }

        [Rewrite]
        public void Draw(Camera2D camera) { }

        [Rewrite]
        public void DropItem(Vector2 position, int dropType, float amount) { }

        [Rewrite]
        public void DropItemWide(Vector2 position, int dropType, float amount) { }

        [Rewrite]
        public void Initialize() { }

        [Rewrite]
        public void PauseAllAnimations() { }

        [Rewrite]
        public void ResumeAllAnimations() { }
    }
}
