using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

namespace ParityTesting
{
    /// <summary>
    /// AutoFixture with every primitive drawn from one seeded Random, so generated objects (and
    /// therefore every fixture blob) are the same on every run.
    /// </summary>
    public static class SeededFixture
    {
        public const int Seed = 20261102;

        public static Fixture Create(int seed = Seed)
        {
            var fixture = new Fixture { RepeatCount = 2 };
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customizations.Insert(0, new SeededPrimitives(new Random(seed)));
            return fixture;
        }

        /// <summary>Tells AutoFixture which concrete type to build for an interface or abstract type.</summary>
        public static Fixture Map<TAbstract, TConcrete>(this Fixture fixture) where TConcrete : TAbstract
        {
            fixture.Customizations.Add(new TypeRelay(typeof(TAbstract), typeof(TConcrete)));
            return fixture;
        }

        public static object? CreateInstance(Fixture fixture, Type type)
        {
            return new SpecimenContext(fixture).Resolve(type);
        }
    }

    internal sealed class SeededPrimitives : ISpecimenBuilder
    {
        private readonly Random random;
        private int counter;

        private static readonly string[] Samples =
        {
            "plain",
            "with \"quotes\" and \\backslash\\",
            "line\nbreak\ttab",
            "unicode é ü 漢字 😀",
            "<html>&amp;'",
            "",
            " padded ",
        };

        public SeededPrimitives(Random random)
        {
            this.random = random;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (request is not Type type)
            {
                if (request is PropertyInfo p)
                {
                    return CreateFor(p.PropertyType, p.Name);
                }
                if (request is ParameterInfo pi)
                {
                    return CreateFor(pi.ParameterType, pi.Name ?? "param");
                }
                if (request is FieldInfo f)
                {
                    return CreateFor(f.FieldType, f.Name);
                }
                return new NoSpecimen();
            }

            return CreateFor(type, type.Name);
        }

        private object CreateFor(Type type, string name)
        {
            counter++;
            if (type == typeof(string))
            {
                return random.Next(6) == 0 ? Samples[random.Next(Samples.Length)] : $"{name}-{counter}";
            }
            if (type == typeof(int))
            {
                return random.Next(1, 100000);
            }
            if (type == typeof(long))
            {
                return random.Next(4) == 0 ? (long)random.Next() * 1000 + random.Next(1000) : random.Next(1, 100000);
            }
            if (type == typeof(short))
            {
                return (short)random.Next(1, short.MaxValue);
            }
            if (type == typeof(byte))
            {
                return (byte)random.Next(1, 255);
            }
            if (type == typeof(uint))
            {
                return (uint)random.Next(1, 100000);
            }
            if (type == typeof(ulong))
            {
                return (ulong)random.Next(1, 100000);
            }
            if (type == typeof(double))
            {
                return random.Next(3) == 0 ? (double)random.Next(1, 1000) : Math.Round(random.NextDouble() * 1000, random.Next(1, 6));
            }
            if (type == typeof(float))
            {
                return (float)Math.Round(random.NextDouble() * 100, 2);
            }
            if (type == typeof(decimal))
            {
                return Math.Round((decimal)random.NextDouble() * 1000, random.Next(0, 4));
            }
            if (type == typeof(bool))
            {
                return random.Next(2) == 0;
            }
            if (type == typeof(char))
            {
                return (char)random.Next('a', 'z');
            }
            if (type == typeof(DateTime))
            {
                var kind = (DateTimeKind)random.Next(3);
                var ticks = new DateTime(2015, 1, 1).Ticks + (long)(random.NextDouble() * TimeSpan.FromDays(4000).Ticks);
                if (random.Next(2) == 0)
                {
                    ticks -= ticks % TimeSpan.TicksPerSecond;
                }
                return new DateTime(ticks, kind);
            }
            if (type == typeof(DateTimeOffset))
            {
                var ticks = new DateTime(2015, 1, 1).Ticks + (long)(random.NextDouble() * TimeSpan.FromDays(4000).Ticks);
                return new DateTimeOffset(ticks, TimeSpan.FromMinutes(30 * random.Next(-20, 24)));
            }
            if (type == typeof(TimeSpan))
            {
                return TimeSpan.FromSeconds(random.Next(1, 100000));
            }
            if (type == typeof(Guid))
            {
                var bytes = new byte[16];
                random.NextBytes(bytes);
                return new Guid(bytes);
            }
            if (type == typeof(Uri))
            {
                return new Uri($"https://example.com/{name}/{counter}?q={random.Next()}");
            }
            if (type.IsEnum)
            {
                var values = Enum.GetValues(type);
                return values.Length == 0 ? Activator.CreateInstance(type)! : values.GetValue(random.Next(values.Length))!;
            }
            return new NoSpecimen();
        }
    }
}
