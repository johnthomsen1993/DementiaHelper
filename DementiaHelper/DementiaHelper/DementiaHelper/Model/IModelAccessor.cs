using DementiaHelper.Model.Controllers;

namespace DementiaHelper.Model
{
    public interface IModelAccessor
    {
        IAccountController AccountController { get; set; }
    }
}