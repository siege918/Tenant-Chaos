using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    public class Apartment
    {
        public static Texture2D tex_wall;
        public static Texture2D tex_window;
        public static Texture2D tex_plaque;
        public const int width = 512;
        public const int height = 256;
        public const int DEFAULTRENT = 400;
        public const double DEFAULTCOMPLAINTODDS = 0.0001d;
        public const double DEFAULTCOMPLAINTVELOCITY = 0.000002d;
        private static Vector2[] _drawPositions;
        double complaintOdds = DEFAULTCOMPLAINTODDS;
        double complaintVelocity = DEFAULTCOMPLAINTVELOCITY;
        int Trust
        {
            get
            {
                return Tenant == null ? 100 : Tenant.Trust;
            }
        }
        public static int baseRent = DEFAULTRENT;
        int rentMod
        {
            get
            {
                return Tenant == null ? 0 : Tenant.RentModifier;
            }
        }
        public string TenantName
        {
            get
            {
                return Tenant == null ? "" : Tenant.Name;
            }
        }
        public double TemperModifier
        {
            get
            {
                return Tenant == null ? 1 : Tenant.TemperModifier;
            }
        }
        Calendar.Date buildDate = null;
        Calendar.Date moveInDate = null;
        public bool HasTenantWaiting = false;
        Calendar calendar;

        Tenant Tenant;

        public string Greeting
        {
            get
            {
                if (Tenant == null)
                {
                    return "Dear Landlord,";
                }
                else
                {
                    return Tenant.GetGreetingLine();
                }
            }
        }

        public string Farewell
        {
            get
            {
                if (Tenant == null)
                {
                    return "Sincerely,";
                }
                else
                {
                    return Tenant.GetFarewellLine();
                }
            }
        }

        public bool IsOccupied
        {
            get
            {
                return moveInDate != null;
            }
        }

        public int Rent
        {
            get
            {
                return rentMod + baseRent;
            }
        }

        public string ApartmentNumber
        {
            get
            {
                if (index.X > 9)
                {
                    return "" + (index.Y + 1) + "" + index.X;
                }
                else
                {
                    return "" + (index.Y + 1) + "0" + index.X;
                }
            }
        }

        public static Vector2[] drawPositions
        {
            get
            {
                if (_drawPositions == null)
                {
                    List<Vector2> drawPos = new List<Vector2>();
                    for (int x = 0; x < width; x += tex_wall.Width)
                    {
                        for (int y = 0; y < height; y += tex_wall.Height)
                        {
                            drawPos.Add(new Vector2(x, y - height));
                        }
                    }
                    _drawPositions = drawPos.ToArray();
                }

                return _drawPositions;
            }
        }

        Rectangle windowRect1;
        Rectangle windowRect2;
        Rectangle plaqueRect;
        Rectangle bounds;
        Vector2 plaquePos;
        Apartment[,] apartments;

        Color color;

        Vector2 position;
        Point index;

        bool stopAlerts = false;

        public Apartment(Vector2 position, Point index, Calendar calendar, Color? color = null)
        {
            buildDate = calendar.GetDate();
            this.calendar = calendar;
            this.position = position;
            this.index = index;

            if (color == null)
            {
                this.color = Color.Gray;
            }
            else
            {
                this.color = color.Value;
            }

            windowRect1 = new Rectangle((int)(position.X + 150), (int)(position.Y - 206), 100, 150);
            windowRect2 = new Rectangle((int)(position.X + 350), (int)(position.Y - 206), 100, 150);
            plaqueRect = new Rectangle((int)(position.X + 55), (int)(position.Y - 175), 64, 32);
            Vector2 plaqueSize = Game1.plaqueFont.MeasureString(ApartmentNumber);
            plaquePos = new Vector2(plaqueRect.X, plaqueRect.Y)
                + ((new Vector2(plaqueRect.Width, plaqueRect.Height) - plaqueSize) / 2);
            bounds = new Rectangle((int)position.X, (int)(position.Y - height), width, height);
        }

        public double AgeInMonths
        {
            get
            {
                return calendar.MonthsSince(buildDate);
            }
        }

        public Alert Update(Game1 game, Apartment[,] apartments, int alertX)
        {
            Alert alert = null;

            if (!stopAlerts && IsOccupied && Trust < 10)
            {
                alert = new MoveOutAlert(game, this, alertX);
                stopAlerts = true;
            }

            this.apartments = apartments;

            if (!IsOccupied)
            {
                if (!HasTenantWaiting)
                {
                    if (Game1.rand.NextDouble() < .005d)
                    {
                        HasTenantWaiting = true;
                        return new MoveInAlert(game, this, alertX);
                    }
                }
                return null;
            }
            if (Game1.rand.NextDouble() < complaintOdds && !stopAlerts)
            {
                double alertTypeCheck = Game1.rand.NextDouble();
                if (alertTypeCheck < .35)
                    alert = new RepairAlert(game, this, alertX);
                else if (alertTypeCheck < .70)
                {
                    List<Point> neighbors = Neighbors;
                    if (neighbors.Count > 0)
                    {
                        Point apartmentIndex = neighbors[Game1.rand.Next(neighbors.Count)];
                        alert = new ComplaintAlert(game, this, alertX, apartments[apartmentIndex.X, apartmentIndex.Y]);
                    }
                    else
                    {
                        alert = new RepairAlert(game, this, alertX);
                    }
                }
                else
                {
                    alert = new RepaintAlert(game, this, alertX);
                }

                //alert = new Alert(game, this, alertX);
                complaintOdds = DEFAULTCOMPLAINTODDS;
            }
            else
            {
                complaintOdds += complaintVelocity;
            }

            return alert;
        }

        public static bool Occupied(Apartment apartment)
        {
            return apartment != null && apartment.IsOccupied;
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }

        private List<Point> neighbors;
        public List<Point> Neighbors
        {
            get
            {
                neighbors = new List<Point>();
                if (index.X > 0 && Occupied(apartments[index.X - 1, index.Y]))
                    neighbors.Add(new Point(index.X - 1, index.Y));
                if (index.Y > 0 && Occupied(apartments[index.X, index.Y - 1]))
                    neighbors.Add(new Point(index.X, index.Y - 1));
                if (Occupied(apartments[index.X + 1, index.Y]))
                    neighbors.Add(new Point(index.X + 1, index.Y));
                if (Occupied(apartments[index.X, index.Y + 1]))
                    neighbors.Add(new Point(index.X, index.Y + 1));
                    
                return neighbors;
            }
        }

        public void MoveIn(Tenant tenant)
        {
            Tenant = tenant;
            this.complaintVelocity *= tenant.TemperModifier;
            moveInDate = calendar.GetDate();
            stopAlerts = false;
            complaintVelocity = DEFAULTCOMPLAINTVELOCITY;
            complaintOdds = DEFAULTCOMPLAINTODDS;
            this.color = Color.Red;
        }

        public void MoveOut()
        {
            moveInDate = null;
            Tenant = null;
            this.color = Color.Gray;
        }

        public void ClearWaitingTenant()
        {
            HasTenantWaiting = false;
        }

        public void ModifyTrust(double rate)
        {
            if (Tenant != null)
                Tenant.ModifyTrust(rate);
        }

        public bool isMouseIn(Point mousePos)
        {
            return mousePos.X > position.X &&
                mousePos.X < position.X + width &&
                mousePos.Y < position.Y &&
                mousePos.Y > position.Y - height;
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            if (!camera.Intersects(bounds))
                return;

            Vector2[] drawPos = drawPositions;
            for (int i = 0; i < drawPos.Length; i++)
            {
                spriteBatch.Draw(tex_wall, position + drawPos[i], color);
            }
            spriteBatch.Draw(tex_window, windowRect1, Color.White);
            spriteBatch.Draw(tex_window, windowRect2, Color.White);
            spriteBatch.Draw(tex_plaque, plaqueRect, Color.White);
            spriteBatch.DrawString(Game1.plaqueFont, ApartmentNumber, plaquePos, Color.Black);
        }
    }
}
