using Application.DTOs.AuthorDto;
using Domain.Models;
using MediatR;

namespace Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommand : IRequest<Author>
    {
        public Guid AuthorId { get; }
        public UpdateAuthorDto UpdateAuthorDto { get; }

        public UpdateAuthorCommand(Guid authorId, UpdateAuthorDto updateAuthorDto)
        {
            AuthorId = authorId;
            UpdateAuthorDto = updateAuthorDto;
        }
    }
    

    
}
