using BusinessLogic.Core.Entities;

namespace BusinessLogic.Infrastructure.Specifications;

internal sealed class UserByPhoneSpecification : SpecificationBase<User>
{
    public UserByPhoneSpecification(string phoneNumber)
        : base(user => user.PhoneNumber.Equals(phoneNumber))
    { 
    }
}