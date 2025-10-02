using System.Linq;
using AutoMapper;
using TaskManagement.Application.Features.Tasks.Queries;
using TaskManagement.Application.Features.Tasks.Commands;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Common.Mappings;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.AssigneeIds, opt => opt.MapFrom(src => src.Assignees.Select(a => a.UserId)));

        CreateMap<TaskItem, TaskDetailsDto>()
            .IncludeBase<TaskItem, TaskDto>()
            .ForMember(dest => dest.BoardName, opt => opt.Ignore())
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Select(comment =>
                new TaskCommentDto(comment.Id, comment.UserId, comment.Body, comment.CreatedAtUtc))))
            .ForMember(dest => dest.SubTasks, opt => opt.MapFrom(src => src.SubTasks.Select(subTask =>
                new SubTaskDto(subTask.Id, subTask.Title, subTask.IsCompleted))))
            .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments.Select(attachment =>
                new TaskAttachmentDto(attachment.Id, attachment.FileName, attachment.ContentType, attachment.SizeBytes, attachment.Url))));
    }
}
