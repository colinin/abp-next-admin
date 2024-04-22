using System;
using System.Collections.Generic;
using System.Linq;

namespace Dapr.Actors.Runtime
{
    public static class ActorRegistrationExtensions
    {
        public static bool Contains(
            this ICollection<ActorRegistration> registrations,
            Type implementationType)
        {
            return registrations.Any(x => x.Type.ImplementationType == implementationType);
        }
    }
}
