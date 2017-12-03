using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    public class Camera2D
    {
        const float CAMERASCALEFACTOR = .0005f;
        const float MINCAMERASCALE = .1f;
        const float MAXCAMERASCALE = 2;
        const float MINX = -200;
        const float MINY = -16500;
        const float MAXX = 35000;
        const float MAXY = 100;

        public float Zoom { get; set; }
        public Vector2 Location { get; set; }
        public float Rotation { get; set; }

        private Rectangle Bounds { get; set; }
        private Rectangle WorldBounds { get; set; }

        private int oldScrollValue = 0;
        private Point? mouseDownPos;
        private Vector2? mouseDownCameraPos;

        public Matrix TransformMatrix
        {
            get {
                return
                    Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            }
        }

        public Camera2D(Viewport viewport)
        {
            Bounds = viewport.Bounds;
        }

        public bool Intersects(Rectangle rect)
        {
            return WorldBounds.Intersects(rect);
        }

        public void Update(int mouseScroll, Point mousePos, MouseState mouseState, bool interactWithCamera = true)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && mouseDownPos == null && mousePos.Y < 960 && interactWithCamera)
            {
                mouseDownPos = mousePos;
                mouseDownCameraPos = Location;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && mouseDownPos != null && mouseDownCameraPos != null)
            {
                Location = mouseDownCameraPos.Value - (new Vector2(mousePos.X - mouseDownPos.Value.X, mousePos.Y - mouseDownPos.Value.Y) / Zoom);
            }
            else
            {
                mouseDownPos = null;
                mouseDownCameraPos = null;
            }

            if (Location.X < MINX)
                Location = new Vector2(MINX, Location.Y);
            if (Location.X > MAXX)
                Location = new Vector2(MAXX, Location.Y);
            if (Location.Y < MINY)
                Location = new Vector2(Location.X, MINY);
            if (Location.Y > MAXY)
                Location = new Vector2(Location.X, MAXY);

            
            int scrollDiff = mouseScroll - oldScrollValue;

            Zoom += scrollDiff * CAMERASCALEFACTOR;
            if (Zoom > MAXCAMERASCALE)
                Zoom = MAXCAMERASCALE;
            if (Zoom < MINCAMERASCALE)
                Zoom = MINCAMERASCALE;


            oldScrollValue = mouseScroll;

            Vector2 topLeft = Vector2.Transform(Vector2.Zero, Matrix.Invert(TransformMatrix));
            Vector2 bottomRight = Vector2.Transform(new Vector2(Bounds.Right, Bounds.Bottom), Matrix.Invert(TransformMatrix));

            WorldBounds = new Rectangle(
                    (int)topLeft.X,
                    (int)topLeft.Y,
                    (int)(bottomRight.X - topLeft.X),
                    (int)(bottomRight.Y - topLeft.Y)
                );

            WorldBounds.Inflate(50, 50);
        }
    }
}
