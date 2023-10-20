using Dapr.Actors;
using Dapr.Actors.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.Abp;

namespace System
{
    internal static class TypeExtensions
    {
        public static bool IsActor(this Type actorType)
        {
            Type baseType = actorType.GetTypeInfo().BaseType;
            while (baseType != null)
            {
                if (baseType == typeof(Actor))
                {
                    return true;
                }

                actorType = baseType;
                baseType = actorType.GetTypeInfo().BaseType;
            }

            return false;
        }

        public static Type[] GetActorInterfaces(this Type type)
        {
            List<Type> list = new List<Type>(from t in type.GetInterfaces()
                                             where typeof(IActor).IsAssignableFrom(t)
                                             select t);
            list.RemoveAll((Type t) => t.GetNonActorParentType() != null);
            return list.ToArray();
        }

        public static RemoteServiceAttribute GetRemoteServiceAttribute(this Type type)
        {
            return type.GetInterfaces()
                .Where(t => t.IsDefined(typeof(RemoteServiceAttribute), false))
                .Select(t => t.GetCustomAttribute<RemoteServiceAttribute>())
                .FirstOrDefault();
        }

        public static Type GetNonActorParentType(this Type type)
        {
            List<Type> list = new List<Type>(type.GetInterfaces());
            if (list.RemoveAll((Type t) => t == typeof(IActor)) == 0)
            {
                return type;
            }

            foreach (Type item in list)
            {
                Type nonActorParentType = item.GetNonActorParentType();
                if (nonActorParentType != null)
                {
                    return nonActorParentType;
                }
            }

            return null;
        }

        public static ActorTypeInformation GetActorTypeInfo(
            this Type actorType)
        {
            if (!actorType.IsActor())
            {
                throw new ArgumentException(
                    string.Format("The type '{0}' is not an Actor. An actor type must derive from '{1}'.", actorType.FullName, typeof(Actor).FullName),
                    nameof(actorType));
            }

            Type[] actorInterfaces = actorType.GetActorInterfaces();
            if (actorInterfaces.Length == 0 && !actorType.GetTypeInfo().IsAbstract)
            {
                throw new ArgumentException(
                    string.Format("The actor type '{0}' does not implement any actor interfaces or one of the interfaces implemented is not an actor interface." +
                    " All interfaces(including its parent interface) implemented by actor type must be actor interface. " +
                    "An actor interface is the one that ultimately derives from '{1}' type.", actorType.FullName, typeof(IActor).FullName),
                    nameof(actorType));
            }

            var actorTypeInfo = ActorTypeInformation.Get(actorType, null);

            var remoteServiceAttr = actorType.GetRemoteServiceAttribute();
            if (remoteServiceAttr != null && 
                !string.Equals(actorTypeInfo.ActorTypeName, remoteServiceAttr.Name))
            {
                typeof(ActorTypeInformation)
                    .GetProperty(nameof(ActorTypeInformation.ActorTypeName), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    ?.SetValue(actorTypeInfo, remoteServiceAttr.Name);
            }

            return actorTypeInfo;
        }
    }
}
