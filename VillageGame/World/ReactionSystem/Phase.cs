using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.VillageGame.World.ReactionSystem
{
    public class Phase
    {
        private double amount; // mol
        private double volume; // m3
        private double mass; // kg

        private Temperature temperature;

        public Temperature Temperature
        {
            get => temperature;
            set
            {
                Aggregate newAggregate = Substance.GetAggregate(value);
                if (newAggregate != aggregate)
                {
                    switch(newAggregate)
                    {
                        case Aggregate.Solid:
                            aggregateChange = AggregateChange.Solidifying;
                            break;
                        case Aggregate.Liquid:
                            aggregateChange = AggregateChange.Liquifying;
                            break;
                        case Aggregate.Gaseous:
                            aggregateChange = AggregateChange.Vaporizing;
                            break;
                        case Aggregate.Plasma:
                            aggregateChange = AggregateChange.Ionizating;
                            break;
                        default:
                            aggregateChange = AggregateChange.Stable;
                            break;
                    }
                }
                else
                {
                    aggregateChange = AggregateChange.Stable;
                }
                temperature = value;
            }
        }

        private Aggregate aggregate; // Derzeitiger Aggregatszustand
        private AggregateChange aggregateChange; // In welcher Umwandlung ist die Phase?

        private Material material; // Das derzeitige Material.

        public Substance Substance => material.Substance;
        public Material Material => material;

        /// <summary>
        /// Die Stoffmenge in mol.
        /// </summary>
        public double Amount
        {
            get => amount;

            set
            {
                amount = value;

                mass = amount * material.Substance.Mass / 1000;               
                volume = mass / material.Substance.GetDensity(aggregate);
            }
        }

        /// <summary>
        /// Das Volumen in m3.
        /// </summary>
        public double Volume
        {
            get => volume;

            set
            {
                volume = value;

                mass = volume * material.Substance.GetDensity(aggregate);
                amount = mass / material.Substance.Mass / 100;
            }
        }

        /// <summary>
        /// Die derzeitige Masse der Phase in kg.
        /// </summary>
        public double Mass
        {
            get => mass;

            set
            {
                mass = value;

                amount = mass / material.Substance.Mass / 100;
                volume = mass / material.Substance.GetDensity(aggregate);
            }
        }

        /// <summary>
        /// Initialisiert eine Phase mit allen wichtigen Parametern.
        /// </summary>
        /// <param name="amount">Die Stoffmenge in mol. Daraus werden Masse und Volumen hergeleitet.</param>
        /// <param name="material">Das Material.</param>
        public Phase(double amount, Material material, Temperature tmp)
        {
            temperature = tmp;
            aggregate = material.Substance.GetAggregate(temperature);
            Amount = amount;
            aggregateChange = AggregateChange.Stable;
        }
    }
}
