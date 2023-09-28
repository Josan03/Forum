﻿using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class CommentLogic : ICommentLogic
{
    private readonly IPostDao postDao;
    private readonly IUserDao userDao;
    private readonly ICommentDao commentDao;

    public CommentLogic(IPostDao postDao, IUserDao userDao, ICommentDao commentDao)
    {
        this.postDao = postDao;
        this.userDao = userDao;
        this.commentDao = commentDao;
    }
    
    public async Task<Comment> CreateCommentAsync(CommentCreationDto dto)
    {
        User? user = await userDao.GetByIdAsync(dto.OwnerId);
        if (user == null)
            throw new Exception($"User with id {dto.OwnerId} was not found.");
        
        Post? post = await postDao.GetByIdAsync(dto.PostId);
        if (post == null)
            throw new Exception($"Post with id {dto.PostId} was not found.");

        DateTime currentDate = DateTime.Now;
        
        Comment toCreate = new Comment(dto.Content, user, post, currentDate);
        Comment created = await commentDao.CreateCommentAsync(toCreate);
        return created;
    }
}