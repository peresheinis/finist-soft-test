using BusinessLogic.Core.Entities;

namespace BusinessLogic.Infrastructure.Specifications;

internal sealed class UserByIdSpecification : SpecificationBase<User>
{
    public UserByIdSpecification(Guid userId) 
        : base(user => user.Id.Equals(userId))
    {
    }
}