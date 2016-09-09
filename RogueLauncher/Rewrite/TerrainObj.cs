using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.TerrainObj")]
    public class TerrainObj : BlankObj
    {
        public bool ShowTerrain = true;

        [Rewrite]
        public Rectangle NonRotatedBounds { get { return new Rectangle((int)base.X, (int)base.Y, this.Width, this.Height); } }

        public TerrainObj(int width, int height) : base(width, height)
        {
            base.CollisionTypeTag = 1;
            base.IsCollidable = true;
            base.IsWeighted = false;
        }

        [Rewrite]
        protected override GameObj CreateCloneInstance()
        {
            return new TerrainObj(this._width, this._height);
        }

        [Rewrite]
        public override void Draw(Camera2D camera)
        {
            if (this.ShowTerrain && CollisionMath.Intersects(this.Bounds, camera.Bounds) || base.ForceDraw)
                camera.Draw(Game.GenericTexture, base.Position, new Rectangle(0, 0, this.Width, this.Height), base.TextureColor, MathHelper.ToRadians(base.Rotation), Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }

        [Rewrite]
        protected override void FillCloneInstance(object obj)
        {
            base.FillCloneInstance(obj);

            var terrainObj = obj as TerrainObj;
            terrainObj.ShowTerrain = this.ShowTerrain;
            foreach (CollisionBox collisionBox in base.CollisionBoxes)
                terrainObj.AddCollisionBox(collisionBox.X, collisionBox.Y, collisionBox.Width, collisionBox.Height, collisionBox.Type);
        }

        //public override void PopulateFromXMLReader(XmlReader reader, CultureInfo ci)
        //{
        //    base.PopulateFromXMLReader(reader, ci);
        //    base.SetWidth(this._width);
        //    base.SetHeight(this._height);
        //    base.AddCollisionBox(0, 0, this._width, this._height, 0);
        //    base.AddCollisionBox(0, 0, this._width, this._height, 2);
        //    if (base.CollidesTop && !base.CollidesBottom && !base.CollidesLeft && !base.CollidesRight)
        //    {
        //        base.SetHeight(this.Height / 2);
        //    }
        //}

        //private struct CornerPoint
        //{
        //    public Vector2 Position;

        //    public float Rotation;

        //    public CornerPoint(Vector2 position, float rotation)
        //    {
        //        this.Position = position;
        //        this.Rotation = MathHelper.ToRadians(rotation);
        //    }
        //}
    }
}
