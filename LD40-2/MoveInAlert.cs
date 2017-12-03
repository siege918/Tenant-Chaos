using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD40_2
{
    class MoveInAlert : Alert
    {
        public Tenant tenant;
        public MoveInAlert(Game1 Game, Apartment apartment, int X) : base(Game, apartment, X)
        {
        }

        public override void Initialize(Apartment apartment, Apartment toApartment)
        {
            //TODO: Make tenants more dynamic
            this.tenant = new Tenant(apartment);
            greeting = tenant.GetGreetingLine();
            body = WrapText(
                    "I would like to move in to Apartment " + apartment.ApartmentNumber + ". I am willing to pay $"
                    + (Apartment.baseRent + tenant.RentModifier) + " a month."
                );
            farewell = tenant.GetFarewellLine() + "\n\n" + tenant.Name;
            choices = new string[2];
            choices[0] = "Let them move in.";
            choices[1] = "Wait for someone else.";
            expirationTime = new TimeSpan(0, 0, 30);


        }

        public override void Dismiss()
        {
            apartment.ClearWaitingTenant();
            base.Dismiss();
        }

        public override void Answer(int index)
        {
            if (index == 0)
                apartment.MoveIn(tenant);
            base.Answer(index);
        }
    }
}
