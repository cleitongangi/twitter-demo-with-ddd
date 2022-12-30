using FluentValidation;
using Posterr.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.Domain.Interfaces.Validations
{
    public interface INewPostValidation : IValidator<PostEntity>
    {
    }
}
