using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace DementiaHelper.WebApi.model
{
    public class AccountInformation
    {
        public int AccountInformationId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public virtual AccountPicture Picture { get; set; }
    }

    public class AccountPicture
    {
        public int AccountPictureId { get; set; }
        public byte[] Image { get; set; }
        public int AccountInformationForeignKey { get; set; }
        public AccountInformation AccountInformation { get; set; }

    }
}
