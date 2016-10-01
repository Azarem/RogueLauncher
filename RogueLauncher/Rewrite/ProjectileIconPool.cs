using AssemblyTranslator;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;

namespace RogueLauncher.Rewrite
{
    [Rewrite("RogueCastle.ProjectileIconPool")]
    public class ProjectileIconPool : IDisposable
    {
        [Rewrite]
        public void Dispose() { }
        [Rewrite]
        public void Draw(Camera2D camera) { }

        [Rewrite]
        private DS2DPool<ProjectileIconObj> m_resourcePool;


        [Obfuscation(Exclude = true), Rewrite(action: RewriteAction.Replace)]
        public void DestroyIcon(RogueAPI.Projectiles.ProjectileObj projectile)
        {
            ProjectileIconObj attachedIcon = projectile.AttachedIcon as ProjectileIconObj;
            attachedIcon.Visible = false;
            attachedIcon.Rotation = 0f;
            attachedIcon.TextureColor = Color.White;
            attachedIcon.Opacity = 1f;
            attachedIcon.Flip = SpriteEffects.None;
            attachedIcon.Scale = new Vector2(1f, 1f);

            m_resourcePool.CheckIn(attachedIcon);
            attachedIcon.AttachedProjectile = null;
            projectile.AttachedIcon = null;
        }
    }
}
