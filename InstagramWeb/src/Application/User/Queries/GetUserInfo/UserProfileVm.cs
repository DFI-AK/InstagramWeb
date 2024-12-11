﻿using InstagramWeb.Application.User.Queries.GetUsers;

namespace InstagramWeb.Application.User.Queries.GetUserInfo;
public record UserProfileVm
{
    public IReadOnlyCollection<PostCategoryDto> PostCategories { get; set; } = [];
    public UserDto UserProfile { get; set; } = null!;
}

