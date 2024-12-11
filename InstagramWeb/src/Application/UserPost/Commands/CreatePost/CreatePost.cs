using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Models;
using InstagramWeb.Domain.Entities;

namespace InstagramWeb.Application.UserPost.Commands.CreatePost;

public record CreatePostCommand : IRequest<Result>
{
    public string Content { get; init; } = null!;
    public List<string>? ImageUrls { get; init; } = new();
}

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required.")
                               .MaximumLength(1000).WithMessage("Content cannot exceed 1000 characters.");
        //RuleFor(x => x.ImageUrls).NotNull().WithMessage("Images cannot be null.");
    }
}


public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IUser _user;

    public CreatePostCommandHandler(IApplicationDbContext context, IUser user)
    {
        _context = context;
        _user = user;
    }

    public async Task<Result> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //var user = await _context.UserProfiles.FindAsync([request.UserId], cancellationToken: cancellationToken);
            if (_user == null)
            {
                throw new ArgumentNullException("User does not exist");
            }

            // Create the post entity
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                UserId = _user.Id,
                Content = request.Content,
            };

            // Add associated images
            //foreach (var imageUrl in request.ImageUrls)
            //{
            //    post.Images.Add(new PostImages
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        PostId = post.Id,
            //        ImageUrl = imageUrl
            //    });
            //}

            // Add the post to the database
            _context.Posts.Add(post);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (ArgumentNullException ex)
        {
            return Result.Failure([ex.Message]);
        }
    }
}
