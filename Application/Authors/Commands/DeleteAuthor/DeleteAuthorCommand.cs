using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<Author>
    {
        public DeleteAuthorCommand(Guid authorId)
        {
            AuthorId = authorId;
        }

        public Guid AuthorId { get; set; }
    }
}
