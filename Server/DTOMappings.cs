﻿namespace Server.Data;

internal sealed class DTOMappings : Profile
{
    public DTOMappings()
    {
        CreateMap<Post, PostDTO>().ReverseMap();
    }
}