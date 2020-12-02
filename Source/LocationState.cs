using System;

namespace ArtilleryStrike
{
    public class LocationState
    {
        public double Distance { get; set; }
        public double Azimuth { get; set; }
        public bool Default { get; set; }

        public LocationState()
        {
            Clear();
        }

        public LocationState(double distance, double azimuth)
        {
            SetCoords(distance, azimuth);
        }

        public LocationState(LocationState state)
        {
            SetCoords(state.Distance, state.Azimuth);
        }

        public void Clear()
        {
            Distance = 1;
            Azimuth = 0;
            Default = true;
        }

        public Tuple<double, double> GetCoords(out double distance, out double azimuth)
        {
            distance = Distance;
            azimuth = Azimuth;
            return Tuple.Create(Distance, Azimuth);
        }

        public void SetCoords(double distance, double azimuth)
        {
            Distance = distance;
            Azimuth = azimuth;
            Default = false;
        }

        public static bool operator ==(LocationState lhs, LocationState rhs) =>
            lhs.Azimuth == rhs.Azimuth && lhs.Distance == rhs.Distance;
        public static bool operator !=(LocationState lhs, LocationState rhs) =>
            !(lhs == rhs);
        public override bool Equals(object obj) =>
            base.Equals(obj);
        public override int GetHashCode() =>
            base.GetHashCode();

        public void Assign(LocationState rhs)
        {
            Distance = rhs.Distance;
            Azimuth = rhs.Azimuth;
            Default = rhs.Default;
        }

        public static LocationState CalculateVector(LocationState friend, LocationState target)
        {
            if (friend == target)
            {
                return new LocationState(0, 0);
            }

            Side SF = new Side(friend.Distance);    // Spotter to Friend
            Side ST = new Side(target.Distance);    // Spotter to Target
            Side FT;                                // Friend  to Target (result)

            Angle NSF = new Angle(Angle.ToRadians(friend.Azimuth)); // North   to Spotter to Friend
            Angle NST = new Angle(Angle.ToRadians(target.Azimuth)); // North   to Spotter to Target
            Angle NFT;                                              // North   to Friend  to Target (result)
            
            // Calculated values
            Angle SFT;                                              // Spotter to Friend  to Target
            Angle delta;                                            // Change between NSF and NST
            Angle PI = new Angle(Math.PI);                          // 180 degree angle for easy computing.

            delta = new Angle(Math.Abs(NSF.Radians - NST.Radians));
            FT = MathExtension.LawOfCosines(SF, ST, delta);
            // Order matters here. Side ST must always be the 3rd parameter.
            SFT = MathExtension.LawOfCosines(SF, FT, ST);
            double SFTdegrees = Math.Round(SFT.Degrees);

            // Make sure Angle stays between 0 - 360 degrees or 0 and 2PI radians
            delta.Validate();
            NFT = NSF + PI + ( (delta.Degrees > PI.Degrees ^ NST > NSF) ? -SFT : SFT );
            NFT.Validate();

            return new LocationState(FT.Length, NFT.Degrees);
        }
    }
}
