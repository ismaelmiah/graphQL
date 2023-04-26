using System.Reflection;
using System.Runtime.CompilerServices;
using HotChocolate.Types.Descriptors;

public class UseUserAttribute : ObjectFieldDescriptorAttribute
{
    public UseUserAttribute([CallerLineNumber] int order = 0)
    {
        Order = order;
    }

    public override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Use<UserMiddleware>();
    }
}