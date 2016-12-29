using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Tools.Components
{
    public class Camera
    {
        public Camera(Viewport view, Vector2 position)
        {
            View = view;
            Position = position;
            Zoom = 1.0f;
            Rotation = 0.0f;
            FocusPoint = Point.Zero;

        }

        #region Propriétés
        public Viewport View { get; set; }
        public Vector2 Position { get; set; }
        public Point FocusPoint { get; set; }
        public float Zoom { get; set; }
        public float Rotation { get; set; }
        public Matrix Transform
        {
            get
            {
                //Faire l'exemple avec les positifs et les négatifs
                return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                       Matrix.CreateRotationZ(Rotation) *
                       Matrix.CreateScale(Zoom) *
                       Matrix.CreateTranslation(new Vector3(FocusPoint.X, FocusPoint.Y, 0));
            }

        }
        #endregion
    }
}
