using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.Api.Controllers;
using TaskManagement.Api.Models;
using TaskManagement.Api.Services;
using Xunit;
using System.Collections.Generic;

namespace TaskManagement.Tests
{
    public class TasksControllerTests
    {
        [Fact]
        public void GetTasks_ReturnsOkResultWithTasks()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();
            mockTaskService.Setup(service => service.GetAllTasks())
                .Returns(new List<TaskModel> { new TaskModel { Id = 1, Title = "Task 1" }, new TaskModel { Id = 2, Title = "Task 2" } });

            var controller = new TasksController(mockTaskService.Object);

            // Act
            var result = controller.GetTasks() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var tasks = Assert.IsType<List<TaskModel>>(result.Value);
            Assert.Equal(2, tasks.Count);
        }

        [Fact]
        public void GetTask_ReturnsNotFoundResult_WhenTaskNotFound()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();
            mockTaskService.Setup(service => service.GetTaskById(1))
                .Returns((TaskModel)null);

            var controller = new TasksController(mockTaskService.Object);

            // Act
            var result = controller.GetTask(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void CreateTask_ReturnsCreatedAtActionResult_WhenTaskIsCreated()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();
            var taskModel = new TaskModel { Title = "New Task" };
            var controller = new TasksController(mockTaskService.Object);
            controller.ModelState.Clear(); // Simulate valid model state.

            // Act
            var result = controller.CreateTask(taskModel) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
        }


        [Fact]
        public void UpdateTask_ReturnsNotFoundResult_WhenTaskNotFound()
        {
            // Arrange
            var mockTaskService = new Mock<ITaskService>();
            mockTaskService.Setup(service => service.UpdateTask(It.IsAny<TaskModel>()))
                .Throws(new Exception("Task Not Found"));

            var controller = new TasksController(mockTaskService.Object);
            var taskModel = new TaskModel { Id = 1, Title = "Updated Task" };

            // Act
            var result = controller.UpdateTask(1, taskModel) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("Task Not Found", result.Value);
        }
    }
}