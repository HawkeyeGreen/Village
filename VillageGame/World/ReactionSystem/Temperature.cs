using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.World.ReactionSystem
{
    public class Temperature
    {
        public static Double INFINITE = -1;
        private double kelvin = 0.0;

        public double Kelvin { get => kelvin; set => kelvin = value; }
        public double Celsius { get => kelvin - 273.15; set => kelvin = value + 273.15; }

        public Temperature(double v)
        {
            kelvin = v;
        }

        public static Temperature operator +(Temperature t1, Temperature t2)
        {
            return new Temperature(t1.Kelvin + t2.Kelvin);
        }

        public static Temperature operator -(Temperature t1, Temperature t2)
        {
            return new Temperature(t1.Kelvin - t2.Kelvin);
        }

        public static Temperature operator ++(Temperature t)
        {
            return new Temperature(t.Kelvin++);
        }

        public static Temperature operator --(Temperature t)
        {
            return new Temperature(t.Kelvin--);
        }

        public static Temperature operator *(Temperature t1, Temperature t2)
        {
            return new Temperature(t1.Kelvin * t2.Kelvin);
        }

        public static Temperature operator /(Temperature t1, Temperature t2)
        {
            return new Temperature(t1.Kelvin / t2.Kelvin);
        }

        public static bool operator ==(Temperature t1, Temperature t2)
        {
            return (t1.Kelvin == t2.Kelvin);
        }

        public static bool operator !=(Temperature t1, Temperature t2)
        {
            return (t1.Kelvin != t2.Kelvin);
        }

        public static bool operator <(Temperature t1, Temperature t2)
        {
            return (t1.Kelvin < t2.Kelvin);
        }

        public static bool operator >(Temperature t1, Temperature t2)
        {
            return (t1.Kelvin > t2.Kelvin);
        }

        public static bool operator <=(Temperature t1, Temperature t2)
        {
            return (t1.Kelvin <= t2.Kelvin);
        }

        public static bool operator >=(Temperature t1, Temperature t2)
        {
            return (t1.Kelvin >= t2.Kelvin);
        }

        public static implicit operator Temperature(double t)
        {
            return new Temperature(t);
        }

        public static implicit operator Temperature(int t)
        {
            return new Temperature(t);
        }

        public static implicit operator string(Temperature t)
        {
            return t.ToString();
        }

        public override bool Equals(object obj)
        {
            var temperature = obj as Temperature;
            return temperature != null &&
                   kelvin == temperature.kelvin;
        }

        public override int GetHashCode()
        {
            return 213693268 + kelvin.GetHashCode();
        }

        public override string ToString()
        {
            if(kelvin >= 0)
            {
                return "T=" + kelvin + " K";
            }
            return "T= " + Localization.Local.GetInstance().GetString("INFINITY");
        }
    }
}
